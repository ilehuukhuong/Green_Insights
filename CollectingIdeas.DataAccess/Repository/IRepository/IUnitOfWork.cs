using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingIdeas.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IDepartmentRepository Department { get; }
        ITopicRepository Topic { get; }
        IIdeaRepository Idea { get; }
        IViewRepository View { get; }
        void Save();
    }
}
