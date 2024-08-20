using EmployeesClasses01.Dto;
using EmployeesClasses01.Models;
using EmployeesClasses01.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesClasses01.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUser _userService;
        public AccountController(IUser userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(LoginDto loginDto = null)
        {
            if (loginDto.Username is null && loginDto.Password == null) return View();
            var result = await _userService.Login(loginDto);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            if (result.IsLockedOut)
            {
                return BadRequest(new string[] { "info", "No es posible iniciar sesion", "El usuario esta bloqueado" });
            }
            if (result.IsNotAllowed)
            {
                return BadRequest(new string[] { "error", "No es posible iniciar sesion", "No tiene permitido el ingreso al recurso" });
            }
            return BadRequest(new string[] { "error", "No es posible iniciar sesion", "credenciales incorrectas" });
        }



        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(RegisterDto user)
        {
            var result = await _userService.RegisterUser(user);
            return result.Succeeded ?
                Ok(new string[] { "success", "Usuario agregado exitosamente", "El usuario sera agregado al listado" }) :
                BadRequest(new string[] { "error", "El usuario no se ha guardado", $"{String.Join(',', result.Errors.Select(x=>x.Description).ToList())}" });
        }
    }
}
