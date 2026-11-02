using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NV.Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Authentication.Domain.Static.Roles;
using TN.Inventory.Domain.Entities;
using TN.Setup.Domain.Entities;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.StockCenterEntities;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.Data;
using static NV.Payment.Domain.Entities.PaymentMethod;
using static TN.Authentication.Domain.Entities.Company;

namespace TN.Shared.Infrastructure.DataSeed
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public DataSeeder(ApplicationDbContext applicationDbContext, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
        {
            _context = applicationDbContext;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            
        }

        public async Task Seed()
        {

            var rootGroup = new ItemGroup { Id = Guid.NewGuid().ToString(), Name = "Root Group",IsPrimary=true, ItemsGroupId = null };
            _context.ItemGroups.Add(rootGroup);
            await _context.SaveChangesAsync();

            var childGroup = new ItemGroup { Id = Guid.NewGuid().ToString(), Name = "General", IsPrimary=true, ItemsGroupId = rootGroup.Id };
            _context.ItemGroups.Add(childGroup);
            await _context.SaveChangesAsync();


            await SeedProvince();
            await SeedDistrict();
            await SeedMunicipality();
            await SeedVdc();
            await SeedMaster();
            await SeedLedgerGroup();
            await SeedSubLedgerGroup();
            await SeedUnits();
            await SeedRole();
            await SeedModules();
            await SeedSubModules();
            await SeedMenus();
            await SeedLedger();
            await SeedItemsGroup();
            await SeedFiscalYear();
            await SeedPaymentMethod();
            await SeedDemoOrganization();
            await SeedInstitution();
            await SeedDemoCompany();
            await SeedStockCenter();
            await SeedBillSundry();

        }

        #region BillSundry

        private async Task SeedBillSundry()
        {
            if (!await _context.BillSundries.AnyAsync())
            {
                var billSundry = new List<BillSundry>()
                {
                    new BillSundry("63b9dfb3-7d15-48a0-9cad-9f3df5c56417","Discount",BillSundryType.Subtractive,null,BillSundryNature.DiscountOrIncomeCr,true,true, false,false,false,false, false,false,false,null,null,null,null,CalculationType.Percentage,CalculationTypeOf.SubTotalAmount,"","",DateTime.Now,true),
                    new BillSundry("6b2bc3d5-0080-4196-88ba-5525a6f0f4c4","VAT",BillSundryType.Additive,null,BillSundryNature.TaxOrInputDr,false,false, false,true,true  ,false,   false,true, true,"5fa06b9c-ba4f-4d37-b049-a2e742bfc8d3",null,"5fa06b9c-ba4f-4d37-b049-a2e742bfc8d3",null,CalculationType.Percentage,CalculationTypeOf.TaxableAmount,"","",DateTime.Now,true)

                };

                await _context.BillSundries.AddRangeAsync(billSundry);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        #endregion


        #region DemoOrganization

        private async Task SeedDemoOrganization()
        {
            if (!await _context.Organizations.AnyAsync())
            {
                var organization = new List<Organization>()
                {
                    new Organization("7c92db17-6d74-4e59-9311-3a3dc2b0b1f4","Demo Organization","Demo Address","Demo Email","987654321","9829999323","",1)

                };

                await _context.Organizations.AddRangeAsync(organization);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        #endregion

        #region DemoInstitution

        private async Task SeedInstitution()
        {
            if (!await _context.Institutions.AnyAsync())
            {
                var institution = new List<Institution>()
                {
                    new Institution("b4e3de2f-9781-4075-9676-8e2a98c3e70e","Demo Institution","Demo Address","Demo Email","DI","987654321","Demo Institution","1234","",default,"",default,"", false,"7c92db17-6d74-4e59-9311-3a3dc2b0b1f4")

                };

                await _context.Institutions.AddRangeAsync(institution);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        #endregion

        #region DemoCompany

        private async Task SeedDemoCompany()
        {
            if (!await _context.Companies.AnyAsync())
            {
                var company = new List<Company>()
                {
                    new Company("3da81047-fab3-4f8f-ae67-26f1c1bd03d8","Demo Company","Demo Address","DC","Demo Email","987654321","Netra Verse","1234","",true,"b4e3de2f-9781-4075-9676-8e2a98c3e70e",default,"Demo",default,"",false,BillNumberGenerationType.Manual, BillNumberGenerationType.Manual)

                };

                await _context.Companies.AddRangeAsync(company);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        #endregion

        #region Roles

        private async Task SeedRole()
        {
            if(!await _roleManager.Roles.AnyAsync())
            {
                var roles = new List<IdentityRole>()
                {
                    new IdentityRole(){Name = Role.DeveloperUser},
                    new IdentityRole(){Name = Role.SuperAdmin},
                    new IdentityRole(){Name = Role.Admin},
                    new IdentityRole(){Name = Role.DemoUserRole},
                    new IdentityRole(){Name = Role.DemoExpiryRole}
                };

                foreach(var role in roles)
                {
                    await _roleManager.CreateAsync(role);
                }
            }
        }

        #endregion

        #region Province
        private async Task SeedProvince()
        {
            if(!await _context.Provinces.AnyAsync())
            {
                var province = new List<Province>()
                {
                    new Province(1,"कोशी","Koshi Province"),
                    new Province(2,"मधेश प्रदेश","Madhesh Province"),
                    new Province(3,"बाग्मती प्रदेश","Bagmati Province"),
                    new Province(4,"गण्डकी प्रदेश","Gandaki Province"),
                    new Province(5,"लुम्बिनी प्रदेश","Lumbini Province"),
                    new Province(6,"कर्णाली प्रदेश","Karnali Province"),
                    new Province(7,"सुदूरपश्चिम प्रदेश","Sudurpashchim Province"),
                };

                await _context.Provinces.AddRangeAsync(province);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        #endregion

        #region District
        private async Task SeedDistrict()
        {
            if(!await _context.Districts.AnyAsync())
            {
                var districts = new List<District>()
                {
                    new District (1,"ताप्लेजुङ","Taplejung",  1),
                      new District (2, "पाँचथर", "Panchthar",  1),
                      new District (3, "ईलाम", "Ilam",  1),
                      new District (4, "झापा", "Jhapa",  1),
                      new District (5, "मोरङ्ग", "Morang",  1),
                      new District (6, "सुनसरी", "Sunsari",  1),
                      new District (7, "धनकुटा", "Dhankuta",  1),
                      new District (8, "तेहथुम", "Terhathum",  1),
                      new District (9, "संखुवासभा", "Sankhuwasabha",  1),
                      new District (10, "भोजपुर", "Bhojpur", 1),
                      new District (11, "सोलुखुम्बु", "Solukhumbu",  1),
                      new District (12, "ओखलढुंगा", "Okhaldhunga",  1),
                      new District (13, "खोटाङ", "Khotang",  1),
                      new District (14, "उदयपुर", "Udayapur",  1),
                      new District (15, "सप्तरी", "Saptari",  2),
                      new District (16, "सिराहा", "Siraha",  2),
                      new District (17, "महोत्तरी", "Mahottari",  2),
                      new District (18, "सर्लाही", "Sarlahi",  2),
                      new District (19, "धनुषा", "Dhanusa",  2),
                      new District (20, "रौतहट", "Rautahat",  2),
                      new District (21, "बारा", "Bara",  2),
                      new District (22, "पर्सा", "Parsa",  2),
                      new District (23, "सिन्धुली", "Sindhuli",  3),
                      new District (24, "रामेछाप", "Ramechhap",  3),
                      new District (25, "दोलखा", "Dolakha",  3),
                      new District (26, "भक्तपुर", "Bhaktapur",  3),
                      new District (27, "सिन्धुपाल्चोक", "Sindhupalchok",  3),
                      new District (28, "काठमाडौँ", "Kathmandu",  3),
                      new District (29, "काभ्रेपलान्चोक", "Kavrepalanchok",  3),
                      new District (30, "ललितपुर", "Lalitpur",  3),
                      new District (31, "नुवाकोट", "Nuwakot",  3),
                      new District (32, "रसुवा", "Rasuwa",  3),
                      new District (33, "धादिङ", "Dhading",  3),
                      new District (34, "चितवन", "Chitwan",  3),
                      new District (35, "मकवानपुर", "Makwanpur",  3),
                      new District (36, "गोरखा", "Gorkha",  4),
                      new District (37, "लमजुङ", "Lamjung",  4),
                      new District (38, "कास्की", "Kaski",  4),
                      new District (39, "बागलुङ", "Baglung",  4),
                      new District (40, "मनाङ", "Manang",  4),
                      new District (41, "मुस्ताङ", "Mustang",  4),
                      new District (42, "म्याग्दी", "Myagdi",  4),
                      new District (43, "नवलपुर", "Nawalpur",  4),
                      new District (44, "पर्वत", "Parbat",  4),
                      new District (45, "स्याङग्जा", "Syangja",  4),
                      new District (46, "तनहुँ", "Tanahun",  4),
                      new District (47, "गुल्मी", "Gulmi",  5),
                      new District (48, "पाल्पा", "Palpa",  5),
                      new District (49, "रुपन्देही", "Rupandehi",  5),
                      new District (50, "कपिलवस्तु", "Kapilvastu",  5),
                      new District (51, "अर्घाखाँची", "Arghakhanchi",  5),
                      new District (52, "प्युठान", "Pyuthan",  5),
                      new District (53, "रोल्पा", "Rolpa",  5),
                      new District (54, "दाङ", "Dang",  5),
                      new District (55, "बाँके", "Banke",  5),
                      new District (56, "बर्दिया", "Bardiya",  5),
                      new District (57, "परासी", "Parasi",  5),
                      new District (58, "रूकुम(पूर्वी)", "Rukum(Eastern)",  5),
                      new District (59, "सल्यान", "Salyan",  6),
                      new District (60, "रूकुम(पश्चिमी)", "Rukum(Western)",  6),
                      new District (61, "डोल्पा", "Dolpa",  6),
                      new District (62, "हुम्ला", "Humla",  6),
                      new District (63, "जुम्ला", "Jumla",  6),
                      new District (64, "कालिकोट", "Kalikot",  6),
                      new District (65, "मुगु", "Mugu",  6),
                      new District (66, "सुर्खेत", "Surkhet",  6),
                      new District (67, "दैलेख", "Dailekh",  6),
                      new District (68, "जाजरकोट", "Jajarkot",  6),
                      new District (69, "कैलाली", "Kailali",  7),
                      new District (70, "अछाम", "Achham",  7),
                      new District (71, "डोटी", "Doti",  7),
                      new District (72, "बझाङ", "Bajhang",  7),
                      new District (73, "बाजुरा", "Bajura",  7),
                      new District (74, "कंचनपुर", "Kanchanpur",  7),
                      new District (75, "डडेलधुरा", "Dadeldhura",  7),
                      new District (76, "बैतडी", "Baitadi",  7),
                      new District (77, "दार्चुला", "Darchula",  7),

                };

                await _context.Districts.AddRangeAsync(districts); 
                await _unitOfWork.SaveChangesAsync();
            }

        }

        #endregion

        #region Municipality
        public async Task SeedMunicipality()
        {
            if(!await _context.Municipality.AnyAsync())
            {
                var municipalities = new List<Municipality>()
                {
                    new Municipality(1,"फुङलिङ नगरपालिका","Phungling Municipality", 1),

                     new Municipality(2,"फिदिम नगरपालिका","Phidim Municipality", 2),

                     new Municipality(3,"ईलाम नगरपालिका","Ilam Municipality", 3),

                     new Municipality(4,"देउमाई नगरपालिका","Deumai Municipality", 3),

                     new Municipality(5,"माई नगरपालिका","Mai Municipality", 3),

                     new Municipality(6,"सूर्योदय नगरपालिका","Suryodaya Municipality", 3),

                     new Municipality(7,"मेचीनगर नगरपालिका","MechiNagar Municipality", 4),

                     new Municipality(8,"दमक नगरपालिका","Damak  Municipality", 4),

                     new Municipality(9,"कन्काई नगरपालिका","Kankai  Municipality", 4),

                     new Municipality(10,"भद्रपुर नगरपालिका","Bhadrapur  Municipality", 4),

                     new Municipality(11,"अर्जुनधारा नगरपालिका","Arjundhara  Municipality", 4),

                     new Municipality(12,"शिवशताक्षी नगरपालिका","ShivaSataxi  Municipality", 4),

                     new Municipality(13,"गौरादह नगरपालिका","Gauradaha  Municipality", 4),

                     new Municipality(14,"विर्तामोड नगरपालिका","Birtamod  Municipality", 4),

                     new Municipality(15,"विराटनगर महानगरपालिका","Biratnagar  Metropolitan City", 5),

                     new Municipality(16,"बेलवारी नगरपालिका","Belbari Municipality", 5),

                     new Municipality(17,"लेटाङ नगरपालिका","Letang Municipality", 5),

                     new Municipality(18,"पथरी शनिश्चरे नगरपालिका","Pathari-Sanishchare  Municipality", 5),

                     new Municipality(19,"रंगेली नगरपालिका","Rangeli  Municipality", 5),

                     new Municipality(20,"रतुवामाई नगरपालिका","Ratuwamai  Municipality", 5),

                     new Municipality(21,"सुनवर्षि नगरपालिका","Sunbarshi  Municipality", 5),

                     new Municipality(22,"उर्लावारी नगरपालिका","Urlabari  Municipality", 5),

                     new Municipality(23,"सुन्दरहरैचा नगरपालिका","SundarHaraincha  Municipality", 5),

                     new Municipality(24,"ईटहरी उपमहानगरपालिका","Ithari Sub Metropolitan City", 6),

                     new Municipality(25,"धरान उपमहानगरपालिका","Dharan Sub Metropolitan City", 6),

                     new Municipality(26,"ईनरुवा नगरपालिका","Inaruwa  Municipality", 6),

                     new Municipality(27,"दुहवी नगरपालिका","Duhabi  Municipality", 6),

                     new Municipality(28,"रामधुनी नगरपालिका","Ramdhuni  Municipality", 6),

                     new Municipality(29,"बराहक्षेत्र नगरपालिका","Baraha  Municipality", 6),

                     new Municipality(30,"पाख्रिबास नगरपालिका","Pakhribas  Municipality", 7),

                     new Municipality(31,"धनकुटा नगरपालिका","Dhankuta  Municipality", 7),

                     new Municipality(32,"महालक्ष्मी नगरपालिका","Mahalaxmi  Municipality", 7),

                     new Municipality(33,"म्याङलुङ नगरपालिका","Myanglung  Municipality", 8),

                     new Municipality(34,"लालीगुराँस नगरपालिका","Laligurans  Municipality", 8),

                     new Municipality(35,"चैनपुर नगरपालिका","Chainpur  Municipality", 9),

                     new Municipality(36,"धर्मदेवी नगरपालिका","DharmaDevi  Municipality", 9),

                     new Municipality(37,"खाँदवारी नगरपालिका","Khadbari  Municipality", 9),

                     new Municipality(38,"मादी नगरपालिका","Madi  Municipality", 9),

                     new Municipality(39,"पाँचखपन नगरपालिका","PanchKhapan  Municipality", 9),

                     new Municipality(40,"भोजपुर नगरपालिका","Bhojpur  Municipality", 10),

                     new Municipality(41,"षडानन्द नगरपालिका","Shadananda  Municipality", 10),

                     new Municipality(42,"सोलुदुधकुण्ड नगरपालिका","SoluDudhakund  Municipality", 11),

                     new Municipality(43,"सिद्दिचरण नगरपालिका","SiddiCharan  Municipality", 12),

                     new Municipality(44,"हलेसी तुवाचुङ नगरपालिका","Halesi Tuwachung  Municipality", 13),

                     new Municipality(45,"दिक्तेल रुपाकोट मझुवागढी नगरपालिका","Diktel Rupakot Majhuwagadhi Municipality", 13),

                     new Municipality(46,"कटारी नगरपालिका","Katari  Municipality", 14),

                     new Municipality(47,"चौदण्डीगढी नगरपालिका","ChaudandiGadhi  Municipality", 14),

                     new Municipality(48,"त्रियुगा नगरपालिका","Triyuga  Municipality", 14),

                     new Municipality(49,"वेलका नगरपालिका","Belaka  Municipality", 14),

                     new Municipality(50,"राजविराज नगरपालिका","Rajbiraj  Municipality", 15),

                     new Municipality(51,"कञ्चनरुप नगरपालिका","Kanchanrup  Municipality", 15),

                     new Municipality(52,"डाक्नेश्वरी नगरपालिका","Dakneshwori  Municipality", 15),

                     new Municipality(53,"बोदेबरसाईन नगरपालिका","BodeBarsain  Municipality", 15),

                     new Municipality(54,"खडक नगरपालिका","Khadak  Municipality", 15),

                     new Municipality(55,"शम्भुनाथ नगरपालिका","Shambhunath  Municipality", 15),

                     new Municipality(56,"सुरुङ्‍गा नगरपालिका","Surunga  Municipality", 15),

                     new Municipality(57,"हनुमाननगर कङ्‌कालिनी नगरपालिका","HanumanNagar Kankalini Municipality", 15),

                     new Municipality(58,"सप्तकोशी नगरपालिका","Saptakoshi Municipality", 15),

                     new Municipality(59,"लहान नगरपालिका","Lahan  Municipality", 16),

                     new Municipality(60,"धनगढीमाई नगरपालिका","DhanagadhiMai  Municipality", 16),

                     new Municipality(61,"सिरहा नगरपालिका","Siraha  Municipality", 16),

                     new Municipality(62,"गोलबजार नगरपालिका","GolBazaar  Municipality", 16),

                     new Municipality(63,"मिर्चैयाँ नगरपालिका","Mirchaiya  Municipality", 16),

                     new Municipality(64,"कल्याणपुर नगरपालिका","Kalyanpur  Municipality", 16),

                     new Municipality(65,"कर्जन्हा नगरपालिका","Karjanha Municipality", 16),

                     new Municipality(66,"सुखीपुर नगरपालिका","Sukhipur Municipality", 16),

                     new Municipality(67,"जनकपुरधाम उपमहानगरपालिका","Janakpurdham Sub MetroCity", 19),

                     new Municipality(68,"क्षिरेश्वरनाथ नगरपालिका","Chhireshwarnath  Municipality", 19),

                     new Municipality(69,"गणेशमान चारनाथ नगरपालिका","Ganeshman Charnath  Municipality", 19),

                     new Municipality(70,"धनुषाधाम नगरपालिका","Dhanusadham  Municipality", 19),

                     new Municipality(71,"नगराइन नगरपालिका","Nagaraen  Municipality", 19),

                     new Municipality(72,"विदेह नगरपालिका","Bideh  Municipality", 19),

                     new Municipality(73,"मिथिला नगरपालिका","Mithila  Municipality", 19),

                     new Municipality(74,"शहीदनगर नगरपालिका","Shahid Nagar  Municipality", 19),

                     new Municipality(75,"सबैला नगरपालिका","Sabaila  Municipality", 19),

                     new Municipality(76,"कमला नगरपालिका","Kamala  Municipality", 19),

                     new Municipality(77,"मिथिला बिहारी नगरपालिका","Mithila Bihari Municipality", 19),

                     new Municipality(78,"हंसपुर नगरपालिका","Hanspur Municipality", 19),

                     new Municipality(79,"जलेश्वर नगरपालिका","Jaleshwor  Municipality", 17),

                     new Municipality(80,"बर्दिबास नगरपालिका","Bardibas  Municipality", 17),

                     new Municipality(81,"गौशाला नगरपालिका","Gaushala  Municipality", 17),

                     new Municipality(82,"लोहरपट्टी नगरपालिका","Loharpatti Municipality", 17),

                     new Municipality(83,"रामगोपालपुर नगरपालिका","RamGopalpur Municipality", 17),

                     new Municipality(84,"मनरा शिसवा नगरपालिका","Manara Shisawa Municipality", 17),

                     new Municipality(85,"मटिहानी नगरपालिका","Matihani Municipality", 17),

                     new Municipality(86,"भँगाहा नगरपालिका","Bhangaha Municipality", 17),

                     new Municipality(87,"बलवा नगरपालिका","Balawa Municipality", 17),

                     new Municipality(88,"औरही नगरपालिका","Aaurahi Municipality", 17),

                     new Municipality(89,"ईश्वरपुर नगरपालिका","Ishworpur  Municipality", 18),

                     new Municipality(90,"मलंगवा नगरपालिका","Malangawa  Municipality", 18),

                     new Municipality(91,"लालबन्दी नगरपालिका","Lalbandi  Municipality", 18),

                     new Municipality(92,"हरिपुर नगरपालिका","Haripur  Municipality", 18),

                     new Municipality(93,"हरिपुर्वा नगरपालिका","Haripurwa  Municipality", 18),

                     new Municipality(94,"हरिवन नगरपालिका","Hariwan  Municipality", 18),

                     new Municipality(95,"बरहथवा नगरपालिका","Barahathawa  Municipality", 18),

                     new Municipality(96,"बलरा नगरपालिका","Balara  Municipality", 18),

                     new Municipality(97,"गोडैटा नगरपालिका","Godaita  Municipality", 18),

                     new Municipality(98,"बागमती नगरपालिका","Bagamati  Municipality", 18),

                     new Municipality(99,"कविलासी नगरपालिका","Kabilasi Municipality", 18),

                     new Municipality(100,"चन्द्रपुर नगरपालिका","Chandrapur  Municipality", 20),


                     new Municipality(101,"गरुडा नगरपालिका","Garuda  Municipality", 20),

                     new Municipality(102,"गौर नगरपालिका","Gaur  Municipality", 20),

                     new Municipality(103,"बौधीमाई नगरपालिका","BoudhiMai Municipality", 20),

                     new Municipality(104,"बृन्दावन नगरपालिका","Brindaban Municipality", 20),

                     new Municipality(105,"देवाही गोनाही नगरपालिका","Devahi Gonahi Municipality", 20),

                     new Municipality(106,"गढीमाई नगरपालिका","GadhiMai Municipality", 20),

                     new Municipality(107,"गुजरा नगरपालिका","Gujara Municipality", 20),

                     new Municipality(108,"कटहरिया नगरपालिका","Katahariya Municipality", 20),

                     new Municipality(109,"माधव नारायण नगरपालिका","Madhav Narayan Municipality", 20),

                     new Municipality(110,"मौलापुर नगरपालिका","Moulapur Municipality", 20),

                     new Municipality(111,"फतुवाबिजयपुर नगरपालिका","Phatuwa Bijayapur Municipality", 20),

                     new Municipality(112,"ईशनाथ नगरपालिका","IshNath Municipality", 20),

                     new Municipality(113,"परोहा नगरपालिका","Paroha Municipality", 20),

                     new Municipality(114,"राजपुर नगरपालिका","Rajpur Municipality", 20),

                     new Municipality(115,"राजदेवी नगरपालिका","RajDevi Municipality", 20),

                     new Municipality(116,"कलैया उपमहानगरपालिका","Kalaiya Sub Metropolitan City", 21),

                     new Municipality(117,"जीतपुर सिमरा उपमहानगरपालिका","Jitpur Simara Sub Metropolitan City", 21),

                     new Municipality(118,"कोल्हवी नगरपालिका","Kolhabi  Municipality", 21),

                     new Municipality(119,"निजगढ नगरपालिका","Nijgadh  Municipality", 21),

                     new Municipality(120,"महागढीमाई नगरपालिका","Maha Gahdimai  Municipality", 21),

                     new Municipality(121,"सिम्रौनगढ नगरपालिका","Simraun Gadh  Municipality", 21),

                     new Municipality(122,"पचरौता नगरपालिका","PachaRouta Municipality", 21),

                     new Municipality(123,"बिरगंज महानगरपालिका","Birjung Metropolitan City", 22),

                     new Municipality(124,"पोखरिया नगरपालिका","Pokhariya  Municipality", 22),

                     new Municipality(125,"बहुदरमाई नगरपालिका","Bahudarmai Municipality", 22),

                     new Municipality(126,"पर्सागढी नगरपालिका","Parsagadhi Municipality", 22),

                     new Municipality(127,"कमलामाई नगरपालिका","Kamalamai  Municipality", 23),

                     new Municipality(128,"दुधौली नगरपालिका","Dudhauli  Municipality", 23),

                     new Municipality(129,"मन्थली नगरपालिका","Manthali  Municipality", 24),

                     new Municipality(130,"रामेछाप नगरपालिका","Ramechhap  Municipality", 24),

                     new Municipality(131,"जिरी नगरपालिका","Jiri  Municipality", 25),

                     new Municipality(132,"भिमेश्वर नगरपालिका","Bhimeshwor  Municipality", 25),

                     new Municipality(133,"चौतारा साँगाचोकगढी नगरपालिका","Chautara Sangachokgadhi Municipality", 27),

                     new Municipality(134,"बाह्रविसे नगरपालिका","Barhabise Municipality", 27),

                     new Municipality(135,"मेलम्ची नगरपालिका","Melamchi Municipality", 27),

                     new Municipality(136,"धुलिखेल नगरपालिका","Dhulikhel  Municipality", 29),

                     new Municipality(137,"बनेपा नगरपालिका","Baneps  Municipality", 29),

                     new Municipality(138,"पनौती नगरपालिका","Panauti  Municipality", 29),

                     new Municipality(139,"पाँचखाल नगरपालिका","Panchkhal Municipality", 29),

                     new Municipality(140,"नमोबुद्ध नगरपालिका","Namobuddha  Municipality", 29),

                     new Municipality(141,"मण्डनदेउपुर नगरपालिका"," Mandan Deupur Municipality", 29),

                     new Municipality(142,"ललितपुर महानगरपालिका","Lalitpur Metropolitan City", 30),

                     new Municipality(143,"गोदावरी नगरपालिका","Godawari Municipality", 30),

                     new Municipality(144,"महालक्ष्मी नगरपालिका","MahaLaxmi  Municipality", 30),

                     new Municipality(145,"चाँगुनारायण नगरपालिका","Changunarayan  Municipality", 26),

                     new Municipality(146,"भक्तपुर नगरपालिका","Bhaktapur  Municipality", 26),

                     new Municipality(147,"मध्यपुर थिमी नगरपालिका","Madhyapur Thimi Municipality", 26),

                     new Municipality(148,"सूर्यविनायक नगरपालिका","Surya Binayak  Municipality", 26),

                     new Municipality(149,"काठमाण्डौं महानगरपालिका","Kathmandu Metropolitan City", 28),

                     new Municipality(150,"कागेश्वरी मनोहरा नगरपालिका","Kageswori-Manohara  Municipality", 28),

                     new Municipality(151,"कीर्तिपुर नगरपालिका","Kirtipur  Municipality", 28),

                     new Municipality(152,"गोकर्णेश्वर नगरपालिका","Gokarneshwor  Municipality", 28),

                     new Municipality(153,"चन्द्रागिरी नगरपालिका","Chandragiri  Municipality", 28),

                     new Municipality(154,"टोखा नगरपालिका","Tokha  Municipality", 28),

                     new Municipality(155,"तारकेश्वर नगरपालिका","Tarkeshwor  Municipality", 28),

                     new Municipality(156,"दक्षिणकाली नगरपालिका","Daxinkali  Municipality", 28),

                     new Municipality(157,"नागार्जुन नगरपालिका","Nagarjun  Municipality", 28),

                     new Municipality(158,"बुढानिलकण्ठ नगरपालिका","Budhanialkantha  Municipality", 28),

                     new Municipality(159,"शङ्खरापुर नगरपालिका","Sankharapur  Municipality", 28),

                     new Municipality(160,"विदुर नगरपालिका","Bidur  Municipality", 31),

                     new Municipality(161,"बेलकोटगढी नगरपालिका","Belkot Gadhi  Municipality", 31),

                     new Municipality(162,"धुनीबेंशी नगरपालिका","Dhunibesi  Municipality", 33),

                     new Municipality(163,"निलकण्ठ नगरपालिका","Nilkantha  Municipality", 33),

                     new Municipality(164,"हेटौडा उपमहानगरपालिका","Hetauda Sub Metropolitan City", 35),

                     new Municipality(165,"थाहा नगरपालिका","Thaha Municipality", 35),

                     new Municipality(166,"भरतपुर महानगरपालिका","Bharatpur Metropolitan City", 34),

                     new Municipality(167,"कालिका नगरपालिका","Kalika  Municipality", 34),

                     new Municipality(168,"खैरहनी नगरपालिका","Khairhani  Municipality", 34),

                     new Municipality(169,"माडी नगरपालिका","Madi  Municipality", 34),

                     new Municipality(170,"रत्ननगर नगरपालिका","Ratna Nagar  Municipality", 34),

                     new Municipality(171,"राप्ती नगरपालिका","Rapti  Municipality", 34),

                     new Municipality(172,"गोरखा नगरपालिका","Gorkha  Municipality", 36),

                     new Municipality(173,"पालुङटार नगरपालिका","Palungtar  Municipality", 36),

                     new Municipality(174,"बेसीशहर नगरपालिका","Besishahar  Municipality", 37),

                     new Municipality(175,"मध्यनेपाल नगरपालिका","Madhya Nepal  Municipality", 37),

                     new Municipality(176,"रार्इनास नगरपालिका","Rainas  Municipality", 37),

                     new Municipality(177,"सुन्दरबजार नगरपालिका","Sundarbazar  Municipality", 37),

                     new Municipality(178,"भानु नगरपालिका","Bhanu  Municipality", 46),

                     new Municipality(179,"भिमाद नगरपालिका","Bhimad  Municipality", 46),

                     new Municipality(180,"व्यास नगरपालिका","Byas  Municipality", 46),

                     new Municipality(181,"शुक्लागण्डकी नगरपालिका","Sukla Gandaki  Municipality", 46),

                     new Municipality(182,"गल्याङ नगरपालिका","Galyang  Municipality", 45),

                     new Municipality(183,"चापाकोट नगरपालिका","Chapakot  Municipality", 45),

                     new Municipality(184,"पुतलीबजार नगरपालिका","Putalibazar  Municipality", 45),

                     new Municipality(185,"भीरकोट नगरपालिका","Bhirkot  Municipality", 45),

                     new Municipality(186,"वालिङ नगरपालिका","Waling  Municipality", 45),

                     new Municipality(187,"पोखरा महानगरपालिका","Pokhara  Metropolitan City", 38),

                     new Municipality(188,"बेनी नगरपालिका","Beni Municipality", 42),

                     new Municipality(189,"कुश्मा नगरपालिका","Kushma  Municipality", 44),

                     new Municipality(190,"फलेवास नगरपालिका","Phalebas  Municipality", 44),

                     new Municipality(191,"बागलुङ नगरपालिका","Baglung  Municipality", 39),

                     new Municipality(192,"गल्कोट नगरपालिका","Galkot  Municipality", 39),

                     new Municipality(193,"जैमूनी नगरपालिका","Jaimini  Municipality", 39),

                     new Municipality(194,"ढोरपाटन नगरपालिका","Dhorpatan  Municipality", 39),

                     new Municipality(195,"कावासोती नगरपालिका","Kawasoti  Municipality", 43),

                     new Municipality(196,"गैडाकोट नगरपालिका","Gaindakot  Municipality", 43),

                     new Municipality(197,"देवचुली नगरपालिका","Devchuli  Municipality", 43),

                     new Municipality(198,"मध्यविन्दु नगरपालिका","Madhya Bindu Municipality", 43),

                     new Municipality(199,"मुसिकोट नगरपालिका","Musikot  Municipality", 47),

                     new Municipality(200,"रेसुङ्गा नगरपालिका","Resunga  Municipality", 47),


                     new Municipality(201,"रामपुर नगरपालिका","Rampur  Municipality", 48),

                     new Municipality(202,"तानसेन नगरपालिका","Tansen  Municipality", 48),

                     new Municipality(203,"बुटवल उपमहानगरपालिका","Butwal Municipality", 49),

                     new Municipality(204,"देवदह नगरपालिका","Devdaha  Municipality", 49),

                     new Municipality(205,"लुम्बिनी सांस्कृतिक नगरपालिका","Lumbini Sanskritik  Municipality", 49),

                     new Municipality(206,"सैनामैना नगरपालिका","Siddharthanagar  Municipality", 49),

                     new Municipality(207,"सिद्धार्थनगर नगरपालिका","Saina Maina  Municipality", 49),

                     new Municipality(208,"तिलोत्तमा नगरपालिका","Tilottama  Municipality", 49),

                     new Municipality(209,"कपिलवस्तु नगरपालिका","Kapilbastu  Municipality", 50),

                     new Municipality(210,"बुद्धभुमी नगरपालिका","Buddabhumi  Municipality", 50),

                     new Municipality(211,"शिवराज नगरपालिका","Shivaraj  Municipality", 50),

                     new Municipality(212,"महाराजगंज नगरपालिका","Maharajganj  Municipality", 50),

                     new Municipality(213,"कृष्णनगर नगरपालिका","Krishna Nagar  Municipality", 50),

                     new Municipality(214,"बाणगंगा नगरपालिका","Banganga  Municipality", 50),

                     new Municipality(215,"सन्धिखर्क नगरपालिका","Sandhikharka  Municipality", 51),

                     new Municipality(216,"शितगंगा नगरपालिका","Shit Ganga  Municipality", 51),

                     new Municipality(217,"भूमिकास्थान नगरपालिका","Bhumikasthan  Municipality", 51),

                     new Municipality(218,"प्यूठान नगरपालिका","Pyuthan Municipality", 52),

                     new Municipality(219,"स्वर्गद्वारी नगरपालिका","Swargadwari  Municipality", 52),

                     new Municipality(220,"रोल्पा नगरपालिका","Rolpa Municipality", 53),

                     new Municipality(221,"तुल्सीपुर उपमहानगरपालिका","Tulsipur Sub Metropolitan City", 54),

                     new Municipality(222,"घोराही उपमहानगरपालिका","Ghorahi Sub Metropolitan City", 54),

                     new Municipality(223,"लमही नगरपालिका","Lamahi  Municipality", 54),

                     new Municipality(224,"नेपालगंज उपमहानगरपालिका","Nepalgunj Sub Metropolitan City", 55),

                     new Municipality(225,"कोहलपुर नगरपालिका","Kohalpur  Municipality", 55),

                     new Municipality(226,"गुलरिया नगरपालिका","Gulariya  Municipality", 56),

                     new Municipality(227,"मधुवन नगरपालिका","Maduvan  Municipality", 56),

                     new Municipality(228,"राजापुर नगरपालिका","Rajapur Municipality", 56),

                     new Municipality(229,"ठाकुरबाबा नगरपालिका","Thakura Baba  Municipality", 56),

                     new Municipality(230,"बाँसगढी नगरपालिका","Bansgadhi  Municipality", 56),

                     new Municipality(231,"बारबर्दिया नगरपालिका","Bar Bardiya  Municipality", 56),

                     new Municipality(232,"बर्दघाट नगरपालिका","Bardaghat Municipality", 57),

                     new Municipality(233,"रामग्राम नगरपालिका","Ramgram  Municipality", 57),

                     new Municipality(234,"सुनवल नगरपालिका","Sunwal  Municipality", 57),

                     new Municipality(235,"शारदा नगरपालिका","Sharada  Municipality", 59),

                     new Municipality(236,"बागचौर नगरपालिका","Bagchaur  Municipality", 59),

                     new Municipality(237,"बनगाड कुपिण्डे नगरपालिका","Bangad Kupinde  Municipality", 59),

                     new Municipality(238,"बीरेन्द्रनगर नगरपालिका","Birendra Nagar  Municipality", 66),

                     new Municipality(239,"भेरीगंगा नगरपालिका","Bheri Ganga  Municipality", 66),

                     new Municipality(240,"गुर्भाकोट नगरपालिका","Gurbhakot  Municipality", 66),

                     new Municipality(241,"पञ्चपुरी नगरपालिका","Panchapuri  Municipality", 66),

                     new Municipality(242,"लेकवेशी नगरपालिका","Lek Besi  Municipality", 66),

                     new Municipality(243,"नारायण नगरपालिका","Narayan  Municipality", 67),

                     new Municipality(244,"दुल्लु नगरपालिका","Dullu  Municipality", 67),

                     new Municipality(245,"चामुण्डा विन्द्रासैनी नगरपालिका","Chamunda Bindrasaini  Municipality", 67),

                     new Municipality(246,"आठबीस नगरपालिका","Aathabis  Municipality", 67),

                     new Municipality(247,"भेरी नगरपालिका","Bheri  Municipality", 68),

                     new Municipality(248,"छेडागाड नगरपालिका","Chhedagad  Municipality", 68),

                     new Municipality(249,"नलगाड नगरपालिका","Triveni Nalgad  Municipality", 68),

                     new Municipality(250,"ठूली भेरी नगरपालिका","Thuli Bheri  Municipality", 61),

                     new Municipality(251,"त्रिपुरासुन्दरी नगरपालिका","Tripura Sundari  Municipality", 61),

                     new Municipality(252,"चन्दननाथ नगरपालिका","Chandannath  Municipality", 63),

                     new Municipality(253,"खाँडाचक्र नगरपालिका","Khandachakra  Municipality", 64),

                     new Municipality(254,"रास्कोट नगरपालिका","Raskot  Municipality", 64),

                     new Municipality(255,"तिलागुफा नगरपालिका","Tila Gupha  Municipality", 64),

                     new Municipality(256,"छायाँनाथ रारा नगरपालिका","Chhaya Nath  Municipality", 65),

                     new Municipality(257,"मुसिकोट नगरपालिका","Musikot  Municipality", 60),

                     new Municipality(258,"चौरजहारी नगरपालिका","Chaurjahari  Municipality", 60),

                     new Municipality(259,"आठबिसकोट नगरपालिका","Aathabiskot  Municipality", 60),

                     new Municipality(260,"बडीमालिका नगरपालिका","Badi Malika  Municipality", 73),

                     new Municipality(261,"त्रिवेणी नगरपालिका","Triveni  Municipality", 73),

                     new Municipality(262,"बुढीगंगा नगरपालिका","Budhi Ganga  Municipality", 73),

                     new Municipality(263,"बुढीनन्दा नगरपालिका","Budhi Nanda  Municipality", 73),

                     new Municipality(264,"जयपृथ्वी नगरपालिका","Jaya Prithvi  Municipality", 72),

                     new Municipality(265,"बुंगल नगरपालिका","Bungal  Municipality", 72),

                     new Municipality(266,"मंगलसेन नगरपालिका","Mangalsen  Municipality", 70),

                     new Municipality(267,"कमलबजार नगरपालिका","Kamalbazar Municipality", 70),

                     new Municipality(268,"साँफेबगर नगरपालिका","Sanfebagar Municipality", 70),

                     new Municipality(269,"पन्चदेवल विनायक नगरपालिका","Panchdeval Binayak  Municipality", 70),

                     new Municipality(270,"दिपायल सिलगढी नगरपालिका","Dipayal-Silgadi  Municipality", 71),

                     new Municipality(271,"शिखर नगरपालिका","Shikhar  Municipality", 71),

                     new Municipality(272,"धनगढी उपमहानगरपालिका","Dhangadhi Sub Metropolitan City", 69),

                     new Municipality(273,"टिकापुर नगरपालिका","Tikapur  Municipality", 69),

                     new Municipality(274,"घोडाघोडी नगरपालिका","Ghodaghodi  Municipality", 69),

                     new Municipality(275,"लम्कीचुहा नगरपालिका","Lamki-Chuha  Municipality", 69),

                     new Municipality(276,"भजनी नगरपालिका","Bhajani  Municipality", 69),

                     new Municipality(277,"गोदावरी नगरपालिका","Godavari  Municipality", 69),

                     new Municipality(278,"गौरीगंगा नगरपालिका","Gauri Ganga  Municipality", 69),

                     new Municipality(279,"भीमदत्त नगरपालिका","Bhimdatta  Municipality", 74),

                     new Municipality(280,"पुर्नवास नगरपालिका","Punarbas  Municipality", 74),

                     new Municipality(281,"वेदकोट नगरपालिका","Bedkot  Municipality", 74),

                     new Municipality(282,"महाकाली नगरपालिका","Mahakali  Municipality", 74),

                     new Municipality(283,"शुक्लाफाँटा नगरपालिका","Shuklaphanta  Municipality", 74),

                     new Municipality(284,"बेलौरी नगरपालिका","Belauri  Municipality", 74),

                     new Municipality(285,"कृष्णपुर नगरपालिका","Krishnapur  Municipality", 74),

                     new Municipality(286,"अमरगढी नगरपालिका","Amargadhi  Municipality", 75),

                     new Municipality(287,"परशुराम नगरपालिका","Parashuram  Municipality", 75),

                     new Municipality(288,"दशरथचन्द नगरपालिका","Dasharath Chanda  Municipality", 76),

                     new Municipality(289,"पाटन नगरपालिका","Patan  Municipality", 76),

                     new Municipality(290,"मेलौली नगरपालिका","Melauli  Municipality", 76),

                     new Municipality(291,"पुर्चौडी नगरपालिका","Purchaundi  Municipality", 76),

                     new Municipality(292,"महाकाली नगरपालिका","Mahakali  Municipality", 77),

                     new Municipality(293,"शैल्यशिखर नगरपालिका","Sailya Shikhar  Municipality", 77),
                };

                await _context.Municipality.AddRangeAsync(municipalities);
                await _unitOfWork.SaveChangesAsync();

            }

        }

        #endregion

        #region Vdc
        public async Task SeedVdc()
        {
            if(!await _context.Vdcs.AnyAsync())
            {
             
                    var Vdcs = new List<Vdc>()
            {
                  new Vdc(1, "हतुवागढी गाउँपालिका", "Hatuwagadhi Rural Municipality", 10),

                  new Vdc(2, "रामप्रसाद राई गाउँपालिका", "Ramprasad Rai Rural Municipality", 10),

                  new Vdc(3, "आमचोक गाउँपालिका", "Aamchok Rural Municipality", 10),

                  new Vdc(4, "टेम्केमैयुङ गाउँपालिका", "Tyamkemaiyum Rural Municipality", 10),

                  new Vdc(5, "अरुण गाउँपालिका", "Arun Rural Municipality", 10),

                  new Vdc(6, "पौवादुङमा गाउँपालिका", "Pauwadungma Rural Municipality", 10),

                  new Vdc(7, "साल्पासिलिछो गाउँपालिका", "Salpa Silichho Rural Municipality", 10),

                  new Vdc(8, "सागुरीगढी गाउँपालिका", "Sangurigadhi Rural Municipality", 7),

                  new Vdc(9, "चौविसे गाउँपालिका", "Chaubise Rural Municipality", 7),

                  new Vdc(10, "सहिदभुमी गाउँपालिका", "Sahidbhumi Rural Municipality", 7),

                  new Vdc(11, "छथर जोरपाटी गाउँपालिका", "Chhathar Jorpati Rural Municipality", 7),

                  new Vdc(12, "फाकफोकथुम गाउँपालिका", "Phakphokthum Rural Municipality", 3),

                  new Vdc(13, "माईजोगमाई गाउँपालिका", "Mai Jogmai Rural Municipality", 3),

                  new Vdc(14, "चुलाचुली गाउँपालिका", "Chulachuli Rural Municipality", 3),

                  new Vdc(15, "रोङ गाउँपालिका", "Rong Rural Municipality", 3),

                  new Vdc(16, "माङसेबुङ गाउँपालिका", "Mangsebung Rural Municipality", 3),

                  new Vdc(17, "सन्दकपुर गाउँपालिका", "Sandakpur Rural Municipality", 3),

                  new Vdc(18, "कमल गाउँपालिका", "Kamal Rural Municipality", 4),

                  new Vdc(19, "बुद्धशान्ति गाउँपालिका", "Buddha Shanti Rural Municipality", 4),

                  new Vdc(20, "कचनकवल गाउँपालिका", "Kachankawal Rural Municipality", 4),

                  new Vdc(21, "झापा गाउँपालिका", "Jhapa Rural Municipality", 4),

                  new Vdc(22, "बाह्रदशी गाउँपालिका", "Barhadashi Rural Municipality", 4),

                  new Vdc(23, "गौरीगंज गाउँपालिका", "Gaurigunj Rural Municipality", 4),

                  new Vdc(24, "हल्दीवारी गाउँपालिका", "Haldibari Rural Municipality", 4),

                  new Vdc(25, "खोटेहाङ गाउँपालिका", "Khotehang Rural Municipality", 13),

                  new Vdc(26, "दिप्रुङ चुइचुम्मा गाउँपालिका", "Diprung Chuichumma Rural Municipality", 13),

                  new Vdc(27, "ऐसेलुखर्क गाउँपालिका", "Aiselukharka Rural Municipality", 13),

                  new Vdc(28, "जन्तेढुंगा गाउँपालिका", "Jantedhunga Rural Municipality", 13),

                  new Vdc(29, "केपिलासगढी गाउँपालिका", "Kepilasgadhi Rural Municipality", 13),

                  new Vdc(30, "बराहपोखरी गाउँपालिका", "Barahpokhari Rural Municipality", 13),

                  new Vdc(31, "रावा बेसी गाउँपालिका", "Rawa Besi Rural Municipality", 13),

                  new Vdc(32, "साकेला गाउँपालिका", "Sakela Rural Municipality", 13),

                  new Vdc(33, "जहदा गाउँपालिका", "Jahada Rural Municipality", 5),

                  new Vdc(34, "बुढीगंगा गाउँपालिका", "Budi Ganga Rural Municipality", 5),

                  new Vdc(35, "कटहरी गाउँपालिका", "Katahari Rural Municipality", 5),

                  new Vdc(36, "धनपालथान गाउँपालिका", "Dhanpalthan Rural Municipality", 5),

                  new Vdc(37, "कानेपोखरी गाउँपालिका", "Kanepokhari Rural Municipality", 5),

                  new Vdc(38, "ग्रामथान गाउँपालिका", "Gramthan Rural Municipality", 5),

                  new Vdc(39, "केरावारी गाउँपालिका", "Kerabari Rural Municipality", 5),

                  new Vdc(40, "मिक्लाजुङ गाउँपालिका", "Miklajung Rural Municipality", 5),

                  new Vdc(41, "मानेभञ्ज्याङ गाउँपालिका", "Mane Bhanjyang Rural Municipality", 12),

                  new Vdc(42, "चम्पादेवी गाउँपालिका", "Champadevi Rural Municipality", 12),

                  new Vdc(43, "सुनकोशी गाउँपालिका", "Sunkoshi Rural Municipality", 12),

                  new Vdc(44, "मोलुङ गाउँपालिका", "Molung Rural Municipality", 12),

                  new Vdc(45, "चिसंखुगढी गाउँपालिका", "Chisankhugadhi Rural Municipality", 12),

                  new Vdc(46, "खिजिदेम्बा गाउँपालिका", "Khiji Demba Rural Municipality", 12),

                  new Vdc(47, "लिखु गाउँपालिका", "Likhu Rural Municipality", 12),

                  new Vdc(48, "मिक्लाजुङ गाउँपालिका", "Miklajung Rural Municipality", 2),

                  new Vdc(49, "फाल्गुनन्द गाउँपालिका", "Phalgunanda Rural Municipality", 2),

                  new Vdc(50, "हिलिहाङ गाउँपालिका", "Hilihang Rural Municipality", 2),

                  new Vdc(51, "फालेलुङ गाउँपालिका", "Phalelung Rural Municipality", 2),

                  new Vdc(52, "याङवरक गाउँपालिका", "Yangbarak Rural Municipality", 2),

                  new Vdc(53, "कुम्मायक गाउँपालिका", "Kummayak Rural Municipality", 2),

                  new Vdc(54, "तुम्बेवा गाउँपालिका", "Tumbewa Rural Municipality", 2),

                  new Vdc(55, "मकालु गाउँपालिका", "Makalu Rural Municipality", 9),

                  new Vdc(56, "सिलीचोङ गाउँपालिका", "Silichong Rural Municipality", 9),

                  new Vdc(57, "सभापोखरी गाउँपालिका", "Sabhapokhari Rural Municipality", 9),

                  new Vdc(58, "चिचिला गाउँपालिका", "Chichila Rural Municipality", 9),

                  new Vdc(59, "भोटखोला गाउँपालिका", "Bhot Khola Rural Municipality", 9),

                  new Vdc(60, "थुलुङ दुधकोशी गाउँपालिका", "Thulung Dudhkoshi Rural Municipality", 11),

                  new Vdc(61, "नेचासल्यान गाउँपालिका", "Necha Salyan Rural Municipality", 11),

                  new Vdc(62, "माप्य दुधकोशी गाउँपालिका", "Mapya Dudhkoshi Rural Municipality", 11),

                  new Vdc(63, "महाकुलुङ गाउँपालिका", "Maha Kulung Rural Municipality", 11),

                  new Vdc(64, "सोताङ गाउँपालिका", "Sotang Rural Municipality", 11),

                  new Vdc(65, "खुम्बु पासाङल्हमु गाउँपालिका", "Khumbu Pasang Lhamu Rural Municipality", 11),

                  new Vdc(66, "लिखुपिके  गाउँपालिका", "Likhu Pike Rural Municipality", 11),

                  new Vdc(67, "कोशी गाउँपालिका", "Koshi Rural Municipality", 6),

                  new Vdc(68, "हरिनगर गाउँपालिका", "Harinagar Rural Municipality", 6),

                  new Vdc(69, "भोक्राहा गाउँपालिका", "Bhokraha Rural Municipality", 6),

                  new Vdc(70, "देवानगन्ज गाउँपालिका", "Dewangunj Rural Municipality", 6),

                  new Vdc(71, "गढी गाउँपालिका", "Gadhi Rural Municipality", 6),

                  new Vdc(72, "बर्जु गाउँपालिका", "Barju Rural Municipality", 6),

                  new Vdc(73, "सिरीजङ्घा गाउँपालिका", "Sirijangha Rural Municipality", 1),

                  new Vdc(74, "आठराई त्रिवेणी गाउँपालिका", "Aathrai Triveni Rural Municipality", 1),

                  new Vdc(75, "पाथिभरा याङवरक गाउँपालिका", "Pathibhara Yangbarak Rural Municipality", 1),

                  new Vdc(76, "मेरिङदेन गाउँपालिका", "Meringden Rural Municipality", 1),

                  new Vdc(77, "सिदिङ्वा गाउँपालिका", "Sidingwa Rural Municipality", 1),

                  new Vdc(78, "फक्ताङलुङ गाउँपालिका", "Phaktanglung Rural Municipality", 1),

                  new Vdc(79, "मैवाखोला गाउँपालिका", "Maiwa Khola Rural Municipality", 1),

                  new Vdc(80, "मिक्वाखोला गाउँपालिका", "Mikwa Khola Rural Municipality", 1),

                  new Vdc(81, "आठराई गाउँपालिका", "Aathrai Rural Municipality", 8),

                  new Vdc(82, "फेदाप गाउँपालिका", "Phedap Rural Municipality", 8),

                  new Vdc(83, "छथर गाउँपालिका", "Chhathar Rural Municipality", 8),

                  new Vdc(84, "मेन्छयायेम गाउँपालिका", "Menchayayem Rural Municipality", 8),

                  new Vdc(85, "उदयपुरगढी गाउँपालिका", "Udayapurgadhi Rural Municipality", 14),

                  new Vdc(86, "रौतामाई गाउँपालिका", "Rautamai Rural Municipality", 14),

                  new Vdc(87, "ताप्ली गाउँपालिका", "Tapli Rural Municipality", 14),

                  new Vdc(88, "लिम्चुङ्ग्बुङ गाउँपालिका", "Sunkoshi Rural Municipality", 14),

                  new Vdc(89, "सुवर्ण  गाउँपालिका", "Subarna Rural Municipality", 21),

                  new Vdc(90, "आदर्श कोतवाल गाउँपालिका", "Adarsha Kotwal Rural Municipality", 21),

                  new Vdc(91, "बारागढी गाउँपालिका", "Baragadhi Rural Municipality", 21),

                  new Vdc(92, "फेटा गाउँपालिका", "Pheta Rural Municipality", 21),

                  new Vdc(93, "करैयामाई गाउँपालिका", "Karaiyamai Rural Municipality", 21),

                  new Vdc(94, "प्रसौनी गाउँपालिका", "Prasauni Rural Municipality", 21),

                  new Vdc(95, "विश्रामपुर गाउँपालिका", "Bishrampur Rural Municipality", 21),

                  new Vdc(96, "देवताल गाउँपालिका", "Devtal Rural Municipality", 21),

                  new Vdc(97, "परवानीपुर गाउँपालिका", "Parawanipur Rural Municipality", 21),

                  new Vdc(98, "धनौजी   गाउँपालिका", "Mithila Bihari Rural Municipality", 19),

                  new Vdc(99, "औरही   गाउँपालिका", "Aaurahi Rural Municipality", 19),

                  new Vdc(100, "लक्ष्मीनिया  गाउँपालिका", "Laksminiya Rural Municipality", 19),


                  new Vdc(101, "मुखियापट्टी मुसहरमिया   गाउँपालिका", "Mukhiyapatti Musaharmiya Rural Municipality", 19),

                  new Vdc(102, "जनकनन्दिनी   गाउँपालिका", "Janak Nandini Rural Municipality", 19),

                  new Vdc(103, "बटेश्वर   गाउँपालिका", "Bateshwar Rural Municipality", 19),

                  new Vdc(104, "सोनमा गाउँपालिका", "Sonama Rural Municipality", 17),

                  new Vdc(105, "पिपरा गाउँपालिका", "Pipara Rural Municipality", 17),

                  new Vdc(106, "साम्सी गाउँपालिका", "Samsi Rural Municipality", 17),

                  new Vdc(107, "एकडारा गाउँपालिका", "Ekdara Rural Municipality", 17),

                  new Vdc(108, "महोत्तरी गाउँपालिका", "Mahottari Rural Municipality", 17),

                  new Vdc(109, "सखुवा प्रसौनी गाउँपालिका", "Sakhuwa Prasauni Rural Municipality", 22),

                  new Vdc(110, "जगरनाथपुर गाउँपालिका", "Jagarnathpur Rural Municipality", 22),

                  new Vdc(111, "छिपहरमाई गाउँपालिका", "Chhipaharmai Rural Municipality", 22),

                  new Vdc(112, "बिन्दबासिनी गाउँपालिका", "Bindabasini Rural Municipality", 22),

                  new Vdc(113, "पटेर्वा सुगौली गाउँपालिका", "Paterwa Sugauli Rural Municipality", 22),

                  new Vdc(114, "जिरा भवानी गाउँपालिका", "Jeera Bhavani Rural Municipality", 22),

                  new Vdc(115, "कालिकामाई गाउँपालिका", "Kalikamai Rural Municipality", 22),

                  new Vdc(116, "पकाहा मैनपुर गाउँपालिका", "Pakaha Mainpur Rural Municipality", 22),

                  new Vdc(117, "ठोरी गाउँपालिका", "Thori Rural Municipality", 22),

                  new Vdc(118, "धोबीनी गाउँपालिका", "Dhobini Rural Municipality", 22),

                  new Vdc(119, "दुर्गा भगवती गाउँपालिका", "Durga Bhagawati Rural Municipality", 20),

                  new Vdc(120, "यमुनामाई गाउँपालिका", "Yamunamai Rural Municipality", 20),

                  new Vdc(121, "तिलाठी कोईलाडी गाउँपालिका", "Tilathi Koiladi Rural Municipality", 15),

                  new Vdc(122, "बेल्ही चपेना गाउँपालिका", "Belhi Chapena Rural Municipality", 15),

                  new Vdc(123, "छिन्नमस्ता गाउँपालिका", "Chhinnamasta Rural Municipality", 15),

                  new Vdc(124, "महादेवा गाउँपालिका", "Mahadeva Rural Municipality", 15),

                  new Vdc(125, "अग्निसाइर कृष्णासवरन गाउँपालिका", "Aagnisaira Krishnasawaran Rural Municipality", 15),

                  new Vdc(126, "रुपनी गाउँपालिका", "Rupani Rural Municipality", 15),

                  new Vdc(143, "बलान-बिहुल गाउँपालिका", "Balan-Bihul Rural Municipality", 15),

                  new Vdc(144, "बिष्णुपुर गाउँपालिका", "Bishnupur Rural Municipality", 15),

                  new Vdc(152, "तिरहुत गाउँपालिका", "Tirhut Rural Municipality", 15),

                  new Vdc(153, "चक्रघट्टा  गाउँपालिका", "Chakraghatta Rural Municipality", 18),

                  new Vdc(154, "रामनगर   गाउँपालिका", "Ramnagar Rural Municipality", 18),

                  new Vdc(155, "विष्णु   गाउँपालिका", "Bishnu Rural Municipality", 18),

                  new Vdc(156, "ब्रह्मपुरी   गाउँपालिका", "Bramhapuri Rural Municipality", 18),

                  new Vdc(157, "चन्द्रनगर   गाउँपालिका", "Chandranagar Rural Municipality", 18),

                  new Vdc(158, "धनकौल   गाउँपालिका", "Dhankaul Rural Municipality", 18),

                  new Vdc(159, "कौडेना   गाउँपालिका", "Kaudena Rural Municipality", 18),

                  new Vdc(160, "पर्सा   गाउँपालिका", "Parsa Rural Municipality", 18),

                  new Vdc(165, "बसबरीया   गाउँपालिका", "Basbariya Rural Municipality", 18),

                  new Vdc(166, "लक्ष्मीपुर पतारी गाउँपालिका", "Laksmipur Patari Rural Municipality", 16),

                  new Vdc(167, "बरियारपट्टी गाउँपालिका", "Bariyarpatti Rural Municipality", 16),

                  new Vdc(168, "औरही गाउँपालिका", "Aaurahi Rural Municipality", 16),

                  new Vdc(169, "अर्नमा गाउँपालिका", "Arnama Rural Municipality", 16),

                  new Vdc(170, "भगवानपुर गाउँपालिका", "Bhagawanpur Rural Municipality", 16),

                  new Vdc(171, "नरहा गाउँपालिका", "Naraha Rural Municipality", 16),

                  new Vdc(172, "नवराजपुर गाउँपालिका", "Nawarajpur Rural Municipality", 16),

                  new Vdc(173, "सखुवानान्कारकट्टी गाउँपालिका", "Sakhuwanankarkatti Rural Municipality", 16),

                  new Vdc(174, "विष्णुपुर गाउँपालिका", "Bishnupur Rural Municipality", 16),

                  new Vdc(177, "इच्छाकामना गाउँपालिका", "Ichchhakamana Rural Municipality", 34),

                  new Vdc(178, "थाक्रे गाउँपालिका", "Thakre Rural Municipality", 33),

                  new Vdc(179, "बेनीघाट रोराङ्ग गाउँपालिका", "Benighat Rorang Rural Municipality", 33),

                  new Vdc(180, "गल्छी गाउँपालिका", "Galchhi Rural Municipality", 33),

                  new Vdc(181, "गजुरी गाउँपालिका", "Gajuri Rural Municipality", 33),

                  new Vdc(182, "ज्वालामूखी गाउँपालिका", "Jwalamukhi Rural Municipality", 33),

                  new Vdc(183, "सिद्धलेक गाउँपालिका", "Siddhalekh Rural Municipality", 33),

                  new Vdc(186, "त्रिपुरासुन्दरी गाउँपालिका", "Tripura Sundari Rural Municipality", 33),

                  new Vdc(187, "गङ्गाजमुना गाउँपालिका", "Gangajamuna Rural Municipality", 33),

                  new Vdc(188, "नेत्रावती डबजोङ गाउँपालिका", "Netrawati Dabjong Rural Municipality", 33),

                  new Vdc(189, "खनियाबास गाउँपालिका", "Khaniyabas Rural Municipality", 33),

                  new Vdc(190, "रुवी भ्याली गाउँपालिका", "Ruby Valley Rural Municipality", 33),

                  new Vdc(191, "कालिन्चोक गाउँपालिका", "Kalinchok Rural Municipality", 25),

                  new Vdc(194, "मेलुङ्ग गाउँपालिका", "Melung Rural Municipality", 25),

                  new Vdc(195, "शैलुङ्ग गाउँपालिका", "Shailung Rural Municipality", 25),

                  new Vdc(196, "वैतेश्वर गाउँपालिका", "Baiteshwar Rural Municipality", 25),

                  new Vdc(197, "तामाकोशी गाउँपालिका", "Tamakoshi Rural Municipality", 25),

                  new Vdc(198, "विगु गाउँपालिका", "Bigu Rural Municipality", 25),

                  new Vdc(199, "गौरीशङ्कर गाउँपालिका", "Gaurishankar Rural Municipality", 25),

                  new Vdc(200, "रोशी गाउँपालिका", "Roshi Rural Municipality", 29),

                  new Vdc(204, "तेमाल गाउँपालिका", "Temal Rural Municipality", 29),

                  new Vdc(205, "चौंरी देउराली गाउँपालिका", "Chaunri Deurali Rural Municipality", 29),

                  new Vdc(206, "भुम्लु गाउँपालिका", "Bhumlu Rural Municipality", 29),

                  new Vdc(207, "महाभारत गाउँपालिका", "Mahabharat Rural Municipality", 29),

                  new Vdc(208, "बेथानचोक गाउँपालिका", "Bethanchok Rural Municipality", 29),

                  new Vdc(209, "खानीखोला गाउँपालिका", "Khanikhola Rural Municipality", 29),

                  new Vdc(210, "बाग्मति गाउँपालिका", "Bagmati Rural Municipality", 30),

                  new Vdc(211, "कोन्ज्योसोम गाउँपालिका", "Konjyosom Rural Municipality", 30),

                  new Vdc(212, "महाङ्काल गाउँपालिका", "Mahankal Rural Municipality", 30),

                  new Vdc(219, "बकैया गाउँपालिका", "Bakaiya Rural Municipality", 35),

                  new Vdc(220, "मनहरी गाउँपालिका", "Manhari Rural Municipality", 35),

                  new Vdc(221, "बाग्मति गाउँपालिका", "Bagmati Rural Municipality", 35),

                  new Vdc(222, "राक्सिराङ्ग गाउँपालिका", "Raksirang Rural Municipality", 35),

                  new Vdc(223, "मकवानपुरगढी गाउँपालिका", "Makawanpurgadhi Rural Municipality", 35),

                  new Vdc(224, "कैलाश गाउँपालिका", "Kailash Rural Municipality", 35),

                  new Vdc(225, "भीमफेदी गाउँपालिका", "Bhimphedi Rural Municipality", 35),

                  new Vdc(229, "ईन्द्र सरोवर गाउँपालिका", "Indrasarowar Rural Municipality", 35),

                  new Vdc(230, "ककनी गाउँपालिका", "Kakani Rural Municipality", 31),

                  new Vdc(231, "दुप्चेश्वर गाउँपालिका", "Dupcheshwar Rural Municipality", 31),

                  new Vdc(232, "शिवपुरी गाउँपालिका", "Shivapuri Rural Municipality", 31),

                  new Vdc(233, "तादी गाउँपालिका", "Tadi Rural Municipality", 31),

                  new Vdc(234, "लिखु गाउँपालिका", "Likhu Rural Municipality", 31),

                  new Vdc(235, "सुर्यगढी गाउँपालिका", "Suryagadhi Rural Municipality", 31),

                  new Vdc(236, "पञ्चकन्या गाउँपालिका", "Panchakanya Rural Municipality", 31),

                  new Vdc(237, "तारकेश्वर गाउँपालिका", "Tarkeshwar Rural Municipality", 31),

                  new Vdc(238, "किस्पाङ गाउँपालिका", "Kispang Rural Municipality", 31),

                  new Vdc(239, "म्यागङ गाउँपालिका", "Myagang Rural Municipality", 31),

                  new Vdc(240, "खाँडादेवी गाउँपालिका", "Khandadevi Rural Municipality", 24),

                  new Vdc(241, "लिखु तामाकोशी गाउँपालिका", "Likhu Tamakoshi Rural Municipality", 24),

                  new Vdc(242, "दोरम्बा गाउँपालिका", "Doramba Rural Municipality", 24),

                  new Vdc(243, "गोकुलगङ्गा गाउँपालिका", "Gokulganga Rural Municipality", 24),

                  new Vdc(244, "सुनापती गाउँपालिका", "Sunapati Rural Municipality", 24),

                  new Vdc(245, "उमाकुण्ड गाउँपालिका", "Umakunda Rural Municipality", 24),


                  new Vdc(246, "नौकुण्ड गाउँपालिका", "Naukunda Rural Municipality", 32),

                  new Vdc(247, "कालिका गाउँपालिका", "Kalika Rural Municipality", 32),

                  new Vdc(248, "उत्तरगया गाउँपालिका", "Uttargaya Rural Municipality", 32),

                  new Vdc(249, "गोसाईकुण्ड गाउँपालिका", "Gosaikund Rural Municipality", 32),

                  new Vdc(250, "आमाछोदिङमो गाउँपालिका", "Aamachodingmo Rural Municipality", 32),

                  new Vdc(251, "तिनपाटन गाउँपालिका", "Tinpatan Rural Municipality", 23),

                  new Vdc(252, "मरिण गाउँपालिका", "Marin Rural Municipality", 23),

                  new Vdc(253, "हरिहरपुरगढी गाउँपालिका", "Hariharpurgadhi Rural Municipality", 23),

                  new Vdc(254, "सुनकोशी गाउँपालिका", "Sunkoshi Rural Municipality", 23),

                  new Vdc(255, "गोलन्जर गाउँपालिका", "Golanjor Rural Municipality", 23),

                  new Vdc(256, "फिक्कल गाउँपालिका", "Phikkal Rural Municipality", 23),

                  new Vdc(257, "घ्याङलेख गाउँपालिका", "Ghyanglekh Rural Municipality", 23),

                  new Vdc(260, "र्इन्द्रावती गाउँपालिका", "Indrawati Rural Municipality", 27),

                  new Vdc(261, "पाँचपोखरी थाङपाल गाउँपालिका", "Panchpokhari Thangpal Rural Municipality", 27),

                  new Vdc(262, "जुगल गाउँपालिका", "Jugal Rural Municipality", 27),

                  new Vdc(263, "बलेफी गाउँपालिका", "Balephi Rural Municipality", 27),

                  new Vdc(264, "हेलम्बु गाउँपालिका", "Helambu Rural Municipality", 27),

                  new Vdc(265, "भोटेकोशी गाउँपालिका", "Bhotekoshi Rural Municipality", 27),

                  new Vdc(266, "सुनकोशी गाउँपालिका", "Sunkoshi Rural Municipality", 27),

                  new Vdc(267, "लिसंखु पाखर गाउँपालिका", "Lisankhu Pakhar Rural Municipality", 27),

                  new Vdc(274, "त्रिपुरासुन्दरी गाउँपालिका", "Tripura Sundari Rural Municipality", 27),

                  new Vdc(277, "वडिगाड गाउँपालिका", "Badigad Rural Municipality", 39),

                  new Vdc(278, "काठेखोला गाउँपालिका", "Kathekhola Rural Municipality", 39),

                  new Vdc(279, "निसीखोला गाउँपालिका", "Nisikhola Rural Municipality", 39),

                  new Vdc(280, "बरेङ गाउँपालिका", "Bareng Rural Municipality", 39),

                  new Vdc(281, "ताराखोला गाउँपालिका", "Tarakhola Rural Municipality", 39),

                  new Vdc(282, "तमानखोला गाउँपालिका", "Tamankhola Rural Municipality", 39),

                  new Vdc(283, "शहिद लखन गाउँपालिका", "Shahid Lakhan Rural Municipality", 36),

                  new Vdc(284, "बारपाक सुलीकोट गाउँपालिका", "Barpak Sulikot Rural Municipality", 36),

                  new Vdc(285, "आरूघाट गाउँपालिका", "Aarughat Rural Municipality", 36),

                  new Vdc(290, "सिरानचोक गाउँपालिका", "Siranchok Rural Municipality", 36),

                  new Vdc(291, "गण्डकी गाउँपालिका", "Gandaki Rural Municipality", 36),

                  new Vdc(292, "भिमसेनथापा गाउँपालिका", "Bhimsen Thapa Rural Municipality", 36),

                  new Vdc(293, "अजिरकोट गाउँपालिका", "Ajirkot Rural Municipality", 36),

                  new Vdc(298, "धार्चे गाउँपालिका", "Dharche Rural Municipality", 36),

                  new Vdc(299, "चुम नुव्री गाउँपालिका", "Chum Nubri Rural Municipality", 36),

                  new Vdc(300, "अन्नपुर्ण गाउँपालिका", "Annapurna Rural Municipality", 38),

                  new Vdc(301, "माछापुछ्रे गाउँपालिका", "Machhapuchhre Rural Municipality", 38),

                  new Vdc(302, "मादी गाउँपालिका", "Madi Rural Municipality", 38),

                  new Vdc(303, "रूपा गाउँपालिका", "Rupa Rural Municipality", 38),

                  new Vdc(309, "मर्स्याङदी गाउँपालिका", "Marsyangdi Rural Municipality", 37),

                  new Vdc(310, "दोर्दी गाउँपालिका", "Dordi Rural Municipality", 37),

                  new Vdc(311, "दूधपोखरी गाउँपालिका", "Dudhpokhari Rural Municipality", 37),

                  new Vdc(312, "क्व्होलासोथार गाउँपालिका", "Kwaholasothar Rural Municipality", 37),

                  new Vdc(313, "मनाङ डिस्याङ गाउँपालिका", "Manang Disyang Rural Municipality", 40),

                  new Vdc(314, "नासोँ गाउँपालिका", "Nason Rural Municipality", 40),

                  new Vdc(316, "चामे गाउँपालिका", "Chame Rural Municipality", 40),

                  new Vdc(317, "नार्पा भूमि  गाउँपालिका", "Narpa Bhumi Rural Municipality", 40),

                  new Vdc(318, "घरपझोङ गाउँपालिका", "Gharpajhong Rural Municipality", 41),

                  new Vdc(319, "थासाङ गाउँपालिका", "Thasang Rural Municipality", 41),

                  new Vdc(320, "बारागुङ मुक्तिक्षेत्र गाउँपालिका", "Baragung Muktichhetra Rural Municipality", 41),

                  new Vdc(321, "लोमन्थाङ गाउँपालिका", "Lomanthang Rural Municipality", 41),

                  new Vdc(322, "लो-थेकर दामोदरकुण्ड गाउँपालिका", "Lo-Thekar Damodarkunda Rural Municipality", 41),

                  new Vdc(323, "मालिका गाउँपालिका", "Malika Rural Municipality", 42),

                  new Vdc(324, "मंगला गाउँपालिका", "Mangala Rural Municipality", 42),

                  new Vdc(325, "रघुगंगा गाउँपालिका", "Raghuganga Rural Municipality", 42),

                  new Vdc(326, "धवलागिरी गाउँपालिका", "Dhaulagiri Rural Municipality", 42),

                  new Vdc(327, "अन्नपुर्ण गाउँपालिका", "Annapurna Rural Municipality", 42),

                  new Vdc(328, "हुप्सेकोट गाउँपालिका", "Hupsekot Rural Municipality", 43),

                  new Vdc(330, "विनयी त्रिवेणी गाउँपालिका", "Binayi Triveni Rural Municipality", 43),

                  new Vdc(331, "बुलिङटार गाउँपालिका", "Bulingtar Rural Municipality", 43),

                  new Vdc(332, "बौदीकाली गाउँपालिका", "Baudikali Rural Municipality", 43),

                  new Vdc(333, "जलजला गाउँपालिका", "Jaljala Rural Municipality", 44),

                  new Vdc(334, "मोदी गाउँपालिका", "Modi Rural Municipality", 44),

                  new Vdc(337, "पैयूं गाउँपालिका", "Painyu Rural Municipality", 44),

                  new Vdc(338, "विहादी गाउँपालिका", "Bihadi Rural Municipality", 44),

                  new Vdc(339, "महाशिला गाउँपालिका", "Mahashila Rural Municipality", 44),

                  new Vdc(340, "कालीगण्डकी गाउँपालिका", "Kaligandaki Rural Municipality", 45),

                  new Vdc(341, "विरुवा गाउँपालिका", "Biruwa Rural Municipality", 45),

                  new Vdc(346, "हरिनास गाउँपालिका", "Harinas Rural Municipality", 45),

                  new Vdc(347, "आँधीखोला गाउँपालिका", "Aandhikhola Rural Municipality", 45),

                  new Vdc(348, "अर्जुन चौपारी गाउँपालिका", "Arjun Chaupari Rural Municipality", 45),

                  new Vdc(349, "फेदीखोला गाउँपालिका", "Phedikhola Rural Municipality", 45),

                  new Vdc(350, "ऋषिङ्ग गाउँपालिका", "Rishing Rural Municipality", 46),

                  new Vdc(351, "म्याग्दे गाउँपालिका", "Myagde Rural Municipality", 46),

                  new Vdc(356, "आँबुखैरेनी गाउँपालिका", "Aanbu Khaireni Rural Municipality", 46),

                  new Vdc(357, "बन्दिपुर गाउँपालिका", "Bandipur Rural Municipality", 46),

                  new Vdc(358, "घिरिङ गाउँपालिका", "Ghiring Rural Municipality", 46),

                  new Vdc(359, "देवघाट गाउँपालिका", "Devghat Rural Municipality", 46),

                  new Vdc(362, "मालारानी गाउँपालिका", "Malarani Rural Municipality", 51),

                  new Vdc(363, "पाणिनी गाउँपालिका", "Pandini Rural Municipality", 51),

                  new Vdc(364, "छत्रदेव गाउँपालिका", "Chhatradev Rural Municipality", 51),

                  new Vdc(365, "राप्ती सोनारी गाउँपालिका", "Raptisonari Rural Municipality", 55),

                  new Vdc(366, "वैजनाथ गाउँपालिका", "Baijnath Rural Municipality", 55),

                  new Vdc(367, "खजुरा गाउँपालिका", "Khajura Rural Municipality", 55),

                  new Vdc(368, "जानकी गाउँपालिका", "Janaki Rural Municipality", 55),

                  new Vdc(369, "डुडुवा गाउँपालिका", "Duduwa Rural Municipality", 55),

                  new Vdc(370, "नरैनापुर गाउँपालिका", "Narainapur Rural Municipality", 55),

                  new Vdc(371, "बढैयाताल गाउँपालिका", "Badhaiyatal Rural Municipality", 56),

                  new Vdc(374, "गेरुवा गाउँपालिका", "Geruwa Rural Municipality", 56),

                  new Vdc(375, "राप्ती गाउँपालिका", "Rapti Rural Municipality", 54),

                  new Vdc(376, "गढवा गाउँपालिका", "Gadhawa Rural Municipality", 54),

                  new Vdc(377, "बबई गाउँपालिका", "Babai Rural Municipality", 54),

                  new Vdc(378, "शान्तिनगर गाउँपालिका", "Shantinagar Rural Municipality", 54),

                  new Vdc(379, "राजपुर गाउँपालिका", "Rajpur Rural Municipality", 54),

                  new Vdc(380, "वंगलाचुली गाउँपालिका", "Banglachuli Rural Municipality", 54),

                  new Vdc(381, "दंगीशरण गाउँपालिका", "Dangisharan Rural Municipality", 54),

                  new Vdc(388, "सत्यवती गाउँपालिका", "Satyawati Rural Municipality", 47),

                  new Vdc(389, "धुर्कोट गाउँपालिका", "Dhurkot Rural Municipality", 47),

                  new Vdc(390, "गुल्मीदरवार गाउँपालिका", "Gulmi Durbar Rural Municipality", 47),


                  new Vdc(391, "मदाने गाउँपालिका", "Madane Rural Municipality", 47),

                  new Vdc(392, "चन्द्रकोट गाउँपालिका", "Chandrakot Rural Municipality", 47),

                  new Vdc(393, "मालिका गाउँपालिका", "Malika Rural Municipality", 47),

                  new Vdc(394, "छत्रकोट गाउँपालिका", "Chhatrakot Rural Municipality", 47),

                  new Vdc(395, "ईस्मा गाउँपालिका", "Isma Rural Municipality", 47),

                  new Vdc(396, "कालीगण्डकी गाउँपालिका", "Kaligandaki Rural Municipality", 47),

                  new Vdc(397, "रुरुक्षेत्र गाउँपालिका", "Rurukshetra Rural Municipality", 47),

                  new Vdc(404, "मायादेवी गाउँपालिका", "Mayadevi Rural Municipality", 50),

                  new Vdc(405, "शुद्धोधन गाउँपालिका", "Shuddhodhan Rural Municipality", 50),

                  new Vdc(406, "यसोधरा गाउँपालिका", "Yasodhara Rural Municipality", 50),

                  new Vdc(407, "विजयनगर गाउँपालिका", "Bijaynagar Rural Municipality", 50),

                  new Vdc(411, "सुस्ता गाउँपालिका", "Susta Rural Municipality", 57),

                  new Vdc(412, "प्रतापपुर गाउँपालिका", "Pratappur Rural Municipality", 57),

                  new Vdc(413, "सरावल गाउँपालिका", "Sarawal Rural Municipality", 57),

                  new Vdc(416, "पाल्हीनन्दन गाउँपालिका", "Palhi Nandan Rural Municipality", 57),

                  new Vdc(417, "रैनादेवी छहरा गाउँपालिका", "Rainadevi Chhahara Rural Municipality", 48),

                  new Vdc(418, "माथागढी गाउँपालिका", "Mathagadhi Rural Municipality", 48),

                  new Vdc(419, "निस्दी गाउँपालिका", "Nisdi Rural Municipality", 48),

                  new Vdc(420, "बगनासकाली  गाउँपालिका", "Bagnaskali Rural Municipality", 48),

                  new Vdc(421, "रम्भा गाउँपालिका", "Rambha Rural Municipality", 48),

                  new Vdc(422, "पूर्वखोला गाउँपालिका", "Purbakhola Rural Municipality", 48),

                  new Vdc(424, "तिनाउ गाउँपालिका", "Tinau Rural Municipality", 48),

                  new Vdc(425, "रिब्दीकोट गाउँपालिका", "Ribdikot Rural Municipality", 48),

                  new Vdc(426, "नौबहिनी गाउँपालिका", "Naubahini Rural Municipality", 52),

                  new Vdc(427, "झिमरुक गाउँपालिका", "Jhimaruk Rural Municipality", 52),

                  new Vdc(428, "गौमुखी गाउँपालिका", "Gaumukhi Rural Municipality", 52),

                  new Vdc(429, "ऐरावती गाउँपालिका", "Airawati Rural Municipality", 52),

                  new Vdc(430, "सरुमारानी गाउँपालिका", "Sarumarani Rural Municipality", 52),

                  new Vdc(431, "मल्लरानी गाउँपालिका", "Mallarani Rural Municipality", 52),

                  new Vdc(432, "माण्डवी गाउँपालिका", "Mandavi Rural Municipality", 52),

                  new Vdc(436, "सुनिल स्मृति गाउँपालिका", "Sunil Smriti Rural Municipality", 53),

                  new Vdc(437, "रुन्टीगढी गाउँपालिका", "Runtigadhi Rural Municipality", 53),

                  new Vdc(438, "लुङ्ग्री गाउँपालिका", "Lungri Rural Municipality", 53),

                  new Vdc(439, "त्रिवेणी गाउँपालिका", "Triveni Rural Municipality", 53),

                  new Vdc(440, "परिवर्तन गाउँपालिका", "Paribartan Rural Municipality", 53),

                  new Vdc(441, "गंगादेव गाउँपालिका", "Gangadev Rural Municipality", 53),

                  new Vdc(442, "माडी गाउँपालिका", "Madi Rural Municipality", 53),

                  new Vdc(445, "सुनछहरी गाउँपालिका", "Sunchhahari Rural Municipality", 53),

                  new Vdc(446, "थवाङ गाउँपालिका", "Thawang Rural Municipality", 53),

                  new Vdc(447, "भूमे गाउँपालिका", "Bhume Rural Municipality", 58),

                  new Vdc(448, "पुथा उत्तरगंगा गाउँपालिका", "Putha Uttarganga Rural Municipality", 58),

                  new Vdc(449, "सिस्ने गाउँपालिका", "Sisne Rural Municipality", 58),

                  new Vdc(450, "गैडहवा गाउँपालिका", "Gaidhawa Rural Municipality", 49),

                  new Vdc(457, "मायादेवी गाउँपालिका", "Mayadevi Rural Municipality", 49),

                  new Vdc(458, "कोटहीमाई गाउँपालिका", "Kotahimai Rural Municipality", 49),

                  new Vdc(462, "मर्चवारी गाउँपालिका", "Marchawari Rural Municipality", 49),

                  new Vdc(463, "सियारी गाउँपालिका", "Siyari Rural Municipality", 49),

                  new Vdc(464, "सम्मरीमाई गाउँपालिका", "Sammarimai Rural Municipality", 49),

                  new Vdc(465, "रोहिणी गाउँपालिका", "Rohini Rural Municipality", 49),

                  new Vdc(466, "शुद्धोधन गाउँपालिका", "Shuddhodhan Rural Municipality", 49),

                  new Vdc(467, "ओमसतीया गाउँपालिका", "Om Satiya Rural Municipality", 49),

                  new Vdc(468, "कञ्चन गाउँपालिका", "Kanchan Rural Municipality", 49),

                  new Vdc(472, "गुराँस गाउँपालिका", "Gurans Rural Municipality", 67),

                  new Vdc(473, "भैरवी गाउँपालिका", "Bhairabi Rural Municipality", 67),

                  new Vdc(474, "नौमुले गाउँपालिका", "Naumule Rural Municipality", 67),

                  new Vdc(475, "महावु गाउँपालिका", "Mahabu Rural Municipality", 67),

                  new Vdc(476, "ठाँटीकाँध गाउँपालिका", "Thantikandh Rural Municipality", 67),

                  new Vdc(477, "भगवतीमाई गाउँपालिका", "Bhagwatimai Rural Municipality", 67),

                  new Vdc(478, "डुंगेश्वर गाउँपालिका", "Dungeshwar Rural Municipality", 67),

                  new Vdc(484, "मुड्केचुला गाउँपालिका", "Mudkechula Rural Municipality", 61),

                  new Vdc(485, "काईके गाउँपालिका", "Kaike Rural Municipality", 61),

                  new Vdc(486, "शे फोक्सुन्डो गाउँपालिका", "She Phoksundo Rural Municipality", 61),

                  new Vdc(487, "जगदुल्ला गाउँपालिका", "Jagadulla Rural Municipality", 61),

                  new Vdc(492, "डोल्पो बुद्ध गाउँपालिका", "Dolpo Buddha Rural Municipality", 61),

                  new Vdc(493, "छार्का ताङसोङ गाउँपालिका", "Chharka Tongsong Rural Municipality", 61),

                  new Vdc(494, "सिमकोट गाउँपालिका", "Simkot Rural Municipality", 62),

                  new Vdc(495, "सर्केगाड गाउँपालिका", "Sarkegad Rural Municipality", 62),

                  new Vdc(496, "अदानचुली गाउँपालिका", "Adanchuli Rural Municipality", 62),

                  new Vdc(497, "खार्पुनाथ गाउँपालिका", "Kharpunath Rural Municipality", 62),

                  new Vdc(498, "ताँजाकोट गाउँपालिका", "Tanjakot Rural Municipality", 62),

                  new Vdc(502, "चंखेली गाउँपालिका", "Chankheli Rural Municipality", 62),

                  new Vdc(503, "नाम्खा गाउँपालिका", "Namkha Rural Municipality", 62),

                  new Vdc(504, "जुनीचाँदे गाउँपालिका", "Junichande Rural Municipality", 68),

                  new Vdc(505, "कुसे गाउँपालिका", "Kuse Rural Municipality", 68),

                  new Vdc(508, "बारेकोट गाउँपालिका", "Barekot Rural Municipality", 68),

                  new Vdc(509, "शिवालय गाउँपालिका", "Shivalaya Rural Municipality", 68),

                  new Vdc(510, "तातोपानी गाउँपालिका", "Tatopani Rural Municipality", 63),

                  new Vdc(511, "पातारासी गाउँपालिका", "Patarasi Rural Municipality", 63),

                  new Vdc(512, "तिला गाउँपालिका", "Tila Rural Municipality", 63),

                  new Vdc(513, "कनकासुन्दरी गाउँपालिका", "Kanaka Sundari Rural Municipality", 63),

                  new Vdc(515, "सिंजा गाउँपालिका", "Sinja Rural Municipality", 63),

                  new Vdc(516, "हिमा गाउँपालिका", "Hima Rural Municipality", 63),

                  new Vdc(517, "गुठिचौर गाउँपालिका", "Guthichaur Rural Municipality", 63),

                  new Vdc(518, "नरहरिनाथ गाउँपालिका", "Narharinath Rural Municipality", 64),

                  new Vdc(519, "पलाता गाउँपालिका", "Palata Rural Municipality", 64),

                  new Vdc(520, "शुभ कालिका गाउँपालिका", "Shubha Kalika Rural Municipality", 64),

                  new Vdc(521, "सान्नी त्रिवेणी गाउँपालिका", "Sanni Triveni Rural Municipality", 64),

                  new Vdc(525, "पचालझरना गाउँपालिका", "Pachaljharana Rural Municipality", 64),

                  new Vdc(526, "महावै गाउँपालिका", "Mahawai Rural Municipality", 64),

                  new Vdc(527, "खत्याड गाउँपालिका", "Khatyad Rural Municipality", 65),

                  new Vdc(528, "सोरु गाउँपालिका", "Soru Rural Municipality", 65),

                  new Vdc(529, "मुगुम कार्मारोंग गाउँपालिका", "Mugum Karmarong Rural Municipality", 65),

                  new Vdc(530, "सानीभेरी गाउँपालिका", "Sani Bheri Rural Municipality", 60),

                  new Vdc(532, "त्रिवेणी गाउँपालिका", "Triveni Rural Municipality", 60),

                  new Vdc(533, "बाँफिकोट गाउँपालिका", "Banphikot Rural Municipality", 60),

                  new Vdc(534, "कुमाख गाउँपालिका", "Kumakh Rural Municipality", 59),

                  new Vdc(535, "कालीमाटी गाउँपालिका", "Kalimati Rural Municipality", 59),

                  new Vdc(536, "छत्रेश्वरी गाउँपालिका", "Chhatreshwari Rural Municipality", 59),

                  new Vdc(537, "दार्मा गाउँपालिका", "Darma Rural Municipality", 59),

                  new Vdc(538, "कपुरकोट गाउँपालिका", "Kapurkot Rural Municipality", 59),


                  new Vdc(539, "त्रिवेणी गाउँपालिका", "Triveni Rural Municipality", 59),

                  new Vdc(540, "सिद्ध कुमाख गाउँपालिका", "Siddha Kumakh Rural Municipality", 59),

                  new Vdc(541, "बराहताल गाउँपालिका", "Barahatal Rural Municipality", 66),

                  new Vdc(545, "सिम्ता गाउँपालिका", "Simta Rural Municipality", 66),

                  new Vdc(546, "चौकुने गाउँपालिका", "Chaukune Rural Municipality", 66),

                  new Vdc(547, "चिङ्गाड गाउँपालिका", "Chingad Rural Municipality", 66),

                  new Vdc(552, "रामारोशन गाउँपालिका", "Ramaroshan Rural Municipality", 70),

                  new Vdc(553, "चौरपाटी गाउँपालिका", "Chaurpati Rural Municipality", 70),

                  new Vdc(554, "तुर्माखाँद गाउँपालिका", "Turmakhand Rural Municipality", 70),

                  new Vdc(555, "मेल्लेख गाउँपालिका", "Mellekh Rural Municipality", 70),

                  new Vdc(556, "ढँकारी गाउँपालिका", "Dhankari Rural Municipality", 70),

                  new Vdc(559, "बान्नीगडीजैगड गाउँपालिका", "Bannigadi Jayagad Rural Municipality", 70),

                  new Vdc(560, "दोगडाकेदार गाउँपालिका", "Dogdakedar Rural Municipality", 76),

                  new Vdc(561, "डिलाशैनी गाउँपालिका", "Dilashaini Rural Municipality", 76),

                  new Vdc(562, "सिगास गाउँपालिका", "Sigas Rural Municipality", 76),

                  new Vdc(563, "पञ्चेश्वर गाउँपालिका", "Pancheshwar Rural Municipality", 76),

                  new Vdc(564, "सुर्नया गाउँपालिका", "Surnaya Rural Municipality", 76),

                  new Vdc(565, "शिवनाथ गाउँपालिका", "Shivanath Rural Municipality", 76),

                  new Vdc(566, "केदारस्यु गाउँपालिका", "Kedarsyu Rural Municipality", 72),

                  new Vdc(567, "थलारा गाउँपालिका", "Thalara Rural Municipality", 72),

                  new Vdc(568, "बित्थडचिर गाउँपालिका", "Bitthadchir Rural Municipality", 72),

                  new Vdc(573, "छब्बीसपाथिभेरा गाउँपालिका", "Chhabis Pathibhera Rural Municipality", 72),

                  new Vdc(574, "छान्ना गाउँपालिका", "Chhanna Rural Municipality", 72),

                  new Vdc(575, "मष्टा गाउँपालिका", "Masta Rural Municipality", 72),

                  new Vdc(576, "दुर्गाथली गाउँपालिका", "Durgathali Rural Municipality", 72),

                  new Vdc(577, "तलकोट गाउँपालिका", "Talkot Rural Municipality", 72),

                  new Vdc(578, "सुर्मा गाउँपालिका", "Surma Rural Municipality", 72),

                  new Vdc(581, "सइपाल गाउँपालिका", "Saipal Rural Municipality", 72),

                  new Vdc(582, "खप्तड छेडेदह गाउँपालिका", "Khaptad Chhededaha Rural Municipality", 73),

                  new Vdc(583, "स्वामिकार्तिक खापर गाउँपालिका", "Swami Kartik Khapar Rural Municipality", 73),

                  new Vdc(584, "जगन्‍नाथ  गाउँपालिका", "Jagannath Rural Municipality", 73),

                  new Vdc(585, "हिमाली गाउँपालिका", "Himali Rural Municipality", 73),

                  new Vdc(586, "गौमुल गाउँपालिका", "Gaumul Rural Municipality", 73),

                  new Vdc(587, "नवदुर्गा गाउँपालिका", "Navadurga Rural Municipality", 75),

                  new Vdc(595, "आलिताल गाउँपालिका", "Aalitaal Rural Municipality", 75),

                  new Vdc(596, "गन्यापधुरा गाउँपालिका", "Ganyapadhura Rural Municipality", 75),

                  new Vdc(597, "भागेश्वर गाउँपालिका", "Bhageshwar Rural Municipality", 75),

                  new Vdc(598, "अजयमेरु गाउँपालिका", "Ajaymeru Rural Municipality", 75),

                  new Vdc(599, "नौगाड गाउँपालिका", "Naugad Rural Municipality", 77),

                  new Vdc(600, "मालिकार्जुन गाउँपालिका", "Malikarjun Rural Municipality", 77),

                  new Vdc(608, "मार्मा गाउँपालिका", "Marma Rural Municipality", 77),

                  new Vdc(609, "लेकम गाउँपालिका", "Lekam Rural Municipality", 77),

                  new Vdc(612, "दुहु गाउँपालिका", "Duhu Rural Municipality", 77),

                  new Vdc(613, "ब्यास गाउँपालिका", "Byas Rural Municipality", 77),

                  new Vdc(614, "अपि हिमाल गाउँपालिका", "Api Himal Rural Municipality", 77),

                  new Vdc(615, "आदर्श गाउँपालिका", "Aadarsha Rural Municipality", 71),

                  new Vdc(616, "पूर्वीचौकी गाउँपालिका", "Purbichauki Rural Municipality", 71),

                  new Vdc(621, "के.आई.सिं.  गाउँपालिका", "K.I. Singh Rural Municipality", 71),

                  new Vdc(622, "जोरायल गाउँपालिका", "Jorayal Rural Municipality", 71),

                  new Vdc(623, "सायल गाउँपालिका", "Sayal Rural Municipality", 71),

                  new Vdc(624, "बोगटान गाउँपालिका", "Bogatan Rural Municipality", 71),

                  new Vdc(625, "बड्डी केदार गाउँपालिका", "Badikedar Rural Municipality", 71),

                  new Vdc(626, "जानकी गाउँपालिका", "Janaki Rural Municipality", 69),

                  new Vdc(629, "कैलारी गाउँपालिका", "Kailari Rural Municipality", 69),

                  new Vdc(630, "जोशीपुर गाउँपालिका", "Joshipur Rural Municipality", 69),

                  new Vdc(631, "बर्गगोरिया गाउँपालिका", "Bargagoriya Rural Municipality", 69),

                  new Vdc(632, "मोहन्याल गाउँपालिका", "Mohanyal Rural Municipality", 69),

                  new Vdc(633, "चुरे गाउँपालिका", "Chure Rural Municipality", 69),

                  new Vdc(634, "लालझाँडी गाउँपालिका", "Laljhadi Rural Municipality", 74),

                  new Vdc(635, "बेलडाँडी गाउँपालिका", "Beldandi Rural Municipality", 74),
            };
                    await _context.Vdcs.AddRangeAsync(Vdcs);
                    await _unitOfWork.SaveChangesAsync();


                };

           
        }

        #endregion

        #region PaymentMethod
        public async Task SeedPaymentMethod()
        {
            if (!await _context.PaymentMethod.AnyAsync())
            {
                var paymentMethod = new List<PaymentMethod>()
            {
                new PaymentMethod("d1e43e64-9d48-4c85-83f3-3d2d5c9f6c44","Credit","f5c2cba4-e4c7-496a-9f07-f2060c426e06","","",DateTime.Now,"",DateTime.Now,false,false,false,false,false,false,false),
                new PaymentMethod("9a7f0c12-1c3b-4f39-baa3-2c9806ec9b7d","Cash","3d5c1e24-d0ae-4f74-9c88-bf9f4b5c4d0b","","",DateTime.Now,"",DateTime.Now,false,false,false,false,false,false,false),
                new PaymentMethod("89bfd7e7-b7f7-456f-9a03-8af2811d38f7","Cheque","7a9a6c6f-3b4a-4e58-b13c-c61e7bba9d72","","",DateTime.Now,"",DateTime.Now,true,true,true,false,false,false,false),
                //new PaymentMethod("d1e43e64-9d48-4c85-83f3-3d2d5c9f6c44","Accounts Receivable (Credit Sales)","f5c2cba4-e4c7-496a-9f07-f2060c426e06",PaymentType.CreditCard,"","",DateTime.Now,"",DateTime.Now),
                //new PaymentMethod("9a7f0c12-1c3b-4f39-baa3-2c9806ec9b7d","Accounts Payable (Credit Purchases)","dff66bb4-11e6-4e5f-8bb9-f00c01b90284",PaymentType.CreditCard,"","",DateTime.Now,"",DateTime.Now),
    
            };


                await _context.PaymentMethod.AddRangeAsync(paymentMethod);
                await _unitOfWork.SaveChangesAsync();

            }
        }

        #endregion

        #region MasterLedger
        public async Task<List<Master>> SeedMaster()
        {

            var masters = new List<Master>()
            {
                //new Master("e8f6a4c7-3b92-4a2e-8d65-91c3b5f2e7a9", "Current Assets"),
                //new Master("47c8f2e1-9d3a-412b-b6f7-821a4e5d9c73", "Current Liabilities"),
                //new Master("a3f8b7d2-5c91-4e6d-8a42-6e7d3c5f9b21", "Revenue Accounts"),
                //new Master("d9e2f4c7-8b13-4a56-9d27-3f5a8c1b67e4", "Capital Account"),
                //new Master("f2b7c8a1-4d63-49e5-9a27-6e3c5d8f71b2", "Loans Liability"),
                //new Master("7c3f9a5d-8b21-4e6d-92f4-1b7e2c56a3d8", "Fixed Assets"),
                //new Master("9d5a3f8c-7b42-4e1b-62d7-4c8a5f2e9d13", "Investments"),
                //new Master("5b7c2f8d-4a91-49e3-6d27-3c5a8f1b9d42", "Pre-Operative Expenses"),
                //new Master("a1f8c7d2-5b93-4e6d-82d4-9c5a3f7b1e62", "Profit & Loss"),
                //new Master("e3c5f7a1-9b42-4d6d-82f8-2a7c5b9d13e4", "Suspense Account"),

                new Master("e8f6a4c7-3b92-4a2e-8d65-91c3b5f2e7a9", "Assets",true),
                new Master("47c8f2e1-9d3a-412b-b6f7-821a4e5d9c73", "Liabilities",true),
                new Master("a3f8b7d2-5c91-4e6d-8a42-6e7d3c5f9b21", "Income",true),
                new Master("a1f8c7d2-5b93-4e6d-82d4-9c5a3f7b1e62", "Expenses",true)


            };
            if (!await _context.Masters.AnyAsync())
            {
                await _context.Masters.AddRangeAsync(masters);
                await _unitOfWork.SaveChangesAsync();
               

            }

             return masters;
        }

        #endregion

        #region LedgerGroup
        public async Task<List<LedgerGroup>> SeedLedgerGroup()
        {
            var ledgerGroup = new List<LedgerGroup>()
                    {
                #region ledgergroup

                //new LedgerGroup("a1d2f3b4-c5e6-7890-a123-b456c789d012", "Bank Accounts", true, "e8f6a4c7-3b92-4a2e-8d65-91c3b5f2e7a9", false,"","","",default,"",default),
                //new LedgerGroup("b2e3f4c5-d6a7-8901-b234-c567d890e123", "Bank O/D Accounts", true, "f2b7c8a1-4d63-49e5-9a27-6e3c5d8f71b2", false,"","","",default,"",default),
                //new LedgerGroup("c3f4b5d6-a789-0123-c456-d789e012f345", "Capital Accounts", true, "d9e2f4c7-8b13-4a56-9d27-3f5a8c1b67e4", true, ""," ","",default,"",default),
                //new LedgerGroup("d4b5c6f7-a890-1234-d567-e890f123g456", "Cash-in-hand", true, "e8f6a4c7-3b92-4a2e-8d65-91c3b5f2e7a9", false,"", "","",default,"",default),
                //new LedgerGroup("e5c6d7f8-a901-2345-e678-f901g234h567", "Current Assets", true, "e8f6a4c7-3b92-4a2e-8d65-91c3b5f2e7a9", true, "","","",default,"",default),
                //new LedgerGroup("f6d7e8b9-a012-3456-f789-g012h345i678", "Current Liabilities", true, "47c8f2e1-9d3a-412b-b6f7-821a4e5d9c73", true,"", "","",default,"",default),
                //new LedgerGroup("g7e8f9c0-a123-4567-g890-h123i456j789", "Duties & Taxes", true, "47c8f2e1-9d3a-412b-b6f7-821a4e5d9c73", false, "","","",default,"",default),
                //new LedgerGroup("h8f9c0d1-a234-5678-h901-i234j567k890", "Expenses (Direct/Mfg.)", true, "a3f8b7d2-5c91-4e6d-8a42-6e7d3c5f9b21", false,"", "","",default,"",default),
                //new LedgerGroup("i9c0d1e2-a345-6789-i012-j345k678l901", "Expenses (Indirect/Admn.)", true, "a3f8b7d2-5c91-4e6d-8a42-6e7d3c5f9b21", false, "", "","",default,"",default),
                //new LedgerGroup("j0d1e2f3-a456-7890-j123-k456l789m012", "Fixed Assets", true, "7c3f9a5d-8b21-4e6d-92f4-1b7e2c56a3d8", true,"", "","",default,"",default),
                //new LedgerGroup("k1e2f3g4-a567-8901-k234-l567m890n123", "Income (Direct/Opr.)", true, "a3f8b7d2-5c91-4e6d-8a42-6e7d3c5f9b21", false, "","","",default,"",default),
                //new LedgerGroup("l2f3g4h5-a678-9012-l345-m678n901o234", "Income (Indirect)", true, "a3f8b7d2-5c91-4e6d-8a42-6e7d3c5f9b21", false,"", "","",default,"",default),
                //new LedgerGroup("m3g4h5i6-a789-0123-m456-n789o012p345", "Investments", true, "9d5a3f8c-7b42-4e1b-62d7-4c8a5f2e9d13", true,"","","",default,"",default),
                //new LedgerGroup("n4h5i6j7-a890-1234-n567-o890p123q456", "Loan & Advances (Assets)", true, "e8f6a4c7-3b92-4a2e-8d65-91c3b5f2e7a9", false, "", "","",default,"",default),
                //new LedgerGroup("o5i6j7k8-a901-2345-o678-p901q234r567", "Loans (Liability)", true, "f2b7c8a1-4d63-49e5-9a27-6e3c5d8f71b2", true, "","","",default,"",default),
                //new LedgerGroup("p6j7k8l9-a012-3456-p789-q012r345s678", "Pre-Operative Expenses", true, "5b7c2f8d-4a91-49e3-6d27-3c5a8f1b9d42", true,"", "","",default,"",default),
                //new LedgerGroup("q7k8l9m0-a123-4567-q890-r123s456t789", "Profit & Loss", true, "a1f8c7d2-5b93-4e6d-82d4-9c5a3f7b1e62", true, "","","",default,"",default),
                //new LedgerGroup("r8l9m0n1-a234-5678-r901-s234t567u890", "Provisions/Expenses Payable", true, "47c8f2e1-9d3a-412b-b6f7-821a4e5d9c73", false,"", "","",default,"",default),
                //new LedgerGroup("s9m0n1o2-a345-6789-s012-t345u678v901", "Purchase", true, "a3f8b7d2-5c91-4e6d-8a42-6e7d3c5f9b21", false, "","","",default,"",default),
                //new LedgerGroup("t0n1o2p3-a456-7890-t123-u456v789w012", "Reserve & Surplus", true, "d9e2f4c7-8b13-4a56-9d27-3f5a8c1b67e4", false,"", "","",default,"",default),
                //new LedgerGroup("u1o2p3q4-a567-8901-u234-v567w890x123", "Revenue Accounts", true, "a3f8b7d2-5c91-4e6d-8a42-6e7d3c5f9b21", true, "","","",default,"",default),
                //new LedgerGroup("v2p3q4r5-a678-9012-v345-w678x901y234", "Sale", true, "e8f6a4c7-3b92-4a2e-8d65-91c3b5f2e7a9", false,"", "","",default,"",default),
                //new LedgerGroup("w3q4r5s6-a789-0123-w456-x789y012z345", "Secured Loans", true, "f2b7c8a1-4d63-49e5-9a27-6e3c5d8f71b2", false, "","","",default,"",default),
                //new LedgerGroup("x4r5s6t7-a890-1234-x567-y890z123a456", "Securities & Deposits (Asset)", true, "e8f6a4c7-3b92-4a2e-8d65-91c3b5f2e7a9", false,"","","",default,"",default),
                //new LedgerGroup("y5s6t7u8-a901-2345-y678-z901a234b567", "Stock-in-hand", true, "e8f6a4c7-3b92-4a2e-8d65-91c3b5f2e7a9", false, "", "","",default,"",default),
                //new LedgerGroup("z6t7u8v9-a012-3456-z789-a012b345c678", "Sundry Creditors", true, "47c8f2e1-9d3a-412b-b6f7-821a4e5d9c73", false,"","","",default,"",default),
                //new LedgerGroup("aed6b705-11aa-4b87-8681-49615d576a6d", "Sundry Debtors", true, "e8f6a4c7-3b92-4a2e-8d65-91c3b5f2e7a9", true,"", "","",default,"",default),
                //new LedgerGroup("a7u8v9w0-a123-4567-a890-b123c456d789", "Suspense Account", true, "e3c5f7a1-9b42-4d6d-82f8-2a7c5b9d13e4", true,"","","",default,"",default),
                //new LedgerGroup("b8v9w0x1-a234-5678-b901-c234d567e890", "Unsecured Loans", true, "f2b7c8a1-4d63-49e5-9a27-6e3c5d8f71b2", false,"","","",default,"",default),

#endregion
                new LedgerGroup("a1d2f3b4-c5e6-7890-a123-b456c789d012", "Current Assets", true, "e8f6a4c7-3b92-4a2e-8d65-91c3b5f2e7a9", false, "", "", "", default, "", default, true,true),
                new LedgerGroup("d4b5c6f7-a890-1234-d567-e890f123g456", "Fixed Assets", true, "e8f6a4c7-3b92-4a2e-8d65-91c3b5f2e7a9", false, "", "", "", default, "", default,true,true),
                new LedgerGroup("e5c6d7f8-a901-2345-e678-f901g234h567", "Current Liabilities", true, "47c8f2e1-9d3a-412b-b6f7-821a4e5d9c73", true, "", "", "", default, "", default,true,true),
                new LedgerGroup("j0d1e2f3-a456-7890-j123-k456l789m012", "Long Term Liabilities", true, "47c8f2e1-9d3a-412b-b6f7-821a4e5d9c73", true,"", "","",default,"",default,true,true),
                new LedgerGroup("aed6b705-11aa-4b87-8681-49615d576a6d", "Loans(Liability)", true, "47c8f2e1-9d3a-412b-b6f7-821a4e5d9c73", true, "", "", "", default, "", default,true,true),
                new LedgerGroup("y5s6t7u8-a901-2345-y678-z901a234b567", "Share Holder's Equity", true, "47c8f2e1-9d3a-412b-b6f7-821a4e5d9c73", false, "", "", "", default, "", default,true,true),
                new LedgerGroup("x4r5s6t7-a890-1234-x567-y890z123a456", "Direct Income", true, "a3f8b7d2-5c91-4e6d-8a42-6e7d3c5f9b21", false, "", "", "", default, "", default,true,true),
                new LedgerGroup("n4h5i6j7-a890-1234-n567-o890p123q456", "Indirect Income", true, "a3f8b7d2-5c91-4e6d-8a42-6e7d3c5f9b21", false, "", "", "", default, "", default,true,true),

                new LedgerGroup("f6d7e8b9-a012-3456-f789-g012h345i678", "Direct Expenses", true, "a1f8c7d2-5b93-4e6d-82d4-9c5a3f7b1e62", true, "", "", "", default, "", default,true,true),
                new LedgerGroup("g7e8f9c0-a123-4567-g890-h123i456j789", "Indirect Expenses", true, "a1f8c7d2-5b93-4e6d-82d4-9c5a3f7b1e62", false, "", "", "", default, "", default,true,true),
                

                    };

            if (!await _context.LedgerGroups.AnyAsync())
            {
                await _context.LedgerGroups.AddRangeAsync(ledgerGroup);
                await _unitOfWork.SaveChangesAsync();

            }
            return ledgerGroup;
        }

        #endregion

        #region SeedSubLedgerGroup
        public async Task<List<SubLedgerGroup>> SeedSubLedgerGroup()
        {

            var subledgergroup = new List<SubLedgerGroup>()
            {

                new SubLedgerGroup("3d5c1e24-d0ae-4f74-9c88-bf9f4b5c4d0b", "Cash-In-Hand","a1d2f3b4-c5e6-7890-a123-b456c789d012","","",default,"",default,true,true),
                new SubLedgerGroup("7a9a6c6f-3b4a-4e58-b13c-c61e7bba9d72", "Cash-At-Bank","a1d2f3b4-c5e6-7890-a123-b456c789d012","","",default,"",default,true,true),
                new SubLedgerGroup("b112f8f1-5390-4700-b8e1-0cf5a78a92d4", "Loan & Advance(Assets)","a1d2f3b4-c5e6-7890-a123-b456c789d012","","",default,"",default,true,true),
                new SubLedgerGroup("987ed926-b312-4a7e-ae64-3c0f268f88d8", "Deposits(Assets)", "a1d2f3b4-c5e6-7890-a123-b456c789d012", "", "", default, "", default, true,true),

                new SubLedgerGroup("1f91a98b-9d6e-4a59-ade1-ef145ae08f5e", "Stock-In-Hand", "a1d2f3b4-c5e6-7890-a123-b456c789d012", "", "", default, "", default, true,true),
                new SubLedgerGroup("21955bd7-6827-4b0b-b3e1-967099b54f6a", "Inventory", "a1d2f3b4-c5e6-7890-a123-b456c789d012", "", "", default, "", default, true,true),
                new SubLedgerGroup("dff66bb4-11e6-4e5f-8bb9-f00c01b90284", "Sundry Debtors", "a1d2f3b4-c5e6-7890-a123-b456c789d012", "", "", default, "", default, true,true),
                new SubLedgerGroup("78cbf013-2532-4427-8b87-fbd32e2aef1d", "Fixed Assets A/C", "d4b5c6f7-a890-1234-d567-e890f123g456", "", "", default, "", default, true,true),



                new SubLedgerGroup("f5c2cba4-e4c7-496a-9f07-f2060c426e06", "Sundry Creditors","e5c6d7f8-a901-2345-e678-f901g234h567","","",default,"",default, true,true),
                new SubLedgerGroup("e84cfc6a-2289-4b4a-9ec3-88dc7f0bdf34", "Duties & Taxes","e5c6d7f8-a901-2345-e678-f901g234h567","","",default,"",default, true,true),
                new SubLedgerGroup("623c3133-f4c7-41e5-a9c1-382c749d3a8a", "Sales Account","x4r5s6t7-a890-1234-x567-y890z123a456","","",default,"",default, true,true),
                new SubLedgerGroup("8a6d5de6-5607-497e-8d7c-90d7494d7aa7", "Purchase Account","f6d7e8b9-a012-3456-f789-g012h345i678","","",default,"",default, true,true),
                new SubLedgerGroup("0423b6c3-51fa-44c4-bd9a-28fa9697ff55", "Cost Of Goods Sold(Cogs) A/c","f6d7e8b9-a012-3456-f789-g012h345i678","","",default,"",default, true,true),
                new SubLedgerGroup("34a841a6-339c-47d2-95d9-f3c9cb66c55a", "Salary Expenses","g7e8f9c0-a123-4567-g890-h123i456j789","","",default,"",default, true,true),
                new SubLedgerGroup("d4e7f9b2-1c3a-4f6d-9b8e-2a7c5d1f0e9a", "Share Capital","y5s6t7u8-a901-2345-y678-z901a234b567","","",default,"",default, true,true),
                new SubLedgerGroup("c2d70742-b341-491d-87d2-d301dc3b853d", "Capitals A/C","y5s6t7u8-a901-2345-y678-z901a234b567","","",default,"",default, true,true),
                new SubLedgerGroup("f20db8f0-a3f3-4033-a274-f92611f2d5ad", "Indirect Expenses","g7e8f9c0-a123-4567-g890-h123i456j789","","",default,"",default, true,true),
                new SubLedgerGroup("f021ca46-74ae-4a3c-8797-d21b8e4e6091", "Stock Loss(Indirect Expenses)","g7e8f9c0-a123-4567-g890-h123i456j789","","",default,"",default, true,true),
                new SubLedgerGroup("aec78dce-10c3-410e-a89e-e29dcf3faa3d", "Stock Gain(Indirect  Income))","n4h5i6j7-a890-1234-n567-o890p123q456","","",default,"",default, true,true),
                new SubLedgerGroup("f613be60-2268-4d65-8877-f794c9816569", "Marketing and Advertisement(Indirect Expenses)","g7e8f9c0-a123-4567-g890-h123i456j789","","",default,"",default, true,true),
 


            };
            if (!await _context.SubLedgerGroups.AnyAsync())
            {
                await _context.SubLedgerGroups.AddRangeAsync(subledgergroup);
                await _unitOfWork.SaveChangesAsync();


            }

            return subledgergroup;
        }
        #endregion



        #region SeedStockCenter
        private async Task SeedStockCenter()
        {
            if (!await _context.StockCenters.AnyAsync())
            {
                var stockCenters = new List<StockCenter>()
                {
                    new StockCenter("c8f1df0e-9124-42b3-8f2e-d0c6cf7b2f2e","Main Store","main Address","","", default)
                };

                await _context.StockCenters.AddRangeAsync(stockCenters);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        #endregion




        #region SeedItemGroups
        private async Task SeedItemsGroup()
        {
            if(!await _context.ItemGroups.AnyAsync())
            {
                var itemsGroup = new List<ItemGroup>()
                {
                    new ItemGroup("c4f9d84a-7ad9-4a75-b1b3-eec78f6a0a1b","General","General Item Groups",true,"","","",DateTime.Now,"",DateTime.Now,true)
                };

                await _context.ItemGroups.AddRangeAsync(itemsGroup);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        #endregion

        #region SeedUnits
        private async Task SeedUnits()
        {
            if (!await _context.Units.AnyAsync())
            {
                var units = new List<Units>()
                {
                    new Units("4e3fd50f-689d-4fc0-8b5a-1317f9c7977a", "Pcs", DateTime.UtcNow, " ", DateTime.UtcNow, "-", true, " "),
                    new Units("e7880602-4aaf-4927-b9b9-f75aee13f935", "Kg", DateTime.UtcNow, " ", DateTime.UtcNow, "_", true, " "),
                    new Units("ca2a4cc9-3c6d-4b8a-b367-61efe61327aa", "Meter", DateTime.UtcNow, " ", DateTime.UtcNow, "-", true, " ")


                };

                await _context.Units.AddRangeAsync(units);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        #endregion


        #region SeedModules
        private async Task SeedModules()
        {
            if (!await _context.Modules.AnyAsync())
            {
                var modules = new List<Modules>()
                 {

                    new Modules("005b6875-807d-43c3-988e-f439f478c8b4","Inventory","2","-","/Inventory", true),
                    new Modules("19ebc109-36ce-4803-8042-5c4ea4b3fd5b","Income/ Expense","6","-","/income", true),
                    new Modules("1ce0e9b3-3795-49d0-a78d-68025f6f0273","Payment Method","11","-","/PaymentMethod", true),
                    new Modules("25bc79d7-33ea-42e3-a3d1-70dfe9868ab3","Report","8","-","/report", true),
                    new Modules("3b3756bd-4523-49d0-90a7-221f49bff237","Tax Report","9","-","/TaxReport", true),
                    new Modules("4f1b558f-62f9-4a4d-99f7-1940d2127a23","Sales","3","-","/sales", true),
                    new Modules("8c834e6b-2510-48ee-b255-4994ffa49197","User","2","_","/User", true),
                    new Modules("95f57bfe-70d4-46f5-82ec-c287833824d8","Account","1","-","/Account", true),
                    new Modules("a1ce024a-c12f-41a7-a7b9-9e92bee29e70","Administration","10","-","/Administration", true),
                    new Modules("b62bcaca-b269-42d2-be33-aed2a9b79b0a","Role","4","-","/Role", true),
                    new Modules("baf4bcec-354c-49f3-ad0f-55406d5720fb","AR/ AP","5","-","/payment", false),
                    new Modules("be68dd5c-626c-44e5-b0c4-a71ac70bcccd","Voucher","7","-","/voucher", true),
                    new Modules("c19d9bf6-27a7-4898-bdda-2b23409fd1e3","Purchase","4","-","/purchase", true),


                 };

                await _context.Modules.AddRangeAsync(modules);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        #endregion

        #region SeedSubModules
        private async Task SeedSubModules()
        {
            if (!await _context.SubModules.AnyAsync())
            {
                var subModules = new List<SubModules>()
                 {
                    new SubModules("160cf130-a172-4005-bfe2-27479e2954d1", "Conversion Factors", "-", "conversionFactor/all-ConversionFactor", "005b6875-807d-43c3-988e-f439f478c8b4", "7", true),
                    //new SubModules("20fb85a6-ad41-434a-a0bd-92272553bd14", "Stock Report", "-", "stock/StockReport", "25bc79d7-33ea-42e3-a3d1-70dfe9868ab3", "11", true),
                    new SubModules("26f19645-9a63-4164-82ce-3574a1d3ca14", "Purchase Return", "-", "purchaseReturn/All-purchaseReturn", "c19d9bf6-27a7-4898-bdda-2b23409fd1e3", "4", true),
                    new SubModules("2850f32a-d537-4b15-be20-141ee129fc61", "Stock Center", "-", "stockCenter/all-StockCenter", "005b6875-807d-43c3-988e-f439f478c8b4", "3", true),
                    new SubModules("2b54da4f-8826-45df-86fc-eecae8e4b3ff", "Expense", "-", "expense/All-Expense", "19ebc109-36ce-4803-8042-5c4ea4b3fd5b", "4", true),
                    new SubModules("32e4708d-fb86-49b0-8d77-56b38efdedeb", "VAT Sales Register", "-", "AccountBook/SalesRegister", "3b3756bd-4523-49d0-90a7-221f49bff237", "2", true),
                    new SubModules("3a86a670-db6f-430b-8d2d-d02a24c93ea9", "Ar/Ap", "-", "arap/ArAp", "baf4bcec-354c-49f3-ad0f-55406d5720fb", "2", true),
                    new SubModules("3f5fa4c7-beb3-4c81-8679-823ed2cd71b0", "Ledger", "-", "ledger/all-ledger", "95f57bfe-70d4-46f5-82ec-c287833824d8", "1", true),
                    new SubModules("41fe19cf-37ab-4c8e-be2a-9ab5696dab69", "Sales", "-", "sales/All-Sales", "4f1b558f-62f9-4a4d-99f7-1940d2127a23", "2", true),
                    //new SubModules("a4d7f9c2-6b1e-4f0d-8e3b-7c9a2b1d5e6f", "Item wise Sales Report", "-", "salesReport/SalesReport", "25bc79d7-33ea-42e3-a3d1-70dfe9868ab3", "4", true),
                    //new SubModules("45790b30-68b3-4e42-ae4d-0dfef6bca2b3", "Trading Report", "-", "tradingReport/TradingReport", "25bc79d7-33ea-42e3-a3d1-70dfe9868ab3", "7", true),
                    new SubModules("46fdf094-2c42-47e1-b9fe-1beab735ba82", "Role Permission", "-", "role-permission/all-roles-permission", "b62bcaca-b269-42d2-be33-aed2a9b79b0a", "3", true),
                    new SubModules("50a3eced-9954-4c75-a0cc-a6e2d11c7b3c", "Payment", "-", "payment/All-Payment", "baf4bcec-354c-49f3-ad0f-55406d5720fb", "4", true),
                    new SubModules("52197ced-2cf3-45c9-a8e0-9c17942e44a3", "Users", "-", "user/all-users", "8c834e6b-2510-48ee-b255-4994ffa49197", "3", true),
                    //new SubModules("9f1c2e5a-7d3b-4b8a-9f2a-1e6c4d7a2f3b", "Purchase Quotation", "-", "Purchase/All-PurchaseQuotation", "c19d9bf6-27a7-4898-bdda-2b23409fd1e3", "3", true),
                    new SubModules("6506d801-f911-4f0c-919b-930ffd589a3d", "Roles", "-", "role/all-Roles", "b62bcaca-b269-42d2-be33-aed2a9b79b0a", "4", true),
                    new SubModules("6902a024-e5d8-43d0-bdc0-eeee2e87afed", "Items/Service", "-", "item/all-Item", "005b6875-807d-43c3-988e-f439f478c8b4", "2", true),
                    new SubModules("6ce5ba46-8e55-45c3-8ebe-5d1f5c444b01", "SubLedgerGroup", "-", "subLedgerGroup/all-SubLedgerGroup", "95f57bfe-70d4-46f5-82ec-c287833824d8", "2", true),
                    new SubModules("763a636e-02df-4d5a-964d-f3f6441df59d", "Parties", "-", "parties/all-parties", "95f57bfe-70d4-46f5-82ec-c287833824d8", "4", true),
                    new SubModules("76bb8af2-2e6e-462d-8256-f755cf8323b1", "Ledger Group", "-", "ledgerGroup/all-ledgerGroup", "95f57bfe-70d4-46f5-82ec-c287833824d8", "3", true),
                    new SubModules("830808d4-ef0b-4654-ae95-ac9b6a392f27", "Settings", "-", "settings/settings", "a1ce024a-c12f-41a7-a7b9-9e92bee29e70", "1", true),
                    new SubModules("8643b2a1-c892-4467-ac39-85b83fc5399f", "Company", "-", "company/all-company", "a1ce024a-c12f-41a7-a7b9-9e92bee29e70", "13", true),
                    new SubModules("87fcd714-7ca1-4a61-9537-44c9b18233e4", "Day Book", "-", "dayBook/DayBook", "25bc79d7-33ea-42e3-a3d1-70dfe9868ab3", "3", true),
                    new SubModules("8adb240a-3330-4836-8722-4c9f9ddc7b31", "Inventory Ledger", "-", "inventory/all-Inventory", "005b6875-807d-43c3-988e-f439f478c8b4", "8", true),
                    new SubModules("925a4a9d-9905-4cde-af7d-4154cb6a8ab8", "Trial Balance", "-", "trialBalance/TrialBalance", "25bc79d7-33ea-42e3-a3d1-70dfe9868ab3", "7", true),
                    new SubModules("93dec1ad-c9e8-4dac-99e6-77ff50bc408e", "Payment Method List", "-", "paymentMethod/All-PaymentMethod", "1ce0e9b3-3795-49d0-a78d-68025f6f0273", "2", true),
                    new SubModules("a26248e1-8b5b-4aaa-87d6-2a998adaa439", "Party Balance Report", "-", "balanceReport/BalanceReport", "25bc79d7-33ea-42e3-a3d1-70dfe9868ab3", "1", true),
                    new SubModules("a5468442-30ea-4c98-bf8e-a572801df892", "Profit & Loss Statement", "-", "profitLoss/ProfitLoss", "25bc79d7-33ea-42e3-a3d1-70dfe9868ab3", "10", true),
                    new SubModules("a7462784-78e3-4841-ab5e-6d5c0faec695", "Sales Return", "-", "salesReturn/All-SalesReturn", "4f1b558f-62f9-4a4d-99f7-1940d2127a23", "4", true),
                    //new SubModules("a983ec1f-ee73-4234-998f-893cf9920e82", "Item wise Purchase Report", "-", "purchaseReport/PurchaseReport", "25bc79d7-33ea-42e3-a3d1-70dfe9868ab3", "5", true),
                    new SubModules("b7707a4a-4c0b-44cf-ac85-4f115ad38a81", "VAT Purchase Register", "-", "accountBook/PurchaseRegister", "3b3756bd-4523-49d0-90a7-221f49bff237", "1", true),
                    new SubModules("bbbc095b-bb13-4008-a3e6-9eb3e00103f9", "Chart of Accounts", "-", "account/ChartOfAccount", "95f57bfe-70d4-46f5-82ec-c287833824d8", "5", true),
                    new SubModules("cbae46cc-16f6-45b1-80d1-1eec2e2abac7", "Balance Sheet", "-", "balanceSheet/BalanceSheet", "25bc79d7-33ea-42e3-a3d1-70dfe9868ab3", "9", true),
                    new SubModules("ce0fda6f-82d2-4270-8c3b-efea0288d873", "Purchase", "-", "purchase/All-PurchaseList", "c19d9bf6-27a7-4898-bdda-2b23409fd1e3", "2", true),
                    new SubModules("cefa696b-0085-4b5f-88de-1579f6471a69", "Journal Entries", "-", "journal/all-journal", "be68dd5c-626c-44e5-b0c4-a71ac70bcccd", "2", true),
                    new SubModules("d7b6ace6-8e62-43cb-b9c5-b92489fb796e", "Units", "-", "unit/all-Units", "005b6875-807d-43c3-988e-f439f478c8b4", "6", true),
                    new SubModules("d8a5be6b-3a83-4749-8cdf-26b5522970c3", "Item Group", "-", "itemGroup/all-ItemGroup", "005b6875-807d-43c3-988e-f439f478c8b4", "4", true),
                    new SubModules("e0b833dc-10da-4ea0-b220-6cfb3e7eafbf", "Income", "-", "income/All-Income", "19ebc109-36ce-4803-8042-5c4ea4b3fd5b", "2", true),
                    new SubModules("e636a032-d9d9-4c4e-9559-dc279d721534", "Payment Method wise Statement", "-", "paymentMethod/PaymentMethodReport", "1ce0e9b3-3795-49d0-a78d-68025f6f0273", "3", true),
                    new SubModules("febed6b3-4a44-4a96-80a7-ebb62540b11a", "Annex 13", "-", "annex/AnnexReport", "25bc79d7-33ea-42e3-a3d1-70dfe9868ab3", "10", true),
                    new SubModules("03328854-b2f5-415a-922f-06c7dfcf4f2d", "Stock Adjustment", "-", "inventory/All-Stock-Adjustment", "005b6875-807d-43c3-988e-f439f478c8b4", "5", true),
                    new SubModules("98da1bc8-e5aa-44fa-9848-3ebf441e5065", "Ledger Statement", "-", "ledgerStatement/LedgerStatement", "25bc79d7-33ea-42e3-a3d1-70dfe9868ab3", "3", true),
                    new SubModules("2aab881d-c9fb-4123-9490-2ccb2fe34c6c", "Receipt", "-", "receipt/All-Receipt", "baf4bcec-354c-49f3-ad0f-55406d5720fb", "3", true),
                    new SubModules("8973491b-3272-4190-9adb-30c9f9900037", "Sales Quotation", "-", "sales/All-SalesQuotation", "4f1b558f-62f9-4a4d-99f7-1940d2127a23", "2", true),
                    new SubModules("56ef86ad-7b81-4dc6-a0d1-70bb4022fa7c", "Purchase Order", "-", "Purchase/All-PurchaseQuotation", "c19d9bf6-27a7-4898-bdda-2b23409fd1e3", "3", true),
                    new SubModules("e00cf7ea-fa03-470f-8013-3d4d4054f13b", "Party Statement", "-", "partiesStatement/PartiesStatement", "25bc79d7-33ea-42e3-a3d1-70dfe9868ab3", "1", true),
                    new SubModules("43d97c20-7f6e-40c6-8167-c4403b098656", "Sales Reports", "-", "salesReport/SalesReport", "25bc79d7-33ea-42e3-a3d1-70dfe9868ab3", "4", true),
                    new SubModules("c7e2b1d4-5f8a-4c3e-9b6d-2a1f7e4c8b9d", "Purchase Reports", "-", "purchaseReport/PurchaseReport", "25bc79d7-33ea-42e3-a3d1-70dfe9868ab3", "5", true),
                    new SubModules("5391b012-d493-4e52-8cd9-cba48f8ebf08", "Inventory Reports", "-", "InventoryReport/InventoryReports", "25bc79d7-33ea-42e3-a3d1-70dfe9868ab3", "6", true),
                    new SubModules("236724ac-8db1-45f3-b66f-d834ce79b990", "Bill Sundry", "-", "billSundry/BillSundry", "95f57bfe-70d4-46f5-82ec-c287833824d8", "6", true),
                    new SubModules("2251bb74-27c1-4ecd-95a9-6634b5425cf2", "Stock Transfer", "-", "stockTransfer/all-StockTransfer", "005b6875-807d-43c3-988e-f439f478c8b4", "3", true),




                 };

                await _context.SubModules.AddRangeAsync(subModules);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        #endregion


        #region SeedMenus
        private async Task SeedMenus()
        {
            if (!await _context.Menus.AnyAsync())
            {
                var menus = new List<Menu>()
                {
                        new Menu("06426815-10d9-4617-8040-5da17b46e269", "Delete", "delete/itemsGroup", "-", "d8a5be6b-3a83-4749-8cdf-26b5522970c3", 1, true),
                        new Menu("071b1e3a-d5a2-4053-8dfb-a436a2311435", "Delete", "delete/conversionFactor", "-", "160cf130-a172-4005-bfe2-27479e2954d1", 1, true),
                        new Menu("0ce5fece-831f-40c8-827f-46e5a47a8f68", "Add", "add/rolePermission", "-", "46fdf094-2c42-47e1-b9fe-1beab735ba82", 1, true),
                        new Menu("106105c6-dd4a-43b1-9a72-32085a3f1f1a", "Edit", "edit/journal", "-", "cefa696b-0085-4b5f-88de-1579f6471a69", 1, true),
                        new Menu("1450dbe5-f304-493b-817d-f4f79968bdb0", "Delete", "delete/units", "-", "d7b6ace6-8e62-43cb-b9c5-b92489fb796e", 1, true),
                        new Menu("17dc739a-5b3d-44bf-83a2-5f38dbeda851", "Edit", "edit/items", "-", "6902a024-e5d8-43d0-bdc0-eeee2e87afed", 1, true),
                        new Menu("1c96afd1-591b-49bf-b0e4-0eb4315dec53", "Add", "add/All-StockCenter", "-", "2850f32a-d537-4b15-be20-141ee129fc61", 2, true),
                        new Menu("1ca49895-54a1-4590-9b4b-1af0b676adff", "Edit", "edit/ledger", "-", "3f5fa4c7-beb3-4c81-8679-823ed2cd71b0", 1, true),
                        new Menu("225a047a-7a21-4fa3-82d9-4ef7f3db2459", "Edit", "edit/conversionFactor", "-", "160cf130-a172-4005-bfe2-27479e2954d1", 1, true),
                        new Menu("238fa256-62e7-4198-9f92-e275a4730829", "Add", "Add/Receipt", "-", "2aab881d-c9fb-4123-9490-2ccb2fe34c6c", 1, true),
                        new Menu("33ea0146-4220-477d-ba3e-9febc43c4a4b", "Add", "add/ledger", "-", "3f5fa4c7-beb3-4c81-8679-823ed2cd71b0", 1, true),
                        new Menu("38b5e29d-a3c8-4383-9b3f-6a79f5389377", "Delete", "User/Delete", "-", "52197ced-2cf3-45c9-a8e0-9c17942e44a3", 1, true),
                        new Menu("3da68c38-6bc8-4f15-8d07-3274e57230de", "Delete", "delete/rolePermission", "-", "46fdf094-2c42-47e1-b9fe-1beab735ba82", 1, true),
                        new Menu("428dfd7a-7541-4df2-bb40-8071c553428e", "Assign", "modulePermission/roles", "-", "6506d801-f911-4f0c-919b-930ffd589a3d", 1, true),
                        new Menu("447da577-e1ee-421d-849f-1831df07f2f2", "Edit", "Edit/Income", "-", "e0b833dc-10da-4ea0-b220-6cfb3e7eafbf", 2, true),
                        new Menu("489bb34a-49c7-45d1-8fb3-2b95ce7f425a", "Delete", "d06426815-10d9-4617-8040-5da17b46e269elete/ledger", "-", "3f5fa4c7-beb3-4c81-8679-823ed2cd71b0", 1, true),
                        new Menu("4afbd064-c66a-49cc-a5d7-b06037e314a4", "Add", "journal/add", "-", "cefa696b-0085-4b5f-88de-1579f6471a69", 1, true),
                        new Menu("529bab23-3cf4-499c-acc2-a3fdae8811b9", "Add", "Add/Company", "-", "8643b2a1-c892-4467-ac39-85b83fc5399f", 1, true),
                        new Menu("52ecf9e2-1804-4c35-9215-6528dea8dc9e", "Add", "Add/Parties", "-", "763a636e-02df-4d5a-964d-f3f6441df59d", 1, true),
                        new Menu("53c1b49e-3ef7-417a-ba60-fe06434f04d8", "Edit", "Edit/Parties", "-", "763a636e-02df-4d5a-964d-f3f6441df59d", 2, true),
                        new Menu("567a9575-b871-48c1-bd6e-b9e0a0b59091", "Delete", "delete/roles", "-", "6506d801-f911-4f0c-919b-930ffd589a3d", 1, true),
                        new Menu("56d1ac2f-6d1c-47f8-a4b1-279d6fa227d1", "Add", "Add/User", "-", "52197ced-2cf3-45c9-a8e0-9c17942e44a3", 1, true),
                        new Menu("583a1cfe-9e93-4efe-a5b2-2e343bc5c481", "Edit", "edit/itemsGroup", "-", "d8a5be6b-3a83-4749-8cdf-26b5522970c3", 1, true),
                        new Menu("588f65e9-5f05-475f-8927-ff87237cb6b4", "Delete", "Delete/purchase-Return", "-", "26f19645-9a63-4164-82ce-3574a1d3ca14", 3, true),
                        new Menu("5bff0e61-7b16-40b5-842a-d6259107f37c", "Edit", "edit/roles", "-", "6506d801-f911-4f0c-919b-930ffd589a3d", 1, true),
                        new Menu("5db70aee-af8f-4a74-90e1-28ae81a560ad", "Edit", "user/", "-", "52197ced-2cf3-45c9-a8e0-9c17942e44a3", 1, true),
                        new Menu("5dc6e81a-d98b-43b0-9768-88a42e75c328", "Delete", "Delete/All-StockCenter", "-", "2850f32a-d537-4b15-be20-141ee129fc61", 2, true),
                        new Menu("5fbfeedc-e53b-4388-86ae-766cc295bebd", "Add", "Add/subledgergroup", "-", "6ce5ba46-8e55-45c3-8ebe-5d1f5c444b01", 1, true),
                        new Menu("60cf00ee-3ffb-4e59-90bf-d13b95f28437", "Add", "purchase/purchase-Settings", "-", "ce0fda6f-82d2-4270-8c3b-efea0288d873", 1, true),
                        new Menu("60fd7631-605e-4275-92fe-0cd9ad80558f", "Delete", "Delete/subledgergroup", "-", "6ce5ba46-8e55-45c3-8ebe-5d1f5c444b01", 3, true),
                        new Menu("618e7a80-edb8-404c-b39f-3381af262524", "Edit", "Edit/Ledgergroup", "-", "76bb8af2-2e6e-462d-8256-f755cf8323b1", 2, true),
                        new Menu("641cc6f3-c289-4fa6-9701-0926796d36ea", "Edit", "Edit/Payment", "-", "50a3eced-9954-4c75-a0cc-a6e2d11c7b3c", 2, true),
                        new Menu("66ac75c2-23c2-43d5-87ba-6023281d10e1", "Add", "add/conversionFactor", "-", "160cf130-a172-4005-bfe2-27479e2954d1", 1, true),
                        new Menu("66f6dbca-d2db-4af8-9682-68f0dfb09aa5", "Edit", "Edit/InventoryLedger", "-", "8adb240a-3330-4836-8722-4c9f9ddc7b31", 1, true),
                        new Menu("6844e691-4985-4fc4-8124-7d4acc7c330b", "Add", "Add/Income", "-", "e0b833dc-10da-4ea0-b220-6cfb3e7eafbf", 1, true),
                        new Menu("68bfc5b9-767b-4ef4-b802-3aea36c9bd87", "Add", "Add/All-Receivable", "-", "3a86a670-db6f-430b-8d2d-d02a24c93ea9", 1, true),
                        new Menu("6beca6df-f7fb-4066-9edf-3007e735be3b", "Delete", "sales/All-Sales", "-", "41fe19cf-37ab-4c8e-be2a-9ab5696dab69", 1, true),
                        new Menu("6ee9a440-4772-43e7-9c13-b34722e040b5", "Delete", "delete/items", "-", "6902a024-e5d8-43d0-bdc0-eeee2e87afed", 1, true),
                        new Menu("71d4cdd5-b9cc-457b-9108-7927ec243bc5", "Add", "Add/salesQuotation", "-", "8973491b-3272-4190-9adb-30c9f9900037", 1, true),
                        new Menu("75cc9941-e151-4001-84d1-c57533fd631e", "Add", "Add/PurchaseOrder", "-", "56ef86ad-7b81-4dc6-a0d1-70bb4022fa7c", 1, true),
                        new Menu("784fa5e4-719e-42ea-8f42-2f9498d9d6ec", "Delete", "Delete/LedgerGroup", "-", "76bb8af2-2e6e-462d-8256-f755cf8323b1", 3, true),
                        new Menu("7a9cd2b4-edc3-4c2e-9989-66180c7bb155", "Assign", "assignRoleToUser/rolePermission", "-", "46fdf094-2c42-47e1-b9fe-1beab735ba82", 1, true),
                        new Menu("7daf643c-41c8-448b-b2bc-c898dfb030ad", "Add", "add/roles", "-", "6506d801-f911-4f0c-919b-930ffd589a3d", 1, true),
                        new Menu("8a316bcd-3f31-4659-9a70-6506771f0873", "Add", "Add/Salesreturn", "-", "a7462784-78e3-4841-ab5e-6d5c0faec695", 1, true),
                        new Menu("8ebfcadd-0222-414c-8b8c-c76df97a1bcf", "Edit", "edit/units", "-", "d7b6ace6-8e62-43cb-b9c5-b92489fb796e", 1, true),
                        new Menu("99ef8be0-948e-4512-bc77-51837243aae9", "Delete", "Delete/Parties", "-", "763a636e-02df-4d5a-964d-f3f6441df59d", 3, true),
                        new Menu("a02b5d23-948d-4cc8-b606-2d2dc082a9cf", "add", "sales/All-Sales", "-", "41fe19cf-37ab-4c8e-be2a-9ab5696dab69", 1, true),
                        new Menu("ac2ec6b3-e495-4da1-b2d5-5ac2d96582f8", "Add", "Add/purchase-Return", "-", "26f19645-9a63-4164-82ce-3574a1d3ca14", 1, true),
                        new Menu("b2fd5264-63b5-422b-9717-2041120128f9", "Add", "add/itemsGroup", "-", "d8a5be6b-3a83-4749-8cdf-26b5522970c3", 1, true),
                        new Menu("b3d0da91-e6b9-4b76-90d5-d1327fdcc0af", "edit", "edit/Receivable", "-", "3a86a670-db6f-430b-8d2d-d02a24c93ea9", 2, true),
                        new Menu("c07e18dd-4d54-4eab-a9dc-12caafe58d28", "Add", "Add/Expenses", "-", "2b54da4f-8826-45df-86fc-eecae8e4b3ff", 1, true),
                        new Menu("c4f1c3f4-9bec-4f60-9e5b-968ab378c858", "edit", "sales/All-Sales", "-", "41fe19cf-37ab-4c8e-be2a-9ab5696dab69", 1, true),
                        new Menu("c5383f8f-655d-47a9-a156-08b3a8bf2556", "Assign", "assignRole/User", "-", "52197ced-2cf3-45c9-a8e0-9c17942e44a3", 1, true),
                        new Menu("cb1b124b-a0da-4bef-a2a3-a3c2f9fbb157", "Delete", "delete/journal", "-", "cefa696b-0085-4b5f-88de-1579f6471a69", 1, true),
                        new Menu("cb9ce62b-faaa-4658-8ac5-5a1d91b6c1e8", "Add", "Add/Payment", "-", "50a3eced-9954-4c75-a0cc-a6e2d11c7b3c", 1, true),
                        new Menu("cf11eefe-3cf1-49fd-967c-d40fe644d62c", "Edit", "purchase/purchase-Settings", "-", "ce0fda6f-82d2-4270-8c3b-efea0288d873", 1, true),
                        new Menu("cf180f4f-16e7-47f4-81fa-4125cc17008b", "Add", "Add/Paymentmethodlist", "-", "93dec1ad-c9e8-4dac-99e6-77ff50bc408e", 1, true),
                        new Menu("cf89cc79-7bbd-41f6-93bd-af59005c5e07", "Edit", "Edit/All-StockCenter", "-", "2850f32a-d537-4b15-be20-141ee129fc61", 3, true),
                        new Menu("d3da16d7-e75c-4111-965e-e16eed51f703", "Delete", "purchase/purchase-Settings", "-", "ce0fda6f-82d2-4270-8c3b-efea0288d873", 1, true),
                        new Menu("d953d9a5-2462-441a-ace5-55fffa65c463", "Add", "Add/Stock Adjustment", "-", "03328854-b2f5-415a-922f-06c7dfcf4f2d", 2, true),
                        new Menu("e1e685db-bd6f-48f6-b69d-02822cd1162c", "Delete", "Delete/salesReturn", "-", "a7462784-78e3-4841-ab5e-6d5c0faec695", 2, true),
                        new Menu("e2f9b158-fe55-4ccf-9787-a837d9997548", "Edit", "Edit/subledgergroup", "-", "6ce5ba46-8e55-45c3-8ebe-5d1f5c444b01", 2, true),
                        new Menu("e5544b94-0a75-4f14-bce6-99f443f43a40", "Add", "add/units", "-", "d7b6ace6-8e62-43cb-b9c5-b92489fb796e", 1, true),
                        new Menu("f0a70229-54b0-485c-9589-dbcccad9f407", "Edit", "Edit/Paymentmethodlist", "-", "93dec1ad-c9e8-4dac-99e6-77ff50bc408e", 2, true),
                        new Menu("f75b502d-d524-484b-bd80-4c3fe08941c3", "Edit", "edit/rolePermission", "-", "46fdf094-2c42-47e1-b9fe-1beab735ba82", 1, true),
                        new Menu("f7bb99df-beb8-4409-a026-9cf00ff6f327", "Edit", "Edit/Receipt", "-", "2aab881d-c9fb-4123-9490-2ccb2fe34c6c", 2, true),
                        new Menu("fa446e15-a67c-4a17-9b3b-4a9573ea0430", "Add", "Add/Ledgergroup", "-", "76bb8af2-2e6e-462d-8256-f755cf8323b1", 1, true),
                        new Menu("fad88bc9-9b1c-4b71-a0a9-b654fa7fdc21", "Edit", "Edit/Expenses", "-", "2b54da4f-8826-45df-86fc-eecae8e4b3ff", 2, true),
                        new Menu("fd7fd33a-4ace-409f-9725-690056f03e56", "Add", "add/items", "-", "6902a024-e5d8-43d0-bdc0-eeee2e87afed", 1, true),









                };

                await _context.Menus.AddRangeAsync(menus);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        #endregion

        #region SeedLedger
        public async Task<List<Ledger>> SeedLedger()
        {
            var ledger = new List<Ledger>()
                {

                //new Ledger("5f08c4d7-2a6e-4b21-9b7d-72a8d64bcd7a", "Advertisement & Publicity", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("0722c653-4a75-4375-ad78-b1a4a7bab948", "Bank Charges", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("380e6fb8-e16e-4336-a250-e70584fca610", "Books & Periodicals", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("668f4147-d22b-4ce1-a193-e40e8f682006", "Capital Equipments", DateTime.UtcNow, true, "", "", "", "", "", "j0d1e2f3-a456-7890-j123-k456l789m012", "", ""),
                //new Ledger("dc5a0bbc-b768-47a9-bed7-b0375a48c534", "Cash", DateTime.UtcNow, true, "", "", "", "", "", "d4b5c6f7-a890-1234-d567-e890f123g456", "", ""),
                //new Ledger("931731ad-31d7-4280-ace0-00e856d2640e", "Charity & Donation", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("aa650485-c8f8-424d-81fb-47c06837983d", "Commission on Sales", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("856d6231-ad00-40a3-82a6-94222fc790ea", "Computers", DateTime.UtcNow, true, "", "", "", "", "", "j0d1e2f3-a456-7890-j123-k456l789m012", "", ""),
                //new Ledger("ea556d52-d2e7-4a74-8da8-8873c2bd4cb2", "Conveyance Expenses", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("095b8243-3df8-4de9-ae15-db0f75973af3", "Customer Entertainment Expenses", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("e21ca252-98dc-428c-87b4-fde284081975", "Depreciation A/C", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("91829070-fb2b-4c49-b6a6-e1016ca5323c", "Earnest Money", DateTime.UtcNow, true, "", "", "", "", "", "x4r5s6t7-a890-1234-x567-y890z123a456", "", ""),
                //new Ledger("4079c634-1442-48dd-ac64-d0475b3d62c9", "Freight & Forwarding Charges", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("96081e9f-e7dc-4a57-93bf-d14fed2329c1", "Furniture & Fixtures", DateTime.UtcNow, true, "", "", "", "", "", "j0d1e2f3-a456-7890-j123-k456l789m012", "", ""),
                //new Ledger("786f382b-03a5-44ef-9b90-ee50435161b3", "Legal Expenses", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("14246de5-dcfe-46d6-9d42-9be27fe4065d", "Miscellaneous Expenses", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("5baf1091-dd7b-47cd-a0c5-1c8a14e1c7f3", "Office Equipments", DateTime.UtcNow, true, "", "", "", "", "", "j0d1e2f3-a456-7890-j123-k456l789m012", "", ""),
                //new Ledger("c2e28eef-8cda-4741-a4f4-28c84a5516e8", "Office Maintenance Expenses", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("30aca4bf-91df-42bf-8d31-0f39f3afc7f2", "Office Rent", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("3fb9eae6-d82c-4ddd-8ef5-da8bc4b53bb2", "Plant & Machinery", DateTime.UtcNow, true, "", "", "", "", "", "j0d1e2f3-a456-7890-j123-k456l789m012", "", ""),
                //new Ledger("312e2e68-4efa-4c1a-9fda-0c30f9bf0707", "Postal Expenses", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("e4d3c579-5a5b-49cc-9b37-90f6f282bc93", "Printing & Stationary", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),

                //// Additional Ledgers
                //new Ledger("c3f4a2a5-2b0b-4c9f-9b37-3e0bb5a2f97e", "Profit & Loss", DateTime.UtcNow, true, "", "", "", "", "", "q7k8l9m0-a123-4567-q890-r123s456t789", "", ""),
                //new Ledger("e375cf83-149f-44ae-8f00-69019f9f61bb", "Purchase", DateTime.UtcNow, true, "", "", "", "", "", "s9m0n1o2-a345-6789-s012-t345u678v901", "", ""),
                //new Ledger("c04fe4fb-f761-4d55-b9bf-8e57cecbea18", "Rounded Off", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("7966736d-649d-4c63-ada8-3089a2bdf8c6", "Salary", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("204b702f-12b5-43f8-a8eb-b3cb6e4ca60a", "Salary & Bonus Payable", DateTime.UtcNow, true, "", "", "", "", "", "r8l9m0n1-a234-5678-r901-s234t567u890", "", ""),
                //new Ledger("5fa06b9c-ba4f-4d37-b049-a2e742bfc8d3", "Sales", DateTime.UtcNow, true, "", "", "", "", "", "v2p3q4r5-a678-9012-v345-w678x901y234", "", ""),
                //new Ledger("da288c41-f087-41b4-b9ee-ebd2098ca4a9", "Sales Promotion Expenses", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("5ed3243e-f7f1-4d91-b39f-24da38d65c0b", "Service Charges Paid", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("d119a2de-3406-4368-8eb7-68602e3d864b", "Service Charges Receipts", DateTime.UtcNow, true, "", "", "", "", "", "l2f3g4h5-a678-9012-l345-m678n901o234", "", ""),
                //new Ledger("2a4222cc-cab5-4c66-98f1-7fd36c933208", "Staff Welfare Expenses", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("b11ac192-4fe6-425f-9398-caec0e1ca9aa", "Telephone Expenses", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("f3bbb573-d6de-4245-a7e2-05c279207355", "Travelling Expenses", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("2a1828f8-0210-4839-986b-2410d2207aad", "Water & Electricity Expenses", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("d03d5a2e-7798-4b2e-9e8a-5c54a43a5311", "Cost of Good Sold", DateTime.UtcNow, true, "", "", "", "", "", "h8f9c0d1-a234-5678-h901-i234j567k890", "", ""),

                //new Ledger("e1a3b5f6-8d4c-4c2f-9e2e-3a6f4b7d8e9f", "Bad Debts Written off", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("e8f32b99-639a-4a5f-b3c3-3aaff1c79d65", "Discount Received", DateTime.UtcNow, true, "", "", "", "", "", "u1o2p3q4-a567-8901-u234-v567w890x123", "", ""),
                //new Ledger("b5e7d3c9-4a6f-4d1e-92b0-7c2f8a9d6f3b", "Vat", DateTime.UtcNow, true, "", "", "", "", "", "g7e8f9c0-a123-4567-g890-h123i456j789", "", ""),
                //new Ledger("6c2f8a7d-3b4e-41d9-91f0-a5e7b6d3c9f2", "Discount Allowed", DateTime.UtcNow, true, "", "", "", "", "", "i9c0d1e2-a345-6789-i012-j345k678l901", "", ""),
                //new Ledger("1e4f1b9d-8c3d-4f2a-a9e3-b6e1c7f521a2", "Stock", DateTime.UtcNow, true, "", "", "", "", "", "y5s6t7u8-a901-2345-y678-z901a234b567", "", ""),

                //new Ledger("a1b2c3d4-e567-890f-gh12-ijkl34567890", "Accounts Receivable (Credit Sales)", DateTime.UtcNow, true, "", "", "", "", "", "e5c6d7f8-a901-2345-e678-f901g234h567", "", ""),
               
                //new Ledger("b2c3d4e5-f678-901g-hijk-123456789012", "Accounts Payable (Credit Purchases)", DateTime.UtcNow, true, "", "", "", "", "", "f6d7e8b9-a012-3456-f789-g012h345i678", "", ""),


                
                // Additional Ledgers
                new Ledger("e375cf83-149f-44ae-8f00-69019f9f61bb", "Debit Card A/C", DateTime.Now, true, "", "", "", "", "", "7a9a6c6f-3b4a-4e58-b13c-c61e7bba9d72", "", "",0,true,true),
                new Ledger("c04fe4fb-f761-4d55-b9bf-8e57cecbea18", "Cheque Collection A/C", DateTime.Now, true, "", "", "", "", "", "7a9a6c6f-3b4a-4e58-b13c-c61e7bba9d72", "", "",0,true,true),
                new Ledger("7966736d-649d-4c63-ada8-3089a2bdf8c6", "Stock In Hand A/C", DateTime.Now, true, "", "", "", "", "", "1f91a98b-9d6e-4a59-ade1-ef145ae08f5e", "", "",0,true,true),
                new Ledger("204b702f-12b5-43f8-a8eb-b3cb6e4ca60a", "RoundOff A/C", DateTime.Now, true, "", "", "", "", "", "e84cfc6a-2289-4b4a-9ec3-88dc7f0bdf34", "", "",0,true,true),
                new Ledger("5fa06b9c-ba4f-4d37-b049-a2e742bfc8d3", "VAT A/C", DateTime.Now, true, "", "", "", "", "", "e84cfc6a-2289-4b4a-9ec3-88dc7f0bdf34", "", "", 0, true,true),
                new Ledger("da288c41-f087-41b4-b9ee-ebd2098ca4a9", "TDS A/C", DateTime.Now, true, "", "", "", "", "", "e84cfc6a-2289-4b4a-9ec3-88dc7f0bdf34", "", "", 0, true,true),
                new Ledger("5ed3243e-f7f1-4d91-b39f-24da38d65c0b", "Salary TDS A/C", DateTime.Now, true, "", "", "", "", "", "e84cfc6a-2289-4b4a-9ec3-88dc7f0bdf34", "", "", 0, true,true),
                new Ledger("d119a2de-3406-4368-8eb7-68602e3d864b", "Sales A/C", DateTime.Now, true, "", "", "", "", "", "623c3133-f4c7-41e5-a9c1-382c749d3a8a", "", "", 0, true,true),
                new Ledger("2a4222cc-cab5-4c66-98f1-7fd36c933208", "Discount A/C", DateTime.Now, true, "", "", "", "", "", "623c3133-f4c7-41e5-a9c1-382c749d3a8a", "", "", 0, true,true),
                new Ledger("b11ac192-4fe6-425f-9398-caec0e1ca9aa", "Sales Return A/C", DateTime.Now, true, "", "", "", "", "", "623c3133-f4c7-41e5-a9c1-382c749d3a8a", "", "", 0, true,true),
                new Ledger("f3bbb573-d6de-4245-a7e2-05c279207355", "Purchase A/C", DateTime.Now, true, "", "", "", "", "", "8a6d5de6-5607-497e-8d7c-90d7494d7aa7", "", "",0,true,true),
                new Ledger("c5bb7f79-0af7-4c11-9bfa-2a746ec8a99d", "COGS A/C", DateTime.Now, true, "", "", "", "", "", "0423b6c3-51fa-44c4-bd9a-28fa9697ff55", "", "", 0, true,true),
                new Ledger("2a1828f8-0210-4839-986b-2410d2207aad", "PurchaseReturn A/C", DateTime.Now, true, "", "", "", "", "", "8a6d5de6-5607-497e-8d7c-90d7494d7aa7", "", "", 0, true,true),
                new Ledger("f3e7a50f-ec7d-4b8c-b05e-46b7cbcfb4e1", "Cash A/C", DateTime.Now, true, "", "", "", "", "", "3d5c1e24-d0ae-4f74-9c88-bf9f4b5c4d0b", "", "", 0, true,true),
                new Ledger("b3f8d2a1-9c4e-4e1b-8f32-7a2d5c9f0e6b", "Share Capital A/C", DateTime.Now, true, "", "", "", "", "", "d4e7f9b2-1c3a-4f6d-9b8e-2a7c5d1f0e9a", "", "", 0, true,true),
                new Ledger("30085bd6-f231-45fb-84c6-e57a5788317b", "Stock Loss A/C", DateTime.Now, true, "", "", "", "", "", "f021ca46-74ae-4a3c-8797-d21b8e4e6091", "", "", 0, true,true),
                new Ledger("693f6f36-639b-458b-9303-e48e701b2c3c", "Marketing and Advertisement A/C", DateTime.Now, true, "", "", "", "", "", "f613be60-2268-4d65-8877-f794c9816569", "", "", 0, true,true),
                new Ledger("e2a41054-92ee-4212-a3f7-e4a22022a107", "Stock Gain A/C", DateTime.Now, true, "", "", "", "", "", "aec78dce-10c3-410e-a89e-e29dcf3faa3d", "", "", 0, true,true),



                };
            if (!await _context.Ledgers.AnyAsync())
            {
                await _context.Ledgers.AddRangeAsync(ledger);
                await _unitOfWork.SaveChangesAsync();
            }
            return ledger;
        }

        #endregion


        #region SeedFiscalYear

        private async Task SeedFiscalYear()
        {
            if (!await _context.FiscalYears.AnyAsync())
            {
                var fiscalYears = new List<FiscalYears>
        {
                new FiscalYears(Guid.NewGuid().ToString(), "2060/61", new DateTime(2003, 7, 16), new DateTime(2004, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2061/62", new DateTime(2004, 7, 16), new DateTime(2005, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2062/63", new DateTime(2005, 7, 16), new DateTime(2006, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2063/64", new DateTime(2006, 7, 17), new DateTime(2007, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2064/65", new DateTime(2007, 7, 17), new DateTime(2008, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2065/66", new DateTime(2008, 7, 16), new DateTime(2009, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2066/67", new DateTime(2009, 7, 16), new DateTime(2010, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2067/68", new DateTime(2010, 7, 17), new DateTime(2011, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2068/69", new DateTime(2011, 7, 17), new DateTime(2012, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2070/71", new DateTime(2013, 7, 16), new DateTime(2014, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2071/72", new DateTime(2014, 7, 17), new DateTime(2015, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2072/73", new DateTime(2015, 7, 17), new DateTime(2016, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2073/74", new DateTime(2016, 7, 16), new DateTime(2017, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2074/75", new DateTime(2017, 7, 16), new DateTime(2018, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2075/76", new DateTime(2018, 7, 17), new DateTime(2019, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2076/77", new DateTime(2019, 7, 17), new DateTime(2020, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2077/78", new DateTime(2020, 7, 16), new DateTime(2021, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2078/79", new DateTime(2021, 7, 16), new DateTime(2022, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2079/80", new DateTime(2022, 7, 17), new DateTime(2023, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2080/81", new DateTime(2023, 7, 17), new DateTime(2024, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2081/82", new DateTime(2024, 7, 16), new DateTime(2025, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2082/83", new DateTime(2025, 7, 16), new DateTime(2026, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2083/84", new DateTime(2026, 7, 17), new DateTime(2027, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2084/85", new DateTime(2027, 7, 17), new DateTime(2028, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2085/86", new DateTime(2028, 7, 16), new DateTime(2029, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2086/87", new DateTime(2029, 7, 16), new DateTime(2030, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2087/88", new DateTime(2030, 7, 17), new DateTime(2031, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2088/89", new DateTime(2031, 7, 17), new DateTime(2032, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2089/90", new DateTime(2032, 7, 16), new DateTime(2033, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2090/91", new DateTime(2033, 7, 16), new DateTime(2034, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2091/92", new DateTime(2034, 7, 17), new DateTime(2035, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2092/93", new DateTime(2035, 7, 17), new DateTime(2036, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2093/94", new DateTime(2036, 7, 16), new DateTime(2037, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2094/95", new DateTime(2037, 7, 17), new DateTime(2038, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2095/96", new DateTime(2038, 7, 17), new DateTime(2039, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2096/97", new DateTime(2039, 7, 17), new DateTime(2040, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2097/98", new DateTime(2040, 7, 16), new DateTime(2041, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2098/99", new DateTime(2041, 7, 17), new DateTime(2042, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2099/00", new DateTime(2042, 7, 17), new DateTime(2043, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2100/01", new DateTime(2043, 7, 17), new DateTime(2044, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2101/02", new DateTime(2044, 7, 16), new DateTime(2045, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2102/03", new DateTime(2045, 7, 17), new DateTime(2046, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2103/04", new DateTime(2046, 7, 17), new DateTime(2047, 7, 16, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2104/05", new DateTime(2047, 7, 17), new DateTime(2048, 7, 15, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2105/06", new DateTime(2048, 7, 16), new DateTime(2049, 7, 17, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2106/07", new DateTime(2049, 7, 18), new DateTime(2050, 7, 17, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2107/08", new DateTime(2050, 7, 18), new DateTime(2051, 7, 17, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2108/09", new DateTime(2051, 7, 18), new DateTime(2052, 7, 17, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2109/10", new DateTime(2052, 7, 18), new DateTime(2053, 7, 17, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2110/11", new DateTime(2053, 7, 18), new DateTime(2054, 7, 17, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2111/12", new DateTime(2054, 7, 18), new DateTime(2055, 7, 17, 23, 59, 0)),
                new FiscalYears(Guid.NewGuid().ToString(), "2112/13", new DateTime(2055, 7, 18), new DateTime(2056, 7, 17, 23, 59, 0))
        };

                await _context.FiscalYears.AddRangeAsync(fiscalYears);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        #endregion






    }
}
