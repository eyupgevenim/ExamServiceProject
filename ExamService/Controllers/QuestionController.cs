using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamService.Data;
using Microsoft.AspNetCore.Identity;
using ExamService.Data.Tables;
using ExamService.Models.QuestionViewModels;
using ExamService.Models;
using Microsoft.AspNetCore.Authorization;

namespace ExamService.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public QuestionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Question
        public ActionResult Index()
        {
            var lessonsQuestions = _context.Lessons
                        .Where(l => l.UserId == GetUser.Id && l.Delete == false)
                        .Select(l => new QuestionSummaryViewModel
                        {
                            LessonName = l.Name,
                            LessonGuid = l.Guid,
                            Questions = l.QuestionPools
                                            .Where(x=>x.Delete == false)
                                            .GroupBy(q => q.ExamType)
                                            .Select(q => new GrupQuestion
                                            {
                                                ExamTypeName = q.Key,
                                                Count = q.Count()
                                            }).ToList()
                        }).ToList();

            return View(lessonsQuestions);
        }

        // GET: Question/Details/5?page=2
        public ActionResult Details(string id, int page = 1)
        {
            int lessonCount = _context.QuestionPools.Where(x => x.Lesson.Guid == id && x.Delete == false).Count();
            lessonCount = (lessonCount % 20) == 0 ? (lessonCount / 20) : ((int)(lessonCount / 20) + 1);
            if (page > lessonCount) return Redirect("~/Question/Details/" + id);

            var questionsList = _context.Lessons
                                    .Where(x => x.Guid == id && x.UserId == GetUser.Id && x.Delete == false)
                                    .Select(l => l.QuestionPools.Where(y=>y.Delete == false).Select(q => new ListQuestionViewModel
                                    {
                                        Id = q.Id,
                                        ExamFormat = q.ExamFormat,
                                        ExamType = q.ExamType,
                                        Question = q.Question,
                                        DescriptionJ = q.ExamFormat == "Test" ? Newtonsoft.Json.JsonConvert.DeserializeObject<Option>(q.Description) : null,
                                        Description = q.ExamFormat == "Klasik" ? q.Description : "",
                                        Answer = q.Answer,
                                        Subject = q.Subcject.Name,
                                        LessonGuid = l.Guid,
                                        LessonName = l.Name
                                    }).OrderBy(g => g.Id).Skip((page - 1) * 20).Take(20).ToList()).FirstOrDefault();


            ViewData["page"] = lessonCount;
            ViewData["active"] = page;

            return View(questionsList);
        }

        // GET: Question/Create
        public ActionResult Create(string id)
        {
            if (id == "Test") return View("CreateTest",new QuestionViewModel());
            else if (id == "Classic") return View("createClassic");
            else return RedirectToAction("Index");
        }
        
        // POST: Question/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(QuestionViewModel model)
        {
            try
            {
                string examFormat = model.qExamFormat == "Test" ? "Test" : "Classic";

                if (model.Question == "")
                {
                    ViewData["result"] = new Result { error = true, message = "Soru alaný boþ býrakýlamaz !" };
                    return View("Create" + examFormat, model);
                }
                else if (examFormat == "Test" && model.DescriptionJ == null)
                {
                    ViewData["result"] = new Result { error = true, message = "Test alaný boþ býrakýlamaz !" };
                    return View("Create" + examFormat, model);
                }
                

                var question = new QuestionPool
                {
                    Question = model.Question,
                    Description = model.qExamFormat == "Klasik" ? model.Description
                            : Newtonsoft.Json.JsonConvert.SerializeObject(model.DescriptionJ),
                    ExamFormat = model.qExamFormat,
                    ExamType = model.qExamType,
                    LessonId = GetLesson(model.qLessonGuid).Id,
                    SubjectId = model.qSubjectId,
                    Answer=model.Answer
                };
                
                _context.QuestionPools.Add(question);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    ViewData["result"] = new Result { error=false, message="Kayýt Baþrlý."};
                    return View("Create" + examFormat, new QuestionViewModel());
                }
                else
                {
                    ViewData["result"] = new Result { error = true, message = "Kayýt sýrasýnda bir hata oluþtu !" };
                    return View("Create" + examFormat, model);
                }
            }
            catch
            {
                ViewData["result"] = new Result { error = true, message = "Kayýt sýrasýnda sitemde bir hata oluþru !" };
                return View();
            }
        }

        // GET: Question/Edit/5
        public ActionResult Edit(string id)
        {

            var question = _context.QuestionPools
                                        .Where(x=>x.Id == id && x.Lesson.UserId == GetUser.Id && x.Delete == false)
                                        .Select(q => new QuestionViewModel
                                        {
                                            Id = q.Id,
                                            Question = q.Question,
                                            DescriptionJ = q.ExamFormat == "Test" ? 
                                                                Newtonsoft.Json.JsonConvert.
                                                                DeserializeObject<Option>(q.Description) : null,
                                            Description = q.ExamFormat == "Klasik" ? q.Description : "",
                                            Answer = q.Answer,
                                            qExamType = q.ExamType,
                                            qExamFormat = q.ExamFormat,
                                            qLessonGuid = q.Lesson.Guid
                                        }).SingleOrDefault();
            if (question != null)
            {
                if(question.qExamFormat == "Test")
                {
                    return View("EditTest",question);
                }
                else if(question.qExamFormat == "Klasik")
                {
                    return View("EditClassic",question);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Question/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, QuestionViewModel model)
        {
            try
            {
                string examFormat = model.qExamFormat == "Test" ? "Test" : "Classic";
                
                if (model.Question == "")
                {
                    return View("Edit"+examFormat, model);
                }
                else if (examFormat == "Test" && model.DescriptionJ == null)
                {
                    return View("Edit"+examFormat, model);
                }
                

                QuestionPool q = _context.QuestionPools
                                            .Where(x => x.Id == model.Id && x.Lesson.UserId == GetUser.Id)
                                            .SingleOrDefault();
                q.Question = model.Question;
                q.Answer = model.Answer;

                if(q.ExamFormat == "Klasik")
                {
                    q.Description = model.Description;
                }
                else if(q.ExamFormat == "Test")
                {
                    q.Description = Newtonsoft.Json.JsonConvert.SerializeObject(model.DescriptionJ);
                }
                
                
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return Redirect("~/Question/Details/" + model.qLessonGuid);
                }
                else
                {
                    return View("Edit"+examFormat, model);
                }
            }
            catch
            {
                return Redirect("~/Question/Edit/"+id);
            }
        }

        // GET: Question/Delete/5
        public ActionResult Delete(string id)
        {
            var question = _context.QuestionPools
                                        .Where(x => x.Id == id 
                                        && x.Delete == false
                                        && x.Lesson.UserId == GetUser.Id)
                                        .Select(q => new QuestionViewModel
                                        {
                                            Id = q.Id,
                                            Question = q.Question,
                                            DescriptionJ = q.ExamFormat == "Test" ?
                                                                Newtonsoft.Json.JsonConvert.
                                                                DeserializeObject<Option>(q.Description) : null,
                                            Description = q.ExamFormat == "Klasik" ? q.Description : "",
                                            qExamType = q.ExamType,
                                            qExamFormat = q.ExamFormat,
                                            qLessonGuid = q.Lesson.Guid
                                        }).SingleOrDefault();
            if (question != null)
            {
                return View("Delete", question);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Question/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, bool confirm = false)
        {
            try
            {
                var question = _context.QuestionPools.Where(x => x.Id == id && x.Lesson.UserId == GetUser.Id).FirstOrDefault();
                if(confirm && question != null)
                {

                    var countQuestions = _context.Exams.ToList()
                        .Where(x => x.Questions.Contains(id)).Count();
                    if (countQuestions > 0)
                    {
                        question.Delete = true;

                    }
                    else
                    {
                        _context.Remove(question);
                    }
                    
                    //question.Delete = true;
                    int result = _context.SaveChanges();
                    if (result > 0) return Redirect("~/Question/Details/" + question.Lesson.Guid);
                    else return Redirect("~/Question/Delete/" + id);

                }
                else
                {
                    return Redirect("~/Question/Details/" + question.Lesson.Guid);
                }
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }


        #region Helpers

        // get login user property fun.
        //private ApplicationUser GetUser => _context.Users.Where(i => i.Id == _userManager.GetUserId(User)).Single();
        private ApplicationUser GetUser => _userManager.GetUserAsync(User).Result;

        // get lesson by guid function
        private Lesson GetLesson(string guid) => _context.Lessons
            .Where(x => x.Guid == guid && x.UserId == GetUser.Id && x.Delete == false).FirstOrDefault();

        #endregion

    }
}