using System.Web.Mvc;

namespace InterviewWeb.Controllers
{
    public class ProductDetailsController : Controller
    {
        // GET: ProductDetail
        public ActionResult Index(int id)
        {
            return View(id);
        }
    }
}