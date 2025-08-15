using ConfigManagerStend.Domain;
using ConfigManagerStend.Domain.Predefineds;

namespace ConfigManagerStend.Infrastructure
{
    internal static class DatabaseInitializer
    {
        internal static void InitializeDatabase()
        {
            using (var context = new AppDbContext())
            {
                context.Database.EnsureCreated();

                if(!context.ConfigStatuses.Any())
                {
                    context.AddRange(new PdConfigStatus().ConfigStatuses);
                    context.SaveChanges();
                }
            }
        }
    }
}
