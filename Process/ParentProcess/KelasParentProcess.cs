using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPVUE.Data;
using ASPVUE.Models;
using ASPVUE.Process.DataProcess;
using Microsoft.EntityFrameworkCore;

namespace ASPVUE.Process.ParentProcess
{
    public class KelasParentProcess
    {
        public ApplicationDbContext _context {get; set;}
        public SiswaProcess _siswaProcess { get; set; }

        public KelasParentProcess(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Kelas>> GetAllKelas()
        {
            return await _context.Kelass.ToListAsync();
        }

        public async Task<Kelas> AddKelas(Kelas kelas)
        {
            Kelas kelasBaru = new Kelas();
            kelasBaru.KelasID = kelas.KelasID;
            kelasBaru.NamaKelas = kelas.NamaKelas;
            await _context.Kelass.AddAsync(kelasBaru);
            await _context.SaveChangesAsync();
            return kelasBaru;
        }

        public async Task<List<Kelas>> Search(Kelas kelas)
        {
            return await _context.Kelass.Where(x => x.NamaKelas.Contains(kelas.NamaKelas)).ToListAsync();
        }

        public async Task<Kelas> Edit(Kelas kelas)
        {
            var exist = await _context.Kelass.FirstOrDefaultAsync(x => x.KelasID.Equals(kelas.KelasID));
            if (exist != null)
            {
                exist.NamaKelas = kelas.NamaKelas;
                await _context.SaveChangesAsync();
            }
            return exist;
        }

        public async Task<Kelas> GetId(int id)
        {
            return await _context.Kelass.FirstOrDefaultAsync(x => x.KelasID.Equals(id));
        }

        public async Task<List<Siswa>> GetListSiswaInKelas(int id)
        {
            return await _context.Siswas.Where(s => s.Kelass.KelasID.Equals(id)).ToListAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var exist = await _context.Kelass.FirstOrDefaultAsync(x => x.KelasID.Equals(id));
            if (exist != null)
            {
                var siswas = await this.GetListSiswaInKelas(exist.KelasID);
                foreach (var item in siswas)
                {
                    item.Kelass = null;
                }
                _context.Kelass.Remove(exist);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}