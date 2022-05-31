using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPVUE.Constants;
using ASPVUE.Data;
using ASPVUE.Models;

namespace ASPVUE.Process.RoleProcess
{
    public class WaliKelasRoleProcess : ParentProcess
    {
        public WaliKelasRoleProcess(ApplicationDbContext context) : base(context)
        {
        }
        public async override Task<List<Siswa>> GetAllSiswa()
        {
            var waliKelas = await _waliKelasProcess.GetIdByUserID(UserAuth.UserID);
            var kelas = await _kelasProcess.GetIdByWaliKelasID(waliKelas.WaliKelasID);
            var siswa = await _kelasProcess.GetListSiswaInKelas(kelas.KelasID);
            return siswa;
        }
    }
}