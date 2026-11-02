using DateConverterNepali;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NV.Payment.Domain.Entities;
using System.Reflection.Emit;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Inventory.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Sales.Domain.Entities;
using TN.Setup.Domain.Entities;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Entities.Inventory;
using TN.Shared.Domain.Entities.Notification;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Payments;
using TN.Shared.Domain.Entities.Purchase;
using TN.Shared.Domain.Entities.Sales;
using TN.Shared.Domain.Entities.StockCenterEntities;
using TN.Shared.Domain.Entities.Transactions;
using TN.Shared.Domain.Primitive;
using static TN.Authentication.Domain.Entities.Company;

namespace TN.Shared.Infrastructure.Data
{
    public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            //AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

        }

        #region Notification

        public DbSet<StockExpiryNotification> StockExpiryNotifications { get; set; }

        #endregion

        #region Sales and purchase quotation
        public DbSet<PurchaseQuotationDetails>  PurchaseQuotationDetails { get; set; }
        public DbSet<PurchaseQuotationItems> PurchaseQuotationItems { get; set; }
        public DbSet<SalesQuotationDetails>  SalesQuotationDetails { get; set; }
        public DbSet<SalesQuotationItems>  SalesQuotationItems { get; set; }
        #endregion

        #region StockCenter
        public DbSet<StockCenter> StockCenters { get; set; }

        #endregion

        #region Initial SetUp
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<UserCompany> UserCompany { get; set; }
        public DbSet<Municipality> Municipality { get; set; }
        public DbSet<Vdc> Vdcs { get; set; }
        public DbSet<Company> Companys { get; set; }

        public DbSet<CompanySettings> CompanySettings { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Branch> Branches { get; set; }

        public DbSet<FiscalYears> FiscalYears { get; set; }

        public DbSet<CompanySettingsFiscalYear> CompanySettingsFiscalYears { get; set; }
        #endregion

        #region Account

        public DbSet<BillSundryDetails> BillSundryDetails { get; set; }
        public DbSet<Master> Masters { get; set; }
        public DbSet<LedgerGroup> LedgerGroups { get; set; }
        public DbSet<SubLedgerGroup> SubLedgerGroups { get; set; }

        public DbSet<Ledger>Ledgers { get; set; }

        public DbSet<Customers> Customers { get; set; }

        public DbSet<CustomerCategory> CustomerCategories { get; set; }

        public DbSet<OpeningBalance> OpeningBalance { get; set; }
        public DbSet<ClosingBalance> ClosingBalance { get; set; }
        public DbSet<BillSundry> BillSundries { get; set; }

        #endregion

        #region Menu,Module,SubModule

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Modules> Modules { get; set; }
        public DbSet<SubModules> SubModules { get; set; }
        public DbSet<Menu> Menus { get; set; }

        public DbSet<RoleModule> RoleModules { get; set; }

        public DbSet<RoleSubModules> RoleSubModules { get; set; }

        #endregion

        #region Inventory
        public DbSet<Units> Units { get; set; }
        public DbSet<BatchNumber> BatchNumbers { get; set; }
        public DbSet<ManufactureAndExpiry> ManufactureAndExpiries { get; set; }
        public DbSet<ConversionFactor> ConversionFactors { get; set; }

        public DbSet<ItemGroup> ItemGroups { get; set; }
        public DbSet<Items> Items { get; set; }

        public DbSet<Inventories> Inventories { get; set; }
        public DbSet<InventoriesLogs> InventoriesLogs { get; set; }

        public DbSet<ItemInstance> ItemInstances { get; set; }

        public DbSet<StockAdjustment> StockAdjustments { get; set; }
        public DbSet<StockTransferDetails> StockTransferDetails { get; set; }
        public DbSet<StockTransferItems> stockTransferItems { get; set; }


        #endregion

        #region Purchase
        public DbSet<PurchaseItems> PurchaseItems { get; set; }
        public DbSet<PurchaseDetails> PurchaseDetails { get; set; }
        #endregion

        #region Sales
        public DbSet<SalesItems> SalesItems { get; set; }
        public DbSet<SalesDetails> SalesDetails { get; set; }

        #endregion

        #region SalesReturn
        public DbSet<SalesReturnDetails> SalesReturnDetails { get; set; }
        public DbSet<SalesReturnItems> SalesReturnItems { get; set; }
        #endregion

        #region PurchaseReturn
        public DbSet<PurchaseReturnDetails> PurchaseReturnDetails { get; set; }
        public DbSet<PurchaseReturnItems> PurchaseReturnItems { get; set; }
        #endregion

        #region Payment
        public DbSet<Payments> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }

        public DbSet<ChequePayment> ChequePayments { get; set; }

        #endregion

        #region PaymentDetails
        public DbSet<PaymentsDetails> PaymentDetails { get; set; }
        #endregion

        #region Transaction
        public DbSet<TransactionDetail> TransactionDetails { get; set; }
        public DbSet<TransactionItems> TransactionItems { get; set; }

        #endregion


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Configure for Inheritance between PaymentsDetails and ChequePayments
            // Configure Table-Per-Hierarchy (TPH) inheritance
            builder.Entity<PaymentsDetails>()
                .HasDiscriminator<string>("PaymentType")  
                .HasValue<PaymentsDetails>("Generic")    
                .HasValue<ChequePayment>("Cheque");        

            // Optional: add indexes for scalability
            builder.Entity<PaymentsDetails>()
                .HasIndex(p => p.TransactionDate);

            builder.Entity<PaymentsDetails>()
                .HasIndex(p => p.PaymentMethodId);
            #endregion

            #region PurchaseQuotationDetails
            #region PurchaseQuotationDetails and PurchaseQuotationItems(1:m)
            builder.Entity<PurchaseQuotationDetails>()
               .HasMany(p => p.PurchaseQuotationItems)
               .WithOne(p => p.PurchaseQuotationDetails)
               .HasForeignKey(p => p.PurchaseQuotationDetailsId)
               .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region PurchaseQuotationItems and PurchaseQuotationDetails(m:1)


            builder.Entity<PurchaseQuotationItems>()
                .HasOne(p => p.PurchaseQuotationDetails)
                .WithMany(p => p.PurchaseQuotationItems)
                .HasForeignKey(p => p.PurchaseQuotationDetailsId)
                .OnDelete(DeleteBehavior.Restrict);



            #endregion

            #endregion

            #region SalesQuotationDetails
            #region SalesQuotationDetails and SalesQuotationItems(1:m)
            builder.Entity<SalesQuotationDetails>()
               .HasMany(p => p.SalesQuotationItems)
               .WithOne(p => p.SalesQuotationDetails)
               .HasForeignKey(p => p.SalesQuotationDetailsId)
               .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region SalesQuotationItems and SalesQuotationDetails(m:1)


            builder.Entity<SalesQuotationItems>()
                .HasOne(p => p.SalesQuotationDetails)
                .WithMany(p => p.SalesQuotationItems)
                .HasForeignKey(p => p.SalesQuotationDetailsId)
                .OnDelete(DeleteBehavior.Restrict);



            #endregion

            #region StockCenter with SalesDetailsQuotation and PurchaseDetailsQuotation(1:m)
            builder.Entity<StockCenter>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Address).IsRequired(false);
                entity.HasMany(sc => sc.PurchaseQuotationDetails)
                      .WithOne(pd => pd.StockCenter)
                      .HasForeignKey(pd => pd.StockCenterId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(sc => sc.SalesQuotationDetails)
                      .WithOne(sd => sd.StockCenter)
                      .HasForeignKey(sd => sd.StockCenterId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region SalesQuatation and unit and Items
            builder.Entity<SalesQuotationItems>()
                .HasOne(s => s.Unit)
                .WithMany()
                .HasForeignKey(s => s.UnitId)
                .OnDelete(DeleteBehavior.Restrict); // Important: avoid multiple cascades

            builder.Entity<SalesQuotationItems>()
                .HasOne(s => s.Item)
                .WithMany()
                .HasForeignKey(s => s.ItemId)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction

            #endregion

            #endregion


            #region SalesReturnDetails and StockItems(m:1)
            builder.Entity<SalesReturnDetails>()
                .HasOne(srd => srd.StockCenter)
                .WithMany(sri => sri.SalesReturnDetails)
                .HasForeignKey(sri => sri.StockCenterId)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction

            #endregion


            #region PurchaseReturnDetails and StockItems(m:1)
            builder.Entity<PurchaseReturnDetails>()
                .HasOne(srd => srd.StockCenter)
                .WithMany(sri => sri.PurchaseReturnDetails)
                .HasForeignKey(sri => sri.StockCenterId)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction

            #endregion


            #region Stock Center and Items(1:m)
            builder.Entity<StockCenter>()
                .HasMany(sc => sc.Items)
                .WithOne(i => i.StockCenter)
                .HasForeignKey(i => i.StockCenterId)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction

            #endregion


            #region Stock Center and Inventories(1:m)
            builder.Entity<StockCenter>()
                .HasMany(sc => sc.Inventories)
                .WithOne(i => i.StockCenters)
                .HasForeignKey(i => i.StockCenterId)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction

            #endregion

            #region StockCenter with SalesDetails and PurchaseDetails(1:m)


            builder.Entity<StockCenter>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Address).IsRequired(false);
                entity.HasMany(sc => sc.PurchaseDetails)
                      .WithOne(pd => pd.StockCenter)
                      .HasForeignKey(pd => pd.StockCenterId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(sc => sc.SalesDetails)
                      .WithOne(sd => sd.StockCenter)
                      .HasForeignKey(sd => sd.StockCenterId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region Fiscal Year

            builder.Entity<CompanySettingsFiscalYear>()
        .HasKey(cs => new { cs.CompanySettingId, cs.FiscalYearId });

            builder.Entity<CompanySettingsFiscalYear>()
                .HasOne(cs => cs.CompanySetting)
                .WithMany(c => c.CompanySettingFiscalYears)
                .HasForeignKey(cs => cs.CompanySettingId);

            builder.Entity<CompanySettingsFiscalYear>()
                .HasOne(cs => cs.FiscalYear)
                .WithMany(f => f.CompanySettingFiscalYears)
                .HasForeignKey(cs => cs.FiscalYearId);


            #endregion

            #region Authentication

            #region Permission and Roles(m:m)

            // Configure many-to-many relationship between roles and Permission
            builder.Entity<RolePermission>()
                .HasKey(rm => new { rm.RoleId, rm.PermissionId });

            builder.Entity<RolePermission>()
                .HasOne(rm => rm.Role)
                .WithMany()
                .HasForeignKey(rm => rm.RoleId);

            builder.Entity<RolePermission>()
                .HasOne(rm => rm.Permissions)
                .WithMany(m => m.RolePermissions)
                .HasForeignKey(rm => rm.PermissionId);

            #endregion



            // Explicitly configure AspNetRoles
            builder.Entity<IdentityRole>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsRequired(false);

                entity.Property(e => e.NormalizedName)
                    .HasMaxLength(256)
                    .IsRequired(false);

                entity.Property(e => e.ConcurrencyStamp)
                    .HasMaxLength(256)
                    .IsRequired(false);
            });

            #region Province,District,Municipality
            // Province Configuration
            builder.Entity<Province>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProvinceNameInNepali).IsRequired(false);
                entity.Property(e => e.ProvinceNameInEnglish).IsRequired(false);

                entity.HasMany(p => p.Districts)
                      .WithOne(d => d.Province)
                      .HasForeignKey(d => d.ProvinceId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            #region User and Company (M:M)
            builder.Entity<UserCompany>()
                .HasKey(uc => new { uc.UserId, uc.CompanyId });

            builder.Entity<UserCompany>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCompany)
                .HasForeignKey(uc => uc.UserId);

            builder.Entity<UserCompany>()
                .HasOne(uc => uc.Company)
                .WithMany(c => c.UserCompany)
                .HasForeignKey(uc => uc.CompanyId);
            #endregion

            #region Institution and Users(1:m)
            builder.Entity<Institution>()
               .HasMany(i => i.Users)
               .WithOne(u => u.Institution)
               .HasForeignKey(u => u.InstitutionId)
               .OnDelete(DeleteBehavior.SetNull);
            #endregion

            #region Users and Institution(m:1)
            builder.Entity<ApplicationUser>()
               .HasOne(i => i.Institution)
               .WithMany(u => u.Users)
               .HasForeignKey(u => u.InstitutionId)
               .OnDelete(DeleteBehavior.SetNull);
            #endregion


            #region Company and User(m:m)
            builder.Entity<UserCompany>()
                .HasKey(cu => new { cu.UserId, cu.CompanyId }); // Composite Key

            builder.Entity<UserCompany>()
                .HasOne(cu => cu.User)
                .WithMany(u => u.UserCompany)
                .HasForeignKey(cu => cu.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserCompany>()
                .HasOne(cu => cu.Company)
                .WithMany(c => c.UserCompany)
                .HasForeignKey(cu => cu.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion



            // District Configuration
            builder.Entity<District>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DistrictNameInNepali).IsRequired(false);
                entity.Property(e => e.DistrictNameInEnglish).IsRequired(false);

                entity.HasOne(p => p.Province)
                      .WithMany(d => d.Districts)
                      .HasForeignKey(d => d.ProvinceId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(d => d.Municipality)
                      .WithOne(m => m.Districts)
                      .HasForeignKey(m => m.DistrictId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(d => d.Vdcs)
                      .WithOne(v => v.Districts)
                      .HasForeignKey(v => v.DistrictId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Municipality Configuration
            builder.Entity<Municipality>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MunicipalityNameInNepali).IsRequired(false);
                entity.Property(e => e.MunicipalityNameInEnglish).IsRequired(false);

                entity.HasOne(d => d.Districts)
                      .WithMany(m => m.Municipality)
                      .HasForeignKey(m => m.DistrictId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Vdc Configuration
            builder.Entity<Vdc>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.VdcNameInEnglish).IsRequired(false);
                entity.Property(v => v.VdcNameInNepali).IsRequired(false);

                entity.HasOne(d => d.Districts)
                      .WithMany(v => v.Vdcs)
                      .HasForeignKey(v => v.DistrictId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion


            #endregion

            #region Account

            #region BillSundryDetails and BillSundry(m:1)

            builder.Entity<BillSundryDetails>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.BillSundry)
                .WithMany(x => x.BillSundryDetails)
                .HasForeignKey(x => x.BillSundryId)
                .OnDelete(DeleteBehavior.Restrict);

            });
            #endregion

            #region JournalEntry and Purchase(1:m)

            builder.Entity<JournalEntry>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.PurchaseDetails)
                .WithOne(x => x.JournalEntry)
                .HasForeignKey(x => x.JournalEntriesId)
                .OnDelete(DeleteBehavior.Restrict);

            });
            #endregion


            #region JournalEntry and Sales(1:m)

            builder.Entity<JournalEntry>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.SalesDetails)
                .WithOne(x => x.JournalEntry)
                .HasForeignKey(x => x.JournalEntriesId)
                .OnDelete(DeleteBehavior.Restrict);

            });
            #endregion




            #region Ledger and OpeningBalance(1:m)

            builder.Entity<Ledger>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.OpeningBalances)
                .WithOne(x => x.Ledger)
                .HasForeignKey(x => x.LedgerId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            #endregion

            #region OpeningBalance and Ledger (m:1)

            builder.Entity<OpeningBalance>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Ledger)
                .WithMany(x => x.OpeningBalances)
                .HasForeignKey(x => x.LedgerId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            #endregion

            #region ClosingBalance and Ledger (m:1)

            builder.Entity<ClosingBalance>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Ledger)
                .WithMany(x => x.ClosingBalances)
                .HasForeignKey(x => x.LedgerId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            #endregion

            #region Ledger and ClosingBalance(1:m)

            builder.Entity<Ledger>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.ClosingBalances)
                .WithOne(x => x.Ledger)
                .HasForeignKey(x => x.LedgerId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            #endregion

            #region Master and LedgerGroup(1:m)

            builder.Entity<Master>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x=>x.Name).IsRequired(false);
                entity.HasMany(x => x.LedgerGroups)
                .WithOne(x => x.Masters)
                .HasForeignKey(x => x.MasterId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            #endregion

            #region LedgerGroup and Master(m:1)
            builder.Entity<LedgerGroup>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired(false);
                entity.HasOne(x => x.Masters)
                .WithMany(x => x.LedgerGroups)
                .HasForeignKey(x => x.MasterId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            #endregion

            #region SubLedgerGroup and LedgerGroup(m:1)
            builder.Entity<SubLedgerGroup>()
           .HasOne(g => g.LedgerGroup)
           .WithMany(s => s.SubLedgerGroups)
           .HasForeignKey(s => s.LedgerGroupId)
           .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region LedgerGroup and SubLedgerGroup(1:m)
            builder.Entity<LedgerGroup>()
           .HasMany(g => g.SubLedgerGroups)
           .WithOne(s => s.LedgerGroup)
           .HasForeignKey(s => s.LedgerGroupId)
           .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region SubLedgerGroup and Ledger(1:m)
            builder.Entity<SubLedgerGroup>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x=>x.Ledgers)
                .WithOne(x=>x.SubLedgerGroup)
                .HasForeignKey(x=>x.SubLedgerGroupId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region Ledger and SubLedgerGroup(m:1)
            builder.Entity<Ledger>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.SubLedgerGroup)
                .WithMany(x => x.Ledgers)
                .HasForeignKey(x => x.SubLedgerGroupId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region Ledger and SubLedgerGroup(m:1)
            builder.Entity<Ledger>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.SubLedgerGroup)
                .WithMany(x => x.Ledgers)
                .HasForeignKey(x => x.SubLedgerGroupId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region Ledger and Customers(1:m)
            builder.Entity<Ledger>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.Customers)
                .WithOne(x => x.Ledger)
                .HasForeignKey(x=>x.LedgerId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            #endregion


            #region Customers and Ledger(m:1)
            builder.Entity<Customers>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x=>x.Ledger)
                .WithMany(x=>x.Customers)
                .HasForeignKey(x=>x.LedgerId)
                .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.MaxCreditLimit)
               .HasPrecision(18, 2); // Adjust precision and scale as needed

                entity.Property(e => e.OpeningBalance)
                .HasPrecision(18, 2); // Adjust precision and scale as needed
            });

            #endregion

            #region Customer and CustomerCategory(1:m)
            builder.Entity<Customers>(entity =>
            {

                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.CustomerCategories)
                .WithOne(x => x.Customers)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

               

            });

            #endregion

            #region CustomerCategory(m:1)
            builder.Entity<CustomerCategory>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Customers)
                .WithMany(x => x.CustomerCategories)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region JournalEntity and Company(M:1)
            builder.Entity<JournalEntry>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne<Company>()
                .WithMany()
                .HasForeignKey(x=>x.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region Company and JournalEntry(1:m)
            builder.Entity<Company>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany<JournalEntry>()
                .WithOne()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region JournalEntry and JournalEntryDetails(1:m)
            builder.Entity<JournalEntry>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.JournalEntryDetails)
                .WithOne(x => x.JournalEntry)
                .HasForeignKey(x => x.JournalEntryId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region JournalEntryDetails and JournalEntry(m:1)
            builder.Entity<JournalEntryDetails>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.JournalEntry)
                .WithMany(x => x.JournalEntryDetails)
                .HasForeignKey(x => x.JournalEntryId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion


            #region JournalEntryDetails and Ledger(m:1)
            builder.Entity<JournalEntryDetails>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x=>x.Ledger)
                .WithMany(x=>x.JournalEntryDetails)
                .HasForeignKey(x=>x.LedgerId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region Ledger and JournalEntryDetails(1:m)
            builder.Entity<Ledger>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.JournalEntryDetails)
                .WithOne(x => x.Ledger)
                .HasForeignKey(x => x.LedgerId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region User and JournalEntries (One-to-Many)
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.JournalEntries)
                .WithOne()
                .HasForeignKey(j => j.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #endregion

            #region RolesModules
            #region Modules and Roles(m:m)

            // Configure many-to-many relationship between roles and modules
            builder.Entity<RoleModule>()
                .HasKey(rm => new { rm.RoleId, rm.ModuleId });

            builder.Entity<RoleModule>()
                .HasOne(rm => rm.Role)
                .WithMany()
                .HasForeignKey(rm => rm.RoleId);

            builder.Entity<RoleModule>()
                .HasOne(rm => rm.Modules)
                .WithMany(m => m.RoleModules)
                .HasForeignKey(rm => rm.ModuleId);

            #endregion

            #region SubModules and Roles(m:m)

            // Configure many-to-many relationship between roles and SubModules
            builder.Entity<RoleSubModules>()
                .HasKey(rm => new { rm.RoleId, rm.SubModulesId });

            builder.Entity<RoleSubModules>()
                .HasOne(rm => rm.Role)
                .WithMany()
                .HasForeignKey(rm => rm.RoleId);

            builder.Entity<RoleSubModules>()
                .HasOne(rm => rm.SubModules)
                .WithMany(m => m.RoleSubModules)
                .HasForeignKey(rm => rm.SubModulesId);

            #endregion

            #region Menus and Roles(m:m)

            // Configure many-to-many relationship between roles and SubModules
            builder.Entity<RoleMenus>()
                .HasKey(rm => new { rm.RoleId, rm.MenusId });

            builder.Entity<RoleMenus>()
                .HasOne(rm => rm.Role)
                .WithMany()
                .HasForeignKey(rm => rm.RoleId);

            builder.Entity<RoleMenus>()
                .HasOne(rm => rm.Menu)
                .WithMany(m => m.RoleMenus)
                .HasForeignKey(rm => rm.MenusId);

            #endregion

            #region Modules and SubModules(1:m)
            builder.Entity<Modules>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.SubModules)
                .WithOne(x => x.Modules)
                .HasForeignKey(x => x.ModulesId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            #endregion

            #region SubModules and Modules(m:1)
            builder.Entity<SubModules>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Modules)
                .WithMany(x => x.SubModules)
                .HasForeignKey(x => x.ModulesId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            #endregion

            #region SubModules and Menu(1:m)
            builder.Entity<SubModules>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.Menu)
                .WithOne(x => x.SubModules)
                .HasForeignKey(x => x.SubModulesId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            #endregion

            #region Menu and SubModules(m:1)
            builder.Entity<Menu>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.SubModules)
                .WithMany(x => x.Menu)
                .HasForeignKey(x => x.SubModulesId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            #endregion

            #endregion

            #region Initial SetUp
            #region Organization and Province(m:1)
            builder.Entity<Organization>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Provinces)
                .WithMany(x => x.Organizations)
                .HasForeignKey(x => x.ProvinceId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            #endregion

            #region Province and Organization(1:m)
            builder.Entity<Province>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.Organizations)
                .WithOne(x => x.Provinces)
                .HasForeignKey(x => x.ProvinceId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            #endregion

            #region Organization and Institution(1:m)
            builder.Entity<Organization>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.Institutions)
                .WithOne(x => x.Organization)
                .HasForeignKey(x => x.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region Institution and Organization(M:1)
            builder.Entity<Institution>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Organization)
                .WithMany(x => x.Institutions)
                .HasForeignKey(x => x.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region Institution and Company(1:m)
            builder.Entity<Institution>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.Companies)
                .WithOne(x => x.Institutions)
                .HasForeignKey(x => x.InstitutionId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region Company and Institution(m:1)
            builder.Entity<Company>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Institutions)
                .WithMany(x => x.Companies)
                .HasForeignKey(x => x.InstitutionId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region Company and Branch(1:m)
            builder.Entity<Company>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.Branches)
                .WithOne(x => x.Companies)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region Branch and Company(m:1)
            builder.Entity<Branch>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Companies)
                .WithMany(x => x.Branches)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region Company and CompanySettings(1:m)
            builder.Entity<Company>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x=>x.CompanySettings)
                .WithOne(x=>x.Company)
                .HasForeignKey(x=>x.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region CompanySettings and Company (m:1)
            builder.Entity<CompanySettings>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Company)
                .WithMany(x => x.CompanySettings)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #endregion

            #region Inventory

            #region Items and ManufactureandExpiry (1:m)

            builder.Entity<Items>()
               .HasMany(p => p.ManufacturingAndExpiries)
               .WithOne(p => p.Item)
               .HasForeignKey(p => p.ItemId)
               .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Items and BatchNumbers (1:m)

            builder.Entity<Items>()
               .HasMany(p => p.BatchNumbers)
               .WithOne(p => p.Item)
               .HasForeignKey(p => p.ItemId)
               .OnDelete(DeleteBehavior.Restrict);

            #endregion






            #region StockTransferDetails and StockTransferItems(1:m)

            builder.Entity<StockTransferDetails>()
               .HasMany(p => p.StockTransferItems)
               .WithOne(p => p.StockTransferDetails)
               .HasForeignKey(p => p.StockTransferDetailsId)
               .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region StockTransferItems and StockTransferDetails(m:1)
            builder.Entity<StockTransferItems>()
           .HasOne(p => p.StockTransferDetails)
           .WithMany(p => p.StockTransferItems)
           .HasForeignKey(p => p.StockTransferDetailsId)
           .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region StockTransferItems → Items
            builder.Entity<StockTransferItems>()
                .HasOne(p => p.Item)
                .WithMany(p => p.StockTransferItems)
                .HasForeignKey(p => p.ItemId)
                .OnDelete(DeleteBehavior.Restrict); // ❌ change CASCADE → RESTRICT
            #endregion

            #region StockTransferItems → Units
            builder.Entity<StockTransferItems>()
                .HasOne(p => p.Unit)
                .WithMany(p => p.StockTransferItems)
                .HasForeignKey(p => p.UnitId)
                .OnDelete(DeleteBehavior.Restrict); // ❌ change CASCADE → RESTRICT
            #endregion

            #region StockAdjustments and Items(m:1)
            builder.Entity<StockAdjustment>(entity =>
            {
                entity.HasOne(sa => sa.Items)
                      .WithMany(i => i.StockAdjustments)
                      .HasForeignKey(sa => sa.ItemId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region Item and ConversionFactor(m:1)
            builder.Entity<Items>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(u => u.ConversionFactor)
                    .WithMany(c => c.Items)
                    .HasForeignKey(c => c.ConversionFactorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion


            #region ConversionFactor and Item(1:m)
            builder.Entity<ConversionFactor>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasMany(u => u.Items)
                    .WithOne(c => c.ConversionFactor)
                    .HasForeignKey(c => c.ConversionFactorId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region BillNumber and Company
            builder.Entity<Company>()
                .Property(c => c.BillNumberGenerationTypeForPurchase)
                .HasConversion<string>() 
                .HasDefaultValue(BillNumberGenerationType.Manual);

            builder.Entity<Company>()
                .Property(c => c.BillNumberGenerationTypeForSales)
                .HasConversion<string>()
                .HasDefaultValue(BillNumberGenerationType.Manual);
            #endregion

            #region Unit and ConversionFactor (1:M)
            builder.Entity<Units>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasMany(u => u.FromConversions)
                    .WithOne(c => c.FromUnits)
                    .HasForeignKey(c => c.FromUnit)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.ToConversions)
                    .WithOne(c => c.ToUnits)
                    .HasForeignKey(c => c.ToUnit)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region ConversionFactor and Unit (M:1)
            builder.Entity<ConversionFactor>(entity =>
            {
                entity.HasKey(c => c.Id); // Ensure primary key is set

                //entity.HasIndex(c => new { c.FromUnit, c.ToUnit })
                //    .IsUnique(); // Prevents duplicate conversion records

                entity.HasOne(c => c.FromUnits)
                    .WithMany(u => u.FromConversions)
                    .HasForeignKey(c => c.FromUnit)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.ToUnits)
                    .WithMany(u => u.ToConversions)
                    .HasForeignKey(c => c.ToUnit)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ConversionFactor>()
        .Property(c => c.ConversionFactors)
        .HasPrecision(18, 4);
            #endregion

            #region Unit and Items(1:M)
            builder.Entity<Units>(entity =>
            {
                entity.HasMany(x => x.Items)
                .WithOne(c => c.Unit)
                .HasForeignKey(c => c.UnitId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region Items and Units(M:1)
            builder.Entity<Items>(entity =>
            {
                entity.HasOne(x => x.Unit)
                .WithMany(c => c.Items)
                .HasForeignKey(c => c.UnitId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Items>()
        .Property(c => c.Price)
        .HasPrecision(18, 4);

            #endregion

            #region ItemsGroup and Items(1:M)
            builder.Entity<ItemGroup>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasMany(c=>c.Items)
                .WithOne(c=>c.ItemGroup)
                .HasForeignKey(c=>c.ItemGroupId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region Inventories and Items (m:1)
            builder.Entity<Inventories>()
            .HasOne(i => i.Items)
            .WithMany(i => i.Inventories) 
            .HasForeignKey(i => i.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Self-Refericing
            builder.Entity<ItemGroup>()
                .HasOne(g => g.ParentGroup)
                .WithMany(g => g.SubGroups)
                .HasForeignKey(g => g.ItemsGroupId)
                .OnDelete(DeleteBehavior.Restrict);


            #endregion

            #region ItemsItemsGroup and Items(M:1)
            builder.Entity<Items>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasOne(c => c.ItemGroup)
                .WithMany(c => c.Items)
                .HasForeignKey(c => c.ItemGroupId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region Items and ItemInstances(1:m)
            builder.Entity<Items>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasMany(c => c.ItemInstances)
                .WithOne(c => c.Items)
                .HasForeignKey(c => c.ItemsId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region ItemInstances and Items (m:1)
            builder.Entity<ItemInstance>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasOne(c => c.Items)
                .WithMany(c => c.ItemInstances)
                .HasForeignKey(c => c.ItemsId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion


            #endregion



            #region PurchaseQuotation
            builder.Entity<PurchaseQuotationItems>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(pd => pd.ItemInstances)
                    .WithOne(x => x.PurchaseQuotationItems)
                    .HasForeignKey(ii => ii.PurchaseQuotationItemsId)
                    .OnDelete(DeleteBehavior.NoAction); // or DeleteBehavior.NoAction
            });

            #endregion




            #region Purchase

            #region PurchaseItems and PurchaseDetails (m:1)
            builder.Entity<PurchaseItems>()
                .HasMany(pd => pd.ItemInstances)
                .WithOne(x=>x.PurchaseItems) 
                .HasForeignKey(ii => ii.PurchaseItemsId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion



            #region PurchaseDetails and PurchaseItems(1:m)
            builder.Entity<PurchaseDetails>()
               .HasMany(p => p.PurchaseItems)
               .WithOne(p => p.PurchaseDetails)
               .HasForeignKey(p => p.PurchaseDetailsId)
               .OnDelete(DeleteBehavior.Cascade); 
            #endregion


            #endregion

            #region PurchaseDetails and Company(m:1)

          //For modular approach This is not recommended
            //builder.Entity<PurchaseDetails>()
            //    .HasOne(x=>x.Company)
            //    .WithMany()
            //    .HasForeignKey(p => p.CompanyId)
            //    .OnDelete(DeleteBehavior.Cascade);  // Use NoAction if needed

            builder.Entity<PurchaseDetails>()
        .HasKey(p => p.Id);

            // Define CompanyId as a required field but WITHOUT a foreign key constraint
            builder.Entity<PurchaseDetails>()
                .Property(p => p.CompanyId)
                .IsRequired();

            // Optional: Add an index for fast lookups
            builder.Entity<PurchaseDetails>()
                .HasIndex(p => p.CompanyId);




            #endregion

            #region SalesQuotation
            builder.Entity<PurchaseQuotationItems>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(pd => pd.ItemInstances)
                    .WithOne(x => x.PurchaseQuotationItems)
                    .HasForeignKey(ii => ii.PurchaseQuotationItemsId)
                    .OnDelete(DeleteBehavior.NoAction); // or DeleteBehavior.NoAction
            });

            #endregion

            #region Sales

            builder.Entity<SalesItems>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(pd => pd.ItemInstances)
                    .WithOne(x => x.SalesItems)
                    .HasForeignKey(ii => ii.SalesItemsId)
                    .OnDelete(DeleteBehavior.NoAction); // or DeleteBehavior.NoAction
            });



            builder.Entity<SalesItems>()
                 .HasOne(p => p.Item)
                 .WithMany()
                 .HasForeignKey(p => p.ItemId)
                 .OnDelete(DeleteBehavior.Cascade); // Keep cascade only for one

            builder.Entity<SalesItems>()
                .HasOne(p => p.Unit)
                .WithMany()
                .HasForeignKey(p => p.UnitId)
                .OnDelete(DeleteBehavior.NoAction); // Prevent multiple cascade paths


            #region SalesDetails and SalesItems(1:m)
            builder.Entity<SalesDetails>()
               .HasMany(p => p.SalesItems)
               .WithOne(p => p.SalesDetails)
               .HasForeignKey(p => p.SalesDetailsId)
               .OnDelete(DeleteBehavior.Cascade);  
            #endregion

            #region SalesItems and SalesDetails(m:1)


            builder.Entity<SalesItems>()
                .HasOne(p => p.SalesDetails)
                .WithMany(p => p.SalesItems)
                .HasForeignKey(p => p.SalesDetailsId)
                .OnDelete(DeleteBehavior.Cascade);  



            #endregion

            #region SalesDetails and Company(m:1)
            builder.Entity<SalesDetails>()
                .HasKey(p => p.Id);

            // Define CompanyId as a required field but WITHOUT a foreign key constraint
            builder.Entity<SalesDetails>()
                .Property(p => p.CompanyId)
                .IsRequired();

            // Optional: Add an index for fast lookups
            builder.Entity<SalesDetails>()
                .HasIndex(p => p.CompanyId);




            #endregion


            #endregion

            #region SalesReturn    

            #region SalesReturnDetails and SalesReturnItems(1: m)

            builder.Entity<SalesReturnDetails>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x=>x.SalesReturnItems)
                .WithOne(x=>x.SalesReturnDetails)
                .HasForeignKey(srx=>srx.SalesReturnDetailsId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region SalesReturnItems and SalesReturnDetails (m: 1)

            builder.Entity<SalesReturnItems>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.SalesReturnDetails)
                .WithMany(x => x.SalesReturnItems)
                .HasForeignKey(srx => srx.SalesReturnDetailsId)
                .OnDelete(DeleteBehavior.Restrict);
            });

   


            #endregion


            #region SalesDetails and SalesReturnDetails(1:m)

            builder.Entity<SalesDetails>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.SalesReturnDetails)
                .WithOne(x => x.SalesDetails)
                .HasForeignKey(sd => sd.SalesDetailsId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion


            #region SalesReturnDetails and SalesDetails (m:1)

            builder.Entity<SalesReturnDetails>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.SalesDetails)
                .WithMany(x => x.SalesReturnDetails)
                .HasForeignKey(sd => sd.SalesDetailsId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region SalesItems and SalesReturnItems(1:m)

            builder.Entity<SalesItems>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.salesReturnItems)
                .WithOne(x => x.SalesItems)
                .HasForeignKey(sd => sd.SalesItemsId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region SalesReturnItems and SalesItems(m:1)

            builder.Entity<SalesReturnItems>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.SalesItems)
                .WithMany(x => x.salesReturnItems)
                .HasForeignKey(sd => sd.SalesItemsId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #endregion

            #region PurchaseReturn
            #region PurchaseReturnDetails and PurchaseDetails(m:1)
            builder.Entity<PurchaseReturnDetails>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.PurchaseDetails)
                .WithMany(x => x.PurchaseReturnDetails)
                .HasForeignKey(sd => sd.PurchaseDetailsId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region PurchaseDetails and PurchaseReturnDetails(1:m)
            builder.Entity<PurchaseDetails>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.PurchaseReturnDetails)
                .WithOne(x => x.PurchaseDetails)
                .HasForeignKey(sd => sd.PurchaseDetailsId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion


            #region PurchaseReturnDetails and PurchaseReturnItems(1:m)
            builder.Entity<PurchaseReturnDetails>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.PurchaseReturnItems)
                .WithOne(x => x.PurchaseReturnDetails)
                .HasForeignKey(sd => sd.PurchaseReturnDetailsId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region PurchaseReturnItems and PurchaseReturnDetails(m:1)
            builder.Entity<PurchaseReturnItems>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.PurchaseReturnDetails)
                .WithMany(x => x.PurchaseReturnItems)
                .HasForeignKey(sd => sd.PurchaseReturnDetailsId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region PurchaseReturnItems and PurchaseItems(m:1)
            builder.Entity<PurchaseReturnItems>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.PurchaseItems)
                .WithMany(x => x.PurchaseReturnItems)
                .HasForeignKey(sd => sd.PurchaseItemsId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region PurchaseItems and PurchaseReturnItems(1:m)
            builder.Entity<PurchaseItems>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.PurchaseReturnItems)
                .WithOne(x => x.PurchaseItems)
                .HasForeignKey(sd => sd.PurchaseItemsId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #endregion

            #region Decimal Precesion

            // Apply precision to all decimal properties
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties().Where(p => p.ClrType == typeof(decimal)))
                {
                    property.SetPrecision(18); // Set precision
                    property.SetScale(4);      // Set scale
                }
            }
            #endregion

            #region Payment


            #region TransactionDetails and PaymentMethod(m:1)
            builder.Entity<TransactionDetail>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.PaymentMethods)
                .WithMany(x => x.TransactionDetails)
                .HasForeignKey(sd => sd.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region PaymentMethod and TransactionDetails(1:m)
            builder.Entity<PaymentMethod>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.TransactionDetails)
                .WithOne(x => x.PaymentMethods)
                .HasForeignKey(sd => sd.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region SubLedgerGroups and PaymentMethod (1:m)
            builder.Entity<SubLedgerGroup>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.PaymentMethods)
                    .WithOne(x=>x.SubLedgerGroups)  
                    .HasForeignKey(sd => sd.SubLedgerGroupsId)  
                    .OnDelete(DeleteBehavior.Restrict);
            });

            #region PaymentMethod and SubLedgerGroup (m:1)
            builder.Entity<PaymentMethod>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(x=>x.SubLedgerGroups)
                    .WithMany(x=>x.PaymentMethods)
                    .HasForeignKey(x => x.SubLedgerGroupsId)
                    .OnDelete(DeleteBehavior.Restrict);
            });



            #endregion

            #endregion
            #endregion

            #region PaymentDetails




            //// PaymentsDetails and PaymentMethod (1:m)

            #region PaymentMethod and PaymentDetails (1:m)
            builder.Entity<PaymentMethod>()
               .HasMany(p => p.PaymentsDetails)
               .WithOne(p=>p.PaymentMethod)
               .HasForeignKey(p => p.PaymentMethodId)
               .OnDelete(DeleteBehavior.Cascade);
            #endregion



            #endregion

            #region Transaction

            #region TransactionItems and Ledger(m:1)
            builder.Entity<TransactionItems>()
               .HasOne(p => p.Ledgers)
               .WithMany(p => p.TransactionItems)
               .HasForeignKey(p => p.LedgerId)
               .OnDelete(DeleteBehavior.Cascade);
            #endregion



            #region Ledger and TransactionItems(1:m)
            builder.Entity<Ledger>()
               .HasMany(p => p.TransactionItems)
               .WithOne(p => p.Ledgers)
               .HasForeignKey(p => p.LedgerId)
               .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region TransactionsDetails and TransactionItems(1:m)
            builder.Entity<TransactionDetail>()
               .HasMany(p => p.TransactionsItems)
               .WithOne(p => p.TransactionDetail)
               .HasForeignKey(p => p.TransactionDetailId)
               .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region TransactionItems and TransactionDetails(m:1)
            builder.Entity<TransactionItems>()
               .HasOne(p => p.TransactionDetail)
               .WithMany(p => p.TransactionsItems)
               .HasForeignKey(p => p.TransactionDetailId)
               .OnDelete(DeleteBehavior.Cascade);
            #endregion



            #endregion


            #region Decimal Precesion

            foreach (var property in builder.Model
                 .GetEntityTypes()
                 .SelectMany(e => e.GetProperties())
                 .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                // Set precision and scale explicitly
                property.SetPrecision(18);
                property.SetScale(4);
            }





            #endregion


            #region Master, LedgerGroup, SubLedgerGroup and Ledger Relationships
            // Ledger → SubLedgerGroup
            builder.Entity<Ledger>()
                .HasKey(l => l.Id);

            builder.Entity<Ledger>()
                .HasOne(l => l.SubLedgerGroup)
                .WithMany(slg => slg.Ledgers)
                .HasForeignKey(l => l.SubLedgerGroupId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // SubLedgerGroup → LedgerGroup
            builder.Entity<SubLedgerGroup>()
                .HasKey(slg => slg.Id);

            builder.Entity<SubLedgerGroup>()
                .HasOne(slg => slg.LedgerGroup)
                .WithMany(lg => lg.SubLedgerGroups)
                .HasForeignKey(slg => slg.LedgerGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            // LedgerGroup → Master
            builder.Entity<LedgerGroup>()
                .HasKey(lg => lg.Id);

            builder.Entity<LedgerGroup>()
                .HasOne(lg => lg.Masters)
                .WithMany(m => m.LedgerGroups)
                .HasForeignKey(lg => lg.MasterId)
                .OnDelete(DeleteBehavior.Restrict);

            // Master
            builder.Entity<Master>()
                .HasKey(m => m.Id);

            #endregion


        }



    }
}
