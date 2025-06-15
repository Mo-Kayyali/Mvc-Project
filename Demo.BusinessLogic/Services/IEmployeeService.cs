using Demo.BusinessLogic.DataTransferObjects.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BusinessLogic.Services
{
    public interface IEmployeeService
    {
        int Add(EmployeeRequest request);
        bool Delete(int id);
        IEnumerable<EmployeeResponse> GetAll(string? SearchValue);
        EmployeeDetailsResponse? GetById(int id);
        int Update(EmployeeUpdateRequest request);
    }
}
