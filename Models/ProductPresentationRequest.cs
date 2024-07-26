using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class ProductPresentationRequest
    {
        public int? ClassCode { get; set; }
        public int? TherapeuticalClassCode { get; set; }
        public int? BusinessUnitCode { get; set; }
    }
}