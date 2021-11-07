using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace PAX.Next.EntityFrameworkCore
{
    public static class NextDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<NextDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<NextDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}