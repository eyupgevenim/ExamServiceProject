using ExamService.Contracts.Repositories;
using ExamService.Contracts.Services;
using ExamService.Contracts.UnitOfWork;
using ExamService.DAL.Data;
using ExamService.Entities.Models;
using ExamService.Service;
using ExamService.Web.Backend.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExamService.Web.Backend.Test
{
    public class LessonControllerTest
    {

        private readonly IServiceProvider _serviceProvider;

        public LessonControllerTest()
        {
            var efServiceProvider = new ServiceCollection()
                                        .AddEntityFrameworkInMemoryDatabase()
                                        .BuildServiceProvider();

            var services = new ServiceCollection();
            services.AddOptions();
            services.AddDbContext<DataContext>(b => b.UseInMemoryDatabase()
                                                    .UseInternalServiceProvider(efServiceProvider));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<DataContext>();

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddLogging();
            services.AddOptions();

            // IHttpContextAccessor is required for SignInManager, and UserManager
            var context = new DefaultHttpContext();
            context.Features.Set<IHttpAuthenticationFeature>(
                new HttpAuthenticationFeature()
                {
                    Handler = new TestAuthHandler()
                });

            services.AddSingleton<IHttpContextAccessor>(
                new HttpContextAccessor()
                {
                    HttpContext = context,
                });

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void When_Get_List_Lessons()
        {
            var lList = new Lesson[]
            {
                new Lesson {Id=1, Name="Test Lesson Name 1",Code="TLN1",Akts=5,Hours=4,Guid=Guid.NewGuid().ToString(), UserId="UserId 1" },
                new Lesson {Id=2, Name="Test Lesson Name 2",Code="TLN2",Akts=6,Hours=3,Guid=Guid.NewGuid().ToString(), UserId="UserId 2"  },
                new Lesson {Id=3, Name="Test Lesson Name 3",Code="TLN3",Akts=7,Hours=2,Guid=Guid.NewGuid().ToString(), UserId="UserId 3"  },
                new Lesson {Id=4, Name="Test Lesson Name 4",Code="TLN4",Akts=8,Hours=1,Guid=Guid.NewGuid().ToString(), UserId="UserId 4"  }
            };

            var mockContext = new Mock<IUnitOfWork>();
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            mockContext.Setup(x => x.Lessons.GetAll()).Returns(() => lList.AsQueryable());
            var lessonController = new LessonController(mockContext.Object,userManager);
            var lessonList = (List<Lesson>)lessonController.Index().Model;

            Assert.Equal(lessonList.Count, 4);
        }



        private class TestAuthHandler : IAuthenticationHandler
        {
            public void Authenticate(AuthenticateContext context)
            {
                context.NotAuthenticated();
            }

            public Task AuthenticateAsync(AuthenticateContext context)
            {
                context.NotAuthenticated();
                return Task.FromResult(0);
            }

            public Task ChallengeAsync(ChallengeContext context)
            {
                throw new NotImplementedException();
            }

            public void GetDescriptions(DescribeSchemesContext context)
            {
                throw new NotImplementedException();
            }

            public Task SignInAsync(SignInContext context)
            {
                throw new NotImplementedException();
            }

            public Task SignOutAsync(SignOutContext context)
            {
                throw new NotImplementedException();
            }
        }
    }
}
