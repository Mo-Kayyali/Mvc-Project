using Demo.BusinessLogic.Services.AttachmentService;


namespace Demo.BusinessLogic.Services
{
    public class EmployeeService(IUnitOfWork unitOfWork,IMapper mapper, IAttachmentService attachmentService) : IEmployeeService
    {
        //private readonly IEmployeeRepository _unitOfWork.Employees = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IAttachmentService _attachmentService = attachmentService;

        //GetAll
        public IEnumerable<EmployeeResponse> GetAll(string? SeachValue)
        {
            #region Old
            //var Employees = _unitOfWork.Employees.GetAll();

            //return _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(Employees);


            //var employees = _unitOfWork.Employees.GetAllQueryable().Select(e => new EmployeeResponse
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
            #endregion

            if (string.IsNullOrWhiteSpace(SeachValue))
            {

                return _unitOfWork.Employees.GetAll(e => new EmployeeResponse
                {
                    Id = e.Id,
                    Age = e.Age,
                    Email = e.Email,
                    EmployeeType = e.EmployeeType.ToString(),
                    Gender = e.Gender.ToString(),
                    IsActive = e.IsActive,
                    Name = e.Name,
                    Salary = e.Salary,
                    Department = e.Department.Name,
                    Image = e.ImageName,
                }, e => !e.IsDeleted,
                e => e.Department);
            }

            return _unitOfWork.Employees.GetAll(e => new EmployeeResponse
            {
                Id = e.Id,
                Age = e.Age,
                Email = e.Email,
                EmployeeType = e.EmployeeType.ToString(),
                Gender = e.Gender.ToString(),
                IsActive = e.IsActive,
                Name = e.Name,
                Salary = e.Salary,
                Department = e.Department.Name
            }, e => !e.IsDeleted&&e.Name.ToLower().Contains(SeachValue.ToLower()),
            e => e.Department);


            //return employees;

        }

        //Get

        public EmployeeDetailsResponse? GetById(int id)
        {
            var Employee = _unitOfWork.Employees.GetById(id);

            return Employee is null ? null : _mapper.Map<Employee, EmployeeDetailsResponse>(Employee);
        }

        //Add
        public int Add(EmployeeRequest request)
        {
            var Employee = _mapper.Map<EmployeeRequest, Employee>(request);
            if(request.Image is not null)
            {
               Employee.ImageName = _attachmentService.Upload(request.Image,"Imgs");
            }
            _unitOfWork.Employees.Add(Employee);
            return _unitOfWork.SaveChanges();
        }

        //Update
        public int Update(EmployeeUpdateRequest request)
        {
            var Employee = _mapper.Map<EmployeeUpdateRequest, Employee>(request);
            _unitOfWork.Employees.Update(Employee);
            return _unitOfWork.SaveChanges();
        }

        //Delete

        public bool Delete(int id)
        {
            var Employee = _unitOfWork.Employees.GetById(id);

            if (Employee is null) return false;

            Employee.IsDeleted=true;
            _unitOfWork.Employees.Update(Employee);
            return _unitOfWork.SaveChanges() > 0 ? true : false;
        }
    }
}
