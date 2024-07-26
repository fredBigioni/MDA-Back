using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Entities
{
    public partial class Class
    {
        public Class()
        {
            TherapeuticalClasses = new HashSet<TherapeuticalClass>();
            CustomMarketDetails = new HashSet<CustomMarketDetail>();
            ProductPresentations = new HashSet<ProductPresentation>();
        }

        public int Code { get; set; }
        public string Imscode { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<TherapeuticalClass> TherapeuticalClasses { get; set; }
        [JsonIgnore]
        public virtual ICollection<CustomMarketDetail> CustomMarketDetails { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProductPresentation> ProductPresentations { get; set; }
    }
}
