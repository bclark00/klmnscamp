using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Klmsncamp.Models;
using Telerik.Web.Mvc;

namespace Klmsncamp.Controllers
{
	public class MaterialController : Controller
	{
		private KlmsnContext db = new KlmsnContext();

		//
		// GET: /Material/

		public ViewResult Index(string ID)
		{
			List<Material> list = db.Materials.ToList();
			int secilenID = Convert.ToInt32(ID);

			ViewBag.kategoriID = ID;

			ViewBag.MaterialCategories = db.MaterialCategories.Include(m => m.ParentMaterialCategory).Include(m => m.MaterialCategoryChilds).Include(m => m.ValidationState).Where(m => m.ParentMaterialCategoryID == 0 || m.ParentMaterialCategoryID == null);

			if (secilenID == 1)
			{
				ViewBag.kategoriAdi = "Genel";

				var materials2 = db.Materials.Include(m => m.Location).Include(m => m.CorporateAccount).Include(m => m.ParentMaterial).Include(m => m.MaterialCategory).Include(m => m.MaterialGroup).Include(m => m.MaterialType).Include(m => m.ValidationState);
				return View(materials2.ToList());
			}
			var materials = db.Materials.Include(m => m.Location).Include(m => m.CorporateAccount).Include(m => m.ParentMaterial).Include(m => m.MaterialCategory).Include(m => m.MaterialGroup).Include(m => m.MaterialType).Include(m => m.ValidationState).Where(s => s.MaterialCategoryID == secilenID);
			return View(materials.ToList());
		}


		public ViewResult Arama(string deger, string kategoriID)
		{
			int kategoriId = Convert.ToInt32(kategoriID);
			ViewBag.MaterialCategories = db.MaterialCategories.Include(m => m.ParentMaterialCategory).Include(m => m.MaterialCategoryChilds).Include(m => m.ValidationState).Where(m => m.ParentMaterialCategoryID == 0 || m.ParentMaterialCategoryID == null);
			ViewBag.kategoriID = kategoriID;
			List<Material> materialList;
			if (kategoriId == 1)
			{
				materialList = db.Materials.Where(s => s.Description.Contains(deger)).ToList();
			}
			else
			{
				materialList = db.Materials.Where(s => s.Description.Contains(deger) && s.MaterialCategoryID == kategoriId).ToList();
			}

			return View("Index", materialList);
		}


		public ViewResult Details(int id)
		{
			Material material = db.Materials.Find(id);
			return View(material);
		}

		//
		// GET: /Material/Create

		public ActionResult Create()
		{
			ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description");
			ViewBag.CorporateAccountID = new SelectList(db.CorporateAccounts, "CorporateAccountID", "Title");
			ViewBag.ParentMaterialID = new SelectList(db.Materials, "MaterialID", "Description");
			ViewBag.MaterialCategoryID = new SelectList(db.MaterialCategories, "MaterialCategoryID", "Description");
			ViewBag.MaterialGroupID = new SelectList(db.MaterialGroups, "MaterialGroupID", "Description");
			ViewBag.MaterialTypeID = new SelectList(db.MaterialTypes, "MaterialTypeID", "Description");
			ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description");
			return View();
		}

		//
		// POST: /Material/Create

		[HttpPost]
		public ActionResult Create(Material material)
		{
			if (ModelState.IsValid)
			{
				db.Materials.Add(material);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description", material.LocationID);
			ViewBag.CorporateAccountID = new SelectList(db.CorporateAccounts, "CorporateAccountID", "Title", material.CorporateAccountID);
			ViewBag.ParentMaterialID = new SelectList(db.Materials, "MaterialID", "Description", material.ParentMaterialID);
			ViewBag.MaterialCategoryID = new SelectList(db.MaterialCategories, "MaterialCategoryID", "Description", material.MaterialCategoryID);
			ViewBag.MaterialGroupID = new SelectList(db.MaterialGroups, "MaterialGroupID", "Description", material.MaterialGroupID);
			ViewBag.MaterialTypeID = new SelectList(db.MaterialTypes, "MaterialTypeID", "Description", material.MaterialTypeID);
			ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", material.ValidationStateID);
			return View(material);
		}

		//
		// GET: /Material/Edit/5

		public ActionResult Edit(int id)
		{
			Material material = db.Materials.Find(id);
			ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description", material.LocationID);
			ViewBag.CorporateAccountID = new SelectList(db.CorporateAccounts, "CorporateAccountID", "Title", material.CorporateAccountID);
			ViewBag.ParentMaterialID = new SelectList(db.Materials, "MaterialID", "Description", material.ParentMaterialID);
			ViewBag.MaterialCategoryID = new SelectList(db.MaterialCategories, "MaterialCategoryID", "Description", material.MaterialCategoryID);
			ViewBag.MaterialGroupID = new SelectList(db.MaterialGroups, "MaterialGroupID", "Description", material.MaterialGroupID);
			ViewBag.MaterialTypeID = new SelectList(db.MaterialTypes, "MaterialTypeID", "Description", material.MaterialTypeID);
			ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", material.ValidationStateID);
			return View(material);
		}

		//
		// POST: /Material/Edit/5

		[HttpPost]
		public ActionResult Edit(Material material)
		{
			if (ModelState.IsValid)
			{
				db.Entry(material).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description", material.LocationID);
			ViewBag.CorporateAccountID = new SelectList(db.CorporateAccounts, "CorporateAccountID", "Title", material.CorporateAccountID);
			ViewBag.ParentMaterialID = new SelectList(db.Materials, "MaterialID", "Description", material.ParentMaterialID);
			ViewBag.MaterialCategoryID = new SelectList(db.MaterialCategories, "MaterialCategoryID", "Description", material.MaterialCategoryID);
			ViewBag.MaterialGroupID = new SelectList(db.MaterialGroups, "MaterialGroupID", "Description", material.MaterialGroupID);
			ViewBag.MaterialTypeID = new SelectList(db.MaterialTypes, "MaterialTypeID", "Description", material.MaterialTypeID);
			ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", material.ValidationStateID);
			return View(material);
		}

		//
		// GET: /Material/Delete/5

		public ActionResult Delete(int id)
		{
			Material material = db.Materials.Find(id);
			return View(material);
		}

		//
		// POST: /Material/Delete/5

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			Material material = db.Materials.Find(id);
			db.Materials.Remove(material);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}