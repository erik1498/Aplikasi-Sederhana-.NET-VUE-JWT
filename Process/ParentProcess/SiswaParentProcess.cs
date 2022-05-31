using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPVUE.Data;
using ASPVUE.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPVUE.Process.ParentProcess
{
    public class SiswaParentProcess
    {
        public ApplicationDbContext _context { get; set; }

        public SiswaParentProcess(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Siswa>> GetAllSiswa()
        {
            return await _context.Siswas.ToListAsync();
        }

        public async Task<Siswa> Create(Siswa siswa)
        {
            Siswa siswaBaru = new Siswa();
            siswaBaru.SiswaID = siswa.SiswaID;
            siswaBaru.NamaSiswa = siswa.NamaSiswa;
            _context.Siswas.Add(siswaBaru);
            await _context.SaveChangesAsync();
            return siswaBaru;
        }

        public async Task<Siswa> GetId(int id)
        {
            var exist = await _context.Siswas.FirstOrDefaultAsync(x => x.SiswaID.Equals(id));
            if (exist != null)
            {
                return exist;
            }else{
                return null;
            }
        }

        public async Task<Siswa> Edit(Siswa siswa)
        {
            var exist = await _context.Siswas.FirstOrDefaultAsync(x => x.SiswaID.Equals(siswa.SiswaID));
            if (exist != null)
            {
                exist.NamaSiswa = siswa.NamaSiswa;
                await _context.SaveChangesAsync();
                return exist;
            }
            return null;
        }

        public async Task<bool> Delete(int id){
            var exist = await _context.Siswas.FirstOrDefaultAsync(x => x.SiswaID.Equals(id));
            if (exist != null)
            {
                _context.Siswas.Remove(exist);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<List<Siswa>> Search(Siswa siswa)
        {
            return await _context.Siswas.Where(x => x.NamaSiswa.Contains(siswa.NamaSiswa)).ToListAsync();
        }
    }
}