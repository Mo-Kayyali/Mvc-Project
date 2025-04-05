using Demo.BusinessLogic.Services;
using Demo.DataAccess.Models.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demo.Presentation.Controllers
{
    public class EmployeesController(IEmployeeService EmployeeService, IWebHostEnvironment webHostEnvironment
        , ILogger<EmployeesController> logger) : Controller
    {
        private readonly IEmployeeService _EmployeeService = EmployeeService;
        private readonly IWebHostEnvironment _env = webHostEnvironment;
        private readonly ILogger<EmployeesController> _logger = logger;

        [HttpGet]
        public IActionResult Index(string? SearchValue)
        {

            var Employees = _EmployeeService.GetAll(SearchValue);
            return View(Employees);
        }

        [HttpGet]
        public IActionResult Create([FromServices] IDepartmentService departmentService)
        {
            var departments = departmentService.GetAll();

            var items = new SelectList(departments,nameof(DepartmentResponse.Id),nameof(DepartmentResponse.Name));
            ViewBag.Departments=items;
            return View();
        } 

        [HttpPost]
        public IActionResult Create(EmployeeRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            try
            {
                var result = _EmployeeService.Add(request);
                if (result > 0) return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, "Can't Create Employee Now");
                return View(request);
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                {
                    ModelState.AddModelError(string.Empty, ex.Message);

                }
                _logger.LogError(ex.Message);

            }
            return View(request);

        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var Employee = _EmployeeService.GetById(id.Value);
            if (Employee is null) return NotFound();
            return View(Employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id, [FromServices] IDepartmentService departmentService)
        {
            if (!id.HasValue) return BadRequest();

            var Employee = _EmployeeService.GetById(id.Value);
            if (Employee is null) return NotFound();

            var employeeRequest = new EmployeeUpdateRequest
            {
                Address = Employee.Address,
                Age = Employee.Age,
                Email = Employee.Email,
                EmployeeType = Enum.Parse<EmployeeType>(Employee.EmployeeType),
                Gender = Enum.Parse<Gender>(Employee.Gender),
                HiringDate = Employee.HiringDate,
                Id = Employee.Id,
                IsActive = Employee.IsActive,
                Name = Employee.Name,
                PhoneNumber = Employee.PhoneNumber,
                Salary = Employee.Salary,
            };
            var departments = departmentService.GetAll();

            var items = new SelectList(departments, nameof(DepartmentResponse.Id), nameof(DepartmentResponse.Name));
            ViewBag.Departments = items;
            return View(employeeRequest);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, EmployeeUpdateRequest request)
        {

            if (id != request.Id) return BadRequest();
            try
            {
                var result = _EmployeeService.Update(request);
                if (result > 0) return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, "Can't Update Employee Now");
                return View(request);
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                {
                    ModelState.AddModelError(string.Empty, ex.Message);

                }
                _logger.LogError(ex.Message);

            }
            return View(request);
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var Employee = _EmployeeService.GetById(id.Value);
            if (Employee is null) return NotFound();
            return View(Employee);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(int? id)
        {
            if (!id.HasValue) return BadRequest();
            try
            {
                var result = _EmployeeService.Delete(id.Value);
                if (result) return RedirectToAction(nameof(Index));

                //ModelState.AddModelError(string.Empty, "Can't Delete Employee Now");
                //return View(request);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsProduction())
                {
                    _logger.LogError(ex.Message);

                    ModelState.AddModelError(string.Empty, "Can't Delete Employee Now");

                }

                //ModelState.AddModelError(string.Empty, ex.Message);
                //return View(request);
                return RedirectToAction(nameof(Index));
            }

        }

    }
}
