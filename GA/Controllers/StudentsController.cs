using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GA.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IStudentRepository _studentRepository;
        public StudentsController(AppDbContext context, IStudentRepository studentRepository)
        {
            _context = context;
            _studentRepository = studentRepository;
        }


        // GET: api/Students/CheckUserBcode/5
        [HttpGet("CheckUserBcode/{id}")]
        public async Task<ActionResult<bool>> CheckUserBcode(int id)
        {
            var student =  _studentRepository.GetStudentByCode(id);
           
            if (student != null)
            {
                return true;
            }
            else
            return NotFound();
        }

        // GET: api/Students/CheckUserBRfid/5
        [HttpGet("CheckUserBRfid/{id}")]
        public async Task<ActionResult<bool>> CheckUserBRfid(ulong id)
        {
            var student = _studentRepository.GetStudentByRFID(id);

            if (student != null)
            {
                return true;
            }
            else
                return NotFound();
        }




        // POST: api/Students/RegUserRfidByCode

        [HttpPost("RegUserRfidByCode")]
        public async Task<ActionResult<Students>> RegUserRfidByCode(StudentsReg studentD)
        {
            var student = _studentRepository.GetStudentByCode(studentD.code);
            if (student != null)
            {
                student.RFID = studentD.RFID;
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            return NotFound();
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Students>> DeleteStudents(int id)
        {
            var students = await _context.students.FindAsync(id);
            if (students == null)
            {
                return NotFound();
            }

            _context.students.Remove(students);
            await _context.SaveChangesAsync();

            return students;
        }

        private bool StudentsExists(int id)
        {
            return _context.students.Any(e => e.Id == id);
        }
    }
}
