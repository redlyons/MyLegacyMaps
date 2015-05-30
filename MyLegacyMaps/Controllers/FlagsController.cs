using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyLegacyMaps.DataAccess;
using MyLegacyMaps.DataAccess.Resources;
using MyLegacyMaps.Models;

namespace MyLegacyMaps.Controllers
{
    public class FlagsController : Controller
    {
        private MyLegacyMapsContext db = new MyLegacyMapsContext();

        // GET: Flags
        public async Task<ActionResult> Index()
        {
            return View(await db.Flags.ToListAsync());
        }

        // GET: Flags/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flag flag = await db.Flags.FindAsync(id);
            if (flag == null)
            {
                return HttpNotFound();
            }
            return View(flag);
        }

        // GET: Flags/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Flags/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FlagTypeId,AdoptedMapId,Name,Xpos,Ypos")] Flag flag)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Flags.Add(flag);
                    await db.SaveChangesAsync();
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }                
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch(Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: Flags/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flag flag = await db.Flags.FindAsync(id);
            if (flag == null)
            {
                return HttpNotFound();
            }
            return View(flag);
        }

        // POST: Flags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,Xpos,Ypos")] int flagId, string name, int xPos, int yPos)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(flag).State = EntityState.Modified;
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // GET: Flags/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flag flag = await db.Flags.FindAsync(id);
            if (flag == null)
            {
                return HttpNotFound();
            }
            return View(flag);
        }

        // POST: Flags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Flag flag = await db.Flags.FindAsync(id);
                if (flag != null)
                {
                    db.Flags.Remove(flag);
                    await db.SaveChangesAsync();
                }
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch(Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
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
