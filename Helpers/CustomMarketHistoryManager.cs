using System;
using System.Collections.Generic;

namespace WebApi.Helpers
{

    public class MarketDetailResult
    {
        public List<VersionDetail> Versions { get; set; }
    }

    public class VersionDetail
    {
        public int VersionCode { get; set; }
        public List<ProductDetail> VersionDetails { get; set; }
        public DateTime versionDate { get; set; }
    }

    public class ProductDetail
    {
        public int ID { get; set; }
        public int ProductCode { get; set; }
        public decimal Modifier { get; set; }
        public decimal INTEmodifier { get; set; }
        public bool OwnProduct { get; set; } 
        public string EntityDesc { get; set; }
        public int EntityID { get; set; }
        public string ProductPresentationDescription { get; set; }
        public int ProductPresentationCode { get; set; }
        public string ProductDescription { get; set; }
        public string LabDescription { get; set; }
        public int LabCode { get; set; }
        public string TCDescription { get; set; }
        public string TCCode { get; set; }
        public int? ClassCode { get; set; }
        public string? PFCode { get; set; }
        public string? PFDescription { get; set; }
        public string Pattern { get; set; }
        public int EnsureVisible { get; set; } 
        public int Resume { get; set; } 
        public int Expand { get; set; } 
        public int Graph { get; set; } 
        public int ItemCondition { get; set; }
        public int Orden { get; set; } 
        public string Drug { get; set; }
        public string BusinessUnit { get; set; }

    }


    public class JsonResult
    {
        public string JsonData { get; set; }
    }



}
