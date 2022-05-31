using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPVUE.Data;
using ASPVUE.Models;
using ASPVUE.Process.DataProcess;
using ASPVUE.Rules.Input;

namespace ASPVUE.Process.RoleProcess
{
    public class AdminRoleProcess : ParentProcess
    {
        public AdminRoleProcess(ApplicationDbContext context) : base(context)
        {
        }

        // Siswa
        public async Task<Siswa> AddSiswa(Siswa siswa)
        {
            return await _siswaProcess.Create(siswa);
        }
        public async Task<Siswa> EditSiswa(Siswa siswa)
        {
            return await _siswaProcess.Edit(siswa);
        }
        public async Task<bool> DeleteSiswa(int id)
        {
            return await _siswaProcess.Delete(id);
        }
        public async Task SetKelasSiswa(SetKelas setKelas)
        {
            await _siswaProcess.SetKelas(setKelas);
        }

        // WaliKelas
        public async Task<WaliKelas> CreateWaliKelas(SetWaliKelas setWaliKelas)
        {
            var user = await _userProcess.CreatewaliKelas(setWaliKelas);
            return await _waliKelasProcess.Create(setWaliKelas, user);
        }
        public async Task<bool> DeleteWaliKelas(int id)
        {
            return await _waliKelasProcess.Delete(id);
        }
        public async Task<WaliKelas> EditWaliKelas(SetWaliKelas setWaliKelas)
        {
            return await _waliKelasProcess.Edit(setWaliKelas);
        }

        // Kelas
        public async Task<Kelas> AddKelas(Kelas kelas)
        {
            return await _kelasProcess.AddKelas(kelas);
        }
        public async Task<Kelas> EditKelas(Kelas kelas)
        {
            return await _kelasProcess.Edit(kelas);
        }
        public async Task<bool> DeleteKelas(int id)
        {
            return await _kelasProcess.Delete(id);
        }
        public async Task<Siswa> HapusSiswaInKelas(int id)
        {
            return await _kelasProcess.HapusSiswaInKelas(id);
        }

        // Jurusan
        public async Task<Jurusan> AddJurusan(Jurusan jurusan)
        {
            return await _jurusanProcess.Create(jurusan);
        }
        public async Task<Jurusan> EditJurusan(Jurusan jurusan)
        {
            return await _jurusanProcess.Edit(jurusan);
        }
        public async Task<bool> DeleteJurusan(int id)
        {
            return await _jurusanProcess.Delete(id);
        }
        public async Task HapusKelasInJurusan(int id)
        {
            await _jurusanProcess.HapusKelasInJurusan(id);
        }
        public async Task SetJurusan(SetJurusan setJurusan)
        {
            await _jurusanProcess.SetJurusan(setJurusan);
        }
    }
}