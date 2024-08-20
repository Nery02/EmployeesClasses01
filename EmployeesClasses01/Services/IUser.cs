using EmployeesClasses01.Dto;
using EmployeesClasses01.Models;
using Microsoft.AspNetCore.Identity;

namespace EmployeesClasses01.Services
{
    public interface IUser
    {
        Task<IdentityResult> RegisterUser(RegisterDto userModel);
        Task<SignInResult> Login(LoginDto loginModel);
    }
}
