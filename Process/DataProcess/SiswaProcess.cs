using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPVUE.Data;
using ASPVUE.Models;
using ASPVUE.Process.ParentProcess;
using ASPVUE.Rules.Input;
using Microsoft.EntityFrameworkCore;

namespace ASPVUE.Process.DataProcess
{
    public class SiswaProcess : SiswaParentProcess
    {
        public SiswaProcess(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Siswa>> GetSiswaTanpaKelas()
        {
            return await _context.Siswas.Include(s => s.Kelass).Where(s => s.Kelass.Equals(null)).ToListAsync();
        }

        public async Task SetKelas(SetKelas setKelas)
        {
            var siswa = await _context.Siswas.Include(s => s.Kelass).Where(s => s.SiswaID.Equals(setKelas.SiswaID)).FirstOrDefaultAsync();
            siswa.Kelass = await _context.Kelass.Where(k => k.KelasID.Equals(setKelas.KelasID)).FirstOrDefaultAsync();
            await _context.SaveChangesAsync();
        }
    }
}