using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPVUE.Models
{
    public class WaliKelas
    {
        [Key]
        public int WaliKelasID { get; set; }
        public string NamaWaliKelas { get; set; }
        public User user { get; set; }
    }
}