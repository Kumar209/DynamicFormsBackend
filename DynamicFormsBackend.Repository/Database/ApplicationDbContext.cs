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

    public virtual DbSet<DynamicFormsRole> DynamicFormsRoles { get; set; }

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
            entity.HasKey(e => e.Id).HasName("PK__AnswerMa__3214EC07B8D83B8A");

            entity.ToTable("AnswerMaster");

            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.AnswerOption).WithMany(p => p.AnswerMasters)
                .HasForeignKey(d => d.AnswerOptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AnswerMas__Answe__2ACC04F9");

            entity.HasOne(d => d.Question).WithMany(p => p.AnswerMasters)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AnswerMas__Quest__29D7E0C0");
        });

        modelBuilder.Entity<AnswerOption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AnswerOp__3214EC07DBBD0D83");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DeletedOn).HasColumnType("datetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.OptionValue)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.AnswerType).WithMany(p => p.AnswerOptions)
                .HasForeignKey(d => d.AnswerTypeId)
                .HasConstraintName("FK__AnswerOpt__Answe__26074FDC");
        });

        modelBuilder.Entity<AnswerType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AnswerTy__3214EC0733007686");

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
            entity.HasKey(e => e.Id).HasName("PK__DynamicF__3214EC070EAF3332");

            entity.ToTable("DynamicFormUser");

            entity.HasIndex(e => e.Email, "UQ__DynamicF__A9D10534AB1149A6").IsUnique();

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

            entity.HasOne(d => d.Role).WithMany(p => p.DynamicFormUsers)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__DynamicFo__RoleI__12F47B68");
        });

        modelBuilder.Entity<DynamicFormsRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DynamicF__3214EC0763CEA48E");

            entity.ToTable("DynamicFormsRole");

            entity.HasIndex(e => e.RoleName, "UQ__DynamicF__8A2B6160B2E2C9FD").IsUnique();

            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FormQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FormQues__3214EC0788D604CC");

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

            entity.HasOne(d => d.User).WithMany(p => p.FormQuestions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__FormQuest__UserI__1F5A524D");
        });

        modelBuilder.Entity<FormResponse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FormResp__3214EC07AB7D1EBB");

            entity.ToTable("FormResponse");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DeletedOn).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

            entity.HasOne(d => d.AnswerMaster).WithMany(p => p.FormResponses)
                .HasForeignKey(d => d.AnswerMasterId)
                .HasConstraintName("FK__FormRespo__Answe__34556F33");

            entity.HasOne(d => d.Form).WithMany(p => p.FormResponses)
                .HasForeignKey(d => d.FormId)
                .HasConstraintName("FK__FormRespo__FormI__33614AFA");
        });

        modelBuilder.Entity<QuestionSectionMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC072C346CA5");

            entity.ToTable("QuestionSectionMapping");

            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionSectionMappings)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__QuestionS__Quest__2E9C95DD");

            entity.HasOne(d => d.Section).WithMany(p => p.QuestionSectionMappings)
                .HasForeignKey(d => d.SectionId)
                .HasConstraintName("FK__QuestionS__Secti__2F90BA16");
        });

        modelBuilder.Entity<SourceTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SourceTe__3214EC073360D80E");

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
                .HasConstraintName("FK__SourceTem__UserI__17B93085");
        });

        modelBuilder.Entity<TemplateSection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Template__3214EC079E329349");

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
                .HasConstraintName("FK__TemplateS__FormI__1B89C169");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
