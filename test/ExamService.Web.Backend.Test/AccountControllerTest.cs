﻿using ExamService.Contracts.Services;
using ExamService.DAL.Data;
using ExamService.Entities.Models;
using ExamService.Service;
using ExamService.Web.Backend.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace ExamService.Web.Backend.Test
{
    public class AccountControllerTest
    {
        private readonly IServiceProvider _serviceProvider;

        public AccountControllerTest()
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
        public async Task Index_ReturnsViewBagMessagesExpected()
        {
            // Arrange
            var userId = "TestUserA";
            var phone = "abcdefg";
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };

            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var userManagerResult = await userManager.CreateAsync(
                new ApplicationUser { Id = userId, UserName = "Test", TwoFactorEnabled = true, PhoneNumber = phone },
                "Pass@word1");
            Assert.True(userManagerResult.Succeeded);

            var signInManager = _serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();

            var httpContext = _serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims));

            var emailSender = _serviceProvider.GetRequiredService<IEmailSender>();
            var smsSender = _serviceProvider.GetRequiredService<ISmsSender>();
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();//.CreateLogger<ManageController>()

            var controller = new AccountController(userManager, signInManager, loggerFactory);
            controller.ControllerContext.HttpContext = httpContext;
            //loggerFactory.CreateLogger<ManageController>();

            // Act
            var result = controller.Login();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);

            Assert.Empty(controller.ViewBag.StatusMessage);

            Assert.NotNull(viewResult.ViewData);
            //var model = Assert.IsType<IndexViewModel>(viewResult.ViewData.Model);
            //Assert.True(model.TwoFactor);
            //Assert.Equal(phone, model.PhoneNumber);
            //Assert.True(model.HasPassword);
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
