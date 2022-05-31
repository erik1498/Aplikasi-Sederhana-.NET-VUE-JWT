using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPVUE.Models;

namespace ASPVUE.Rules.Output
{
    public class WaliKelasDTO
    {
        public int WaliKelasID { get; set; }
        public string NamaWaliKelas { get; set; }
        public int KelasID { get; set; }
        public string NamaKelas { get; set; }
    }
}