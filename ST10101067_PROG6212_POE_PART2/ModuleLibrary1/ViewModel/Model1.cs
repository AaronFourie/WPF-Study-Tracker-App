using ModuleLibrary1.ViewModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace PROG_2B_POE_PART_1
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=con")
        {
        }

        public virtual DbSet<MODULE> MODULEs { get; set; }
        public virtual DbSet<SEMESTER> SEMESTERs { get; set; }
        public virtual DbSet<STUDENT> STUDENTs { get; set; }
        public virtual DbSet<STUDY> STUDies { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MODULE>()
                .Property(e => e.MODULE_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<MODULE>()
                .Property(e => e.MODULE_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<MODULE>()
                .Property(e => e.MODULE_CREDITS)
                .HasPrecision(18, 0);

            modelBuilder.Entity<MODULE>()
                .Property(e => e.CLASS_HOURS_PER_WEEK)
                .HasPrecision(18, 0);

            modelBuilder.Entity<MODULE>()
                .Property(e => e.STUDENT_USERNAME)
                .IsUnicode(false);

            modelBuilder.Entity<SEMESTER>()
                .Property(e => e.STUDENT_USERNAME)
                .IsUnicode(false);

            modelBuilder.Entity<STUDENT>()
                .Property(e => e.STUDENT_USERNAME)
                .IsUnicode(false);

            modelBuilder.Entity<STUDENT>()
                .Property(e => e.STUDENT_PASSWORD)
                .IsUnicode(false);

            modelBuilder.Entity<STUDENT>()
                .HasMany(e => e.MODULEs)
                .WithRequired(e => e.STUDENT)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<STUDENT>()
                .HasMany(e => e.SEMESTERs)
                .WithRequired(e => e.STUDENT)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<STUDENT>()
                .HasMany(e => e.STUDies)
                .WithRequired(e => e.STUDENT)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<STUDY>()
                .Property(e => e.SELF_STUDY_HOURS_PER_WEEK)
                .HasPrecision(18, 0);

            modelBuilder.Entity<STUDY>()
                .Property(e => e.HOURS_STUDIED)
                .HasPrecision(18, 0);

            modelBuilder.Entity<STUDY>()
                .Property(e => e.STUDY_HOURS_REMAINDING)
                .HasPrecision(18, 0);

            modelBuilder.Entity<STUDY>()
                .Property(e => e.MODULE_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<STUDY>()
                .Property(e => e.STUDENT_USERNAME)
                .IsUnicode(false);
        }
    }
}
