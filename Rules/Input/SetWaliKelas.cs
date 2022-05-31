using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPVUE.Rules.Input
{
    public class SetWaliKelas
    {
        public int WaliKelasID { get; set; }
        public string NamaWaliKelas { get; set; }
        public int KelasID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}