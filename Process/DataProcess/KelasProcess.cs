using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPVUE.Data;
using ASPVUE.Models;
using ASPVUE.Process.ParentProcess;
using Microsoft.EntityFrameworkCore;

namespace ASPVUE.Process.DataProcess
{
    public class KelasProcess : KelasParentProcess
    {
        public KelasProcess(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Siswa> HapusSiswaInKelas(int id)
        {
            var siswa = await _context.Siswas.Include(s => s.Kelass).Where(s => s.SiswaID.Equals(id)).FirstOrDefaultAsync();
            siswa.Kelass = null;
            await _context.SaveChangesAsync();
            return siswa;
        }
        public async Task<Kelas> GetIdByWaliKelasID(int id)
        {
            var kelas = await _context.Kelass.Include(k => k.waliKelas).Where(k => k.waliKelas.WaliKelasID.Equals(id)).FirstOrDefaultAsync();
            return kelas;
        }
    }
}