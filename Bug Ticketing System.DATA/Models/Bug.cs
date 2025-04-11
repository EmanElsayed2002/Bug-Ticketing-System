using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bug_Ticketing_System.DATA.Models
{
    public class Bug
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public BugStatus BugStatus { get; set; }
        public BugPriority BugPriority { get; set; }
        public DateTime CreatedDate { get; set; }
        public Project Project { get; set; }
    }
}
