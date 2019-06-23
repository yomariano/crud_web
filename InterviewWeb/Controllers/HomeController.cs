using System.Web.Mvc;

namespace InterviewWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
//            Response.Redirect("~/Products/Index");

            return View();
        }
    }
}