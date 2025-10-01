namespace ConfigManagerStend.Infrastructure.Constants
{
    public class ConfigDbOpt
    {
        public class DbOpt
        {
            public int OrderId { get; set; }
            public string Opt { get; set; } = string.Empty;
        }

        public DbOpt Recreate = new() { OrderId = 1, Opt = "Recreate" };
        public DbOpt No = new() {OrderId = 2, Opt = "NO" };


        public List<DbOpt> All;

        public ConfigDbOpt()
        {
            All = new() { Recreate, No };
        }
    }
}
