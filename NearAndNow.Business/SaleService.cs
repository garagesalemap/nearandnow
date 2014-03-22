using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Data.Entity;
using System.Data.Objects;
using System.Diagnostics;

namespace NearAndNow.Business
{
	public class SaleService
	{
		public bool SaveNewSale(Sale vSale)
		{
			bool returnStatus = true;
			try
			{
				NANEntities dbContext = new NANEntities();
				dbContext.Sales.Add(vSale);
				dbContext.SaveChanges();
				dbContext.Entry<Sale>(vSale).Reload();
			}
			catch (Exception ex)
			{
				Debug.Write(ex.Message);
				returnStatus = false;
			}

			return returnStatus;
		} //SaveNewSale

		/// <summary>
		/// Get the list of sales for just the specified day
		/// </summary>
		/// <param name="vSearchDate"></param>
		/// <returns></returns>
		public List<Sale> ListSalesByDate(DateTime vSearchDate)
		{
			List<Sale> returnList = new List<Sale>();
			NANEntities dbContext = new NANEntities();

			// get the date from the searchdate and tomorrow in order to bracket the search interval
			DateTime justDate = vSearchDate.Date;
			DateTime plusOneDay = justDate.AddDays(1);

			var query = dbContext.Sales.Where(s => EntityFunctions.TruncateTime(s.SaleDate) >= justDate && EntityFunctions.TruncateTime(s.SaleDate) < plusOneDay)
												.Select(s => s);

			returnList = query.ToList();

			return returnList;
		} //ListSalesByDate

		/// <summary>
		/// Get a new empty Sale object ready to fill
		/// </summary>
		/// <returns></returns>
		public Sale GetNewSale()
		{
			NANEntities dbContext = new NANEntities();
			return dbContext.Sales.Create();
		}
	} //class SaleService
       
} //namespace NearAndNow.Business
