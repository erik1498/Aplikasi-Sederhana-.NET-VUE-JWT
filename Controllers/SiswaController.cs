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
using Microsoft.AspNetCore.Authorization;
using ASPVUE.Process.RoleProcess;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

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

        private bool AuthorizeRequest()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                string UserID = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.PrimaryGroupSid).Value;
                string Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier).Value;
                int Role = int.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role).Value);
                if (HttpContext.Session.GetString("UserID") != UserID && HttpContext.Session.GetString("Username") != Username && int.Parse(HttpContext.Session.GetString("Role")) != Role)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public IActionResult Index()
        {
            string tokenValue = HttpContext.Session.GetString("TokenValue");
            if (string.IsNullOrWhiteSpace(tokenValue))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["TokenValue"] = tokenValue;
            ViewData["Role"] = HttpContext.Session.GetString("Role");
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDaftarSiswa(){
            var auth = this.AuthorizeRequest();
            if (auth)
            {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(await _adminProcess.GetAllSiswa()));   
                }
                else if(int.Parse(HttpContext.Session.GetString("Role")) == 2){
                    return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(await _waliKelasProcess.GetAllSiswa(int.Parse(HttpContext.Session.GetString("UserID")))));   
                }
                return BadRequest("Akun Anda Tidak Diizinkan.");
            }
            return Unauthorized();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Siswa siswa){
            var auth = this.AuthorizeRequest();
            if (auth)
            {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    if (ModelState.IsValid){
                        return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(await _adminProcess.AddSiswa(siswa)));
                    }
                    return BadRequest(Newtonsoft.Json.JsonConvert.SerializeObject(ModelState));   
                }
                return BadRequest("Akun Anda Tidak Diizinkan.");
            }
            return Unauthorized();

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Search(Siswa siswa){
            var auth = this.AuthorizeRequest();
            if (auth)
            {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1 || int.Parse(HttpContext.Session.GetString("Role")) == 2)
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
            return Unauthorized();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Edit(Siswa siswa){
            var auth = this.AuthorizeRequest();
            if (auth)
            {
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
            return Unauthorized();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSiswa(int id){
            var auth = this.AuthorizeRequest();
            if (auth)
            {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
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
            return Unauthorized();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int id){
            var auth = this.AuthorizeRequest();
            if (auth)
            {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
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
            return Unauthorized();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSiswaTanpaKelas(){
            var auth = this.AuthorizeRequest();
            if (auth)
            {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
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
            return Unauthorized();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> SetKelas(SetKelas setKelas){
            var auth = this.AuthorizeRequest();
            if (auth)
            {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    await _adminProcess.SetKelasSiswa(setKelas);
                    return NoContent();
                }
                return BadRequest("Akun Anda Tidak Diizinkan");
            }
            return Unauthorized();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string[] strings)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
