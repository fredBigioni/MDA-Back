using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebApi.Entities;
using WebApi.Models;
using WebApi.Views;

namespace WebApi.Helpers
{
    public class DataContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public virtual DbSet<BusinessUnit> BusinessUnits { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<CustomMarket> CustomMarkets { get; set; }
        public virtual DbSet<CustomMarketDetail> CustomMarketDetails { get; set; }
        public virtual DbSet<CustomMarketGroup> CustomMarketGroups { get; set; }
        public virtual DbSet<Drug> Drugs { get; set; }
        public virtual DbSet<DrugGroup> DrugGroups { get; set; }
        public virtual DbSet<DrugGroupDetail> DrugGroupDetails { get; set; }
        public virtual DbSet<Laboratory> Laboratories { get; set; }
        public virtual DbSet<LaboratoryGroup> LaboratoryGroups { get; set; }
        public virtual DbSet<LaboratoryGroupDetail> LaboratoryGroupDetails { get; set; }
        public virtual DbSet<Line> Lines { get; set; }
        public virtual DbSet<LineGroup> LineGroups { get; set; }
        public virtual DbSet<MasterEntityLog> MasterEntityLogs { get; set; }
        public virtual DbSet<PharmaceuticalForm> PharmaceuticalForms { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductGroup> ProductGroups { get; set; }
        public virtual DbSet<ProductGroupDetail> ProductGroupDetails { get; set; }
        public virtual DbSet<ProductMarket> ProductMarkets { get; set; }
        public virtual DbSet<ProductPresentation> ProductPresentations { get; set; }
        public virtual DbSet<ProductPresentationGroup> ProductPresentationGroups { get; set; }
        public virtual DbSet<ProductPresentationGroupDetail> ProductPresentationGroupDetails { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<TherapeuticalClass> TherapeuticalClasses { get; set; }
        public virtual DbSet<UserPermission> UserPermissions { get; set; }
        public virtual DbSet<JsonResult> JsonResults { get; set; }
        public virtual DbSet<CustomMarketTree> CustomMarketTrees { get; set; }
        public virtual DbSet<DrugComponent> DrugComponents { get; set; }
        public virtual DbSet<LaboratoryComponent> LaboratoryComponents { get; set; }
        public virtual DbSet<PharmaceuticalFormComponent> PharmaceuticalFormComponents { get; set; }
        public virtual DbSet<ProductComponent> ProductComponents { get; set; }
        public virtual DbSet<ProductComponentByDrug> ProductComponentByDrugs { get; set; }
        public virtual DbSet<ProductComponentByLaboratory> ProductComponentByLaboratories { get; set; }
        public virtual DbSet<ProductComponentByTherapeuticalClass> ProductComponentByTherapeuticalClasses { get; set; }
        public virtual DbSet<ProductPresentationComponent> ProductPresentationComponents { get; set; }
        public virtual DbSet<Period> Periods { get; set; }
        public virtual DbSet<CustomMarketActualDefinition> CustomMarketActualDefinitions { get; set; }
        public virtual DbSet<CustomMarketPreview> CustomMarketPreviews { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<CustomMarketVersionHistoric> CustomMarketVersionHistorics { get; set; }
        public virtual DbSet<CustomMarketResultVersionHistoric> CustomMarketResultVersionHistorics { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public virtual async Task<int> SP_SignMarket(SignMarketModel sign)
        {
            return Database.ExecuteSqlRaw($"EXEC SignMarket @customMarketCode = {sign.CustomMarketCode} , @signedUser = {sign.SignedUser}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Period>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("period");

                entity.Property(e => e.Code)
                    .ValueGeneratedNever()
                    .HasColumnName("code");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Month).HasColumnName("month");

                entity.Property(e => e.Year).HasColumnName("year");
            });

            modelBuilder.Entity<BusinessUnit>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("businessUnit", "dbo");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("description");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("class", "dbo");

                entity.Property(e => e.Code)
                    .ValueGeneratedNever()
                    .HasColumnName("code");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Imscode)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("IMSCode");
            });

            modelBuilder.Entity<CustomMarket>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("customMarket", "dbo");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.ControlPanel).HasColumnName("controlPanel");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.DrugReport).HasColumnName("drugReport");

                entity.Property(e => e.Footer)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("footer");

                entity.Property(e => e.Header)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("header");

                entity.Property(e => e.IsOtc).HasColumnName("isOTC");

                entity.Property(e => e.LabReport).HasColumnName("labReport");

                entity.Property(e => e.LineCode).HasColumnName("lineCode");

                entity.Property(e => e.MarketFilter).HasColumnName("marketFilter");

                entity.Property(e => e.Order).HasColumnName("order");

                entity.Property(e => e.AdUser).HasColumnName("adUser");

                entity.Property(e => e.ResponsibleName).HasColumnName("responsibleName");

                entity.Property(e => e.ResponsibleLastName).HasColumnName("responsibleLastName");

                entity.Property(e => e.ProductReport)
                    .IsRequired()
                    .HasColumnName("productReport")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Stamp)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("stamp");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("status")
                    .IsFixedLength(true);

                entity.Property(e => e.Tcreport).HasColumnName("TCReport");

                entity.Property(e => e.TestMarket).HasColumnName("testMarket");

                entity.HasOne(d => d.Line)
                    .WithMany(p => p.CustomMarkets)
                    .HasForeignKey(d => d.LineCode)
                    .HasConstraintName("FK_customMarket_line");

                entity.Property(e => e.MarketClass).HasColumnName("marketClass");

                entity.Property(e => e.MarketReference).HasColumnName("marketReference");
            });

            modelBuilder.Entity<CustomMarketDetail>(entity =>
            {
                entity.HasKey(e => new { e.CustomMarketCode, e.Order });

                entity.ToTable("customMarketDetail", "dbo");

                entity.HasIndex(e => e.ProductPresentationCode, "IX_customMarketDetail")
                    .HasFillFactor((byte)90);

                entity.HasIndex(e => e.ProductPresentationGroupCode, "IX_customMarketDetail_1")
                    .HasFillFactor((byte)90);

                entity.HasIndex(e => e.ProductCode, "IX_customMarketDetail_2")
                    .HasFillFactor((byte)90);

                entity.HasIndex(e => e.TherapeuticalClassCode, "IX_customMarketDetail_3")
                    .HasFillFactor((byte)90);

                entity.HasIndex(e => new { e.ProductPresentationCode, e.TherapeuticalClassCode, e.LaboratoryCode, e.ProductPresentationGroupCode, e.LaboratoryGroupCode, e.DrugCode }, "IX_customMarketDetail_4")
                    .HasFillFactor((byte)90);

                entity.Property(e => e.CustomMarketCode).HasColumnName("customMarketCode");

                entity.Property(e => e.Order).HasColumnName("order");

                entity.Property(e => e.ClassCode).HasColumnName("classCode");

                entity.Property(e => e.CustomMarketGroupCode).HasColumnName("customMarketGroupCode");

                entity.Property(e => e.DetailCustomMarketCode).HasColumnName("detailCustomMarketCode");

                entity.Property(e => e.DrugCode).HasColumnName("drugCode");

                entity.Property(e => e.DrugGroupCode).HasColumnName("drugGroupCode");

                entity.Property(e => e.EnsureVisible).HasColumnName("ensureVisible");

                entity.Property(e => e.Expand).HasColumnName("expand");

                entity.Property(e => e.Graphs).HasColumnName("graphs");

                entity.Property(e => e.Intemodifier)
                    .HasColumnName("INTEmodifier")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Intemodifier2).HasColumnName("INTEmodifier2");

                entity.Property(e => e.ItemBrand).HasColumnName("itemBrand");

                entity.Property(e => e.ItemCondition)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("itemCondition");

                entity.Property(e => e.LaboratoryCode).HasColumnName("laboratoryCode");

                entity.Property(e => e.LaboratoryGroupCode).HasColumnName("laboratoryGroupCode");

                entity.Property(e => e.Modifier)
                    .HasColumnName("modifier")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Modifier2).HasColumnName("modifier2");

                entity.Property(e => e.OwnProductsReport).HasColumnName("ownProductsReport");

                entity.Property(e => e.Pattern)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("pattern");

                entity.Property(e => e.PharmaceuticalFormCode).HasColumnName("pharmaceuticalFormCode");

                entity.Property(e => e.ProductCode).HasColumnName("productCode");

                entity.Property(e => e.ProductGroupCode).HasColumnName("productGroupCode");

                entity.Property(e => e.ProductPresentationCode).HasColumnName("productPresentationCode");

                entity.Property(e => e.ProductPresentationGroupCode).HasColumnName("productPresentationGroupCode");

                entity.Property(e => e.ProductTypeCode).HasColumnName("productTypeCode");

                entity.Property(e => e.RegExPattern)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Resume).HasColumnName("resume");

                entity.Property(e => e.TherapeuticalClassCode).HasColumnName("therapeuticalClassCode");

                entity.HasOne(d => d.CustomMarket)
                    .WithMany(p => p.CustomMarketDetails)
                    .HasForeignKey(d => d.CustomMarketCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_customMarketDetail_customMarket");

                entity.HasOne(d => d.Drug)
                    .WithMany(p => p.CustomMarketDetails)
                    .HasForeignKey(d => d.DrugCode)
                    .HasConstraintName("FK_customMarketDetail_drug");

                entity.HasOne(d => d.DrugGroup)
                    .WithMany(p => p.CustomMarketDetails)
                    .HasForeignKey(d => d.DrugGroupCode)
                    .HasConstraintName("FK_customMarketDetail_drugGroup");

                entity.HasOne(d => d.Laboratory)
                    .WithMany(p => p.CustomMarketDetails)
                    .HasForeignKey(d => d.LaboratoryCode)
                    .HasConstraintName("FK_customMarketDetail_laboratory");

                entity.HasOne(d => d.LaboratoryGroup)
                    .WithMany(p => p.CustomMarketDetails)
                    .HasForeignKey(d => d.LaboratoryGroupCode)
                    .HasConstraintName("FK_customMarketDetail_laboratoryGroup");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CustomMarketDetails)
                    .HasForeignKey(d => d.ProductCode)
                    .HasConstraintName("FK_customMarketDetail_product");

                entity.HasOne(d => d.ProductPresentation)
                    .WithMany(p => p.CustomMarketDetails)
                    .HasForeignKey(d => d.ProductPresentationCode)
                    .HasConstraintName("FK_customMarketDetail_productPresentation");

                entity.HasOne(d => d.ProductPresentationGroup)
                    .WithMany(p => p.CustomMarketDetails)
                    .HasForeignKey(d => d.ProductPresentationGroupCode)
                    .HasConstraintName("FK_customMarketDetail_productPresentationGroup");

                entity.HasOne(d => d.TherapeuticalClass)
                    .WithMany(p => p.CustomMarketDetails)
                    .HasForeignKey(d => d.TherapeuticalClassCode)
                    .HasConstraintName("FK_customMarketDetail_therapeuticalClass");

                entity.HasOne(d => d.PharmaceuticalForm)
                    .WithMany(p => p.CustomMarketDetails)
                    .HasForeignKey(d => d.PharmaceuticalFormCode)
                    .HasConstraintName("FK_customMarketDetail_pharmaceuticalForm");

                entity.HasOne(d => d.ProductGroup)
                    .WithMany(p => p.CustomMarketDetails)
                    .HasForeignKey(d => d.ProductGroupCode)
                    .HasConstraintName("FK_customMarketDetail_productGroup");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.CustomMarketDetails)
                    .HasForeignKey(d => d.ClassCode)
                    .HasConstraintName("FK_customMarketDetail_class");

                entity.HasOne(d => d.CustomMarketGroup)
                    .WithMany(p => p.CustomMarketDetails)
                    .HasForeignKey(d => d.CustomMarketGroupCode)
                    .HasConstraintName("FK_customMarketDetail_customMarketGroup");

                entity.HasOne(d => d.DetailCustomMarket)
                    .WithMany(p => p.DetailCustomMarketDetails)
                    .HasForeignKey(d => d.DetailCustomMarketCode)
                    .HasConstraintName("FK_customMarketDetail_detailCustomMarket");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.CustomMarketDetails)
                    .HasForeignKey(d => d.ProductTypeCode)
                    .HasConstraintName("FK_customMarketDetail_productType");
            });

            modelBuilder.Entity<CustomMarketGroup>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("CustomMarketGroup", "dbo");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.CustomMarketCode).HasColumnName("customMarketCode");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("description")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.GroupCondition)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("groupCondition")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.HasOne(d => d.CustomMarket)
                    .WithMany(p => p.CustomMarketGroups)
                    .HasForeignKey(d => d.CustomMarketCode)
                    .HasConstraintName("FK_customMarketGroup_customMarket");
            });

            modelBuilder.Entity<Drug>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("drug", "dbo");

                entity.Property(e => e.Code)
                    .ValueGeneratedNever()
                    .HasColumnName("code");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Imscode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IMSCode");
            });

            modelBuilder.Entity<DrugGroup>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("drugGroup", "dbo");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description");
            });

            modelBuilder.Entity<DrugGroupDetail>(entity =>
            {
                entity.HasKey(e => new { e.DrugGroupCode, e.DrugCode });

                entity.ToTable("drugGroupDetail", "dbo");

                entity.Property(e => e.DrugGroupCode).HasColumnName("drugGroupCode");

                entity.Property(e => e.DrugCode).HasColumnName("drugCode");

                entity.HasOne(d => d.Drug)
                    .WithMany(p => p.DrugGroupDetails)
                    .HasForeignKey(d => d.DrugCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_drugGroupDetail_drug");

                entity.HasOne(d => d.DrugGroup)
                    .WithMany(p => p.DrugGroupDetails)
                    .HasForeignKey(d => d.DrugGroupCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_drugGroupDetail_drugGroup");
            });

            modelBuilder.Entity<Laboratory>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("laboratory", "dbo");

                entity.Property(e => e.Code)
                    .ValueGeneratedNever()
                    .HasColumnName("code");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Imscode)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("IMSCode");

                entity.Property(e => e.OwnLab).HasColumnName("ownLab");
            });

            modelBuilder.Entity<LaboratoryGroup>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("laboratoryGroup", "dbo");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Class)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('T')");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("description");
            });

            modelBuilder.Entity<LaboratoryGroupDetail>(entity =>
            {
                entity.HasKey(e => new { e.LaboratoryGroupCode, e.LaboratoryCode });

                entity.ToTable("laboratoryGroupDetail", "dbo");

                entity.Property(e => e.LaboratoryGroupCode).HasColumnName("laboratoryGroupCode");

                entity.Property(e => e.LaboratoryCode).HasColumnName("laboratoryCode");

                entity.HasOne(d => d.Laboratory)
                    .WithMany(p => p.LaboratoryGroupDetails)
                    .HasForeignKey(d => d.LaboratoryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_laboratoryGroupDetail_laboratory");

                entity.HasOne(d => d.LaboratoryGroup)
                    .WithMany(p => p.LaboratoryGroupDetails)
                    .HasForeignKey(d => d.LaboratoryGroupCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_laboratoryGroupDetail_laboratoryGroup");
            });

            modelBuilder.Entity<Line>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("line", "dbo");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.DrugReportFooter)
                    .HasColumnType("text")
                    .HasColumnName("drugReportFooter");

                entity.Property(e => e.DrugReportHeader)
                    .HasColumnType("text")
                    .HasColumnName("drugReportHeader");

                entity.Property(e => e.LaboratoryReportFooter)
                    .HasColumnType("text")
                    .HasColumnName("laboratoryReportFooter");

                entity.Property(e => e.LaboratoryReportHeader)
                    .HasColumnType("text")
                    .HasColumnName("laboratoryReportHeader");

                entity.Property(e => e.LineGroupCode).HasColumnName("lineGroupCode");

                entity.Property(e => e.Stamp)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("stamp");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("status")
                    .IsFixedLength(true);

                entity.HasOne(d => d.LineGroup)
                    .WithMany(p => p.Lines)
                    .HasForeignKey(d => d.LineGroupCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_line_lineGroup");
            });

            modelBuilder.Entity<LineGroup>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("lineGroup", "dbo");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("description");
            });

            modelBuilder.Entity<MasterEntityLog>(entity =>
            {
                entity.ToTable("masterEntityLog", "dbo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Entity)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("entity");

                entity.Property(e => e.LogJson)
                    .IsUnicode(false)
                    .HasColumnName("logJson");

                entity.Property(e => e.UserId).HasColumnName("userId");
            });

            modelBuilder.Entity<PharmaceuticalForm>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("pharmaceuticalForm", "dbo");

                entity.Property(e => e.Code)
                    .ValueGeneratedNever()
                    .HasColumnName("code");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Imscode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IMSCode");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("product", "dbo");

                entity.HasIndex(e => e.LaboratoryCode, "IX_product_1")
                    .HasFillFactor((byte)90);

                entity.HasIndex(e => new { e.LaboratoryCode, e.Code }, "IX_product_2")
                    .HasFillFactor((byte)90);

                entity.Property(e => e.Code)
                    .ValueGeneratedNever()
                    .HasColumnName("code");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Imscode)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("IMSCode");

                entity.Property(e => e.IsModified).HasColumnName("isModified");

                entity.Property(e => e.LaboratoryCode).HasColumnName("laboratoryCode");

                entity.Property(e => e.LaunchingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("launchingDate");

                entity.Property(e => e.RawDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("rawDescription");

                entity.HasOne(d => d.Laboratory)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.LaboratoryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_product_laboratory");
            });

            modelBuilder.Entity<ProductGroup>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("productGroup", "dbo");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("description");
            });

            modelBuilder.Entity<ProductGroupDetail>(entity =>
            {
                entity.HasKey(e => new { e.ProductGroupCode, e.ProductCode });

                entity.ToTable("productGroupDetail", "dbo");

                entity.HasIndex(e => e.ProductGroupCode, "IX_productGroupDetail")
                    .HasFillFactor((byte)90);

                entity.HasIndex(e => e.ProductCode, "IX_productGroupDetail_1")
                    .HasFillFactor((byte)90);

                entity.Property(e => e.ProductGroupCode).HasColumnName("productGroupCode");

                entity.Property(e => e.ProductCode).HasColumnName("productCode");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductGroupDetails)
                    .HasForeignKey(d => d.ProductCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_productGroupDetail_product");

                entity.HasOne(d => d.ProductGroup)
                    .WithMany(p => p.ProductGroupDetails)
                    .HasForeignKey(d => d.ProductGroupCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_productGroupDetail_productGroup");
            });

            modelBuilder.Entity<ProductMarket>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("productMarket", "dbo");

                entity.Property(e => e.MarketCode).HasColumnName("marketCode");

                entity.Property(e => e.ProductCode).HasColumnName("productCode");

                entity.Property(e => e.ProductPresentationGroupCode).HasColumnName("productPresentationGroupCode");
            });

            modelBuilder.Entity<ProductPresentation>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("productPresentation", "dbo");

                entity.HasIndex(e => new { e.Imscode, e.Description }, "IX_productPresentation")
                    .HasFillFactor((byte)90);

                entity.HasIndex(e => new { e.Code, e.ProductCode }, "IX_productPresentation_1")
                    .HasFillFactor((byte)90);

                entity.HasIndex(e => e.ProductCode, "IX_productPresentation_2")
                    .HasFillFactor((byte)90);

                entity.Property(e => e.Code)
                    .ValueGeneratedNever()
                    .HasColumnName("code");

                entity.Property(e => e.Classcode).HasColumnName("classcode");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.EanCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("eanCode");

                entity.Property(e => e.Imscode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IMSCode");

                entity.Property(e => e.IsModified).HasColumnName("isModified");

                entity.Property(e => e.LaunchingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("launchingDate");

                entity.Property(e => e.ProductCode).HasColumnName("productCode");

                entity.Property(e => e.TherapeuticalClassCode).HasColumnName("therapeuticalClassCode");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductPresentations)
                    .HasForeignKey(d => d.ProductCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_productPresentation_product");

                entity.HasOne(d => d.TherapeuticalClass)
                    .WithMany(p => p.ProductPresentations)
                    .HasForeignKey(d => d.TherapeuticalClassCode)
                    .HasConstraintName("FK_productPresentation_therapeuticalClass");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.ProductPresentations)
                    .HasForeignKey(d => d.BusinessUnitCode)
                    .HasConstraintName("FK_productPresentation_businessUnit");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ProductPresentations)
                    .HasForeignKey(d => d.Classcode)
                    .HasConstraintName("FK_productPresentation_classCode");
            });

            modelBuilder.Entity<ProductPresentationGroup>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("productPresentationGroup", "dbo");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.ExpandGroup).HasColumnName("expandGroup");
            });

            modelBuilder.Entity<ProductPresentationGroupDetail>(entity =>
            {
                entity.HasKey(e => new { e.ProductPresentationGroupCode, e.ProductPresentationCode });

                entity.ToTable("productPresentationGroupDetail", "dbo");

                entity.HasIndex(e => e.ProductPresentationCode, "IX_productPresentationGroupDetail")
                    .HasFillFactor((byte)90);

                entity.HasIndex(e => e.ProductPresentationCode, "IX_productPresentationGroupDetail_1")
                    .HasFillFactor((byte)90);

                entity.Property(e => e.ProductPresentationGroupCode).HasColumnName("productPresentationGroupCode");

                entity.Property(e => e.ProductPresentationCode).HasColumnName("productPresentationCode");

                entity.HasOne(d => d.ProductPresentation)
                    .WithMany(p => p.ProductPresentationGroupDetails)
                    .HasForeignKey(d => d.ProductPresentationCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_productPresentationGroupDetail_productPresentation");

                entity.HasOne(d => d.ProductPresentationGroup)
                    .WithMany(p => p.ProductPresentationGroupDetails)
                    .HasForeignKey(d => d.ProductPresentationGroupCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_productPresentationGroupDetail_productPresentationGroup");
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("productType", "dbo");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("description");
            });

            modelBuilder.Entity<TherapeuticalClass>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("therapeuticalClass", "dbo");

                entity.Property(e => e.Code)
                    .ValueGeneratedNever()
                    .HasColumnName("code");

                entity.Property(e => e.ClassCode).HasColumnName("classCode");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Imscode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IMSCode");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.TherapeuticalClasses)
                    .HasForeignKey(d => d.ClassCode)
                    .HasConstraintName("FK_therapeuticalClass_class");
            });

            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("userPermission", "dbo");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.FullAccess).HasColumnName("fullAccess");

                entity.Property(e => e.CustomMarketCode).HasColumnName("customMarketCode");

                entity.Property(e => e.LineCode).HasColumnName("lineCode");

                entity.Property(e => e.LineCode).HasColumnName("lineCode");

                entity.Property(e => e.LineGroupCode).HasColumnName("lineGroupCode");

                entity.Property(e => e.UserId).HasColumnName("userId");
            });

            modelBuilder.Entity<CustomMarketTree>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("customMarketTree", "dbo");

                entity.Property(e => e.LineGroupCode).HasColumnName("lineGroupCode");
                entity.Property(e => e.LineGroupDescription).HasColumnName("lineGroupDescription");
                entity.Property(e => e.LineCode).HasColumnName("lineCode");
                entity.Property(e => e.LineDescription).HasColumnName("lineDescription");
                entity.Property(e => e.CustomMarketCode).HasColumnName("customMarketCode");
                entity.Property(e => e.CustomMarketDescription).HasColumnName("customMarketDescription");
                //entity.Property(e => e.CustomMarketOrder).HasColumnName("customMarketOrder");
                entity.Property(e => e.CustomMarketTest).HasColumnName("customMarketTest");
            });

            modelBuilder.Entity<DrugComponent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("drugComponentView", "dbo");

                entity.Property(e => e.DrugGroupCode).HasColumnName("drugGroupCode");

                entity.Property(e => e.DrugCode).HasColumnName("drugCode");

                entity.Property(e => e.Description).HasColumnName("description");
            });

            modelBuilder.Entity<LaboratoryComponent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("laboratoryComponentView", "dbo");

                entity.Property(e => e.LaboratoryGroupCode).HasColumnName("LaboratoryGroupCode");

                entity.Property(e => e.LaboratoryCode).HasColumnName("LaboratoryCode");

                entity.Property(e => e.Imscode).HasColumnName("Imscode");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ownLab).HasColumnName("ownLab");
            });

            modelBuilder.Entity<PharmaceuticalFormComponent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("pharmaceuticalFormComponentView", "dbo");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Imscode).HasColumnName("IMSCode");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ProductCode).HasColumnName("ProductCode");

                entity.Property(e => e.ProductGroupCode).HasColumnName("ProductGroupCode");

                entity.Property(e => e.ProductPresentationCode).HasColumnName("ProductPresentationCode");

                entity.Property(e => e.ProductPresentationGroupCode).HasColumnName("ProductPresentationGroupCode");
            });

            modelBuilder.Entity<ProductComponent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("productComponentView", "dbo");

                entity.Property(e => e.ProductCode).HasColumnName("code");

                entity.Property(e => e.ProductGroupCode).HasColumnName("productGroupCode");

                entity.Property(e => e.Class).HasColumnName("class");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Laboratory).HasColumnName("laboratory");

                entity.Property(e => e.OwnProduct).HasColumnName("ownProduct");
            });

            modelBuilder.Entity<ProductComponentByDrug>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("productComponentByDrugView", "dbo");

                entity.Property(e => e.ProductCode).HasColumnName("code");

                entity.Property(e => e.ProductGroupCode).HasColumnName("productGroupCode");

                entity.Property(e => e.Class).HasColumnName("class");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Laboratory).HasColumnName("laboratory");

                entity.Property(e => e.DrugCode).HasColumnName("drugCode");

                entity.Property(e => e.DrugGroupCode).HasColumnName("drugGroupCode");
            });

            modelBuilder.Entity<ProductComponentByLaboratory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("productComponentByLaboratoryView", "dbo");

                entity.Property(e => e.ProductCode).HasColumnName("code");

                entity.Property(e => e.ProductGroupCode).HasColumnName("productGroupCode");

                entity.Property(e => e.Class).HasColumnName("class");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Laboratory).HasColumnName("laboratory");

                entity.Property(e => e.LaboratoryCode).HasColumnName("laboratoryCode");

                entity.Property(e => e.LaboratoryGroupCode).HasColumnName("laboratoryGroupCode");
            });

            modelBuilder.Entity<ProductComponentByTherapeuticalClass>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("productComponentByTherapeuticalClassView", "dbo");

                entity.Property(e => e.ProductCode).HasColumnName("code");

                entity.Property(e => e.ProductGroupCode).HasColumnName("productGroupCode");

                entity.Property(e => e.Class).HasColumnName("class");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Laboratory).HasColumnName("laboratory");

                entity.Property(e => e.TherapeuticalClassCode).HasColumnName("therapeuticalClassCode");
            });

            modelBuilder.Entity<ProductPresentationComponent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("productPresentationComponentView", "dbo");

                entity.Property(e => e.ProductPresentationGroupCode).HasColumnName("productPresentationGroupCode");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Class).HasColumnName("class");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Laboratory).HasColumnName("laboratory");

                entity.Property(e => e.TherapeuticalClass).HasColumnName("therapeuticalClass");

                entity.Property(e => e.PharmaceuticalFormCode).HasColumnName("pharmaceuticalFormCode");
            });
            modelBuilder.Entity<CustomMarketActualDefinition>(entity =>
            {
                entity.HasKey(e => new { e.CustomMarketCode, e.VersionDate });

                entity.ToTable("customMarketActualDefinition");

                entity.Property(e => e.CustomMarketCode).HasColumnName("customMarketCode");

                entity.Property(e => e.VersionDate)
                    .HasColumnType("datetime")
                    .HasColumnName("versionDate");

                entity.Property(e => e.ControlPanel).HasColumnName("controlPanel");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.DrugReport).HasColumnName("drugReport");

                entity.Property(e => e.Footer)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("footer");

                entity.Property(e => e.Header)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("header");

                entity.Property(e => e.IsOtc).HasColumnName("isOTC");

                entity.Property(e => e.LabReport).HasColumnName("labReport");

                entity.Property(e => e.LineCode).HasColumnName("lineCode");

                entity.Property(e => e.MarketClass).HasColumnName("marketClass");

                entity.Property(e => e.MarketFilter).HasColumnName("marketFilter");

                entity.Property(e => e.MarketReference).HasColumnName("marketReference");

                entity.Property(e => e.Order).HasColumnName("order");

                entity.Property(e => e.ProductReport).HasColumnName("productReport");

                entity.Property(e => e.SignedUser).HasColumnName("signedUser");

                entity.Property(e => e.AdUser).HasColumnName("adUser");

                entity.Property(e => e.ResponsibleName).HasColumnName("responsibleName");

                entity.Property(e => e.ResponsibleLastName).HasColumnName("responsibleLastName");

                entity.Property(e => e.Stamp)
                    .HasMaxLength(8)
                    .HasColumnName("stamp")
                    .IsFixedLength(true);

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("status")
                    .IsFixedLength(true);

                entity.Property(e => e.Tcreport).HasColumnName("TCReport");

                entity.Property(e => e.TestMarket).HasColumnName("testMarket");

                entity.Property(e => e.TravelCrm).HasColumnName("travelCrm");
            });

            modelBuilder.Entity<CustomMarketPreview>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null);
            });

            modelBuilder.Entity<JsonResult>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null);
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("Logs", "dbo");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Type).HasColumnName("Type");

                entity.Property(e => e.Description).HasColumnName("Description");

                entity.Property(e => e.UserLog).HasColumnName("UserLog");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.CustomMarketCode).HasColumnName("customMarketCode");

            });

            modelBuilder.Entity<CustomMarketVersionHistoric>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("customMarketVersionHistoric", "dbo");


                entity.Property(e => e.Code)
                    .IsRequired()
                    .UseIdentityColumn();

                entity.Property(e => e.CustomMarketCode)
                    .IsRequired();

                entity.Property(e => e.LineCode)
                    .IsRequired(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Order)
                    .IsRequired(false);

                entity.Property(e => e.ProductReport)
                    .IsRequired();

                entity.Property(e => e.DrugReport)
                    .IsRequired();

                entity.Property(e => e.IsOTC)
                    .IsRequired();

                entity.Property(e => e.Header)
                    .HasMaxLength(500)
                    .IsRequired(false);

                entity.Property(e => e.Footer)
                    .HasMaxLength(500)
                    .IsRequired(false);

                entity.Property(e => e.MarketFilter)
                    .IsRequired(false);

                entity.Property(e => e.TestMarket)
                    .IsRequired();

                entity.Property(e => e.ControlPanel)
                    .IsRequired();

                entity.Property(e => e.LabReport)
                    .IsRequired(false);

                entity.Property(e => e.TCReport)
                    .IsRequired(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsRequired(false);

                entity.Property(e => e.Stamp)
                    .HasColumnType("binary(50)")
                    .IsRequired(false);

                entity.Property(e => e.MarketClass)
                    .IsRequired(false);

                entity.Property(e => e.MarketReference)
                    .IsRequired(false);

                entity.Property(e => e.TravelCrm)
                    .IsRequired(false);

                entity.Property(e => e.VersionDate)
                    .IsRequired();

                entity.Property(e => e.SignedUser)
                    .IsRequired(false);

                entity.Property(e => e.AdUser)
                    .HasColumnType("varchar(max)")
                    .IsRequired(false);

                entity.Property(e => e.ResponsibleName)
                    .HasColumnType("varchar(max)")
                    .IsRequired(false);

                entity.Property(e => e.ResponsibleLastName)
                    .HasColumnType("varchar(max)")
                    .IsRequired(false);


                entity.HasOne(e => e.SignedUserNavigation)
               .WithMany()
               .HasForeignKey(e => e.SignedUser)
               .HasConstraintName("FK_CustomMarketVersionHistoric_User");
            });

            modelBuilder.Entity<CustomMarketResultVersionHistoric>(entity =>
            {
                entity.ToTable("customMarketResultVersionHistoric","dbo");

                entity.HasKey(e => e.VersionCode);

                entity.Property(e => e.VersionCode)
                    .IsRequired();

                entity.Property(e => e.LineCode);

                entity.Property(e => e.LineDescription)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.CustomMarketCode)
                    .IsRequired();

                entity.Property(e => e.CustomMarketDescription)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ProductPresentationCode)
                    .IsRequired();

                entity.Property(e => e.ProductPresentationDescription)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EAN)
                    .HasMaxLength(50);

                entity.Property(e => e.ProductCode);

                entity.Property(e => e.ProductDescription)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.OwnProduct);

                entity.Property(e => e.LabCode)
                    .IsRequired();

                entity.Property(e => e.LabAbrev)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.LabDescription)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PPG)
                    .HasMaxLength(50);

                entity.Property(e => e.MercadoMarca)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Marca)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TC)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TCDescription)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FF)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.FFDewcription)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Genero)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.GeneroDesc)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LanzamientoProd);

                entity.Property(e => e.LanzamientoPres);

                entity.Property(e => e.Drugs)
                    .HasMaxLength(1000);

                entity.Property(e => e.Modifier);

                entity.Property(e => e.INTEModifier);

                entity.Property(e => e.TAM1_Units);
                entity.Property(e => e.TAM1_Vals);
                entity.Property(e => e.TAM2_Units);
                entity.Property(e => e.TAM2_Vals);
                entity.Property(e => e.TAM3_Units);
                entity.Property(e => e.TAM3_Vals);
                entity.Property(e => e.TAM4_Units);
                entity.Property(e => e.TAM4_Vals);
                entity.Property(e => e.TAM5_Units);
                entity.Property(e => e.TAM5_Vals);
                entity.Property(e => e.YTD_Units);
                entity.Property(e => e.YTD_Vals);
                entity.Property(e => e.YTDAA_Units);
                entity.Property(e => e.YTDAA_Vals);
                entity.Property(e => e.TRIM_Units);
                entity.Property(e => e.TRIM_Vals);
                entity.Property(e => e.TRIM_IA_Units);
                entity.Property(e => e.TRIM_IA_Vals);
                entity.Property(e => e.TRIM_AA_Units);
                entity.Property(e => e.TRIM_AA_Vals);
                entity.Property(e => e.M1_Units);
                entity.Property(e => e.M1_Vals);
                entity.Property(e => e.M2_Units);
                entity.Property(e => e.M2_Vals);
                entity.Property(e => e.M3_Units);
                entity.Property(e => e.M3_Vals);
                entity.Property(e => e.M4_Units);
                entity.Property(e => e.M4_Vals);
                entity.Property(e => e.M5_Units);
                entity.Property(e => e.M5_Vals);
                entity.Property(e => e.M6_Units);
                entity.Property(e => e.M6_Vals);
                entity.Property(e => e.M7_Units);
                entity.Property(e => e.M7_Vals);
                entity.Property(e => e.M8_Units);
                entity.Property(e => e.M8_Vals);
                entity.Property(e => e.M9_Units);
                entity.Property(e => e.M9_Vals);
                entity.Property(e => e.M10_Units);
                entity.Property(e => e.M10_Vals);
                entity.Property(e => e.M11_Units);
                entity.Property(e => e.M11_Vals);
                entity.Property(e => e.M12_Units);
                entity.Property(e => e.M12_Vals);
                entity.Property(e => e.M13_Units);
                entity.Property(e => e.M13_Vals);
                entity.Property(e => e.M14_Units);
                entity.Property(e => e.M14_Vals);
                entity.Property(e => e.M15_Units);
                entity.Property(e => e.M15_Vals);
                entity.Property(e => e.M16_Units);
                entity.Property(e => e.M16_Vals);
                entity.Property(e => e.M17_Units);
                entity.Property(e => e.M17_Vals);
                entity.Property(e => e.M18_Units);
                entity.Property(e => e.M18_Vals);
                entity.Property(e => e.M19_Units);
                entity.Property(e => e.M19_Vals);
                entity.Property(e => e.M20_Units);
                entity.Property(e => e.M20_Vals);
                entity.Property(e => e.M21_Units);
                entity.Property(e => e.M21_Vals);
                entity.Property(e => e.M22_Units);
                entity.Property(e => e.M22_Vals);
                entity.Property(e => e.M23_Units);
                entity.Property(e => e.M23_Vals);
                entity.Property(e => e.M24_Units);
                entity.Property(e => e.M24_Vals);

                entity.Property(e => e.TravelCrm);

                entity.Property(e => e.AdUser)
                    .HasColumnType("varchar(max)");

                entity.Property(e => e.ResponsibleName)
                    .HasColumnType("varchar(max)");

                entity.Property(e => e.ResponsibleLastName)
                    .HasColumnType("varchar(max)");
            });

        }
    }
}