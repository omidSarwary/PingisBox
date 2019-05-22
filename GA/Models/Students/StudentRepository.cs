using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Models
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _appDbCotext;
        public StudentRepository(AppDbContext appDbCotext)
        {
            _appDbCotext = appDbCotext;
        }

        public void AddLog(StudentLog log)
        {
            _appDbCotext.StudentLogs.Add(log);
            _appDbCotext.SaveChanges();
        }

        public void AddStudent(Students student)
        {
            _appDbCotext.students.Add(student);
            _appDbCotext.SaveChanges();
        }

        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public IEnumerable<StudentLog> GetAllLogs()
        {
            return _appDbCotext.StudentLogs;
        }

        public StudentLog GetLastLog()
        {
            return _appDbCotext.StudentLogs.Last();
        }

        public Students GetStudentByCode(int StudentCode)
        {
            return _appDbCotext.students.FirstOrDefault(p => p.code == StudentCode);
        }

        public Students GetStudentById(int StudentId)
        {
            return _appDbCotext.students.FirstOrDefault(p => p.Id == StudentId);
        }
        public Students GetStudentByRFID(ulong Id)
        {
            return _appDbCotext.students.FirstOrDefault(p => p.RFID == Id);
        }
    }
}
