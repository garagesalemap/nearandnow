using System;
using System.ComponentModel.DataAnnotations;

namespace NearAndNow.Models
{
	public class SaleModel
	{
		public int Id { get; set; }

		[Required]
		public string Title { get; set; }

		public string Description { get; set; }

		[Required]
		public string Address { get; set; }

		public string LatLong { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTime SaleDate { get; set; }

		//[Required]
		//[DataType(DataType.Time)]
		//public string SaleTime { get; set; }

		public string LinkUrl { get; set; }

	}
}