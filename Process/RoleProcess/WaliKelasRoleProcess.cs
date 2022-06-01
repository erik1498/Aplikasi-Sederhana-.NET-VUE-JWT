using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPVUE.Data;
using ASPVUE.Models;

namespace ASPVUE.Process.RoleProcess
{
    public class WaliKelasRoleProcess : ParentProcess
    {
        public WaliKelasRoleProcess(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<List<Siswa>> GetAllSiswa(int idWaliKelas)
        {
            var waliKelas = await _waliKelasProcess.GetIdByUserID(idWaliKelas);
            var kelas = await _kelasProcess.GetIdByWaliKelasID(waliKelas.WaliKelasID);
            var siswa = await _kelasProcess.GetListSiswaInKelas(kelas.KelasID);
            return siswa;
        }
    }
}