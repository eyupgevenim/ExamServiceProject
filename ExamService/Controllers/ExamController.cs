using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ExamService.Data;
using Microsoft.AspNetCore.Identity;
using ExamService.Data.Tables;
using ExamService.Models.ExamViewModels;
using ExamService.Models.LessonViewModels;

namespace ExamService.Controllers
{
    [Authorize]
    public class ExamController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ExamController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_context.Lessons.Where(x => x.UserId == GetUser.Id && x.Delete == false).ToList());
        }

        //GET: Exam/List
        public IActionResult List()
        {
            var exams = _context.Exams.Where(x => x.Lesson.UserId == GetUser.Id)
                                        .OrderByDescending(o=>o.CreatedDate)
                                        .Select(e => new ExamSummaryViewModel
                                        {
                                            Id = e.Id,
                                            Name = e.Name,
                                            Title = e.Title,
                                            LessonName = e.Lesson.Name,
                                            CreatedDateTime = e.CreatedDate,
                                            Exam = Newtonsoft.Json.JsonConvert.DeserializeObject<ExamGroupViewModel>(e.Questions)
                                        }).ToList();
            return View(exams);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Create(QuestionAttributeViewModel model)
        {
            var keySubjectIds =Newtonsoft.Json.JsonConvert
                        .DeserializeObject<int[]>(model.eSubjectItems.ToString()).ToList();

            List<string> keyExamType = new List<string>();
            if (model.eVisa == "on") keyExamType.Add("Vize");
            if (model.eFinal == "on") keyExamType.Add("Final");
            if (model.eCompletion == "on") keyExamType.Add("Bütünleme");
            if (model.eExcuse == "on") keyExamType.Add("Mazeret");
            if (model.eSingleLesson == "on") keyExamType.Add("Tek Ders");

            var test = (keySubjectIds.Count() == 0)
                ? _context.QuestionPools
                                   .Where(x =>
                                         x.Lesson.Guid == model.eLessonGuid
                                         && x.Delete == false
                                         && x.Lesson.UserId == GetUser.Id
                                         && keyExamType.Contains(x.ExamType)
                                         && x.ExamFormat == "Test")
                                    .Select(q => new Test
                                    {
                                        Id = q.Id,
                                        Question = q.Question,
                                        DescriptionJ = Newtonsoft.Json.JsonConvert.DeserializeObject<Option>(q.Description),
                                        Answer = q.Answer
                                    })
                                    .OrderBy(g => Guid.NewGuid())
                                    .Take(GetExamQuestionsCount(model.eGroup,model.eGroupFormat,model.eTest))
                                    .ToList()
                  : _context.QuestionPools
                                   .Where(x =>
                                         x.Lesson.Guid == model.eLessonGuid
                                         && x.Delete == false
                                         && x.Lesson.UserId == GetUser.Id
                                         && keyExamType.Contains(x.ExamType)
                                         && x.ExamFormat == "Test" 
                                         && keySubjectIds.Contains(x.SubjectId))
                                    .Select(q => new Test
                                    {
                                        Id = q.Id,
                                        Question = q.Question,
                                        DescriptionJ = Newtonsoft.Json.JsonConvert.DeserializeObject<Option>(q.Description),
                                        Answer = q.Answer
                                    })
                                    .OrderBy(g => Guid.NewGuid())
                                    .Take(GetExamQuestionsCount(model.eGroup, model.eGroupFormat, model.eTest))
                                    .ToList();

            var classic = (keySubjectIds.Count() == 0)
                ? _context.QuestionPools
                                .Where(x =>
                                        x.Lesson.Guid == model.eLessonGuid
                                        && x.Delete == false
                                        && x.Lesson.UserId == GetUser.Id
                                        && keyExamType.Contains(x.ExamType)
                                        && x.ExamFormat == "Klasik")
                                .Select(q => new Classic
                                {
                                    Id = q.Id,
                                    Question = q.Question,
                                    Description = q.Description,
                                    Answer = q.Answer
                                })
                                 .OrderBy(g => Guid.NewGuid())
                                 .Take(GetExamQuestionsCount(model.eGroup, model.eGroupFormat, model.eClassic))
                                 .ToList()
                 : _context.QuestionPools
                                .Where(x =>
                                        x.Lesson.Guid == model.eLessonGuid
                                        && x.Delete == false
                                        && x.Lesson.UserId == GetUser.Id
                                        && keyExamType.Contains(x.ExamType)
                                        && x.ExamFormat == "Klasik" 
                                        && keySubjectIds.Contains(x.SubjectId))
                                .Select(q => new Classic
                                {
                                    Id = q.Id,
                                    Question = q.Question,
                                    Description = q.Description,
                                    Answer = q.Answer
                                })
                                 .OrderBy(g => Guid.NewGuid())
                                 .Take(GetExamQuestionsCount(model.eGroup, model.eGroupFormat, model.eClassic))
                                 .ToList();

            ViewData["lessonGuid"] = GetLesson(model.eLessonGuid).Guid;
            return View(SetExamPages(test, classic, model.eGroup, model.eGroupFormat, model.eTest, model.eClassic));
        }

        //[Route("Exam/Create/Manual/{id}")]
        [HttpGet("Exam/Create/Manual/{lessonGuid}")]
        public IActionResult Manual(string lessonGuid)
        {
            return View(_context.Lessons.Where(x=>x.Delete == false 
                                            && x.Guid == GetLesson(lessonGuid).Guid)
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
                                                QuestionCount = s.QuestionPools.Where(q => q.Delete == false).Count(),
                                                TestCount = s.QuestionPools.Where(q => q.Delete == false 
                                                                                    && q.ExamFormat == "Test").Count(),
                                                ClassicCount = s.QuestionPools.Where(q => q.Delete == false
                                                                                    && q.ExamFormat == "Klasik").Count()
                                            }).ToList()

                                        }).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult GetQuestionForManual(string lessonGuid, string format, string subjectIds, int active=1)
        {
            var keyFormat = Newtonsoft.Json.JsonConvert
                        .DeserializeObject<string[]>(format).ToList();

            var keySubjectIds = Newtonsoft.Json.JsonConvert
                        .DeserializeObject<int[]>(subjectIds).ToList();


            var count = _context.QuestionPools
                                .Where(x => x.Lesson.Id == GetLesson(lessonGuid).Id
                                    && x.Delete == false
                                    && (keyFormat.Contains(x.ExamFormat) || keyFormat.Count() == 0)
                                    && (keySubjectIds.Contains(x.SubjectId) || keySubjectIds.Count() == 0))
                                .Select(q => new
                                {
                                    Id = q.Id,
                                    ExamFormat = q.ExamFormat,
                                    ExamType = q.ExamType,
                                    Question = q.Question,
                                    DescriptionJ = q.ExamFormat == "Test" ? Newtonsoft.Json.JsonConvert.DeserializeObject<Option>(q.Description) : null,
                                    Description = q.ExamFormat == "Klasik" ? q.Description : "",
                                    Answer = q.Answer,
                                    Subject = q.Subcject.Name
                                }).Count();
            count = (count % 3) == 0 ? count / 3 : ((int)(count / 3) + 1);
            if (active > count) active = 1;

            var questions = _context
                            .Lessons
                            .Where(x => x.Guid == lessonGuid && x.UserId == GetUser.Id && x.Delete == false)
                            .Select(l => l.QuestionPools
                                        .Where(r=>r.Delete == false
                                            && (keyFormat.Contains(r.ExamFormat) || keyFormat.Count() == 0)
                                            && (keySubjectIds.Contains(r.SubjectId) || keySubjectIds.Count() == 0))
                                        .Select(q => new
                                        {
                                            Id = q.Id,
                                            ExamFormat = q.ExamFormat,
                                            ExamType = q.ExamType,
                                            Question = q.Question,
                                            DescriptionJ = q.ExamFormat == "Test" ? Newtonsoft.Json.JsonConvert.DeserializeObject<Option>(q.Description) : null,
                                            Description = q.ExamFormat == "Klasik" ? q.Description : "",
                                            Answer = q.Answer,
                                            Subject = q.Subcject.Name
                                        })
                                        .Skip((active - 1) * 3)
                                        .Take(3)
                                        .ToList()
                                ).FirstOrDefault();
            

            return Json(new { questions = questions.ToList(), count = count });
        }

        //ajax
        [HttpGet]
        public IActionResult GetSubjects(string guid)
        {
            return Json(new
            {
                subjects =_context.Subjects
                                    .Where(x => x.LessonId == GetLesson(guid).Id
                                        && x.Delete == false)
                                    .Select(s=>new
                                    {
                                        value = s.Id,
                                        label = s.Name,
                                        count = s.QuestionPools.Where(r=>r.Delete == false).Count()
                                    })
            });
        }

        //ajax
        [HttpGet]
        public IActionResult GetQuestionsTypeCount(string guid, string subjectIds)
        {
            var keySubjectIds = Newtonsoft.Json.JsonConvert
                        .DeserializeObject<int[]>(subjectIds).ToList();

            var questions = (keySubjectIds.Count() == 0) 
                                    ? _context.QuestionPools
                                                .Where(x => x.LessonId == GetLesson(guid).Id 
                                                     && x.Delete == false) 
                                    : _context.QuestionPools
                                                .Where(x => x.LessonId == GetLesson(guid).Id
                                                    && x.Delete == false 
                                                    && keySubjectIds.Contains(x.Subcject.Id));

            var examTypeGroup = questions.GroupBy(q => q.ExamType)
                                            .Select(q => new
                                            {
                                                ExamType = q.Key,
                                                Count = q.Count()
                                            }).ToList();

            var examFormatGroup = questions.GroupBy(g => g.ExamFormat)
                                            .Select(s => new
                                            {
                                                ExamFormat = s.Key,
                                                Count = s.Count()
                                            }).ToList();

            return Json(new { examType = examTypeGroup, examFormat = examFormatGroup });
        }

        //ajax
        [HttpGet]
        public IActionResult GetQuestionsFomatCount(string guid, string types, string subjectIds)
        {
            var keyTypes = Newtonsoft.Json.JsonConvert
                        .DeserializeObject<string[]>(types).ToList();
            
            var keySubjectIds = Newtonsoft.Json.JsonConvert
                        .DeserializeObject<int[]>(subjectIds).ToList();
            
            if(keySubjectIds.Count() == 0)
            {
                return Json(new
                {
                    test = _context.QuestionPools.Where(x => x.Delete == false
                                                    && x.LessonId == GetLesson(guid).Id
                                                    && x.ExamFormat == "Test"
                                                    && keyTypes.Contains(x.ExamType)).Count(),

                    classic = _context.QuestionPools.Where(x => x.Delete == false
                                                    && x.LessonId == GetLesson(guid).Id
                                                    && x.ExamFormat == "Klasik"
                                                    && keyTypes.Contains(x.ExamType)).Count()
                });
            }
            else
            {
                return Json(new
                {
                    test = _context.QuestionPools.Where(x => x.Delete == false
                                                    && x.LessonId == GetLesson(guid).Id
                                                    && x.ExamFormat == "Test"
                                                    && keyTypes.Contains(x.ExamType)
                                                    && keySubjectIds.Contains(x.Subcject.Id)).Count(),

                    classic = _context.QuestionPools.Where(x => x.Delete == false
                                                    && x.LessonId == GetLesson(guid).Id
                                                    && x.ExamFormat == "Klasik"
                                                    && keyTypes.Contains(x.ExamType)
                                                    && keySubjectIds.Contains(x.Subcject.Id)).Count()
                });
            }
        }

        //ajax
        [HttpPost]
        public IActionResult SaveExamGroups(string lessonGuid, string exam , string name)
        {
            _context.Exams.Add(new Exam
            {
                LessonId = GetLesson(lessonGuid).Id,
                Questions = exam,
                Name = name,
                Title="Başlık Yok"
            });
            var result = _context.SaveChanges();
            if(result > 0)
            {
                return Json(new { error = false, message = "Başarıyla kaydedildi." });
            }
            else
            {
                return Json(new { error = true, message = "Kayıt sırasında bir hata oluştu" });
            }
        }
        
        //ajax
        [HttpPost]
        public IActionResult GetExamGroup(string examId, int examGroupIndex)
        {
            var qIds = _context.Exams
                        .Where(x => x.Id == examId 
                            && x.Lesson.UserId == GetUser.Id)
                        .Select(s =>new
                        {
                            name = s.Name,
                            exam = Newtonsoft.Json.JsonConvert
                                    .DeserializeObject<ExamGroupViewModel>(s.Questions)
                                    .QuestionGroups[examGroupIndex]
                        }).FirstOrDefault();

            var testIds = new List<string>(qIds.exam.TestGroups.Select(s => s.Id));
            //testIds.AddRange(qIds.exam.TestGroups.Select(s => s.Id));

            var classicIds = new List<string>(qIds.exam.ClassicGroups.Select(s => s.Id));
            //classicIds.AddRange(qIds.exam.ClassicGroups.Select(s => s.Id));

            var test = _context.QuestionPools
                                .Where(x => testIds.Contains(x.Id))
                                .Select(q => new Test
                                {
                                    Id = q.Id,
                                    Question = q.Question,
                                    DescriptionJ = Newtonsoft.Json.JsonConvert.DeserializeObject<Option>(q.Description),
                                    Answer = q.Answer
                                })
                                .OrderBy(o=> testIds.IndexOf(o.Id))
                                .ToList();

            var classic = _context.QuestionPools
                                .Where(x => classicIds.Contains(x.Id))
                                .Select(q => new Classic
                                {
                                    Id = q.Id,
                                    Question = q.Question,
                                    Description = q.Description,
                                    Answer = q.Answer
                                })
                                .OrderBy(o => classicIds.IndexOf(o.Id))
                                .ToList();

            return Json(new { test, classic, name = qIds.name });
        }

        #region Helpers

        // get login user property fun.
        //private ApplicationUser GetUser => _context.Users.Where(i => i.Id == _userManager.GetUserId(User)).Single();
        private ApplicationUser GetUser => _userManager.GetUserAsync(User).Result;

        // get lesson by guid function
        private Lesson GetLesson(string guid) => _context.Lessons
            .Where(x => x.Guid == guid && x.UserId == GetUser.Id && x.Delete == false).FirstOrDefault();

        //get questions count
        private int GetExamQuestionsCount(string eGroup, string eGroupFormat, int questionsCount)
        {
            int count = 0;
            if (eGroup == "GroupNo")
            {
                count = questionsCount;
            }
            else if (eGroup == "Group2")
            {
                count = eGroupFormat == "FormatSame" ? questionsCount : 2 * questionsCount;
            }
            else if (eGroup == "Group3")
            {
                count = eGroupFormat == "FormatSame" ? questionsCount : 3 * questionsCount;
            }
            else if (eGroup == "Group4")
            {
                count = eGroupFormat == "FormatSame" ? questionsCount : 4 * questionsCount;
            }
            return count;
        }

        //set Exam pages
        private List<ExamViewModel> SetExamPages
            (List<Test> test, 
            List<Classic> classic, 
            string eGroup, 
            string eGroupFormat, 
            int eTest, 
            int eClassic)
        {
            List<ExamViewModel> questions = new List<ExamViewModel>();

            if (eGroup == "GroupNo")
            {
                questions.Add(new ExamViewModel
                {
                    Group =eGroup,
                    GroupFormat =eGroupFormat,
                    Test = test,
                    Classic = classic
                });
            }
            else if (eGroup == "Group2")
            {
                if(eGroupFormat == "FormatSame")
                {
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test,
                        Classic = classic
                    });
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.OrderBy(g=> Guid.NewGuid()).ToList(),
                        Classic = classic.OrderBy(g => Guid.NewGuid()).ToList()
                    });
                }
                else if(eGroupFormat == "FormatMixed")
                {
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.Take(eTest).ToList(),
                        Classic = classic.Take(eClassic).ToList()
                    });
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.OrderBy(g => Guid.NewGuid()).Take(eTest).ToList(),
                        Classic = classic.OrderBy(g => Guid.NewGuid()).Take(eClassic).ToList()
                    });
                }
                else if(eGroupFormat == "FormatDiffernt")
                {
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.Take(eTest).ToList(),
                        Classic = classic.Take(eClassic).ToList()
                    });
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.Skip(eTest).Take(eTest).ToList(),
                        Classic = classic.Skip(eClassic).Take(eClassic).ToList()
                    });
                }
            }
            else if (eGroup == "Group3")
            {
                if (eGroupFormat == "FormatSame")
                {
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test,
                        Classic = classic
                    });
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.OrderBy(g => Guid.NewGuid()).ToList(),
                        Classic = classic.OrderBy(g => Guid.NewGuid()).ToList()
                    });
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.OrderBy(g => Guid.NewGuid()).ToList(),
                        Classic = classic.OrderBy(g => Guid.NewGuid()).ToList()
                    });
                }
                else if (eGroupFormat == "FormatMixed")
                {
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.Take(eTest).ToList(),
                        Classic = classic.Take(eClassic).ToList()
                    });
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.OrderBy(g => Guid.NewGuid()).Take(eTest).ToList(),
                        Classic = classic.OrderBy(g => Guid.NewGuid()).Take(eClassic).ToList()
                    });
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.OrderBy(g => Guid.NewGuid()).Take(eTest).ToList(),
                        Classic = classic.OrderBy(g => Guid.NewGuid()).Take(eClassic).ToList()
                    });
                }
                else if (eGroupFormat == "FormatDiffernt")
                {
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.Take(eTest).ToList(),
                        Classic = classic.Take(eClassic).ToList()
                    });
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.Skip(eTest).Take(eTest).ToList(),
                        Classic = classic.Skip(eClassic).Take(eClassic).ToList()
                    });
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.Skip(2 * eTest).Take(eTest).ToList(),
                        Classic = classic.Skip(2 * eClassic).Take(eClassic).ToList()
                    });
                }
            }
            else if (eGroup == "Group4")
            {
                if (eGroupFormat == "FormatSame")
                {
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test,
                        Classic = classic
                    });
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.OrderBy(g => Guid.NewGuid()).ToList(),
                        Classic = classic.OrderBy(g => Guid.NewGuid()).ToList()
                    });
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.OrderBy(g => Guid.NewGuid()).ToList(),
                        Classic = classic.OrderBy(g => Guid.NewGuid()).ToList()
                    });
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.OrderBy(g => Guid.NewGuid()).ToList(),
                        Classic = classic.OrderBy(g => Guid.NewGuid()).ToList()
                    });
                }
                else if (eGroupFormat == "FormatMixed")
                {
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.Take(eTest).ToList(),
                        Classic = classic.Take(eClassic).ToList()
                    });
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.OrderBy(g => Guid.NewGuid()).Take(eTest).ToList(),
                        Classic = classic.OrderBy(g => Guid.NewGuid()).Take(eClassic).ToList()
                    });
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.OrderBy(g => Guid.NewGuid()).Take(eTest).ToList(),
                        Classic = classic.OrderBy(g => Guid.NewGuid()).Take(eClassic).ToList()
                    });
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.OrderBy(g => Guid.NewGuid()).Take(eTest).ToList(),
                        Classic = classic.OrderBy(g => Guid.NewGuid()).Take(eClassic).ToList()
                    });
                }
                else if (eGroupFormat == "FormatDiffernt")
                {
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.Take(eTest).ToList(),
                        Classic = classic.Take(eClassic).ToList()
                    });
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.Skip(eTest).Take(eTest).ToList(),
                        Classic = classic.Skip(eClassic).Take(eClassic).ToList()
                    });
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.Skip(2 * eTest).Take(eTest).ToList(),
                        Classic = classic.Skip(2 * eClassic).Take(eClassic).ToList()
                    });
                    questions.Add(new ExamViewModel
                    {
                        Group = eGroup,
                        GroupFormat = eGroupFormat,
                        Test = test.Skip(3 * eTest).Take(eTest).ToList(),
                        Classic = classic.Skip(3 * eClassic).Take(eClassic).ToList()
                    });
                }
            }

            return questions;
        }

        #endregion

    }
}
