using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace box.infrastructure.Data.Entities;

public partial class BoxContext : DbContext
{
    public const string ConnectionStringName = "ConnectionString";
    public BoxContext()
    {
    }

    public BoxContext(DbContextOptions<BoxContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TFile> TFiles { get; set; }

    public virtual DbSet<TProject> TProjects { get; set; }

    public class BoxContextFactory : IDesignTimeDbContextFactory<BoxContext>
    {
        public BoxContext CreateDbContext(string[] p_Args)
        {
            IConfigurationRoot v_Configuration = new ConfigurationBuilder()
            .AddUserSecrets<BoxContextFactory>() //user usersecret functionnality
            .Build();

            //get connection string in user secret
            string v_Connstr = v_Configuration?.GetConnectionString(ConnectionStringName);
            if (string.IsNullOrWhiteSpace(v_Connstr))
            {
                throw new InvalidOperationException($"Could not find a connection string named '({ConnectionStringName})'");
            }

            DbContextOptionsBuilder<BoxContext> v_Builder = new DbContextOptionsBuilder<BoxContext>();
            v_Builder.UseSqlServer(v_Connstr);
            return new BoxContext(v_Builder.Options);
        }
    }
}