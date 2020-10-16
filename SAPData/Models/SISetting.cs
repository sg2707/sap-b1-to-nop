using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPData.Models
{
    [Table("@SI7_SETTING")]
    public class SI7_Setting
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string U_Value { get; set; }
    }
}
