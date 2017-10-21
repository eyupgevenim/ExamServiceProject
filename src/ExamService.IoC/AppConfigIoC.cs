using ExamService.Contracts.Repositories;
using ExamService.Contracts.Services;
using ExamService.Contracts.UnitOfWork;
using ExamService.DAL.Data;
using ExamService.DAL.Repository;
using ExamService.DAL.UnitOfWork;
using ExamService.Entities.Models;
using ExamService.Service;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.IoC
{
    public static class AppConfigIoC
    {
        public static void Run(IServiceCollection services)
        {

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();
            
            //DI
            services.AddTransient<IEmailSender, AuthMessageSender>();

            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<IQuestionPoolRepository, QuestionPoolRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //services.AddSingleton<IUserRepository, UserRepository>
        }
    }
}
