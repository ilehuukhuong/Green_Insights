using CollectingIdeas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingIdeas.DataAccess.Repository.IRepository
{
    public interface IIdeaRepository : IRepository<Idea>
    {
        void Update(Idea idea);
        Idea GetIdea(int id);
    }
}
