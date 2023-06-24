using blog.web.Models.ViewModels;
using blog.web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace blog.web.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private readonly UserManager<IdentityUser> userManager;



        public AdminUsersController(IUsersRepository usersRepository, UserManager<IdentityUser> userManager)
        {
            this._usersRepository = usersRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {

          var users  = await _usersRepository.GetAll();

            var usersViewModel = new UserViewModel();

            usersViewModel.Users = new List<User>();

            foreach (var user in users)
            {
                usersViewModel.Users.Add(new Models.ViewModels.User
                {
                    Id = Guid.Parse(user.Id),
                    Username = user.UserName,
                    EmailAdress = user.Email,

                });

            }

            

            return View(usersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> List(UserViewModel request)
        {
            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email
            };

            var identityresult = await userManager.CreateAsync(identityUser, request.Password);

            if (identityresult is not null)
            {
                if (identityresult.Succeeded)
                {
                    // assign roles to this user
                    var roles = new List<string> { "User" };
                    if (request.AdminRoleCheckbox)
                    {
                        roles.Add("Admin");
                    }
                    identityresult = await userManager.AddToRolesAsync(identityUser, roles);

                    if (identityresult is not null && identityresult.Succeeded)
                    {
                        return RedirectToAction("List", "AdminUsers");
                    }
                }
            }
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
          var removeUser =   await userManager.FindByIdAsync(Id.ToString());
            if (removeUser != null)
            {
              var IdenttityResult =   await userManager.DeleteAsync(removeUser);
                
                if (IdenttityResult is not null && IdenttityResult.Succeeded)
                {
                    return RedirectToAction("List", "AdminUsers");
                }

            }

            return View();
        }
    }
}
