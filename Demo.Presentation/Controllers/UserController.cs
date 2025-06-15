using Demo.DataAccess.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Demo.Presentation.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UserController(UserManager<ApplicationUser> userManager, IWebHostEnvironment env) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IWebHostEnvironment _env = env;

        [HttpGet]
        public async Task<IActionResult> Index(string SearchValue)
        {
            var usersQuery = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(SearchValue))
            {
                usersQuery = usersQuery.Where(U => U.Email.ToLower().Contains(SearchValue.ToLower()));
            }
            var userList = await usersQuery.Select(U => new UserViewModel
            {
                Id = U.Id,
                FName = U.FirstName,
                LName = U.LastName,
                Email = U.Email,
            }).ToListAsync();

            foreach (var user in userList)
            {
                user.Roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id));
            }
            return View(userList);

        }





        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            if (id is null) return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();
            var userViewModel = new UserViewModel()
            {
                Id = user.Id,
                FName = user.FirstName,
                LName = user.LastName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
            };
            return View(userViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id is null) return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();

            return View(new UserViewModel
            {
                Email = user.Email,
                FName = user.FirstName,
                LName = user.LastName,
                Id = user.Id,
                Roles = _userManager.GetRolesAsync(user).Result
            });
        }


        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserViewModel userViewModel)
        {

            if (!ModelState.IsValid) return View(userViewModel);
            var message = string.Empty;
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is null) return NotFound();
                user.FirstName = userViewModel.FName;
                user.LastName = userViewModel.LName;
                user.Email = userViewModel.Email;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    message = "User can not be updated";
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
                    message = "User can not be updated";
                }
            }
            return View(userViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id is null) return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();
            return View(new UserViewModel
            {
                Email = user.Email,
                FName = user.FirstName,
                LName = user.LastName,
                Id = user.Id
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string? id)
        {
            if (id is null) return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            var message = string.Empty;
            try
            {
                if (user is not null)
                {
                    await _userManager.DeleteAsync(user);
                    return RedirectToAction(nameof(Index));
                }
                message = "Error while deleting the user";
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                {
                    message = ex.Message;
                }
                else
                {
                    message = "Error while deleting the user";
                }
            }
            ModelState.AddModelError(string.Empty, message);
            return View(nameof(Index));

        }

    }
}
