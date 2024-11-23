using Microsoft.EntityFrameworkCore;
using TMAWebAPI.Models;

namespace TMAWebAPI.Models
{
    public partial class TMADbContext: DbContext
    {
        public TMADbContext()
        {

        }

        public TMADbContext(DbContextOptions<TMADbContext> options)
       : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<TaskTbl> TaskTbls { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("Data Source=iamanu\\sqlexpress;Initial Catalog=TMADB;Integrated Security=True;TrustServerCertificate=True");

    }
}


 
