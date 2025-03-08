using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PizzaStar.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View("~/Views/Panel/RoleManager.cshtml", roles);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return RedirectToAction("Index");

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string roleId, string newRoleName)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                role.Name = newRoleName;
                await _roleManager.UpdateAsync(role);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }
    }
}