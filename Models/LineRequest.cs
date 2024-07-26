using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class LineRequest
    {
        [Required]
        public string Description { get; set; }
        public int LineGroupCode { get; set; }
        public string LaboratoryReportHeader { get; set; }
        public string LaboratoryReportFooter { get; set; }
        public string DrugReportHeader { get; set; }
        public string DrugReportFooter { get; set; }        
    }
}