using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Entities
{
    public partial class Laboratory
    {
        public Laboratory()
        {
            CustomMarketDetails = new HashSet<CustomMarketDetail>();
            Products = new HashSet<Product>();
            LaboratoryGroupDetails = new HashSet<LaboratoryGroupDetail>();
        }

        public int Code { get; set; }
        public string Imscode { get; set; }
        public string Description { get; set; }
        public bool OwnLab { get; set; }

        [JsonIgnore]
        public virtual ICollection<CustomMarketDetail> CustomMarketDetails { get; set; }

        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }

        [JsonIgnore]
        public virtual ICollection<LaboratoryGroupDetail> LaboratoryGroupDetails { get; set; }
    }
}
