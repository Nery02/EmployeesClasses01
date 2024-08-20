using EmployeesClasses01.Dto;
using EmployeesClasses01.Models;
using Microsoft.AspNetCore.Identity;

namespace EmployeesClasses01.Services
{
    public class UserService : IUser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }

        public async Task<IdentityResult> RegisterUser(RegisterDto userModel)
        {
            var user = new ApplicationUser
            {
                UserName = userModel.Email,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                IsActive = true
            };
            return await _userManager.CreateAsync(user, userModel.Password);
        }

        public async Task<SignInResult> Login(LoginDto loginModel)
        {
            var loggedinUser = await _userManager.FindByLoginAsync(loginModel.Username, loginModel.Password);
            bool withAccess = false;
            if (loggedinUser != null)
            {
                await _userManager.UpdateSecurityStampAsync(loggedinUser);
                withAccess = loggedinUser.IsActive;
            }
            else
            {
                ApplicationUser usuario = await _userManager.FindByEmailAsync(loginModel.Username);
                withAccess = usuario.IsActive;
                UserLoginInfo info = new UserLoginInfo(loginModel.Username, loginModel.Password, loginModel.Username);
                await _userManager.AddLoginAsync(usuario, info);
            }
            return await _signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password, true, false);
        }

        
    }
}
