using CollectingIdeas.DataAccess.Data;
using CollectingIdeas.DataAccess.Repository.IRepository;
using CollectingIdeas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingIdeas.DataAccess.Repository
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private readonly ApplicationDbContext _db;
        public CommentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Comment comment)
        {
            _db.Comments.Update(comment);
        }
        public IEnumerable<Comment> GetAllComment()
        {
            IQueryable<Comment> query = DbSet.Include(i => i.IdentityUser);
            return query.ToList();
        }
    }
}
