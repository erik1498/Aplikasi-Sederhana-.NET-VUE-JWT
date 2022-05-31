using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPVUE.Models
{
    public class Siswa
    {
        [Key]
        public int SiswaID { get; set; }
        
        [Required]
        public string NamaSiswa { get; set; }
        public Kelas Kelass { get; set; }
    }
}