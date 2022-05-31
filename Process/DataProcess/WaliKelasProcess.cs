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
    public class WaliKelasProcess : WaliKelasParentProcess
    {
        public WaliKelasProcess(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<WaliKelas> Create(SetWaliKelas setWaliKelas, User user)
        {
            WaliKelas waliKelasBaru = new WaliKelas();
            waliKelasBaru.NamaWaliKelas = setWaliKelas.NamaWaliKelas;
            waliKelasBaru.WaliKelasID = setWaliKelas.WaliKelasID;
            waliKelasBaru.user = user;
            WaliKelas waliKelasDiBuat = await this.Create(waliKelasBaru);
            var kelas = await _context.Kelass.Include(k => k.waliKelas).Where(k =>k.KelasID.Equals(setWaliKelas.KelasID)).FirstOrDefaultAsync();
            kelas.waliKelas = waliKelasDiBuat;
            await _context.SaveChangesAsync();
            return waliKelasDiBuat;
        }

        public async Task<WaliKelas> Edit(SetWaliKelas setWaliKelas)
        {
            WaliKelas waliKelasEdit = new WaliKelas{
                NamaWaliKelas = setWaliKelas.NamaWaliKelas,
                WaliKelasID = setWaliKelas.WaliKelasID
            };
            waliKelasEdit = await this.Edit(waliKelasEdit);
            var kelasWaliKelasLama = await _context.Kelass.Include(k => k.waliKelas).Where(k =>k.waliKelas.Equals(waliKelasEdit)).FirstOrDefaultAsync();
            if (!setWaliKelas.KelasID.Equals(kelasWaliKelasLama.KelasID))
            {
                kelasWaliKelasLama.waliKelas = null;
                var kelasTerseleksi = await _context.Kelass.Include(k => k.waliKelas).Where(k =>k.KelasID.Equals(setWaliKelas.KelasID)).FirstOrDefaultAsync();
                kelasTerseleksi.waliKelas = waliKelasEdit;
                await _context.SaveChangesAsync();
            }
            return waliKelasEdit;
        }
    }
}