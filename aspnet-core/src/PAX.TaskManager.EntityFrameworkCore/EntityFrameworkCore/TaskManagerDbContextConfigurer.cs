using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace PAX.TaskManager.EntityFrameworkCore
{
    public static class TaskManagerDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<TaskManagerDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<TaskManagerDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}