using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class UserPermissionResponse
    {
        public List<int?> LineGroupCodes { get; set; }
        public List<int?> LineCodes { get; set; }
        public List<int?> CustomMarketCodes { get; set; }
        public bool FullAccess { get; set; }
    }
}