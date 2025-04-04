using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Demo.BusinessLogic.Services
{
    public class EmployeeService(IEmployeeRepository repository,IMapper mapper) : IEmployeeService
    {
        private readonly IEmployeeRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        //GetAll
        public IEnumerable<EmployeeResponse> GetAll()
        {
            //var Employees = _repository.GetAll();

            //return _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(Employees);


            //var employees = _repository.GetAllQueryable().Select(e => new EmployeeResponse
            //{
            //    Id = e.Id,
            //    Age = e.Age,
            //    Email = e.Email,
            //    EmployeeType = e.EmployeeType.ToString(),
            //    Gender = e.Gender.ToString(),
            //    IsActive = e.IsActive,
            //    Name    = e.Name,
            //    Salary = e.Salary,
            //});

            var employees = _repository.GetAll(e=>new EmployeeResponse
            {
                Id = e.Id,
                Age = e.Age,
                Email = e.Email,
                EmployeeType = e.EmployeeType.ToString(),
                Gender = e.Gender.ToString(),
                IsActive = e.IsActive,
                Name = e.Name,
                Salary = e.Salary,
            });
            return employees;

        }

        //Get

        public EmployeeDetailsResponse? GetById(int id)
        {
            var Employee = _repository.GetById(id);

            return Employee is null ? null : _mapper.Map<Employee, EmployeeDetailsResponse>(Employee);
        }

        //Add
        public int Add(EmployeeRequest request)
        {
            var Employee = _mapper.Map<EmployeeRequest, Employee>(request);
            return _repository.Add(Employee);
        }

        //Update
        public int Update(EmployeeUpdateRequest request)
        {
            var Employee = _mapper.Map<EmployeeUpdateRequest, Employee>(request);
            return _repository.Update(Employee);
        }

        //Delete

        public bool Delete(int id)
        {
            var Employee = _repository.GetById(id);

            if (Employee is null) return false;

            Employee.IsDeleted=true;
            return _repository.Update(Employee) > 0 ? true : false;
        }
    }
}
