using ASPVUE.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPVUE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Siswa> Siswas { get; set; }
        public virtual DbSet<WaliKelas> WaliKelas { get; set; }
        public virtual DbSet<Kelas> Kelass { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Jurusan> Jurusans { get; set; }
        public virtual DbSet<KetuaJurusan> KetuaJurusans { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options){
            
        }

        public ApplicationDbContext()
        {
        }
    }
}