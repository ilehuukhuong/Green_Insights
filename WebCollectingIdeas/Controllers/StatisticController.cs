using CollectingIdeas.DataAccess.Repository.IRepository;
using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Mvc;

namespace WebCollectingIdeas.Controllers
{
    public class StatisticController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public StatisticController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public List<object> Percentageofideas()
        {
            List<object> data = new List<object>();
            List<string> labels = _unitOfWork.Department.GetAll().Select(i => i.Name).ToList();
            data.Add(labels);
            int totalIdeas = _unitOfWork.Idea.GetAll().Count();
            List<int> labelsint = _unitOfWork.Department.GetAll().Select(i => i.Id).ToList();
            List<int> datasets = new List<int>();
            foreach (int i in labelsint)
            {
                datasets.Add((int)Math.Round((double)(100 * _unitOfWork.Idea.GetAll(h => h.ApplicationUser.DepartmentId == i).Count()) / totalIdeas));
            }
            data.Add(datasets);
            return data;
        }
        [HttpPost]
        public List<object> Numberofideas()
        {
            List<object> data = new List<object>();
            List<string> labels = _unitOfWork.Department.GetAll().Select(i => i.Name).ToList();
            data.Add(labels);
            List<int> labelsint = _unitOfWork.Department.GetAll().Select(i => i.Id).ToList();
            List<int> datasets = new List<int>();
            foreach (int i in labelsint)
            {
                datasets.Add(_unitOfWork.Idea.GetAll(h => h.ApplicationUser.DepartmentId == i).Count());
            }
            data.Add(datasets);
            return data;
        }
        [HttpPost]
        public List<object> Numberofcontributors()
        {
            List<object> data = new List<object>();
            List<string> labels = _unitOfWork.Department.GetAll().Select(i => i.Name).ToList();
            data.Add(labels);
            List<int> labelsint = _unitOfWork.Department.GetAll().Select(i => i.Id).ToList();
            List<int> datasets = new List<int>();
            foreach (int i in labelsint)
            {
                datasets.Add(_unitOfWork.ApplicationUser.GetAll(h => h.DepartmentId == i).Count());
            }
            data.Add(datasets);
            return data;
        }
    }
}
