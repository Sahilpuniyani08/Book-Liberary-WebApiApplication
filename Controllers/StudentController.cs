using LiberaryApp.Dtos;
using LiberaryApp.Services.StudentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LiberaryApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController( IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("Get")]

        public async Task<IActionResult> GetStudentBooks()
        {
            //int Userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            return Ok( await _studentService.GetAllBooks());
        } 
        
        [HttpPost("ADD")]

        public async Task<IActionResult> AddStudentBook(AddStudnetBookDto newBook)
        {
            return Ok( await _studentService.AddNewBook(newBook));
        }


        [HttpDelete("DELETE")]

        public async Task<IActionResult> DeleteBook(AddStudnetBookDto newBook)
        {
            return Ok(await _studentService.DeleteStudentBook(newBook));
        }
    }
}
