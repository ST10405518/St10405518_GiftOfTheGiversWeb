using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using St10405518_GiftOfTheGiversWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace St10405518_GiftOfTheGiversWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Models.MoneyDonation> MoneyDonation { get; set; }
        public DbSet<Models.GoodsDonation> GoodsDonation { get; set; }
        public DbSet<Models.Disaster> Disaster { get; set; }
        public DbSet<Models.Money> Money { get; set; }
        public DbSet<MoneyAllocation> MoneyAllocation { get; set; }
        public DbSet<GoodsAllocation> GoodsAllocation { get; set; }
        public DbSet<GoodsPurchase> GoodsPurchase { get; set; }
        public DbSet<GoodsInventory> GoodsInventory { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<VolunteerTask> VolunteerTasks { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fix precision for decimal properties
            modelBuilder.Entity<GoodsPurchase>()
                .Property(g => g.GoodsPurchasePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<GoodsPurchase>()
                .Property(g => g.GoodsTotalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Money>()
                .Property(m => m.TotalMoney)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Money>()
                .Property(m => m.RemainingMoney)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MoneyAllocation>()
                .Property(m => m.AllocationAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MoneyDonation>()
                .Property(m => m.AMOUNT)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TaskAssignment>()
                .Property(t => t.HoursWorked)
                .HasPrecision(18, 2);

            // Configure date-only columns
            modelBuilder.Entity<Disaster>()
                .Property(d => d.STARTDATE)
                .HasColumnType("date");

            modelBuilder.Entity<Disaster>()
                .Property(d => d.ENDDATE)
                .HasColumnType("date");

            modelBuilder.Entity<MoneyDonation>()
                .Property(d => d.DATE)
                .HasColumnType("date");

            modelBuilder.Entity<GoodsDonation>()
                .Property(d => d.DATE)
                .HasColumnType("date");

            modelBuilder.Entity<VolunteerTask>()
                .Property(t => t.StartDate)
                .HasColumnType("datetime");

            modelBuilder.Entity<VolunteerTask>()
                .Property(t => t.EndDate)
                .HasColumnType("datetime");

            // Explicitly fix typo: map to Disaster table
            modelBuilder.Entity<Disaster>().ToTable("Disaster");

            // Configure relationships for volunteer management - FIXED
            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.Volunteer)
                .WithMany(v => v.Tasks)  // This now correctly points to ICollection<TaskAssignment>
                .HasForeignKey(ta => ta.VolunteerID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.VolunteerTask)
                .WithMany(vt => vt.Assignments)  // This now correctly points to ICollection<TaskAssignment>
                .HasForeignKey(ta => ta.TaskID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}