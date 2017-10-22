using ExamService.Contracts.Repositories;
using ExamService.Contracts.UnitOfWork;
using ExamService.Entities.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExamService.DAL.Test
{
    public class RepositoryTest
    {
        [Fact]
        public void When_Get_Lessons()
        {
            var lList = new Lesson[]
            {
                new Lesson {Id=1, Name="Test Lesson Name 1",Code="TLN1",Akts=5,Hours=4,Guid=Guid.NewGuid().ToString(), UserId="UserId 1" },
                new Lesson {Id=2, Name="Test Lesson Name 2",Code="TLN2",Akts=6,Hours=3,Guid=Guid.NewGuid().ToString(), UserId="UserId 2"  },
                new Lesson {Id=3, Name="Test Lesson Name 3",Code="TLN3",Akts=7,Hours=2,Guid=Guid.NewGuid().ToString(), UserId="UserId 3"  },
                new Lesson {Id=4, Name="Test Lesson Name 4",Code="TLN4",Akts=8,Hours=1,Guid=Guid.NewGuid().ToString(), UserId="UserId 4"  }
            };

            var mockLesson = new Mock<ILessonRepository>();
            mockLesson.Setup(x => x.GetAll()).Returns(() => lList.AsQueryable());

            Assert.Equal(mockLesson.Object.GetAll().Count(), 4);
        }

        [Fact]
        public void UnitOfWork_When_Get_Lessons()
        {
            var lList = new Lesson[]
            {
                new Lesson {Id=1, Name="Test Lesson Name 1",Code="TLN1",Akts=5,Hours=4,Guid=Guid.NewGuid().ToString(), UserId="UserId 1" },
                new Lesson {Id=2, Name="Test Lesson Name 2",Code="TLN2",Akts=6,Hours=3,Guid=Guid.NewGuid().ToString(), UserId="UserId 2"  },
                new Lesson {Id=3, Name="Test Lesson Name 3",Code="TLN3",Akts=7,Hours=2,Guid=Guid.NewGuid().ToString(), UserId="UserId 3"  },
                new Lesson {Id=4, Name="Test Lesson Name 4",Code="TLN4",Akts=8,Hours=1,Guid=Guid.NewGuid().ToString(), UserId="UserId 4"  }
            };

            var mockLesson = new Mock<IUnitOfWork>();
            mockLesson.Setup(x => x.Lessons.GetAll()).Returns(() => lList.AsQueryable());

            Assert.Equal(mockLesson.Object.Lessons.GetAll().Count(), 4);
        }
    }
}
