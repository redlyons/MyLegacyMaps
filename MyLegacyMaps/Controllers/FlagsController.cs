using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyLegacyMaps.DataAccess;
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public Task<bool> Create([Bind(Include = "FlagTypeId,AdoptedMapId,Name,Xpos,Ypos")] Flag flag)
        public ActionResult Create(int adoptedMapId, int flagTypeId, int xPos, int yPos)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Flags.Add(flag);
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}

            //return Task.FromResult<bool>(true);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
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
        public async Task<ActionResult> Edit([Bind(Include = "FlagId,FlagTypeId,AdoptedMapId,Name,Xpos,Ypos")] Flag flag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flag).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(flag);
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
            Flag flag = await db.Flags.FindAsync(id);
            db.Flags.Remove(flag);
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
    }
}
