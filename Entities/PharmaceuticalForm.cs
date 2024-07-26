using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Entities
{
    public partial class PharmaceuticalForm
    {
        public PharmaceuticalForm()
        {
            CustomMarketDetails = new HashSet<CustomMarketDetail>();
        }
        public int Code { get; set; }
        public string Imscode { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<CustomMarketDetail> CustomMarketDetails { get; set; }        
    }
}
