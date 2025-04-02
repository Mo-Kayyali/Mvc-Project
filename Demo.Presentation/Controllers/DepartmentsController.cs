using Azure.Core;
using Demo.BusinessLogic.DataTransferObjects;
using Demo.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Presentation.Controllers
{
    public class DepartmentsController(IDepartmentService departmentService,IWebHostEnvironment webHostEnvironment
        , ILogger<DepartmentsController> logger) : Controller
    {
        private readonly IDepartmentService _departmentService = departmentService;
        private readonly IWebHostEnvironment _env = webHostEnvironment;
        private readonly ILogger<DepartmentsController> _logger = logger;

        [HttpGet]
        public IActionResult Index()
        {
            var departments = _departmentService.GetAll();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(DepartmentRequest request)
        {
            if(!ModelState.IsValid) return View(request);

            try
            {
                var result = _departmentService.Add(request);
                if (result > 0) return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty,"Can't Create Department Now");
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
            return View(department.ToRequest());
        }

        [HttpPost]
        public IActionResult Edit([FromRoute]int id, DepartmentUpdateRequest request)
        {
            if (id != request.Id) return BadRequest();
            try
            {
                var result = _departmentService.Update(request);
                if (result > 0) return RedirectToAction(nameof(Index));

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
