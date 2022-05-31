using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPVUE.Models
{
    public class Kelas
    {
        [Key]
        public int KelasID { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string NamaKelas { get; set; }
        public Jurusan jurusan { get; set; }
        public WaliKelas waliKelas { get; set; }
    }
}