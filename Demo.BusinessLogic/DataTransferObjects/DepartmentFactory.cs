using Demo.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BusinessLogic.DataTransferObjects
{
    public static class DepartmentFactory
    {
        public static DepartmentResponse ToResponse(this Department department) => new()
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description,
            CreatedOn = DateOnly.FromDateTime(department.CreatedOn),
            Code = department.Code
        };

        public static DepartmentDetailsResponse ToDetailsResponse(this Department department) => new()
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description,
            CreatedBy = department.CreatedBy,
            CreatedOn = department.CreatedOn,
            Code = department.Code,
            IsDeleted = department.IsDeleted,
            LastModifiedBy = department.LastModifiedBy,
            LastModifiedOn = department.LastModifiedOn
        };

        public static Department ToEntity(this DepartmentRequest departmentRequest) => new()
        {
            Name = departmentRequest.Name,
            Description = departmentRequest.Description,
            Code = departmentRequest.Code,
            CreatedOn = departmentRequest.CreatedOn
        };

        public static Department ToEntity(this DepartmentUpdateRequest departmentRequest) => new()
        {
            Id= departmentRequest.Id,
            Name = departmentRequest.Name,
            Description = departmentRequest.Description,
            Code = departmentRequest.Code,
        };

        public static DepartmentUpdateRequest ToRequest(this DepartmentDetailsResponse department) => new()
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description,
            CreatedOn = DateOnly.FromDateTime(department.CreatedOn),
            Code = department.Code,
        };
    }
}
