using ExamService.Contracts.UnitOfWork;
using ExamService.Entities.Models;
using ExamService.Web.Backend.ViewModels.LessonViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ExamService.Web.Backend.Controllers
{
    [Authorize]
    public class LessonController : Controller
    {
        private readonly IUnitOfWork _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public LessonController(IUnitOfWork context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Lesson
        [HttpGet]
        public ViewResult Index()
        {
            var lessons = _context.Lessons.GetAll().Where(x=>x.UserId == GetUser.Id 
            && x.Delete == false).Select(x => new LessonViewModel
            {
                Name = x.Name,
                Guid = x.Guid,
                Code = x.Code,
                Akts = x.Akts,
                Hours = x.Hours,
                Subjects = x.Subjects.Where(r=>r.Delete == false).Select(s=>new SubjectViewModel
                {
                    Id = s.Id,
                    SubjactName = s.Name
                }).ToList()
                
            }).ToList();
            return View(lessons);
        }

        // GET: Lesson/Details/5
        public IActionResult Details(string id)
        {
            var lesson = _context.Lessons
                            .GetAll().Where(x => x.UserId == GetUser.Id 
                                && x.Delete == false && x.Guid == id)
                            .Select(x => new LessonViewModel
                            {
                                Name = x.Name,
                                Guid = x.Guid,
                                Code = x.Code,
                                Akts = x.Akts,
                                Hours = x.Hours,
                                SumQuesion = x.QuestionPools.Where(q => q.Delete == false).Count(),
                                Subjects = x.Subjects
                                            .Where(r => r.Delete == false)
                                            .Select(s => new SubjectViewModel
                                            {
                                                Id = s.Id,
                                                SubjactName = s.Name,
                                                QuestionCount = s.QuestionPools.Where(q=>q.Delete == false).Count()
                                            }).ToList()

                            }).FirstOrDefault();
            return View(lesson);
        }

        // GET: Lesson/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lesson/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LessonViewModel model)
        {
            try
            {
                if (model.Name != "")
                {
                    var lesson = new Lesson
                    {
                        UserId = GetUser.Id,
                        Name = model.Name,
                        Code = model.Code,
                        Hours = model.Hours,
                        Akts = model.Akts
                    };
                    
                    _context.Lessons.Add(lesson);
                    _context.Lessons.SaveChanges();

                    model.Subjects.ForEach(n => _context.Subjects.Add(new Subject
                    {
                        Name = n.SubjactName,
                        LessonId = lesson.Id
                    }));
                    var result = _context.Subjects.SaveChanges();
                    if (result > 0)
                    {
                        //return Redirect("~/Lesson/Index");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(model);
                    }

                }
                else
                {
                    return View(model);
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Lesson/Edit/5
        public ActionResult Edit(string id)
        {
            var lesson = _context.Lessons.GetAll().Where(x => x.Guid == id 
            && x.UserId == GetUser.Id 
            && x.Delete == false).FirstOrDefault();
            if (lesson != null)
            {
                return View(new LessonViewModel
                {
                    Name =lesson.Name,
                    Guid = lesson.Guid,
                    Code = lesson.Code,
                    Akts = lesson.Akts,
                    Hours = lesson.Hours
                });
            }
            else return RedirectToAction("Index");
        }

        // POST: Lesson/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LessonViewModel model)
        {
            try
            {
                var lesson = _context.Lessons.GetAll().Where(x => x.Guid == model.Guid && x.UserId == GetUser.Id).FirstOrDefault();
                if (model.Name != "")
                {
                    lesson.Name = model.Name;
                    lesson.Code = model.Code;
                    lesson.Hours = model.Hours;
                    lesson.Akts = model.Akts;
                    
                    var result = _context.Lessons.SaveChanges();
                    if (result > 0)
                    {
                        //return Redirect("~/Lesson/Index");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(model);
                    }

                }
                else
                {
                    return View(model);
                }
            }
            catch
            {
                return Redirect("~/Lesson/Edit/"+model.Guid);
            }
        }

        // GET: Lesson/Delete/5
        public ActionResult Delete(string id)
        {
            var lesson = _context.Lessons.GetAll().Where(x => x.Guid == id && x.UserId == GetUser.Id).FirstOrDefault();
            if (lesson != null)
            {
                return View(new LessonViewModel
                {
                    Name = lesson.Name,
                    Guid = lesson.Guid,
                    Code = lesson.Code,
                    Akts = lesson.Akts,
                    Hours = lesson.Hours
                });
            }
            else return RedirectToAction("Index");
        }

        // POST: Lesson/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, bool confirm)
        {
            try
            {
                var lesson = _context.Lessons.GetAll().Where(x => x.Guid == id && x.UserId == GetUser.Id).FirstOrDefault();
                if (confirm && lesson != null)
                {
                    lesson.Delete = true;
                    //_context.Remove(lesson);
                    if (_context.Lessons.SaveChanges() > 0) return RedirectToAction("Index");
                    else return Redirect("~/Lesson/Delete/" + id);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // get lessons list ajax
        // GET: Lesson/GetLessons
        [HttpGet]
        public IActionResult GetLessons()
        {
            return Json(_context.Lessons.GetAll().Where(x => x.UserId == GetUser.Id && x.Delete == false)
                                    .Select(l=>new {Name = l.Name, Guid = l.Guid }).ToList());
        }

        // get subjects list ajax
        // GET: Lesson/GetSubjects?guid=5
        [HttpGet]
        public IActionResult GetSubjects(string guid)
        {
            return Json(_context.Subjects.GetAll().Where(x => x.Delete == false
                                                && x.Lesson.Guid == guid
                                                && x.Lesson.UserId == GetUser.Id)
                                         .Select(s => new { Name = s.Name, Id = s.Id })
                                         .ToList());
        }

        // add subject ajax
        // POST: Lesson/AddSubjects?id=5&name=demo
        [HttpPost]
        public IActionResult AddSubject(string lessonGuid, string subjectName)
        {
            var subject = new Subject{ LessonId = GetLesson(lessonGuid).Id, Name = subjectName };

            if (subjectName == "")
            {
                return Json(new { error = true, message = "Alanlarý boþ geçemezsiniz!" });
            }
            else
            {
                _context.Subjects.Add(subject);
            }

            var result = _context.Subjects.SaveChanges();
            if (result > 0)
            {
                return Json(new { error = false, message = "Baþarýyla kaydet.", subject = subject });
            }
            else
            {
                return Json(new { error = true, message = "Güncelleme sýrasýnda bir hata oluþtu !" });
            }
        }

        // edit subject ajax
        // POST: Lesson/EditSubjects?id=5&name=demo
        [HttpPost]
        public IActionResult EditSubject(int subjectId, string subjectName)
        {
            try
            {
                var sub = _context.Subjects.GetAll().Where(x => x.Id == subjectId && x.Lesson.UserId == GetUser.Id).FirstOrDefault();
                sub.Name = subjectName;
                var result = _context.Subjects.SaveChanges();
                if (result > 0)
                {
                    return Json(new { error = false, message = "Baþarýyla güncellendi." });
                }
                else
                {
                    return Json(new { error = true, message = "Kayýt baþarsýz oldu  !" });
                }
            }
            catch (Exception e)
            {
                return Json(new { error = true, message = "Güncelleme sýrasýnda bir hata oluþtu !" , exception = e.Message });
            }
        }

        // delete subject ajax
        // POST: Lesson/DeleteSubjects?id=5
        [HttpPost]
        public IActionResult DeleteSubject(int subjectId)
        {
            var questionCount = _context.QuestionPools.GetAll().Where(x => x.SubjectId == subjectId && x.Lesson.UserId == GetUser.Id).Count();

            if(questionCount > 0)
            {
                return Json(new {
                    error = true,
                    message = "Konuya baðlý "+questionCount+" tane soru var! \n"+
                    "Konuya baðlý sorularý sildikten sonra bu iþlemi gerçekleþtirebilirsiniz..."
                });
            }
            else
            {
                var sub = _context.Subjects.GetAll().Where(x => x.Id == subjectId && x.Lesson.UserId == GetUser.Id).FirstOrDefault();
                //sub.Delete = true;
                _context.Subjects.Delete(sub);
                var result = _context.Subjects.SaveChanges();
                if (result > 0)
                {
                    return Json(new { error = false, message = "Baþarýyla silindi." });
                }
                else
                {
                    return Json(new { error = true, message = "Silme sýrasýnda bir hata oluþtu !" });
                }
            }
        }

        #region Helpers

        // get login user property fun.
        //private ApplicationUser GetUser => _context.Users.Where(i => i.Id == _userManager.GetUserId(User)).Single();
        private ApplicationUser GetUser => _userManager.GetUserAsync(User).Result;

        // get lesson by guid function
        private Lesson GetLesson(string guid) => _context.Lessons
            .GetAll().Where(x => x.Guid == guid && x.UserId == GetUser.Id && x.Delete == false).FirstOrDefault();

        #endregion

    }
}