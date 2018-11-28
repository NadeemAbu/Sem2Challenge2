using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Basketball.Models;
using Microsoft.AspNet.Identity;

namespace Basketball.Controllers
{
    [Authorize]
    public class AspNetUsersController : Controller
    {
        private AspUserModelView userview = new AspUserModelView();
        private Entities db = new Entities();

        // GET: AspNetUsers
        public async Task<ActionResult> Index()
        {
            string userID = User.Identity.GetUserId();
            AspNetUser currentuser = db.AspNetUsers.SingleOrDefault(a => a.Id == userID);
            if (currentuser.Roles == 1 || currentuser.Roles == 2 && currentuser.EmailConfirmed == true)
            {
                userview.Games = db.Games.ToList();
                userview.AspNetUsers = await db.AspNetUsers.Where(a => a.EmailConfirmed == false).ToListAsync();
                return View(userview);
            }
            else
            {
                ViewData["Message"] = "You are not authorized";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: AspNetUsers/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }


        
        // GET: AspNetUsers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(aspNetUser);
        }

        // GET: AspNetUsers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            db.AspNetUsers.Remove(aspNetUser);
            await db.SaveChangesAsync();
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

        public async Task<ActionResult> Approve(AspNetUser item)
        {
            AspNetUser usertoUpdate = db.AspNetUsers.SingleOrDefault(a => a.Id == item.Id);

            usertoUpdate.EmailConfirmed = true;
            usertoUpdate.Roles = 2;

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
