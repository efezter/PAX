using PAX.TaskManager.EntityFrameworkCore;

namespace PAX.TaskManager.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly TaskManagerDbContext _context;

        public InitialHostDbBuilder(TaskManagerDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
