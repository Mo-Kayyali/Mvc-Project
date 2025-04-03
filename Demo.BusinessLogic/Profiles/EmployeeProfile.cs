using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BusinessLogic.Profiles
{
    internal class EmployeeProfile : Profile
    {

        public EmployeeProfile()
        {
            CreateMap<Employee,EmployeeDetailsResponse>();
            CreateMap<Employee, EmployeeResponse>();

            CreateMap<EmployeeRequest, Employee>();
            CreateMap<EmployeeUpdateRequest, Employee>();

        }

    }
}
