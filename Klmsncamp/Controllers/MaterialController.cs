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

				var materials2 = db.Materials.Include(m => m.Location).Include(m => m.CorporateAccount).Include(m => m.ParentMaterial).Include(m => m.MaterialCategory).Include(m => m.MaterialGroup).Include(m => m.MaterialType).Include(m => m.ValidationState).Where(m=>m.ValidationStateID==1);
				return View(materials2.ToList());
			}
			var materials = db.Materials.Include(m => m.Location).Include(m => m.CorporateAccount).Include(m => m.ParentMaterial).Include(m => m.MaterialCategory).Include(m => m.MaterialGroup).Include(m => m.MaterialType).Include(m => m.ValidationState).Where(s => s.MaterialCategoryID == secilenID).Where(m=>m.ValidationStateID==1);
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
		public ActionResult Edit(Material material, IEnumerable<HttpPostedFileBase> files, FormCollection formcollection)
		{
			if (ModelState.IsValid)
			{
				Material secilenMaterial = db.Materials.SingleOrDefault(s => s.MaterialID == material.MaterialID);
				secilenMaterial.Description = material.Description;
				secilenMaterial.ComponentModel = material.ComponentModel;
				secilenMaterial.CorporateAccount = material.CorporateAccount;
				secilenMaterial.CorporateAccountID = material.CorporateAccountID;
				secilenMaterial.Location = secilenMaterial.Location;
				secilenMaterial.LocationID = material.LocationID;
				secilenMaterial.MaterialCategory = material.MaterialCategory;
				secilenMaterial.MaterialCategoryID = secilenMaterial.MaterialCategoryID;
				secilenMaterial.MaterialCodeNum = material.MaterialCodeNum;
				secilenMaterial.MaterialFiles = material.MaterialFiles;
				secilenMaterial.MaterialGroup = material.MaterialGroup;
				secilenMaterial.MaterialGroupID = material.MaterialGroupID;
				secilenMaterial.MaterialType = material.MaterialType;
				secilenMaterial.MaterialTypeID = material.MaterialTypeID;
				secilenMaterial.Note = material.Note;
				secilenMaterial.ParentMaterial = material.ParentMaterial;
				secilenMaterial.ParentMaterialID = material.ParentMaterialID;
				secilenMaterial.RackLocation = material.RackLocation;
				secilenMaterial.ValidationState = material.ValidationState;
				secilenMaterial.ValidationStateID = material.ValidationStateID;

				//db.SaveChanges();

				#region MaterialKlasorleriniKaydet

				if (files != null)
				{
					foreach (var file in files)
					{
						if (file != null)
						{
							string filename = null;
							string fileType = null;
							byte[] fileContents = null;

							fileContents = new byte[file.ContentLength];
							file.InputStream.Read(fileContents, 0, file.ContentLength);
							fileType = file.ContentType;
							filename = file.FileName;

							MaterialFile materialFile = new MaterialFile();
							materialFile.MaterialFileName = filename;
							materialFile.MaterialFileContentType = fileType;
							materialFile.MaterialFileSize = fileContents != null ? fileContents.Length : 0;
							materialFile.MaterialFileContents = fileContents;
							materialFile.MaterialID = material.MaterialID;
							materialFile.MaterialFileDescription = formcollection["EklenenDosyaAciklama"].ToString();

							db.MaterialFiles.Add(materialFile);

							//MaterialFile materialFile = new MaterialFile();
							//materialFile.MaterialFileName = file.FileName;
							//materialFile.MaterialFileContentType = file.ContentType;
							//materialFile.MaterialFileSize = file.ContentLength;
							//materialFile.MaterialFileContents = new byte[file.ContentLength];
							//file.InputStream.Read(materialFile.MaterialFileContents, 0, materialFile.MaterialFileSize);
							//materialFile.MaterialID = material.MaterialID;
							//db.MaterialFiles.Add(materialFile);
							//db.SaveChanges();
						}
					}
				}

				#endregion

				//db.Entry(material).State = EntityState.Modified;
				db.SaveChanges();
				string materialCategoryID = material.MaterialCategoryID.ToString();
				return RedirectToAction("Index", new { ID=materialCategoryID });

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
			//material.ValidationStateID = 2;
			//db.SaveChanges();
			return View(material);
		}

		//
		// POST: /Material/Delete/5

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			Material material = db.Materials.Find(id);
			//db.Materials.Remove(material);
			material.ValidationStateID = 2;
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}


		public ActionResult DeleteMaterialFile(int fileId,int materailId)
		{
			MaterialFile secilenFile = db.MaterialFiles.FirstOrDefault(s => s.MaterialFileID == fileId);
			db.MaterialFiles.Remove(secilenFile);
			db.SaveChanges();
			return RedirectToAction("Edit", new { id = materailId });
		}

		public ActionResult DeleteMaterialFileFromIndex(int fileId, int materailCategoryId)
		{
			MaterialFile secilenFile = db.MaterialFiles.FirstOrDefault(s => s.MaterialFileID == fileId);
			db.MaterialFiles.Remove(secilenFile);
			db.SaveChanges();
			return RedirectToAction("Index", new { ID = materailCategoryId });
		}

		public ActionResult DownloadMaterialFile(int fileId)
		{
			MaterialFile indirilecekFile = db.MaterialFiles.Find(fileId);
			byte[] fileBytes = (byte[])indirilecekFile.MaterialFileContents;
			return File(fileBytes, indirilecekFile.MaterialFileContentType, indirilecekFile.MaterialFileName);
		}
	}
}