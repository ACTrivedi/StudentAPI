using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAPI.DAL;
using StudentAPI.Models;

namespace StudentAPI.Controllers
{
    
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentDBContext _context;
        private readonly Student_DAL _dal;
        

        public StudentController(StudentDBContext context, Student_DAL dal)
        {
            _dal = dal;
            _context = context;
            
            
        }


        private Administrator IsAdmin()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claims = identity.Claims;

                return new Administrator
                {

                    Username = claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value,
                    EmailAddress = claims.FirstOrDefault(a => a.Type == ClaimTypes.Email)?.Value,
                    Role = claims.FirstOrDefault(a => a.Type == ClaimTypes.Role)?.Value

                };
            }

            return null;

        }

       
        [HttpGet]
        public IActionResult AdministratorEndPoint()
        {
            var Admin = IsAdmin();
            if (Admin != null)
            {
                return RedirectToAction("GetStudents");
            }
            else
            {
                return NotFound("No Admin Found");
            }
        }



        // GET: api/Student 

         
       
        [HttpGet]        
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            List<Student> students = new List<Student>();
            try
            {
                students = _dal.GetAllStudents();
            }
            catch (Exception)
            {
                
                throw;
            }

            return students;
        }

        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Student/5
       
        [HttpPut]
        public async Task<IActionResult> PutStudent(Student student)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new StudentDBContext())
            {
                var existingStudent = ctx.Students.Where(s => s.StudentId == student.StudentId)
                                                        .FirstOrDefault<Student>();

                if (existingStudent != null)
                {
                    existingStudent.StudentId = student.StudentId;
                    existingStudent.Name = student.Name;
                    existingStudent.ContactNumber = student.ContactNumber;
                    existingStudent.Age = student.Age;

                    ctx.Update(existingStudent);
                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        // POST: api/Student
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            bool result = _dal.Insert(student);

            if (!result)
            {
                return NotFound();
            }

            return CreatedAtAction("GetStudent", new { id = student.StudentId }, student);

        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return (_context.Students?.Any(e => e.StudentId == id)).GetValueOrDefault();
        }
    }
}
