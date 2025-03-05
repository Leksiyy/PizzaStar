using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzaStar.Data;
using PizzaStar.Models;
using PizzaStar.Models.Pages;
using PizzaStar.ViewModels;

namespace PizzaStar.Controllers
{
    [Authorize(Roles = "Admin,Editor")]
    public class PanelController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _context;
        public PanelController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }


        [Route("/panel")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        [Route("/panel/users")]
        [HttpGet]
        public IActionResult Users(QueryOptions options)
        {
            var pagedList = new PagedList<User>(_userManager.Users, options);
            return View(pagedList);
        }


        [Authorize(Roles = "Admin")]
        [Route("/panel/create-update-user")]
        [HttpGet]
        public async Task<IActionResult> CreateOrUpdateUser(string? userId)
        {
            if (userId is null)
            {
                return View(new CreateOrUpdateUserViewModel());
            }
            else
            {
                User user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }
                CreateOrUpdateUserViewModel model = new CreateOrUpdateUserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    Year = user.Year,
                };
                return View(model);
            }
        }


        [Authorize(Roles = "Admin")]
        [Route("/panel/create-update-user")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateOrUpdateUser(CreateOrUpdateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id is null)
                {
                    var userCheck = await _userManager.FindByEmailAsync(model.Email);
                    if (userCheck != null)
                    {
                        ModelState.AddModelError("Email", "Такой емейл адрес уже занят.");
                        return View(model);
                    }
                    User user = new User { Email = model.Email, UserName = model.Email, PhoneNumber = model.Phone, Year = model.Year };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddToRoleAsync(user, "Client");
                        return RedirectToAction(nameof(Users));
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    User user = await _userManager.FindByIdAsync(model.Id);
                    if (user != null)
                    {
                        if (!user.Email.Equals(model.Email))
                        {
                            var userCheck = await _userManager.FindByEmailAsync(model.Email);
                            if (userCheck != null)
                            {
                                ModelState.AddModelError("Email", "Такой емейл адрес уже занят.");
                                return View(model);
                            }
                        }
                        user.Email = model.Email;
                        user.UserName = model.Email;
                        user.Year = model.Year;
                        user.PhoneNumber = model.Phone;
                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(Users));
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [Route("/panel/delete-user")]
        [HttpDelete]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return Ok();
        }


        [Authorize(Roles = "Admin")]
        [Route("/panel/edit-role")]
        [HttpGet]
        public async Task<IActionResult> EditRole(string userId)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получаем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }
            return NotFound();
        }


        [Authorize(Roles = "Admin")]
        [Route("/panel/edit-role")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditRole(string userId, List<string> roles)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);


                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);


                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);


                return RedirectToAction(nameof(Users));
            }
            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [Route("/panel/statistics")]
        [HttpGet]
        public IActionResult Statistics()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [Route("/panel/set-contact")] 
        [HttpGet]
        public IActionResult Contact(QueryOptions options)
        {
            var pagedList = new PagedList<Contact>(_context.Contacts, options);
            return View(pagedList);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateContact(string email)
        {
            if (ModelState.IsValid)
            {
                await _context.Contacts.AddAsync(new Contact{Email = email});
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Contact));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("/panel/delete-contact")]
        public IActionResult DeleteContact(string id)
        {
            if (!int.TryParse(id, out int contactId))
            {
                return BadRequest("Некорректный ID");
            }

            var email = _context.Contacts.Find(contactId);
            if (email == null)
            {
                return NotFound("Email не найден");
            }

            _context.Contacts.Remove(email);
            _context.SaveChanges();

            return Ok(new { message = "Email удалён успешно" });
        }
    }
}
