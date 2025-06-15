using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Presentation.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController(RoleManager<IdentityRole> roleManager, IWebHostEnvironment env,UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IWebHostEnvironment _env = env;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        [HttpGet]
        public async Task<IActionResult> Index(string SearchValue)
        {
            var rolesQuery = _roleManager.Roles.AsQueryable();

            if (!string.IsNullOrEmpty(SearchValue))
            {
                rolesQuery = rolesQuery.Where(R => R.Name.ToLower().Contains(SearchValue.ToLower()));
            }
            var rolesList = await rolesQuery.Select(R => new RoleViewModel
            {
                Id = R.Id,
                Name = R.Name,
            }).ToListAsync();

            return View(rolesList);

        }





        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            if (id is null) return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound();
            var roleViewModel = new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name,
            };
            return View(roleViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id is null) return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound();

            var users = await _userManager.Users.ToListAsync();
            return View(new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                Users = users.Select(user => new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            });
        }


        [HttpPost]
        public async Task<IActionResult> Edit(string id, RoleViewModel roleViewModel)
        {

            if (!ModelState.IsValid) return View(roleViewModel);
            var message = string.Empty;
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return NotFound();
                role.Name = roleViewModel.Name;
                var result = await _roleManager.UpdateAsync(role);
                foreach(var userRole in roleViewModel.Users)
                {
                    var user = await _userManager.FindByIdAsync(userRole.UserId);
                    if(user is not null)
                    {
                        if (userRole.IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                        {
                            await _userManager.AddToRoleAsync(user, role.Name);
                        }
                        else if (!userRole.IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(user, role.Name);
                        }
                    }
                }
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    message = "Role can not be updated";
                }
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                {
                    message = ex.Message;

                }
                else
                {
                    message = "Role can not be updated";
                }
            }
            return View(roleViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id is null) return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound();
            return View(new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string? id)
        {
            if (id is null) return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            var message = string.Empty;
            try
            {
                if (role is not null)
                {
                    await _roleManager.DeleteAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                message = "Error while deleting the role";
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                {
                    message = ex.Message;
                }
                else
                {
                    message = "Error while deleting the role";
                }
            }
            ModelState.AddModelError(string.Empty, message);
            return View(nameof(Index));

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = roleViewModel.Name,
                });
                return RedirectToAction(nameof(Index));
            }
            return View(roleViewModel);
        }
    }
}
