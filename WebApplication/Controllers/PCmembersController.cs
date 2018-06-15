using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication;

namespace WebApplication.Controllers
{
    public class PCmembersController : Controller
    {
        private UserModelContainer db = new UserModelContainer();

        // GET: PCmembers
        public ActionResult Index()
        {
            return View(db.PCmembers.ToList());
        }

        // GET: PCmembers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PCmember pCmember = db.PCmembers.Find(id);
            if (pCmember == null)
            {
                return HttpNotFound();
            }
            return View(pCmember);
        }

        // GET: PCmembers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PCmembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_pcmember,id_user,id_conference,is_chair,date_invitation_sent,date_invitation_acc,is_valid")] PCmember pCmember)
        {
            if (ModelState.IsValid)
            {
                db.PCmembers.Add(pCmember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pCmember);
        }

        // GET: PCmembers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PCmember pCmember = db.PCmembers.Find(id);
            if (pCmember == null)
            {
                return HttpNotFound();
            }
            return View(pCmember);
        }

        // POST: PCmembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_pcmember,id_user,id_conference,is_chair,date_invitation_sent,date_invitation_acc,is_valid")] PCmember pCmember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pCmember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pCmember);
        }

        // GET: PCmembers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PCmember pCmember = db.PCmembers.Find(id);
            if (pCmember == null)
            {
                return HttpNotFound();
            }
            return View(pCmember);
        }

        // POST: PCmembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PCmember pCmember = db.PCmembers.Find(id);
            db.PCmembers.Remove(pCmember);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
