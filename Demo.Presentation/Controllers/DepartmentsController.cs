using Azure.Core;
using Microsoft.AspNetCore.Authorization;

namespace Demo.Presentation.Controllers
{
    public class DepartmentsController(IDepartmentService departmentService,IWebHostEnvironment webHostEnvironment
        , ILogger<DepartmentsController> logger) : Controller
    {
        private readonly IDepartmentService _departmentService = departmentService;
        private readonly IWebHostEnvironment _env = webHostEnvironment;
        private readonly ILogger<DepartmentsController> _logger = logger;
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            var departments = _departmentService.GetAll();

            
            //ViewData["message"] = new DepartmentDetailsResponse { Name = "Department02" };
            ViewBag.Message = new DepartmentDetailsResponse { Name = "Department02" };

            return View(departments);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepartmentRequest request)
        {
            if(!ModelState.IsValid) return View(request);
            string message;
            try
            {
                var result = _departmentService.Add(request);
                if (result > 0) message = $"Department {request.Name} Created";
                else
                {
                    message=$"Can't Create Department {request.Name}";
                }
                TempData["Message"] = message;
                    
                return RedirectToAction(nameof(Index));

                
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
        [Authorize]
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if(!id.HasValue) return BadRequest();

            var department = _departmentService.GetById(id.Value);
            if(department is null) return NotFound();
            return View(department);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var department = _departmentService.GetById(id.Value);
            if (department is null) return NotFound();
            return View(department.ToUpdateRequest());
        }
        [Authorize]
        [HttpPost]
        public IActionResult Edit([FromRoute]int id, DepartmentUpdateRequest request)
        {
            if (id != request.Id) return BadRequest();
            try
            {
                var result = _departmentService.Update(request);
                if (result > 0)
                {
                    TempData["Message"] = "Employee updated successfully!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, "Can't Update Department Now");
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


        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    if (!id.HasValue) return BadRequest();

        //    var department = _departmentService.GetById(id.Value);
        //    if (department is null) return NotFound();
        //    return View(department);
        //}
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(int? id)
        {
            if (!id.HasValue) return BadRequest();
            try
            {
                var result = _departmentService.Delete(id.Value);
                if (result) return RedirectToAction(nameof(Index));

                //ModelState.AddModelError(string.Empty, "Can't Delete Department Now");
                //return View(request);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsProduction())
                {
                    _logger.LogError(ex.Message);

                    ModelState.AddModelError(string.Empty, "Can't Delete Department Now");

                }

                //ModelState.AddModelError(string.Empty, ex.Message);
                //return View(request);
                return RedirectToAction(nameof(Index));
            }
            
        }


    }
}
