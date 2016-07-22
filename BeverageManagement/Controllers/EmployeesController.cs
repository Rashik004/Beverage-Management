using System;
using System.Linq;
using System.Web.Mvc;
using BeverageManagement.Models.EntityModel;
namespace BeverageManagement.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private BeverageManagementEntities db = new BeverageManagementEntities();
        public ActionResult Index()
        {
            var employees = db.Employees.ToList();
            return View(employees);
        }

        // GET: Beverage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beverage/Create
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Employees.Add(employee);
                    db.SaveChanges();// insert into 
                    return RedirectToAction("Index");

                }

            }
            catch
            {
                throw new Exception("We can't create data.");

            }

            return RedirectToAction("Index");


        }

        // GET: Beverage/Edit/5
        public ActionResult Edit(int id)
        {
            var employee = db.Employees.Find(id);

            return View(employee);
        }

        // POST: Beverage/Edit/5
        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(employee).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges(); // update
                    return RedirectToAction("Index");

                }
            }
            catch
            {
                throw new Exception("We can't save the modified data.");

            }
            return View(employee);
        }

        // GET: Beverage/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Beverage/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection) {
            var employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            try {
                db.SaveChanges();
            }
            catch(Exception exception ) {
                throw exception;
            }
            return RedirectToAction("Index");

        }
    }
}
