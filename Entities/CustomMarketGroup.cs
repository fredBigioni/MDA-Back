using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Entities
{
    public partial class CustomMarketGroup
    {
        public CustomMarketGroup()
        {
            CustomMarketDetails = new HashSet<CustomMarketDetail>();
        }
        
        public int Code { get; set; }
        public int CustomMarketCode { get; set; }
        public string Description { get; set; }
        public string GroupCondition { get; set; }

        public virtual CustomMarket CustomMarket { get; set; }
        [JsonIgnore]
        public virtual ICollection<CustomMarketDetail> CustomMarketDetails { get; set; }        
    }
}
