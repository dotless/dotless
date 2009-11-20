namespace dotless.SampleWeb.Controllers
{
    using System.Web.Mvc;

    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}