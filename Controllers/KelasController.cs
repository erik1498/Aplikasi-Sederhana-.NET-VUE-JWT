using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ASPVUE.Constants;
using ASPVUE.Data;
using ASPVUE.Models;
using ASPVUE.Process.RoleProcess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ASPVUE.Controllers
{
    public class KelasController : Controller
    {
        private readonly ILogger<KelasController> _logger;
        private AdminRoleProcess _adminProcess { get; set; }
        public KelasController(ILogger<KelasController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _adminProcess = new AdminRoleProcess(context);
        }

        private User AuthorizeRequest(){
            User user = new User();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                user.UserID = int.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.PrimaryGroupSid).Value);
                user.Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier).Value.ToString();
                user.Role = int.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role).Value);
                return user;
            }
            return user;
        }

        public IActionResult Index()
        {
            if (TokenConst.TokenValue == string.Empty || UserAuth.Role != 1)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["TokenValue"] = TokenConst.TokenValue;
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDaftarKelas(){
            if (this.AuthorizeRequest().Role == 1)
            {
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(await _adminProcess.GetAllKelas()));
            }
            return BadRequest("Akun Anda Tidak Diizinkan");
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetListSiswa(int id){
            if (this.AuthorizeRequest().Role == 1)
            {
                var siswas = await _adminProcess.GetListSiswaInKelas(id);
                if (siswas != null)
                {
                    return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(siswas));
                }else{
                    return NotFound();
                }
            }
            return BadRequest("Akun Anda Tidak Diizinkan");
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> HapusSiswa(int id){
            if (this.AuthorizeRequest().Role == 1)
            {
                var siswa = await _adminProcess.HapusSiswaInKelas(id);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(siswa));
            }
            return BadRequest("Akun Anda Tidak Diizinkan");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Kelas kelas){
            if (this.AuthorizeRequest().Role == 1)
            {
                if (ModelState.IsValid){
                    return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(await _adminProcess.AddKelas(kelas)));
                }
                return BadRequest(Newtonsoft.Json.JsonConvert.SerializeObject(ModelState));
            }
            return BadRequest("Akun Anda Tidak Diizinkan");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Search(Kelas kelas){
            if (this.AuthorizeRequest().Role == 1)
            {
                if (kelas.NamaKelas != string.Empty)
                {
                    var exist = await _adminProcess.SearchKelas(kelas);
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
        [Authorize]
        public async Task<IActionResult> Edit(Kelas kelas){
            if (this.AuthorizeRequest().Role == 1)
            {
                if (ModelState.IsValid)
                {
                    var exist = await _adminProcess.EditKelas(kelas);
                    if (exist != null)
                    {
                        return Ok(exist);
                    }
                }
                return BadRequest(Newtonsoft.Json.JsonConvert.SerializeObject(ModelState));
            }
            return BadRequest("Akun Anda Tidak Diizinkan");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetKelas(int id){
            if (this.AuthorizeRequest().Role == 1)
            {
                var exist = await _adminProcess.GetIdKelas(id);
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
            if (this.AuthorizeRequest().Role == 1)
            {
                var exist = await _adminProcess.DeleteKelas(id);
                if (exist)
                {
                    return NoContent();
                }else{
                    return NotFound();
                }
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