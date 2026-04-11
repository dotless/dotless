using System.Collections.Generic;
using System.Web.Mvc;
using dotless.SampleWeb.Models;

namespace dotless.SampleWeb.Controllers
{
    [HandleError]
    public class ChildrenController : Controller
    {
        public ActionResult Index()
        {
            var children = new List<ChildViewModel>
            {
                new ChildViewModel { Name = "Alice", Age = 7, Description = "Loves drawing and painting" },
                new ChildViewModel { Name = "Bob", Age = 10, Description = "Enjoys football and reading" },
                new ChildViewModel { Name = "Clara", Age = 5, Description = "Favourite hobby is playing piano" },
                new ChildViewModel { Name = "David", Age = 12, Description = "Keen on science and robotics" },
                new ChildViewModel { Name = "Eva", Age = 8, Description = "Passionate about swimming" }
            };

            var model = new ChildrenOverviewViewModel("Parent", children);

            return View(model);
        }
    }
}
