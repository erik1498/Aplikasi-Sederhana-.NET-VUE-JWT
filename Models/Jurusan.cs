using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPVUE.Models
{
    public class Jurusan
    {
        [Key]
        public int JurusanID { get; set; }
        [MaxLength(20)]
        public string NamaJurusan { get; set; }
    }
}