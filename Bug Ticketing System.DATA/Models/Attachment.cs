using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bug_Ticketing_System.DATA.Models
{
    public class Attachment
    {
        [Key]
        public int ID { get; set; }
        [StringLength(50)]
        public string? FileName { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }
        public int BugId { get; set; }
        public Bug Bug { get; set; }
    }
}
