using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class CustomMarketActualDefinition
    {
        public int CustomMarketCode { get; set; }
        public int? LineCode { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public bool ProductReport { get; set; }
        public bool DrugReport { get; set; }
        public int IsOtc { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public int? MarketFilter { get; set; }
        public bool TestMarket { get; set; }
        public bool ControlPanel { get; set; }
        public bool? LabReport { get; set; }
        public bool? Tcreport { get; set; }
        public string Status { get; set; }
        public byte[] Stamp { get; set; }
        public int? MarketClass { get; set; }
        public int? MarketReference { get; set; }
        public bool? TravelCrm { get; set; }
        public DateTime VersionDate { get; set; }
        public int? SignedUser { get; set; }
    }
}
