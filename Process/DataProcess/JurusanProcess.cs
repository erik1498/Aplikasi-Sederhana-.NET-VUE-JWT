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
    public class JurusanProcess : JurusanParentProcess
    {
        public JurusanProcess(ApplicationDbContext context) : base(context)
        {
        }

        public async Task SetJurusan(SetJurusan setJurusan)
        {
            var kelas = await _context.Kelass.Where(k => k.KelasID.Equals(setJurusan.KelasID)).FirstOrDefaultAsync();
            if (kelas != null)
            {
                kelas.jurusan = await _context.Jurusans.Where(j => j.JurusanID.Equals(setJurusan.JurusanID)).FirstOrDefaultAsync();
                await _context.SaveChangesAsync();
            }
        }
        public async Task HapusKelasInJurusan(int id)
        {
            var kelas = await _context.Kelass.Include(k => k.jurusan).Where(k => k.KelasID.Equals(id)).FirstOrDefaultAsync();
            kelas.jurusan = null;
            await _context.SaveChangesAsync();
        }
    }
}