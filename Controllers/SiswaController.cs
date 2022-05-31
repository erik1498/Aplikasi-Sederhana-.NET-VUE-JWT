using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ASPVUE.Models;
using ASPVUE.Data;
using ASPVUE.Rules.Input;
using ASPVUE.Constants;
using Microsoft.AspNetCore.Authorization;
using ASPVUE.Process.RoleProcess;
using System.Security.Claims;

namespace ASPVUE.Controllers
{
    public class SiswaController : Controller
    {
        private readonly ILogger<SiswaController> _logger;

        public AdminRoleProcess _adminProcess { get; set; }
        public WaliKelasRoleProcess _waliKelasProcess { get; set; }
        
        public SiswaController(ILogger<SiswaController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _adminProcess = new AdminRoleProcess(context);
            _waliKelasProcess = new WaliKelasRoleProcess(context);
        }

        private void AuthorizeRequest(){
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                UserAuth.UserID = int.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.PrimaryGroupSid).Value);
                UserAuth.Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier).Value.ToString();
                UserAuth.Role = int.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role).Value);
            }
        }

        public IActionResult Index()
        {
            if (TokenConst.TokenValue == string.Empty)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["TokenValue"] = TokenConst.TokenValue;
            ViewData["Role"] = UserAuth.Role;
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDaftarSiswa(){
            this.AuthorizeRequest();
            if (UserAuth.Role == 1)
            {
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(await _adminProcess.GetAllSiswa()));   
            }
            else if(UserAuth.Role == 2){
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(await _waliKelasProcess.GetAllSiswa()));   
            }
            return BadRequest("Akun Anda Tidak Diizinkan.");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Siswa siswa){
            this.AuthorizeRequest();
            if (UserAuth.Role == 1)
            {
                if (ModelState.IsValid){
                    return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(await _adminProcess.AddSiswa(siswa)));
                }
                return BadRequest(Newtonsoft.Json.JsonConvert.SerializeObject(ModelState));   
            }
            return BadRequest("Akun Anda Tidak Diizinkan.");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Search(Siswa siswa){
            this.AuthorizeRequest();
            if (UserAuth.Role == 1 || UserAuth.Role == 2)
            {
                if (siswa.NamaSiswa != string.Empty)
                {
                    var exist = await _adminProcess.SearchSiswa(siswa);
                    if (exist != null)
                    {
                        return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(exist));
                    }
                    else{
                        return NotFound();
                    }
                }else{
                    return BadRequest();
                }
            }
            return BadRequest("Akun Anda Tidak Diizinkan");
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Siswa siswa){
            if (ModelState.IsValid)
            {
                var exist = await _adminProcess.EditSiswa(siswa);
                if (exist != null)
                {
                    return Ok(exist);
                }
            }
            return BadRequest(Newtonsoft.Json.JsonConvert.SerializeObject(ModelState));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSiswa(int id){
            this.AuthorizeRequest();
            if (UserAuth.Role == 1)
            {
                var exist = await _adminProcess.GetIdSiswa(id);
                if (exist != null)
                {
                    return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(exist));
                }else{
                    return NotFound();
                }
            }
            return BadRequest("Akun Anda Tidak Diizinkan");
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int id){
            this.AuthorizeRequest();
            if (UserAuth.Role == 1)
            {
                var exist = await _adminProcess.DeleteSiswa(id);
                if (exist)
                {
                    return NoContent();
                }else{
                    return NotFound();
                }
            }
            return BadRequest("Akun Anda Tidak Diizinkan");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSiswaTanpaKelas(){
            this.AuthorizeRequest();
            if (UserAuth.Role == 1)
            {
                var siswa = await _adminProcess.GetSiswaTanpaKelas();
                if (siswa != null)
                {
                    return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(siswa));
                }else{
                    return NotFound();
                }
            }
            return BadRequest("Akun Anda Tidak Diizinkan");
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> SetKelas(SetKelas setKelas){
            this.AuthorizeRequest();
            if (UserAuth.Role == 1)
            {
                await _adminProcess.SetKelasSiswa(setKelas);
                return NoContent();
            }
            return BadRequest("Akun Anda Tidak Diizinkan");
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string[] strings)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
