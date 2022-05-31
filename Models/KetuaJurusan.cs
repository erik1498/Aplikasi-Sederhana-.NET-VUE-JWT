using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPVUE.Models
{
    public class KetuaJurusan
    {
        [Key]
        public int KetuaJurusanID { get; set; }
        public string NamaKetuaJurusan { get; set; }
        public User User { get; set; }
    }
}