using CollectingIdeas.DataAccess.Data;
using CollectingIdeas.DataAccess.Repository.IRepository;
using CollectingIdeas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingIdeas.DataAccess.Repository
{
    public class IdeaRepository : Repository<Idea>, IIdeaRepository
    {
        private readonly ApplicationDbContext _db;
        public IdeaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Idea idea)
        {
            _db.Ideas.Update(idea);
        }

        public Idea GetIdea(int id)
        {
            return _db.Ideas.Include(i => i.Topic).Include(i => i.Category).FirstOrDefault(i => i.Id == id);
        }
    }
}
