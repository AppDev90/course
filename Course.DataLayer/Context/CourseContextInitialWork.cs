
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Course.DataLayer.Context
{
    public class CourseContextInitialWork
    {
        public static void Migrate(CourseDbContext courseDbContext, ILogger logger)
        {
            try
            {
                courseDbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                logger.LogError("StoreContext Migration Error!" + ex.Message);
            }
        }
        public static void SeedData(CourseDbContext courseDbContext, ILogger logger)
        {
            if(!courseDbContext.Users.Any())
            {

            }
        }
    }
}
