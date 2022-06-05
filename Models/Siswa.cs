using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ASPVUE.Models
{
    public class Siswa
    {
        [Key]
        public int SiswaID { get; set; }
        
        [Required]
        public string NamaSiswa { get; set; }

        public string GambarSiswa { get; set; }
        
        public Kelas Kelass { get; set; }
    }
}