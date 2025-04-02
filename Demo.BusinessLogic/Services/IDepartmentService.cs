using Demo.BusinessLogic.DataTransferObjects;

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