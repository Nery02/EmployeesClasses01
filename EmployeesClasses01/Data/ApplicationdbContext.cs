using EmployeesClasses01.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeesClasses01.Data
{
    public class ApplicationdbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationdbContext()
        {
            
        }

        public ApplicationdbContext(DbContextOptions<ApplicationdbContext> options) : base(options)
        {
            
        }

        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>(entity => entity.ToTable("Employee"));
            modelBuilder.Entity<EmployeeType>(entity => entity.ToTable("EmployeeType"));
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);

        }

        

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return (await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken));
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            var now = DateTime.Now;
            foreach (var entry in entries)
            {
                if (entry.Entity is BaseEntity trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Detached:
                            break;
                        case EntityState.Unchanged:
                            break;
                        case EntityState.Deleted:
                            break;
                        case EntityState.Modified:
                            trackable.UpdatedDate = now;
                            entry.Property("CreatedDate").IsModified = false;
                            break;
                        case EntityState.Added:
                            trackable.UpdatedDate = now;
                            trackable.CreatedDate = now;
                            trackable.IsActive = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
