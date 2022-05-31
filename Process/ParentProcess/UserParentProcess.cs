using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPVUE.Data;
using ASPVUE.Models;
using ASPVUE.Rules.Input;

namespace ASPVUE.Process.ParentProcess
{
    public class UserParentProcess
    {
        private ApplicationDbContext _context;

        public UserParentProcess(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<User> CreatewaliKelas(SetWaliKelas setWaliKelas)
        {
            User user = new User{
                Username = setWaliKelas.Username,
                Password = setWaliKelas.Password,
                Role = 2
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}