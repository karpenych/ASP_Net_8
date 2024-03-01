using Dapper_proj.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dapper_proj.Controllers
{
    public class HomeController : Controller
    {
        IMyRepository rep;

        public HomeController(IMyRepository r)
        {
            rep = r;
        } 

        public IActionResult Index()
        {
            return View(rep.GetAll());
        } 

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Tennisist tennisist)
        {
            rep.Create(tennisist);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            Tennisist tennisist = rep.Get(id);
            if (tennisist != null) return View(tennisist);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            rep.Delete(id);
            return RedirectToAction("Index");
        } 

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Tennisist tennisist = rep.Get(id);
            if (tennisist != null)
                return View(tennisist);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(Tennisist tennisist)
        {
            rep.Update(tennisist);
            return RedirectToAction("Index");
        }

        public IActionResult q_Index()
        {
            return View("q_Index");
        }
    }
}
