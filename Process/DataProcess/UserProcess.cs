using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPVUE.Data;
using ASPVUE.Process.ParentProcess;

namespace ASPVUE.Process.DataProcess
{
    public class UserProcess : UserParentProcess
    {
        public UserProcess(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}