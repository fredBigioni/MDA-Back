using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class Drug
    {
        public Drug()
        {
            CustomMarketDetails = new HashSet<CustomMarketDetail>();
            DrugGroupDetails = new HashSet<DrugGroupDetail>();
        }

        public int Code { get; set; }
        public string Imscode { get; set; }
        public string Description { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<CustomMarketDetail> CustomMarketDetails { get; set; }

        [JsonIgnore]
        public virtual ICollection<DrugGroupDetail> DrugGroupDetails { get; set; }        
    }
}
