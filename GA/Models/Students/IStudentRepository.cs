using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Models
{
    public interface IStudentRepository
    {
        Students GetStudentById(int StudentId);
        Students GetStudentByRFID(ulong Id);
        Students GetStudentByCode(int code);
        void AddStudent(Students Student);
        int GenerateRandomNo();

        IEnumerable<StudentLog> GetAllLogs();
        void AddLog(StudentLog log);
        StudentLog GetLastLog();


    }
}
