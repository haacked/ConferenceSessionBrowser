using System.Web.Mvc;

namespace ConferenceSessionBrowser.Controllers {
    public class AccountController : Controller {
        public ActionResult LogOn() {
            return View();
        }

        public ActionResult Register() {
            return View();
        }
    }
}
