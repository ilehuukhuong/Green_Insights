using CollectingIdeas.DataAccess.Data;
using CollectingIdeas.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingIdeas.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public void Save()
        {
            _db.SaveChanges();
        }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Department = new DepartmentRepository(_db);
            Topic = new TopicRepository(_db);
            Idea = new IdeaRepository(_db);
        }
        public ICategoryRepository Category { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public ITopicRepository Topic { get; private set;}
        public IIdeaRepository Idea { get; private set; }
    }
}
