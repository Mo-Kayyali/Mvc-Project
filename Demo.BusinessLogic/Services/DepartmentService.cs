global using Demo.BusinessLogic.DataTransferObjects;
global using Demo.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BusinessLogic.Services
{
    public class DepartmentService(IDepartmentRepository repository) : IDepartmentService
    {
        private readonly IDepartmentRepository _repository = repository;

        //GetAll
        public IEnumerable<DepartmentResponse> GetAll()
        {
            var departments = _repository.GetAll();

            return departments.Select(department => department.ToResponse());
        }

        //Get

        public DepartmentDetailsResponse? GetById(int id)
        {
            var department = _repository.GetById(id);

            return department is null ? null : department.ToDetailsResponse();
        }

        //Add
        public int Add(DepartmentRequest request)
        {
            var department = request.ToEntity();
            return _repository.Add(department);
        }

        //Update
        public int Update(DepartmentUpdateRequest request)
        {
            var department = request.ToEntity();
            return _repository.Update(department);
        }

        //Delete

        public bool Delete(int id)
        {
            var department = _repository.GetById(id);

            if (department is null) return false;

            return _repository.Delete(department) > 0 ? true : false;
        }

    }
}
