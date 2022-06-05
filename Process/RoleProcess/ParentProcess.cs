using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPVUE.Data;
using ASPVUE.Models;
using ASPVUE.Process.DataProcess;
using ASPVUE.Rules.Output;

namespace ASPVUE.Process.RoleProcess
{
    public class ParentProcess
    {
        protected SiswaProcess _siswaProcess { get; set; }
        protected KelasProcess _kelasProcess { get; set; }
        protected JurusanProcess _jurusanProcess { get; set; }
        protected WaliKelasProcess _waliKelasProcess {get; set;}
        protected UserProcess _userProcess { get; set; }

        public ParentProcess(ApplicationDbContext context)
        {
            _siswaProcess = new SiswaProcess(context);
            _jurusanProcess = new JurusanProcess(context);
            _kelasProcess = new KelasProcess(context);
            _waliKelasProcess = new WaliKelasProcess(context);
            _userProcess = new UserProcess(context);
        }

        // Siswa
        public async Task<List<Siswa>> GetAllSiswa()
        {
            return await _siswaProcess.GetAllSiswa();
        }
        public async Task<List<Siswa>> SearchSiswa(Siswa siswa)
        {
            return await _siswaProcess.Search(siswa);
        }
        public async Task<Siswa> GetIdSiswa(int id)
        {
            return await _siswaProcess.GetId(id);
        }
        public async Task<List<Siswa>> GetSiswaTanpaKelas()
        {
            return await _siswaProcess.GetSiswaTanpaKelas();
        }

        // WaliKelas
        public async Task<List<WaliKelasDTO>> GetAllWaliKelas()
        {
            var walikelas = await _waliKelasProcess.GetAllWaliKelas();
            List<WaliKelasDTO> daftarWaliKelas = new List<WaliKelasDTO>();
            foreach (var item in walikelas)
            {
                var kelas = await _kelasProcess.GetIdByWaliKelasID(item.WaliKelasID);
                daftarWaliKelas.Add(new WaliKelasDTO {
                    NamaWaliKelas = item.NamaWaliKelas,
                    WaliKelasID = item.WaliKelasID,
                    KelasID = kelas.KelasID,
                    NamaKelas = kelas.NamaKelas
                });
            }
            return daftarWaliKelas;
        }
        public async Task<List<Kelas>> GetKelasTanpaWaliKelas()
        {
            return await _waliKelasProcess.GetKelasTanpaWaliKelas();
        }
        public async Task<WaliKelasDTO> GetIdWaliKelas(int id)
        {
            var waliKelasId = await _waliKelasProcess.GetId(id);
            var kelasId = await _kelasProcess.GetIdByWaliKelasID(waliKelasId.WaliKelasID);
            WaliKelasDTO waliKelasEditDTO = new WaliKelasDTO {
                NamaWaliKelas = waliKelasId.NamaWaliKelas,
                WaliKelasID = waliKelasId.WaliKelasID,
                KelasID = kelasId.KelasID,
                NamaKelas = kelasId.NamaKelas
            };
            return waliKelasEditDTO;
        }

        // Kelas
        public async Task<List<Kelas>> GetAllKelas(){
            return await _kelasProcess.GetAllKelas();
        }
        public async Task<List<Siswa>> GetListSiswaInKelas(int id)
        {
            return await _kelasProcess.GetListSiswaInKelas(id);
        }
        public async Task<List<Kelas>> SearchKelas(Kelas kelas)
        {
            return await _kelasProcess.Search(kelas);
        }
        public async Task<Kelas> GetIdKelas(int id)
        {
            return await _kelasProcess.GetId(id);
        }

        // Jurusan
        public async Task<List<Jurusan>> GetAllJurusan()
        {
            return await _jurusanProcess.GetAllJurusan();
        }
        public async Task<Jurusan> GetIdJurusan(int id)
        {
            return await _jurusanProcess.GetId(id);
        }
        public async Task<List<Kelas>> GetListKelas(int id)
        {
            return await _jurusanProcess.GetListKelasInJurusan(id);
        }
        public async Task<List<Kelas>> GetKelasTanpaJurusan()
        {
            return await _jurusanProcess.GetKelasTanpaJurusan();
        }
        
    }
}