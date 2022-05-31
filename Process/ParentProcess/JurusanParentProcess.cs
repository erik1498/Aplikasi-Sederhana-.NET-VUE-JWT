using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPVUE.Data;
using ASPVUE.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPVUE.Process.ParentProcess
{
    public class JurusanParentProcess
    {
        public ApplicationDbContext _context { get; set; }

        public JurusanParentProcess(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Jurusan> Create(Jurusan jurusan)
        {
            var exist = await _context.Jurusans.AddAsync(jurusan);
            await _context.SaveChangesAsync();
            return jurusan;
        }
        
        public async Task<List<Jurusan>> GetAllJurusan()
        {
            return await _context.Jurusans.ToListAsync();
        }

        public async Task<Jurusan> GetId(int id)
        {
            return await _context.Jurusans.FirstOrDefaultAsync(x => x.JurusanID.Equals(id));
        }

        public async Task<Jurusan> Edit(Jurusan jurusan)
        {
            var exist = await _context.Jurusans.Where(j => j.JurusanID.Equals(jurusan.JurusanID)).FirstOrDefaultAsync();
            if (exist != null)
            {
                exist.NamaJurusan = jurusan.NamaJurusan;
                await _context.SaveChangesAsync();
                return exist;
            }
            return exist;
        }

        public async Task<bool> Delete(int id)
        {
            var exist = await _context.Jurusans.Where(j => j.JurusanID.Equals(id)).FirstOrDefaultAsync();
            if (exist != null)
            {
                var kelas = await _context.Kelass.Include(k => k.jurusan).Where(k => k.jurusan.Equals(exist)).ToListAsync();
                foreach (var item in kelas)
                {
                    item.jurusan = null;
                }
                await _context.SaveChangesAsync();
                _context.Jurusans.Remove(exist);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<List<Kelas>> GetListKelasInJurusan(int id)
        {
            return await _context.Kelass.Where(k => k.jurusan.JurusanID.Equals(id)).ToListAsync();
        }
        public async Task<List<Kelas>> GetKelasTanpaJurusan()
        {
            return await _context.Kelass.Where(k => k.jurusan.Equals(null)).ToListAsync();
        }
    }
}