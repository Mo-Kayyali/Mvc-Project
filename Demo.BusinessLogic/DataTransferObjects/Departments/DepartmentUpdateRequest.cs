﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BusinessLogic.DataTransferObjects.Departments
{
    public class DepartmentUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }

    }
}
