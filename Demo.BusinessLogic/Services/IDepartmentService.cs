using Demo.BusinessLogic.DataTransferObjects.Departments;

namespace Demo.BusinessLogic.Services
{
    public interface IDepartmentService
    {
        int Add(DepartmentRequest request);
        bool Delete(int id);
        IEnumerable<DepartmentResponse> GetAll();
        DepartmentDetailsResponse? GetById(int id);
        int Update(DepartmentUpdateRequest request);
    }
}