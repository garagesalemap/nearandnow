using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NearAndNow.Models
{
    public class SaleListAndSearchDate
    {
        public string SearchDate { get; set; }
        public List<SaleModel> SaleList { get; set; }
    }
}
