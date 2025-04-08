
namespace Demo.BusinessLogic.Services
{
    public class DepartmentService(IUnitOfWork unitOfWork) : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        //private readonly IDepartmentRepository _unitOfWork.Departments = repository;

        //GetAll
        public IEnumerable<DepartmentResponse> GetAll()
        {
            var departments = _unitOfWork.Departments.GetAll();

            return departments.Select(department => department.ToResponse());
        }

        //Get

        public DepartmentDetailsResponse? GetById(int id)
        {
            var department = _unitOfWork.Departments.GetById(id);

            return department is null ? null : department.ToDetailsResponse();
        }

        //Add
        public int Add(DepartmentRequest request)
        {
            var department = request.ToEntity();
            _unitOfWork.Departments.Add(department);
            return _unitOfWork.SaveChanges();
        }

        //Update
        public int Update(DepartmentUpdateRequest request)
        {
            var department = request.ToEntity();
            _unitOfWork.Departments.Update(department);
            return _unitOfWork.SaveChanges();
        }

        //Delete

        public bool Delete(int id)
        {
            var department = _unitOfWork.Departments.GetById(id);

            if (department is null) return false;

            _unitOfWork.Departments.Delete(department);
            return _unitOfWork.SaveChanges() > 0 ? true : false;
            
        }

    }
}
