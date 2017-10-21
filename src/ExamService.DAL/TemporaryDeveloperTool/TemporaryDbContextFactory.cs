﻿using ExamService.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.DAL.TemporaryDeveloperTool
{
    public class TemporaryDbContextFactory : IDbContextFactory<DataContext>
    {
        public DataContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DemoDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new DataContext(builder.Options);
        }
    }
}
