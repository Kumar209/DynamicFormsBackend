using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DynamicFormsBackend.Models.Entities;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AnswerMaster> AnswerMasters { get; set; }

    public virtual DbSet<AnswerOption> AnswerOptions { get; set; }

    public virtual DbSet<AnswerType> AnswerTypes { get; set; }

    public virtual DbSet<DynamicFormUser> DynamicFormUsers { get; set; }

    public virtual DbSet<FormQuestion> FormQuestions { get; set; }

    public virtual DbSet<FormResponse> FormResponses { get; set; }

    public virtual DbSet<QuestionSectionMapping> QuestionSectionMappings { get; set; }

    public virtual DbSet<SourceTemplate> SourceTemplates { get; set; }

    public virtual DbSet<TemplateSection> TemplateSections { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnswerMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AnswerMa__3214EC07782CB15F");

            entity.ToTable("AnswerMaster");

            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.AnswerOption).WithMany(p => p.AnswerMasters)
                .HasForeignKey(d => d.AnswerOptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AnswerMas__Answe__062DE679");

            entity.HasOne(d => d.Question).WithMany(p => p.AnswerMasters)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AnswerMas__Quest__0539C240");

            entity.HasOne(d => d.AnswerOption).WithMany(p => p.AnswerMasters)
          .HasForeignKey(d => d.AnswerOptionId)
          .OnDelete(DeleteBehavior.Cascade); // Add cascading delete

            entity.HasOne(d => d.Question).WithMany(p => p.AnswerMasters)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.Cascade); // Add cascading delet
        });

        modelBuilder.Entity<AnswerOption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AnswerOp__3214EC0754051D62");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DeletedOn).HasColumnType("datetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.OptionValue)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.AnswerType).WithMany(p => p.AnswerOptions)
                .HasForeignKey(d => d.AnswerTypeId)
                .HasConstraintName("FK__AnswerOpt__Answe__0169315C");

            entity.HasOne(d => d.AnswerType).WithMany(p => p.AnswerOptions)
        .HasForeignKey(d => d.AnswerTypeId)
        .OnDelete(DeleteBehavior.Cascade); // Add cascading delete
        });

        modelBuilder.Entity<AnswerType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AnswerTy__3214EC07F3ECB78E");

            entity.ToTable("AnswerType");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DeletedOn).HasColumnType("datetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.TypeName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DynamicFormUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DynamicF__3214EC074109FBA2");

            entity.ToTable("DynamicFormUser");

            entity.HasIndex(e => e.Email, "UQ__DynamicF__A9D10534BD9BCB6E").IsUnique();

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedOn).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FormQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FormQues__3214EC0797B235E3");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Constraint)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ConstraintValue)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DataType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeletedOn).HasColumnType("datetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Question)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Size)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WarningMessage)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FormResponse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FormResp__3214EC074C9AFFC7");

            entity.ToTable("FormResponse");

            entity.HasIndex(e => e.Email, "UQ__FormResp__A9D10534F41BC516").IsUnique();

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DeletedOn).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

            entity.HasOne(d => d.AnswerMaster).WithMany(p => p.FormResponses)
                .HasForeignKey(d => d.AnswerMasterId)
                .HasConstraintName("FK__FormRespo__Answe__0FB750B3");

            entity.HasOne(d => d.Form).WithMany(p => p.FormResponses)
                .HasForeignKey(d => d.FormId)
                .HasConstraintName("FK__FormRespo__FormI__0EC32C7A");
        });

        modelBuilder.Entity<QuestionSectionMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC07DD0CC560");

            entity.ToTable("QuestionSectionMapping");

            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionSectionMappings)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__QuestionS__Quest__147C05D0");

            entity.HasOne(d => d.Section).WithMany(p => p.QuestionSectionMappings)
                .HasForeignKey(d => d.SectionId)
                .HasConstraintName("FK__QuestionS__Secti__15702A09");
        });

        modelBuilder.Entity<SourceTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SourceTe__3214EC07A311A004");

            entity.ToTable("SourceTemplate");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedOn).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FormName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.SourceTemplates)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SourceTem__UserI__740F363E");
        });

        modelBuilder.Entity<TemplateSection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Template__3214EC079E7DC252");

            entity.ToTable("TemplateSection");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DeletedOn).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.SectionName)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Form).WithMany(p => p.TemplateSections)
                .HasForeignKey(d => d.FormId)
                .HasConstraintName("FK__TemplateS__FormI__77DFC722");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
