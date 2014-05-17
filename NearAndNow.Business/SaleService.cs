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
        private string mLastErrorMessage = string.Empty;

        public string LastErrorMessage { get { return mLastErrorMessage; } }

        public SaleService()
        {
            mLastErrorMessage = string.Empty;
        }

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
                mLastErrorMessage = "error occurred" + ex.Message;
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

            try
            {
                NANEntities dbContext = new NANEntities();

                // get the date from the searchdate and tomorrow in order to bracket the search interval
                DateTime justDate = vSearchDate.Date;
                DateTime plusOneDay = justDate.AddDays(1);

                var query = dbContext.Sales.Where(s => EntityFunctions.TruncateTime(s.SaleDate) >= justDate && EntityFunctions.TruncateTime(s.SaleDate) < plusOneDay)
                                                    .Select(s => s);

                returnList = query.ToList();
            }
            catch (Exception ex)
            {
                mLastErrorMessage = "error occurred" + ex.Message;
            }

			return returnList;
		} //ListSalesByDate

		/// <summary>
		/// Get a new empty Sale object ready to fill
		/// </summary>
		/// <returns></returns>
		public Sale GetNewSale()
		{
            Sale returnValue;
            try
            {
                NANEntities dbContext = new NANEntities();
                returnValue = dbContext.Sales.Create();
            }
            catch (Exception ex)
            {
                returnValue = null;
                mLastErrorMessage = "error occurred" + ex.Message;
            }
            return returnValue;
		}
	} //class SaleService
       
} //namespace NearAndNow.Business
