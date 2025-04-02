using Demo.DataAccess.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DataAccess.Repositories
{
    public class DepartmentRepository(ApplicationDbContext context) 
        : GenericRepository<Department>(context)
        , IDepartmentRepository
    {
        
    }
}
