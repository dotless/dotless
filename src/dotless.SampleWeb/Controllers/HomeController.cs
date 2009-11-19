namespace dotless.SampleWeb.Controllers
{
    using System.Web.Mvc;

    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

    }

    public class SomeDTO : IDTO
    {
        public string Name { get; set; }
    }

    public interface IDTO
    {
        string Name { get; set; }
    }
}