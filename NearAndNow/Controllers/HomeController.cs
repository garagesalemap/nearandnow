using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NearAndNow.Business;
using NearAndNow.Models;
using System.Web.Script.Serialization;

namespace NearAndNow.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your app description page.";

			return View();
		}

		public ActionResult Confirm(SaleModel model)
		{
			return View(model);
		}

		/// <summary>
		/// get the list of sales for today
		/// Called from the map page to populate the map
		/// </summary>
		/// <returns></returns>
		public ActionResult GetSalesList()
		{
			DateTime searchDate = DateTime.Today;
			// save the starting date
			Session["SearchDate"] = searchDate;

            return Json(GetSalesListByDate(searchDate));
		}

		/// <summary>
		/// Change the date by increment of one day up or down
		/// </summary>
		/// <param name="vStep">+1 or -1</param>
		/// <returns></returns>
		public ActionResult GetSalesListIncremental(int vStep)
		{
			DateTime searchDate = DateTime.Parse(Session["SearchDate"].ToString());

			// now increment or decrement the searchdate by the step value
            searchDate = searchDate.AddDays(vStep);
			// save the new date
			Session["SearchDate"] = searchDate;

			return Json(GetSalesListByDate(searchDate));
		}

		public ActionResult AddNew()
		{
			ViewBag.Message = "Add a new garage sale event";

			SaleModel model = new SaleModel();
			return View(model);
		}//AddNew

		[HttpPost]
		public ActionResult AddNew(SaleModel model)
		{
			if (ModelState.IsValid)
			{
				SaleService salesService = new SaleService();
				Sale newSale = salesService.GetNewSale();

				MapSaleModelToSale(model, newSale, true);

				if (salesService.SaveNewSale(newSale))
				{
					model = MapSaleToSaleModel(newSale);
					return View("Confirm", model);
				}
				else
				{
					ModelState.AddModelError("", "Could not save sale details.");
				}
			}
			else
			{
				ModelState.AddModelError("", "Sale event details are not correct.");
			}

			return View(model);
		} //AddNew

		#region Private methods

		private SaleModel MapSaleToSaleModel(Sale vItem)
		{
			return new SaleModel()
			{
				Id = vItem.Id,
				Title = vItem.Title,
				Description = vItem.Description,
				Address = vItem.Address,
				LatLong = vItem.LatLong,
				SaleDate = vItem.SaleDate,
				LinkUrl = vItem.LinkUrl
			};
		} //MapSaleToSaleModel

		private void MapSaleModelToSale(SaleModel vItem, Sale newSale, bool vIsNew)
		{
			if (!vIsNew)
			{
				newSale.Id = vItem.Id;
			}
			newSale.Title = vItem.Title;
			newSale.Description = vItem.Description;
			newSale.Address = vItem.Address;
            newSale.LatLong = "10000,20000";//			vItem.LatLong;
			newSale.SaleDate = vItem.SaleDate;
			newSale.LinkUrl = vItem.LinkUrl;

		} //MapSaleModelToSale


		/// <summary>
		/// Get a list of sales for a specified date
		/// </summary>
		/// <param name="searchDate"></param>
		/// <returns></returns>
        private JsonResult GetSalesListByDate(DateTime searchDate)
		{
			SaleService salesService = new SaleService();

			List<Sale> saleList = salesService.ListSalesByDate(searchDate);

            if (!string.IsNullOrEmpty(salesService.LastErrorMessage))
            {
                Response.StatusCode = 500;
                Response.StatusDescription = salesService.LastErrorMessage;
                return Json(null);
            }
            else
            {
			List<SaleModel> viewList = saleList.Select(s => MapSaleToSaleModel(s)).ToList();

			JavaScriptSerializer js = new JavaScriptSerializer();
            SaleListAndSearchDate jsonData = new SaleListAndSearchDate() { SaleList = viewList, SearchDate = searchDate.ToShortDateString() };
			return Json(jsonData);

            }
        } //SaleListAndSearchDate

		#endregion Private methods

	}
}
