using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GA.Models;

using MimeKit;
using Microsoft.AspNetCore.Authorization;
using static SweetAlertBlog.Enums.Enums;
using GA.Hubs;
using Microsoft.AspNetCore.SignalR;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace GA.Controllers
{
    public class HomeController : BaseController

    {
        private readonly IStudentRepository NstudentRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IBoxRepository _boxRepository;
        private readonly IMail _mail;
        private readonly IHubContext<BoxHub> _boxHub;
        private readonly AppDbContext _appDbCotext;
        private IHostingEnvironment _env;

        public HomeController(IHostingEnvironment env, IMail mail, IStudentRepository studentRepository, IHubContext<BoxHub> boxHub, AppDbContext appDbContext, IBoxRepository boxRepository, IItemRepository itemRepository)
        {
            _env = env;
            _boxHub = boxHub;
            NstudentRepository = studentRepository;
            _appDbCotext = appDbContext;
            _mail = mail;
            _itemRepository = itemRepository;
            _boxRepository = boxRepository;



        }

        public IActionResult Index(string userName)
        {
            if (userName != null)
            Alert("Tack för din registrering! ", "Ett mail har skickats till mailadressen: "+ userName + " innehållande en fyrsiffrig kod.  ", NotificationType.success);
            return View();
        }

        [HttpPost]
        public IActionResult Index(Students student)
        {
           
            if (ModelState.IsValid)
            {
                if (!_appDbCotext.students.Any(p => p.email == student.email))
                {
                    
                    int _code = 0;

                    do
                    {
                        _code = NstudentRepository.GenerateRandomNo();

                    } while (_appDbCotext.students.Any(p => p.code == _code));

                    student.code = _code;
                    ViewBag.Message = "done!";

                    string[] result = student.email.Split('.', '@').ToArray();
                    student.FullName = result[0] + " " + result[1];
                    NstudentRepository.AddStudent(student);
                    
                    return RedirectToAction("SendCode", "Home", new { Code = _code });

                }
                else
                {
                    ViewBag.Message = "Students with this Email Already Exist";
                   
                    return View(student);
                }

            }

            return View(student);
        }

        public IActionResult SendCode(int Code)
        {
           
            var log = new StudentLog
            {
                dateTime = DateTime.Now
            };
            Students student = NstudentRepository.GetStudentByCode(Code);
           
            var webRoot = _env.WebRootPath;
            var pathToFile = _env.WebRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "mail"
                            + Path.DirectorySeparatorChar.ToString()
                            + "RegisterMail.html";
            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {

                builder.HtmlBody = SourceReader.ReadToEnd();

            }
            string messageBody = string.Format(builder.HtmlBody, student.FullName,Code,  DateTime.Now.ToString("dddd, d MMMM yyyy HH:mm:ss"));
          

           BackgroundJob.Schedule(() => Reminder(student.Id), TimeSpan.FromDays(1));

            _mail.GenMail(student.email, "Tack för din registrering! ",messageBody);

            // return RedirectToAction("Index", "Home", new { Ncode = student.code });
          
            log.Message = "New User with email: " + student.email+" Registered and got the code:"+student.code;
            NstudentRepository.AddLog(log);           
            return RedirectToAction("Index", "Home", new { userName = student.email });

        }

        //public IActionResult SendCode(int Code)
        //{
        //    var log = new StudentLog
        //    {
        //        dateTime = DateTime.Now
        //    };
        //    Students student = NstudentRepository.GetStudentByCode(Code);

        //    BackgroundJob.Schedule(() => Reminder(student.Id), TimeSpan.FromMinutes(2));

        //    _mail.GenMail(
        //               student.email, "Your one time pin code to the pingisBox",
        //               "\n Hej " + student.FullName + "\nYour pin code is: " + student.code +
        //               "\nPlease register your card within 24 hours or you will have to request for a new code."
        //               + "Registration Time: " + DateTime.Now.ToString("HH:mm:ss"));

        //    // return RedirectToAction("Index", "Home", new { Ncode = student.code });
        //    Alert("We have sent a onetime pin code to your email address " + student.email, NotificationType.success);
        //    log.Message = "New User with email: " + student.email + " Registered and got the code:" + student.code;
        //    NstudentRepository.AddLog(log);
        //    return RedirectToAction("Index", "Home");

        //}

        public async Task Reminder(int id)
        {
            var log = new StudentLog
            {
                dateTime = DateTime.Now
            };
            Students student = NstudentRepository.GetStudentById(id);
            if (student.RFID == 0)
            {
                _appDbCotext.students.Remove(student);
                await _appDbCotext.SaveChangesAsync();
                log.Message = "The user " + student.email + " did not register his card within 24 hours and has been terminated";
                NstudentRepository.AddLog(log);
            }

        }

        public IActionResult HowTo()
        {
            //_boxRepository.AddBox();
            //_itemRepository.AddItem();
            //_itemRepository.AddItem();

            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
