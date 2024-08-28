using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Entities
{
    public partial class CustomMarket
    {
        public CustomMarket()
        {
            CustomMarketDetails = new HashSet<CustomMarketDetail>();
            CustomMarketGroups = new HashSet<CustomMarketGroup>();
            DetailCustomMarketDetails = new HashSet<CustomMarketDetail>();
        }

        public int Code { get; set; }
        [JsonIgnore]
        public bool ControlPanel { get; set; }
        public string Description { get; set; }
        public bool DrugReport { get; set; }
        public string Footer { get; set; }
        public string Header { get; set; }
        public int IsOtc { get; set; }
        public bool? LabReport { get; set; }
        public int? LineCode { get; set; }
        public int? MarketClass { get; set; }
        public int? MarketFilter { get; set; }
        public int? MarketReference { get; set; }
        public int? Order { get; set; }
        public bool? ProductReport { get; set; }
        public string Status { get; set; }
        public byte[] Stamp { get; set; }
        public bool TestMarket { get; set; }
        public bool? Tcreport { get; set; }
        public bool? TravelCrm { get; set; }
        public string? AdUser { get; set; }
        public string? ResponsibleName { get; set; }
        public string? ResponsibleLastName { get; set; }

        public virtual Line Line { get; set; }
        public virtual ICollection<CustomMarketDetail> CustomMarketDetails { get; set; }
        [JsonIgnore]
        public virtual ICollection<CustomMarketDetail> DetailCustomMarketDetails { get; set; }
        [JsonIgnore]
        public virtual ICollection<CustomMarketGroup> CustomMarketGroups { get; set; }
    }
}
