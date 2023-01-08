using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using prjContact.Models;

namespace prjContact.Controllers
{
    public class HomeController : Controller
    {
        dbContactEntities dbContact = new dbContactEntities();

        public ActionResult Index()
        {
            var contacts = dbContact.Person
                           .OrderBy(m => m.Id)
                           .ToList();

            return View(contacts);
        }

        public ActionResult About()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Person newPerson)
        {
            int Id = dbContact.Person.Count() + 1;
            newPerson.Id = Id;

            if (ModelState.IsValid)
            {
                ViewBag.Error = false;
                var tmpPerson = dbContact.Person
                           .Where(m => m.LastName == newPerson.LastName)
                           .Where(m => m.FirstName == newPerson.FirstName)
                           .FirstOrDefault();

                if (tmpPerson != null)
                {
                    ViewBag.Error = true;

                    return View(tmpPerson);
                }
                dbContact.Person.Add(newPerson);
                dbContact.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(newPerson);
        }

        public ActionResult Delete(int Id)
        {
            var contact = dbContact.Person
                          .Where(m => m.Id == Id)
                          .FirstOrDefault();

            dbContact.Person.Remove(contact);
            dbContact.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int Id)
        {
            var person = dbContact.Person
                       .Where(m => m.Id == Id)
                       .FirstOrDefault();

            return View(person);
        }

        [HttpPost]
        public ActionResult Edit(Person updPerson)
        {
            if (ModelState.IsValid)
            {
                var contact = dbContact.Person
                           .Where(m => m.Id == updPerson.Id)
                           .FirstOrDefault();

                contact.FirstName = updPerson.FirstName;
                contact.LastName = updPerson.LastName;
                contact.Gender = updPerson.Gender;
                contact.Telephone = updPerson.Telephone;
                contact.Email = updPerson.Email;

                dbContact.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(updPerson);
        }
    }
}