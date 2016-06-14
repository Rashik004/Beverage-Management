using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeverageManagement.BusinessLogic;
using BeverageManagement.Models;
using BeverageManagement.Models.EntityModel;
using DevMvcComponent;
using BeverageManagement.Constants;
namespace BeverageManagement.Controllers
{
    public class EmployeesController : Controller
    {
        public void Testing() {
            ExcelConversion test= new ExcelConversion();
            test.test();
            //return View("Index");

        }
        // GET: Beverage
        private BeverageManagementEntities db = new BeverageManagementEntities();
        public ActionResult Index()
        {
            var employees = db.Employees.ToList();
            return View(employees);
        }

        // GET: Beverage/Details/5
        public ActionResult Details(int id)
        {
            return View();
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

            return View(employee);

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
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
