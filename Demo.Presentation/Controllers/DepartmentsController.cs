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
    }
}
