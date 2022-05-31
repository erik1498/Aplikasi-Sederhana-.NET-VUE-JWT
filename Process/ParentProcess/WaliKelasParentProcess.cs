using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPVUE.Data;
using ASPVUE.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPVUE.Process.ParentProcess
{
    public class WaliKelasParentProcess
    {
        protected ApplicationDbContext _context;
        public WaliKelasParentProcess(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<WaliKelas>> GetAllWaliKelas()
        {
            return await _context.WaliKelas.ToListAsync();
        }
        public async Task<List<Kelas>> GetKelasTanpaWaliKelas()
        {
            return await _context.Kelass.Include(k => k.waliKelas).Where(k => k.waliKelas.Equals(null)).ToListAsync();
        }

        public async Task<WaliKelas> Create(WaliKelas walikelas)
        {
            await _context.WaliKelas.AddAsync(walikelas);
            await _context.SaveChangesAsync();
            return walikelas;
        }
        public async Task<WaliKelas> Edit(WaliKelas waliKelas)
        {
            var exist = await _context.WaliKelas.Where(w => w.WaliKelasID.Equals(waliKelas.WaliKelasID)).FirstOrDefaultAsync();
            exist.NamaWaliKelas = waliKelas.NamaWaliKelas;
            await _context.SaveChangesAsync();
            return exist;
        }

        public async Task<bool> Delete(int id)
        {
            var kelas = await _context.Kelass
                                .Include(k => k.waliKelas)
                                .Where(k => k.waliKelas.WaliKelasID.Equals(id))
                                .ToListAsync();
            foreach (var item in kelas)
            {
                item.waliKelas = null;
            }
            var walikelas = await _context.WaliKelas.Include(w => w.user).Where(w => w.WaliKelasID.Equals(id)).FirstOrDefaultAsync();
            int idUser = walikelas.user.UserID;
            _context.WaliKelas.Remove(walikelas);
            await _context.SaveChangesAsync();
            var user = await _context.Users.Where(u => u.UserID.Equals(idUser)).FirstOrDefaultAsync();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<WaliKelas> GetId(int id)
        {
            return await _context.WaliKelas.Where(w => w.WaliKelasID.Equals(id)).FirstOrDefaultAsync();
        }
        public async Task<WaliKelas> GetIdByUserID(int userID)
        {
            return await _context.WaliKelas.Where(w => w.user.UserID.Equals(userID)).FirstOrDefaultAsync();
        }
    }
}