using EmployeesClasses01.Data;
using EmployeesClasses01.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesClasses01.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly ApplicationdbContext _context;

        public EmployeeController(ApplicationdbContext context)
        {
            _context = context;
        }

        // Vista principal para la gestión de empleados
        public IActionResult Management()
        {
            ViewBag.EmployeeTypes = _context.EmployeeTypes.ToList();
            return View();
        }

        // Obtener lista de empleados
        [HttpGet("GetEmployees")]
        public IActionResult GetEmployees()
        {
            var result = _context.Employees.ToList();
            if (!result.Any())
            {
                return Json(new { data = new string[] { } });
            }
            return Json(new { data = result.Where(x => x.IsActive) });
        }

        // Agregar un nuevo empleado
        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            employee.ProfilePic = "";
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return Ok(new { status = "success", message = "Empleado agregado exitosamente", description = "El empleado será agregado al listado" });
        }

        // Actualizar un empleado existente
        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee(Employee employee)
        {
            employee.ProfilePic = "";
            employee.IsActive = true;
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return Ok(new { status = "success", message = "Empleado actualizado exitosamente", description = "El empleado será actualizado en el listado" });
        }

        // Eliminar un empleado
        [HttpDelete("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(int Id)
        {
            var employee = await _context.Employees.FindAsync(Id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(new { status = "success", message = "Empleado eliminado exitosamente", description = "El empleado será removido del listado" });
        }

        // Eliminar lógicamente un empleado
        [HttpDelete("SoftDeleteEmployee")]
        public async Task<IActionResult> SoftDeleteEmployee(int Id)
        {
            var employee = await _context.Employees.FindAsync(Id);
            if (employee == null)
            {
                return NotFound();
            }

            employee.IsActive = false;
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return Ok(new { status = "success", message = "Empleado inhabilitado exitosamente", description = "El empleado será removido del listado" });
        }
    }
}
