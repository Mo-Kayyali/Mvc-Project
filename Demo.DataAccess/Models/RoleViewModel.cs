using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DataAccess.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<UserRoleViewModel> Users { get; set; } = new List<UserRoleViewModel>();
        public RoleViewModel()
        {
            Id= Guid.NewGuid().ToString();
        }
    }
}
