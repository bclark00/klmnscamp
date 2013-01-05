using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Klmsncamp.Models;

namespace Klmsncamp.DAL
{
    public class KlmsnInitializer : DropCreateDatabaseAlways<KlmsnContext>
    {
        protected override void Seed(KlmsnContext context)
        {
            var validationStates = new List<ValidationState>
            {
                new ValidationState { Description = "AKTİF"  },
                new ValidationState{ Description = "PASİF" }
            };
            validationStates.ForEach(s => context.ValidationStates.Add(s));
            context.SaveChanges();

            var requestStates = new List<RequestState>
            {
                new RequestState { Description = "DEVAM EDİYOR" },
                new RequestState { Description = "FİRMA BEKLİYOR" },
                new RequestState { Description = "PARÇA BEKLİYOR" },
                new RequestState { Description = "PLANLANDI" },
                new RequestState { Description = "TAMAMLANDI" }
            };
            requestStates.ForEach(s => context.RequestStates.Add(s));
            context.SaveChanges();

            var requestTypes = new List<RequestType>
            {
                new RequestType {Description = "YAZILIM"},
                new RequestType { Description = "EBA",ParentRequestTypeId=1 },
                new RequestType { Description = "PLIS" ,ParentRequestTypeId=1},
                new RequestType {Description="İşletim Sistemi (WINDOWS ve türevleri)",ParentRequestTypeId=1} ,
                new RequestType {Description ="DONANIM"},
                new RequestType {Description= "PC" ,ParentRequestTypeId=5},
                new RequestType {Description="LAPTOP", ParentRequestTypeId=5},
                new RequestType {Description="DİĞER"}
            };
            requestTypes.ForEach(s => context.RequestTypes.Add(s));
            context.SaveChanges();

            var rqreasons = new List<RequestActualReason>
            {
                new RequestActualReason { Description = "--", ValidationStateID=1},
                new RequestActualReason { Description= "Kullanıcı Kaynaklı", ValidationStateID=1},
                new RequestActualReason { Description = "İç Kaynaklı" , ValidationStateID=1},
                new RequestActualReason { Description = "Dış Kaynaklı" , ValidationStateID=1 }
            };
            rqreasons.ForEach(s => context.RequestActualReasons.Add(s));
            context.SaveChanges();

            var workshops = new List<Workshop>
            {
                new Workshop { Description = "BİLGİ İŞLEM", ValidationStateID=1  },
                new Workshop { Description = "TEKNİK SERVİS", ValidationStateID=1  }
            };
            workshops.ForEach(s => context.Workshops.Add(s));
            context.SaveChanges();

            var modules = new List<Module>
            {
                new Module { Description = "İş Takip Modülü" , Users= new List<User>(), UserGroups= new List<UserGroup>()},
                new Module { Description = "Malzeme-Parça Bul", Users= new List<User>(), UserGroups= new List<UserGroup>() },
            };
            modules.ForEach(s => context.Modules.Add(s));
            context.SaveChanges();

            var invowns = new List<InventoryOwnership>
            {
                new InventoryOwnership { Description="AR" },
                new InventoryOwnership { Description = "DIŞ FİRMA"},
                new InventoryOwnership { Description = "DİĞER"}
            };
            invowns.ForEach(s => context.InventoryOwnerships.Add(s));
            context.SaveChanges();

            var corptypes = new List<CorporateType>
            {
                new CorporateType { Description = "FİRMA" },
                new CorporateType { Description = "YETKİLİ SERVİS"},
                new CorporateType { Description = "ŞAHIS" },
                new CorporateType { Description = "İÇ KULLANICI"}
            };
            corptypes.ForEach(s => context.CorporateTypes.Add(s));
            context.SaveChanges();

            var locationgroups = new List<LocationGroup>
            {
                new LocationGroup { Description = "BİLGİ İŞLEM" },
                new LocationGroup { Description = "İNSAN KAYNAKLARI" },
                new LocationGroup { Description = "LOJİSTİK" },
                new LocationGroup { Description = "MALİ İŞLER"},
                new LocationGroup { Description = "SATIŞ" },
                new LocationGroup { Description = "SATIŞ SONRASI HİZMETLER"},
                new LocationGroup { Description = "ÜRETİM VE TEKNİK İŞLER" },
                new LocationGroup { Description = "YÖNETİM" },
                new LocationGroup { Description = "6 SIGMA"}
            };
            locationgroups.ForEach(s => context.LocationGroups.Add(s));
            context.SaveChanges();

            var materialcategories = new List<MaterialCategory>
            {
                new MaterialCategory { Description = "Devre Elemanları" ,ValidationStateID = 1},
                new MaterialCategory { Description = "Entegreler",ValidationStateID = 1 },
                new MaterialCategory { Description = "Diyotlar" , ParentMaterialCategoryID= 1,ValidationStateID = 1 },
                new MaterialCategory { Description = "Dirençler" , ParentMaterialCategoryID = 1,ValidationStateID = 1 },
                new MaterialCategory { Description = "Kapasiteler", ParentMaterialCategoryID = 1 ,ValidationStateID = 1},
                new MaterialCategory { Description = "Mikroişlemciler",ParentMaterialCategoryID= 2 ,ValidationStateID = 1},
                new MaterialCategory { Description = "Mekanik Komponentler",ValidationStateID = 1 },
                new MaterialCategory { Description = "RS-232 konnektör", ParentMaterialCategoryID =7,ValidationStateID = 1 },
                new MaterialCategory { Description = "RS-486 konnektör", ParentMaterialCategoryID = 7 ,ValidationStateID = 1} ,
                new MaterialCategory { Description = "ARM", ParentMaterialCategoryID = 6,ValidationStateID = 1},
                new MaterialCategory { Description = "Texas", ParentMaterialCategoryID =6 ,ValidationStateID = 1}
            };
            materialcategories.ForEach( s => context.MaterialCategories.Add(s));
            context.SaveChanges();

            var materialgroups = new List<MaterialGroup>
            {
                 new MaterialGroup {Description="Elektronik",ValidationStateID=1},
                 new MaterialGroup { Description = "Diğer" , ValidationStateID=1}
            };
            materialgroups.ForEach(s => context.MaterialGroups.Add(s));
            context.SaveChanges();

            var materialtypes = new List<MaterialType>
            {
                new MaterialType { Description = "Basit Devre"},
                new MaterialType { Description = "Diğer" }
            };
            materialtypes.ForEach(s => context.MaterialTypes.Add(s));
            context.SaveChanges();

            var locations = new List<Location>
            {
                new Location { Description = "AMBAR", LocationGroupID=3, ValidationStateID=1},
                new Location { Description = "AR-GE LABORATUVAR", LocationGroupID=7, ValidationStateID=1},
                new Location { Description = "AR-GE TASARIM", LocationGroupID=7, ValidationStateID=1},
                new Location { Description = "BAKIM", LocationGroupID=7, ValidationStateID=1},
                new Location { Description = "BİLGİ İŞLEM", LocationGroupID=1, ValidationStateID=1},
                new Location { Description = "BOYAHANE", LocationGroupID=7, ValidationStateID=1},
                new Location { Description = "CALL CENTER", LocationGroupID=6, ValidationStateID=1},
                new Location { Description = "CEO", LocationGroupID=8, ValidationStateID=1},
                new Location { Description = "DEPO", LocationGroupID=6, ValidationStateID=1},
                new Location { Description = "FİNANSAL PLANLAMA", LocationGroupID=4, ValidationStateID=1},
                new Location { Description = "FİNANSMAN", LocationGroupID=4, ValidationStateID=1},
                new Location { Description = "GMY (SATIŞ)", LocationGroupID=5, ValidationStateID=1},
                new Location { Description = "GMY (SSH)", LocationGroupID=6, ValidationStateID=1},
                new Location { Description = "GMY (ÜRETİM)", LocationGroupID=7, ValidationStateID=1},
                new Location { Description = "GÜVENLİK", LocationGroupID=2, ValidationStateID=1},
                new Location { Description = "İÇ DENETÇİ", LocationGroupID=8, ValidationStateID=1},
                new Location { Description = "İNSAN KAYNAKLARI", LocationGroupID=2, ValidationStateID=1},
                new Location { Description = "İTHALAT", LocationGroupID=3, ValidationStateID=1},
                new Location { Description = "KALİTE (SSH)", LocationGroupID=6, ValidationStateID=1},
                new Location { Description = "KALİTE (ÜRETİM)", LocationGroupID=7, ValidationStateID=1},
                new Location { Description = "METAL İŞLERİ", LocationGroupID=7, ValidationStateID=1},
                new Location { Description = "MODİFİKASYON", LocationGroupID=7, ValidationStateID=1},
                new Location { Description = "MUHASEBE (MALİ İŞLER)", LocationGroupID=4, ValidationStateID=1},
                new Location { Description = "MUHASEBE (SSH)", LocationGroupID=6, ValidationStateID=1},
                new Location { Description = "PERFORMANS", LocationGroupID=7, ValidationStateID=1},
                new Location { Description = "PERSONEL", LocationGroupID=2, ValidationStateID=1},
                new Location { Description = "PLANLAMA", LocationGroupID=3, ValidationStateID=1},
                new Location { Description = "REVIR", LocationGroupID=2, ValidationStateID=1},
                new Location { Description = "SANTRAL", LocationGroupID=2, ValidationStateID=1},
                new Location { Description = "SATIN ALMA", LocationGroupID=3, ValidationStateID=1},
                new Location { Description = "SATIŞ DİREKTÖRÜ", LocationGroupID=5, ValidationStateID=1},
                new Location { Description = "SERİGRAFİ", LocationGroupID=7, ValidationStateID=1},
                new Location { Description = "SEVKİYAT", LocationGroupID=3, ValidationStateID=1},
                new Location { Description = "TEKNİK DEPARTMAN", LocationGroupID=7, ValidationStateID=1},
                new Location { Description = "ÜRETİM", LocationGroupID=7, ValidationStateID=1},
                new Location { Description = "ÜRETİM OFİS", LocationGroupID=7, ValidationStateID=1},
                new Location { Description = "YATIRIM", LocationGroupID=7, ValidationStateID=1},
                new Location { Description = "YEMEKHANE", LocationGroupID=2, ValidationStateID=1},
                new Location { Description = "YÖNETİCİ ASİSTANI (İK)", LocationGroupID=2, ValidationStateID=1},
                new Location { Description = "YÖNETİCİ ASİSTANI (YÖNETİM)", LocationGroupID=8, ValidationStateID=1},
                new Location { Description = "YURT DIŞI SATIŞ", LocationGroupID=5, ValidationStateID=1},
                new Location { Description = "YURT İÇİ SATIŞ", LocationGroupID=5, ValidationStateID=1},
                new Location { Description = "YURT İÇİ SERVİS", LocationGroupID=6, ValidationStateID=1},
                new Location { Description = "6 SIGMA", LocationGroupID=9, ValidationStateID=1}
            };
            locations.ForEach(s => context.Locations.Add(s));
            context.SaveChanges();

            var usergroups = new List<UserGroup>
            {
                new UserGroup { Name="Yöneticiler" , Users= new List<User>()},
                new UserGroup {Name = "Sorumlular" , Users= new List<User>() },
                new UserGroup {Name = "Çalışanlar" , Users= new List<User>()}
            };
            usergroups.ForEach(s => context.UserGroups.Add(s));
            context.SaveChanges();

            var roles = new List<Role>
            {
                new Role { Description="administrators" , Users= new List<User>() },
                new Role { Description ="moderators" , Users= new List<User>()},
                new Role { Description = "users" , Users= new List<User>()}
            };

            //başlangıç yetkileri
            roles.ForEach(s => context.Roles.Add(s));
            context.SaveChanges();
                        
            
            UserRepository user_ = new UserRepository();
            var MemUser = user_.CreateUser("scully", "Musa", "Fedakar", "qwerty3", "musa.fedakar@arelektronik.com");
            MemUser = user_.CreateUser("ozcane", "Özcan", "Eryalçın", "123456", "ozcan@klimasan.gg");
            context.SaveChanges();
            
            roles[0].Users.Add(context.Users.Find(1));
            roles[1].Users.Add(context.Users.Find(1));
            roles[1].Users.Add(context.Users.Find(2));
            context.SaveChanges();

            usergroups[0].Users.Add(context.Users.Find(1));

            modules[0].Users.Add(context.Users.Find(1));

            context.SaveChanges();
            var corpaccs = new List<CorporateAccount>
            {
                new CorporateAccount{ Title="ADVANTECH", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=1, ValidationStateID=1 },
                new CorporateAccount{ Title="APPLE", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=1, ValidationStateID=1 },
                new CorporateAccount{ Title="DELL", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=1, ValidationStateID=1 },
                new CorporateAccount{ Title="HP", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=1, ValidationStateID=1 },
                new CorporateAccount{ Title="MACPRO", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=1, ValidationStateID=1 },
                new CorporateAccount{ Title="PHILIPS", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=1, ValidationStateID=1 },
                new CorporateAccount{ Title="POWER", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=1, ValidationStateID=1 },
                new CorporateAccount{ Title="SAMSUNG", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=1, ValidationStateID=1 },
                new CorporateAccount{ Title="SIEMENS", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=1, ValidationStateID=1 },
                new CorporateAccount{ Title="SONY", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=1, ValidationStateID=1 },
                new CorporateAccount{ Title="TOSHIBA", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=1, ValidationStateID=1 },
                new CorporateAccount{ Title="APEKS ELEKTRONİK", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="ASAY İLETİŞİM", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="ASPERA", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="BARKENT MÜHENDİSLİK", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="BİDAR BİLGİSAYAR", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="BORUSAN TELEKOM", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="DBI", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="DİNÇAY KIRTASİYE", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="E-COZUM", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="E-ÇÖZÜM", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="EGE DATA", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="ERİŞİM BİLGİSAYAR", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="ESTUR", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="FOREKS", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="HP", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="INFORMATIK", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="INFOTRON", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="INOVA", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="İZMİR BİLİŞİM", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="LİMA BİLGİSAYAR", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="MARKA KIRTASİYE", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="NETLE", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="NETSIS", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="PC ONARIM", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="PLANET BİLGİSAYAR", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="PROKOD", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="SERES", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="SERVUS", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="SİSBİM", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="TEKMAR", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="TEOREM", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="TETRA MÜHENDİSLİK", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="TTNET", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 },
                new CorporateAccount{ Title="ZAFER BÜRO MAKİNALARI", Address="Adresi..", Phone1="Telefonu..", CorporateTypeID=2, ValidationStateID=1 }
            };

            corpaccs.ForEach(s => context.CorporateAccounts.Add(s));
            context.SaveChanges();

            var invs = new List<Inventory>
            {
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=26,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6481ZTT",LocationID=3,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="TOSHIBA PORTEGE R500 U7700", SerialNumber="68050188H",LocationID=8,CorporateAccountID=11,BrandModel=" PORTEGE R500 U7700", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="--",LocationID=36,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538N3",LocationID=30,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFV2",LocationID=27,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6200", SerialNumber="CZC6294N4R",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6200", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=3,CorporateAccountID=null,BrandModel="", Notes="YÜCEL MEŞELİ SORUMLULUĞUNDA ORTAK KULLANIMDADIR. SEVCANB BİLGİSAYARINDAN ALINAN 256 MB RAM BU BİLGİSAYARA TAKILDI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVP",LocationID=7,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVN",LocationID=2,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=20,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6481ZF7",LocationID=23,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTD",LocationID=30,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=40,CorporateAccountID=null,BrandModel="", Notes="FEYZA CAKA'DAN KALAN PC. YERİNE GELEN ASLI ERDEN' E VERİLDİ.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NX 7010", SerialNumber="CNF33621PQ",LocationID=40,CorporateAccountID=4,BrandModel=" NX 7010", Notes="SUMUN YAPMAK İSTEYEN PERSONLİN KULLANIMI İÇİN VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTJ",LocationID=36,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="3 EKSENLİ PUNCH MAKİNASI İÇİN VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=36,CorporateAccountID=null,BrandModel="", Notes="TEBIS PROGRAMI İÇİN VEİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=9,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=3,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="MANİSA FABRİKA",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTR",LocationID=23,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538J0",LocationID=11,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=11,CorporateAccountID=null,BrandModel="", Notes="FOREKS KULLANIMI İÇİN VERİLMİŞTİR. DONANIM YETERSİZLİĞİNDEN DOLAYI BAŞKA BİR BİLGİSAYAR İLE DEĞİŞTİRİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=20,CorporateAccountID=null,BrandModel="", Notes="KALİTE ÇIKIŞ OFİSİ TEST BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=20,CorporateAccountID=null,BrandModel="", Notes="KALİTE ÇIKIŞ OFİSİ TEST BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="BİSS SİSTEM BİLGİSAYARI. LABORATUVARDA GEZİCİ OLARAK KULLANILIYOR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="BİSS SİSTEM BİLGİSAYARI. LABORATUVARDA GEZİCİ OLARAK KULLANILIYOR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="BİSS SİSTEM BİLGİSAYARI. LABORATUVARDA GEZİCİ OLARAK KULLANILIYOR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=23,CorporateAccountID=null,BrandModel="", Notes="SERHAT SARISAKAL'DAN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=6,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=6,CorporateAccountID=null,BrandModel="", Notes="BOYAHANE AMBARINDA KULLANILIYOR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTW",LocationID=11,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTN",LocationID=33,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTL",LocationID=36,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NX 7010", SerialNumber="--",LocationID=41,CorporateAccountID=4,BrandModel=" NX 7010", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538Q0",LocationID=33,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=23,CorporateAccountID=null,BrandModel="", Notes="GÖNÜL GÖRDÜK'TEN KALAN PC. PC DEĞİŞİKLĞİ YAPILARAK CAVİT KUMGEÇ'E ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFT9",LocationID=30,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="CEM KARTAL'DAN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NX 7010", SerialNumber="CND4440GL1",LocationID=41,CorporateAccountID=4,BrandModel=" NX 7010", Notes="BURÇİN TOKSABAY'DAN KALAN NOTEBOOK. CEMİL UYSALCI'NIN KULLANIMINA VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6120", SerialNumber="CNU540244B",LocationID=41,CorporateAccountID=4,BrandModel=" NC 6120", Notes="ERDİNÇ KURDOĞLU'NDAN ALINAN NOTEBBOK.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=25,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=25,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="SONY VAIO VGN-SZ71MN/B", SerialNumber="28271654 5000435",LocationID=41,CorporateAccountID=10,BrandModel=" VAIO VGN-SZ71MN/B", Notes="KULLANMAKTA OLDUĞU NOTEBOOK'TA DVD-RW ARIZALI OLDUĞUNDAN 09.05.2008 TARİHİNDE AYNI MODEL BİR NOTEBOOK İLE DEĞİŞTİRİLMİŞTİR. 28.05.2008 TARİHİNDE YENİ ALINAN SONY VAIO İLE DEĞİŞTİRİLDİ.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6400", SerialNumber="CZC7052SN9",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6400", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540w", SerialNumber="--",LocationID=5,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540w", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFT7",LocationID=26,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6200", SerialNumber="CZC5460BJ4",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6200", Notes="HALİT ESMERAY'IN KULLANDIĞI WORKSTATION. FATİH BZDOĞAN'DAN KALAN MAKİNA. YERİNE GELEN DUYGU KOÇ'A ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=34,CorporateAccountID=null,BrandModel="", Notes="GÜVENLİK CAMERALARINDAN ALINAN BİLGİSAYAR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFV8",LocationID=1,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVO",LocationID=27,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=29,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6481ZJY",LocationID=34,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=18,CorporateAccountID=null,BrandModel="", Notes="GEÇİCİ PERSONEL İÇİN VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVJ",LocationID=27,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVK",LocationID=33,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538J6",LocationID=42,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=7,CorporateAccountID=null,BrandModel="", Notes="SİNEM ÇELİK'TEN KALAN PC. ALPER URGENÇ'TEN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="TOSHIBA PORTEGE R500 U7700", SerialNumber="Y7085208H",LocationID=12,CorporateAccountID=11,BrandModel=" PORTEGE R500 U7700", Notes="KULLANMAKTA OLDUĞU SONY VAIO NOTEBOOK GERİ ALINMIŞTIR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540w", SerialNumber="--",LocationID=5,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540w", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="--",LocationID=5,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=1,CorporateAccountID=null,BrandModel="", Notes="KAAN GÜNDOĞAN'DAN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="--",LocationID=14,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=36,CorporateAccountID=null,BrandModel="", Notes="GÖREV DEĞİŞİKLİĞİ NEDENİYLE İLHAN KENT'TEN ALINAN PC FATMA ALTUNTAŞ'A VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=1,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. Z800", SerialNumber="CZC04556ZF",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. Z800", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=7,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=7,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="SONY VAIO VPCSB1Z9E", SerialNumber="27539266 5000689",LocationID=16,CorporateAccountID=10,BrandModel=" VAIO VPCSB1Z9E", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU641165P",LocationID=27,CorporateAccountID=4,BrandModel=" NC 6320", Notes="CND4440GL1 SERİ NOLU NX7010 GERİ ALINMIŞTIR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=34,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=34,CorporateAccountID=null,BrandModel="", Notes="PLIS TEST KURULARININ ARIZA TESPİTİNDE KULLANILIYOR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538KY",LocationID=30,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTS",LocationID=18,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=15,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=15,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP VECTRA VL 420 DT", SerialNumber="--",LocationID=15,CorporateAccountID=4,BrandModel=" VECTRA VL 420 DT", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538NF",LocationID=44,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="SONY VAIO VGN-SZ71MN/B", SerialNumber="28271654 5000407",LocationID=41,CorporateAccountID=10,BrandModel=" VAIO VGN-SZ71MN/B", Notes="28.05.2008 TARİHİNDE KULLANMAKTA OLDUĞU NC6320 GERİ ALINARAK SONY VAIO NOTEBOOK VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=34,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=42,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="POWER MAC G4", SerialNumber="CK32009HP96",LocationID=32,CorporateAccountID=7,BrandModel=" MAC G4", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="MACPRO", SerialNumber="CK644046UPZ",LocationID=32,CorporateAccountID=5, Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="MACPRO", SerialNumber="--",LocationID=32,CorporateAccountID=5, Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=32,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="ERHAN URAS'TAN KALAN PC. YERİNE GELEN G.DURMAZ'A ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6200", SerialNumber="CZC5460BJ2",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6200", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=34,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6200", SerialNumber="CZC5460BJI",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6200", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6481ZH6",LocationID=23,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVM",LocationID=18,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=6,CorporateAccountID=null,BrandModel="", Notes="SEMRA SOLMAZE'DEN KALAN PC. YERİNE GELEN H.DAĞTEKE'YE ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6200", SerialNumber="CZC5460BJ3",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6200", Notes="YÜCEL DENGEL'DEN ALINAN MAKİNA.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTZ",LocationID=23,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=34,CorporateAccountID=null,BrandModel="", Notes="BARIŞ YÖRÜK'TEN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=1,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="BUĞRA LÜLECİ'DEN KALAN PC. YERİNE GELEN H.OĞUZ'A ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVD",LocationID=3,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=22,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=21,CorporateAccountID=null,BrandModel="", Notes="Üzerinde LANTEK programı çalışıyor. Üretim makinası. (CNC). MEHMET DEMİREL'DEN KALAN PC. YERİNE GELEN İLHAN MICILI'YA ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="DELL PRECISION T1500", SerialNumber="--",LocationID=36,CorporateAccountID=3,BrandModel=" PRECISION T1500", Notes="SALVAGNINI BİLGİSAYARINA YEDEK OLARAK VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFV1",LocationID=36,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="SERVET YILDIRIM'DAN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 4100", SerialNumber="--",LocationID=21,CorporateAccountID=4,BrandModel=" WORKS. xw 4100", Notes="OFİS BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 4100", SerialNumber="FRB4040B6N",LocationID=21,CorporateAccountID=4,BrandModel=" WORKS. xw 4100", Notes="ÜRETİM HATTI BİLGİSAYARII.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVQ",LocationID=43,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFV9",LocationID=41,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="SIEMENS NOTEBOOK", SerialNumber="--",LocationID=4,CorporateAccountID=9,BrandModel=" NOTEBOOK", Notes="POLİÜRETAN MAKİNALARININ ARIZA TESPİTİNDE KULLANILIYOR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=4,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=4,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFV5",LocationID=1,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6200", SerialNumber="CZC6294N4S",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6200", Notes="ADİL TUNCER'DEN KALAN MAKİNA. YERİNE GELEN H. KOYUNCUOĞLU'NA VERİLMİŞTİR. HAKAN KOYUNCUOĞLUNUN İŞTEN AYRILMASI ÜZERİNE YERİNE GELEN K.O.GÖKGÖL'E ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=26,CorporateAccountID=null,BrandModel="", Notes="MANİSA FABRİKA",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=19,CorporateAccountID=null,BrandModel="", Notes="KALİTE ÇIKIŞ OFİSİ AUDIT BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=19,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=19,CorporateAccountID=null,BrandModel="", Notes="MANİSA FABRİKA",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=19,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=19,CorporateAccountID=null,BrandModel="", Notes="MANİSA FABRİKA",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=20,CorporateAccountID=null,BrandModel="", Notes="GİRİŞ KALİTE OFİSİNDE STAJER KULLANIMINA VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=36,CorporateAccountID=null,BrandModel="", Notes="İLHAN MICILI'DAN ALINAN PC",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=20,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=20,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=20,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="OFİS BİLGİSAYARI",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=25,CorporateAccountID=null,BrandModel="", Notes="MANİSA FABRİKA",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="DELL WORKS. PRECISION 690", SerialNumber="--",LocationID=3,CorporateAccountID=3,BrandModel=" WORKS. PRECISION 690", Notes="KAMER ÇELİK'TEN KALAN MAKİNA. YERİNE GELEN CEM EVRENOS'A ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. Z800", SerialNumber="CZC04556ZM",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. Z800", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=33,CorporateAccountID=null,BrandModel="", Notes="MANİSA FABRİKA",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=33,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=33,CorporateAccountID=null,BrandModel="", Notes="KÜRŞAD ÜNVER'DEN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=33,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538HL",LocationID=36,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="POLIURETAN BİLGİSAYARLARINA YEDEK OLARAK VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="POLIURETAN BİLGİSAYARLARINA YEDEK OLARAK VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="POLIURETAN BİLGİSAYARLARINA YEDEK OLARAK VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=36,CorporateAccountID=null,BrandModel="", Notes="SEDAT KOLAT'TAN ALINAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND115381Q",LocationID=37,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=27,CorporateAccountID=null,BrandModel="", Notes="KULAKLIĞINI İADE ETTİ.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=7,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=17,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=37,CorporateAccountID=null,BrandModel="", Notes="MEHMET URHAN'DAN ALINAN PC. GÖREV DEĞİŞİKLİĞİ NEDENİYLE YİĞİT ÇOKDUYAR'DAN ALINARAK MERTHAN SAYLAN'A ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="DELL WORKS. PRECISION 690", SerialNumber="--",LocationID=3,CorporateAccountID=3,BrandModel=" WORKS. PRECISION 690", Notes="LİDER METAL FRIO'DAN ALINAN WORKSTATION.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=1,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="SONY VAIO VGN-SZ2HP/B", SerialNumber="282443575000074",LocationID=31,CorporateAccountID=10,BrandModel=" VAIO VGN-SZ2HP/B", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 8600", SerialNumber="CZC8231BB5",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 8600", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6481ZDZ",LocationID=43,CorporateAccountID=4,BrandModel=" NC 6320", Notes="AYKUT TOKMAKOĞLU'NDAN KALAN NOTEBOOK.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=43,CorporateAccountID=null,BrandModel="", Notes="E-İMZA İÇİN VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538F5",LocationID=37,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. Z800", SerialNumber="CZC00551XW",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. Z800", Notes="KULLANMAKTA OLDUĞU HP WORKSTATION XW6200 DİĞER DONANIMLARI İLE BİRLİKTE GERİ ALINMIŞTIR. KULLANMAKTA OLDUĞU HP WORKS. XW8600 GERİ ALINMIŞTIR",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=23,CorporateAccountID=null,BrandModel="", Notes="ONUR SARÇIN'DAN KALAN PC. YERİNE GELEN G.AKMAN'A ZİMMETLENMİŞTİR. G.AKMAN'IN İŞTEN AYRILMASI NEDENİYLE YERİNE GELEN MERVE TİRİL'E ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTH",LocationID=43,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=41,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVL",LocationID=43,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6481ZF4",LocationID=26,CorporateAccountID=4,BrandModel=" NC 6320", Notes="MELİH YILDIRIM'DAN KALAN NOTEBOOK. YERİNE GELEN OGUN İSTANBUL'A ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 8600", SerialNumber="CZC8231BB3",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 8600", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVS",LocationID=33,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTM",LocationID=30,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTP",LocationID=15,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="ERDEM DEĞER'DEN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6400", SerialNumber="--",LocationID=5,CorporateAccountID=4,BrandModel=" WORKS. xw 6400", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 8430", SerialNumber="CNU6463FVT",LocationID=5,CorporateAccountID=4,BrandModel=" NC 8430", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU7322DKM",LocationID=27,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6400", SerialNumber="CZC70430MF",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6400", Notes="MAHSUBE GEZER'DEN ALINAN MAKİNA ÖZLEM YILMAZER'İN KULLANIMINA VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVF",LocationID=3,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTY",LocationID=34,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="İŞTEN AYRILAN FERDİ ERDEK'TEN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 4600", SerialNumber="--",LocationID=35,CorporateAccountID=4,BrandModel=" WORKS. xw 4600", Notes="POLİÜRETAN SİSTEM BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 4600", SerialNumber="--",LocationID=35,CorporateAccountID=4,BrandModel=" WORKS. xw 4600", Notes="POLİÜRETAN SİSTEM BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 4400", SerialNumber="--",LocationID=35,CorporateAccountID=4,BrandModel=" WORKS. xw 4400", Notes="POLİÜRETAN SİSTEM BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="METALIX BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=34,CorporateAccountID=null,BrandModel="", Notes="LVD PUNCH MAKİNASI İÇİN VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="SONY VAIO VGN-SZ71MN/B", SerialNumber="28271654 5000353",LocationID=41,CorporateAccountID=10,BrandModel=" VAIO VGN-SZ71MN/B", Notes="28.05.2008 TARİHİNDE KULLANMAKTA OLDUĞU NO NAME PC GERİ ALINARAK  SONY VAIO NOTEBOOK VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVG",LocationID=32,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=28,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=1,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6200", SerialNumber="CZC6262D4L",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6200", Notes="SEMİH KAZMA'DAN KALAN WORKSTATION.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6200", SerialNumber="CZC5460BJ0",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6200", Notes="ÖZGÜR İLKUTLU'DAN KALAN MAKİNA. YÖNETİCİSİ HARUN İŞBİLİR'E ZİMMETLENDİ. KAMER ÇELİK'İN KULLANIMINA VERİLMİŞTİR. GÖREV DEĞİŞİKLİĞİ NEDENİYLE KAMER ÇELİK'TEN ALINARAK H.İŞBİLİR'E ZİMMETLENMİŞTİR. İŞE YENİ BAŞLAYAN SARP CANKUL'A VEİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 8200", SerialNumber="CZC62128XL",LocationID=32,CorporateAccountID=4,BrandModel=" WORKS. xw 8200", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFT8",LocationID=10,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=36,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFV7",LocationID=23,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="MİNE ŞAHİN'DEN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTB",LocationID=7,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTQ",LocationID=43,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538J7",LocationID=19,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVC",LocationID=18,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="BERKAN DERYA'DAN KALAN PC",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 8600", SerialNumber="CZC8335N7C",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 8600", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NX 7010", SerialNumber="CND4490PF1",LocationID=3,CorporateAccountID=4,BrandModel=" NX 7010", Notes="CND447291Z SERİ NOLU NX7010 ARIZASI NEDENİYLE GERİ ALINMIŞTIR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=34,CorporateAccountID=null,BrandModel="", Notes="GÜVENLİK CAMERALARINDAN ALINAN BİLGİSAYAR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=23,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 8600", SerialNumber="CZC8335N7G",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 8600", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=32,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFT6",LocationID=3,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538",LocationID=10,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6481ZF0",LocationID=23,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=3,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NX 7010", SerialNumber="--",LocationID=10,CorporateAccountID=4,BrandModel=" NX 7010", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=9,CorporateAccountID=null,BrandModel="", Notes="ERCAN KOÇ'TAN KALAN PC. İŞTEN AYRILMASI NEDENİYLE UĞUR HIZAL'A ZİMMETLENMİŞTİR. MUSTAFA ÜNLÜ'YE ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=9,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTK",LocationID=1,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="TOSHIBA PORTEGE R500 U7700", SerialNumber="68052505H",LocationID=13,CorporateAccountID=11,BrandModel=" PORTEGE R500 U7700", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVD",LocationID=36,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6481ZDP",LocationID=20,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="2. RAM TAKILAMIYOR. SLOT ARIZALI",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. Z800", SerialNumber="CZC00551XZ",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. Z800", Notes="KULLANMAKTA OLDUĞU HP WORKS. XW6200 GERİ ALINMIŞTIR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="TEST ODASI BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="TEST ODASI BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="TEST ODASI BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="TEST ODASI BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="TEST ODASI BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="TEST ODASI BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="TEST ODASI BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="BÜYÜK TEST ODASI BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="BÜYÜK TEST ODASI BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFV6",LocationID=27,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVH",LocationID=41,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTC",LocationID=34,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTT",LocationID=23,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6481ZNK",LocationID=19,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTV",LocationID=3,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="ESİN ALTIN'DAN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NX 7010", SerialNumber="CND448127M",LocationID=9,CorporateAccountID=4,BrandModel=" NX 7010", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538MP",LocationID=37,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540w", SerialNumber="CND0221L1C",LocationID=5,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540w", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTG",LocationID=7,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVR",LocationID=43,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFV4",LocationID=30,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=38,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTF",LocationID=42,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=3,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6403SNS",LocationID=3,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NX 8220", SerialNumber="CNU6092C4N",LocationID=3,CorporateAccountID=4,BrandModel=" NX 8220", Notes="KULLANMAKTA OLDUĞU HP NC 6320 (CNU63726T0) NOTEBOOK EKRAN ARIZASI NEDENİYLE GERİ ALINDI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 8430", SerialNumber="--",LocationID=5,CorporateAccountID=4,BrandModel=" NC 8430", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. Z800", SerialNumber="--",LocationID=2,CorporateAccountID=4,BrandModel=" WORKS. Z800", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=23,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1}
            };

            invs.ForEach(s => context.Inventories.Add(s));
            context.SaveChanges();

            var persons = new List<Personnel>
            {
                new Personnel { ValidationStateID=1,  FirstName="ABDULLAH",LastName="YÜCEL"},
                new Personnel { ValidationStateID=1,  FirstName="AHMET ",LastName="PEKER"},
                new Personnel { ValidationStateID=1,  FirstName="AHMET ",LastName="ŞENOCAK"},
                new Personnel { ValidationStateID=1,  FirstName="ALİ ",LastName="ÖZDEMİR"},
                new Personnel { ValidationStateID=1,  FirstName="ALİ ",LastName="PAKER"},
                new Personnel { ValidationStateID=1,  FirstName="ARDA ",LastName="EKER"},
                new Personnel { ValidationStateID=1,  FirstName="ASLI ",LastName="ERDEN"},
                new Personnel { ValidationStateID=1,  FirstName="AYFER ",LastName="ALTUNGEYİK"},
                new Personnel { ValidationStateID=1,  FirstName="AYŞEGÜL ",LastName="EYLEMER"},
                new Personnel { ValidationStateID=1,  FirstName="AYŞEGÜL ",LastName="İVİT"},
                new Personnel { ValidationStateID=1,  FirstName="AYŞEGÜL ",LastName="KALE"},
                new Personnel { ValidationStateID=1,  FirstName="AYŞEGÜL ",LastName="TANRIÖVEN"},
                new Personnel { ValidationStateID=1,  FirstName="AYTAÇ ",LastName="SAĞIR"},
                new Personnel { ValidationStateID=1,  FirstName="AZİME ",LastName="ÖLÇER"},
                new Personnel { ValidationStateID=1,  FirstName="BANU ",LastName="AKAN"},
                new Personnel { ValidationStateID=1,  FirstName="BARIŞ ",LastName="UÇAR"},
                new Personnel { ValidationStateID=1,  FirstName="BİROL ",LastName="UĞUR"},
                new Personnel { ValidationStateID=1,  FirstName="BURAK ",LastName="ÖZGÜR"},
                new Personnel { ValidationStateID=1,  FirstName="BURCU ",LastName="ÖNCÜER"},
                new Personnel { ValidationStateID=1,  FirstName="BURÇİN ",LastName="ASLANKİRAY"},
                new Personnel { ValidationStateID=1,  FirstName="CAN ",LastName="ÖZES"},
                new Personnel { ValidationStateID=1,  FirstName="BERAT ",LastName="BAYHAN"},
                new Personnel { ValidationStateID=1,  FirstName="CEMİL ",LastName="UYSALCI"},
                new Personnel { ValidationStateID=1,  FirstName="CENK ",LastName="ESKİCİ"},
                new Personnel { ValidationStateID=1,  FirstName="ÇAĞDAŞ ",LastName="KESERCİOĞLU"},
                new Personnel { ValidationStateID=1,  FirstName="ÇAĞLAR ",LastName="KAYALAR"},
                new Personnel { ValidationStateID=1,  FirstName="DEVRİM ",LastName="BÜYÜKÖNDER"},
                new Personnel { ValidationStateID=1,  FirstName="DİLER ",LastName="KOŞAR KIRMIZITOPRAK"},
                new Personnel { ValidationStateID=1,  FirstName="DUYGU ",LastName="KOÇ"},
                new Personnel { ValidationStateID=1,  FirstName="EBRU ",LastName="BARAN"},
                new Personnel { ValidationStateID=1,  FirstName="EBRU ",LastName="GÜNEŞ"},
                new Personnel { ValidationStateID=1,  FirstName="EKREM ",LastName="ŞEN"},
                new Personnel { ValidationStateID=1,  FirstName="EMEL ",LastName="OLGAÇ"},
                new Personnel { ValidationStateID=1,  FirstName="EMİN ",LastName="GEÇER"},
                new Personnel { ValidationStateID=1,  FirstName="EMRE ",LastName="ÖZMEN"},
                new Personnel { ValidationStateID=1,  FirstName="ERDİNÇ ",LastName="DEYER"},
                new Personnel { ValidationStateID=1,  FirstName="ERDİNÇ ",LastName="GÜLER"},
                new Personnel { ValidationStateID=1,  FirstName="ERDİNÇ ",LastName="KURDOĞLU"},
                new Personnel { ValidationStateID=1,  FirstName="ERİM ",LastName="ŞENOCAK"},
                new Personnel { ValidationStateID=1,  FirstName="ERSİN ",LastName="ÇAĞIRGAN"},
                new Personnel { ValidationStateID=1,  FirstName="ESENGÜL",LastName="ERTEM (MLZKABUL)"},
                new Personnel { ValidationStateID=1,  FirstName="ESER ",LastName="BAKİOĞLU"},
                new Personnel { ValidationStateID=1,  FirstName="FATMA ",LastName="ALTUNTAŞ"},
                new Personnel { ValidationStateID=1,  FirstName="FETHİ",LastName="ÖZER  (MLZAMBAR1)"},
                new Personnel { ValidationStateID=1,  FirstName="FİLİZ",LastName="AKIIN"},
                new Personnel { ValidationStateID=1,  FirstName="GAMZE ",LastName="ÖZBÜLBÜL"},
                new Personnel { ValidationStateID=1,  FirstName="GAMZE ",LastName="POLAT"},
                new Personnel { ValidationStateID=1,  FirstName="GENCO ",LastName="KOYUNCUOĞLU"},
                new Personnel { ValidationStateID=1,  FirstName="GÖKHAN ",LastName="MANAV"},
                new Personnel { ValidationStateID=1,  FirstName="GÖKSEL ",LastName="EMRE ÖZMEN"},
                new Personnel { ValidationStateID=1,  FirstName="GÖZE ",LastName="ARI"},
                new Personnel { ValidationStateID=1,  FirstName="GÜLCAN ",LastName="UÇAR"},
                new Personnel { ValidationStateID=1,  FirstName="GÜLÇİN ",LastName="SINAR"},
                new Personnel { ValidationStateID=1,  FirstName="GÜLER ",LastName="ÖZTÜRK"},
                new Personnel { ValidationStateID=1,  FirstName="GÜLHAN ",LastName="BELO"},
                new Personnel { ValidationStateID=1,  FirstName="GÜRAY ",LastName="GÜRŞAHİN"},
                new Personnel { ValidationStateID=1,  FirstName="GÜRCAN ",LastName="DURMAZ"},
                new Personnel { ValidationStateID=1,  FirstName="GÜRKAN ",LastName="VİRDİL"},
                new Personnel { ValidationStateID=1,  FirstName="HAKAN ",LastName="DEĞER"},
                new Personnel { ValidationStateID=1,  FirstName="HAKAN ",LastName="KILIÇ"},
                new Personnel { ValidationStateID=1,  FirstName="HAKAN ",LastName="SEYRAN"},
                new Personnel { ValidationStateID=1,  FirstName="HANDE ",LastName="ARSLAN"},
                new Personnel { ValidationStateID=1,  FirstName="HARUN ",LastName="DAĞTEKE"},
                new Personnel { ValidationStateID=1,  FirstName="HARUN ",LastName="ÖZGENOĞLU"},
                new Personnel { ValidationStateID=1,  FirstName="HATİCE ",LastName="GÜDEN"},
                new Personnel { ValidationStateID=1,  FirstName="HAYRİYE ",LastName="BARIŞ"},
                new Personnel { ValidationStateID=1,  FirstName="HURİYE ",LastName="OĞUZ"},
                new Personnel { ValidationStateID=1,  FirstName="HÜLYA ",LastName="DELEN"},
                new Personnel { ValidationStateID=1,  FirstName="İLHAN ",LastName="KENT"},
                new Personnel { ValidationStateID=1,  FirstName="İLHAN ",LastName="MICILI"},
                new Personnel { ValidationStateID=1,  FirstName="İLHAN ",LastName="MICILI (SALVAGNINI)"},
                new Personnel { ValidationStateID=1,  FirstName="İPEK ",LastName="DİKEROL"},
                new Personnel { ValidationStateID=1,  FirstName="İPEK ",LastName="KAYAN"},
                new Personnel { ValidationStateID=1,  FirstName="İSMAİL ",LastName="ALTINEL"},
                new Personnel { ValidationStateID=1,  FirstName="İSMAİL ",LastName="DURHAN"},
                new Personnel { ValidationStateID=1,  FirstName="KADİR ",LastName="ONUR GOKGOL"},
                new Personnel { ValidationStateID=1,  FirstName="KEREM ",LastName="BİLGİÇ"},
                new Personnel { ValidationStateID=1,  FirstName="KORAY ",LastName="EROL"},
                new Personnel { ValidationStateID=1,  FirstName="M. CEM",LastName="EVRENOS"},
                new Personnel { ValidationStateID=1,  FirstName="MAHSUBE ",LastName="GEZER"},
                new Personnel { ValidationStateID=1,  FirstName="MEHMET ",LastName="ALAY"},
                new Personnel { ValidationStateID=1,  FirstName="MEHMET ",LastName="ALİ AYDEMİR"},
                new Personnel { ValidationStateID=1,  FirstName="MEHMET ",LastName="SEZER"},
                new Personnel { ValidationStateID=1,  FirstName="MEHMET ",LastName="URHAN"},
                new Personnel { ValidationStateID=1,  FirstName="MEHTAP ",LastName="SAYHAN ÖZDEMİR"},
                new Personnel { ValidationStateID=1,  FirstName="MELİS ",LastName="COŞGUNDAĞ"},
                new Personnel { ValidationStateID=1,  FirstName="MERT ",LastName="ALKANAT"},
                new Personnel { ValidationStateID=1,  FirstName="MERTHAN ",LastName="SAYLAN"},
                new Personnel { ValidationStateID=1,  FirstName="METİN ",LastName="DURAN"},
                new Personnel { ValidationStateID=1,  FirstName="MURAT ",LastName="ERTEKİN"},
                new Personnel { ValidationStateID=1,  FirstName="MURAT ",LastName="YARAÇ"},
                new Personnel { ValidationStateID=1,  FirstName="MUSTAFA ",LastName="AREL"},
                new Personnel { ValidationStateID=1,  FirstName="MUSTAFA ",LastName="MİVCAN"},
                new Personnel { ValidationStateID=1,  FirstName="MUSTAFA ",LastName="REŞİT ÖZBALCI"},
                new Personnel { ValidationStateID=1,  FirstName="MÜJDE ",LastName="TANIRLI"},
                new Personnel { ValidationStateID=1,  FirstName="NECLA ",LastName="ÖNCÜ"},
                new Personnel { ValidationStateID=1,  FirstName="NESLİHAN ",LastName="ANIL"},
                new Personnel { ValidationStateID=1,  FirstName="NİL ",LastName="UMUTLU"},
                new Personnel { ValidationStateID=1,  FirstName="OGÜN ",LastName="İSTANBUL"},
                new Personnel { ValidationStateID=1,  FirstName="OGÜN ",LastName="KUNT"},
                new Personnel { ValidationStateID=1,  FirstName="ORHAN ",LastName="OYMAKOĞLU"},
                new Personnel { ValidationStateID=1,  FirstName="ORHAN ",LastName="YILMAZ"},
                new Personnel { ValidationStateID=1,  FirstName="OSMAN ",LastName="ÖZERDEN"},
                new Personnel { ValidationStateID=1,  FirstName="ÖYKÜ ",LastName="YILMAZ"},
                new Personnel { ValidationStateID=1,  FirstName="ÖZCAN ",LastName="ERYALÇIN"},
                new Personnel { ValidationStateID=1,  FirstName="ÖZGÜR ",LastName="AYHAN"},
                new Personnel { ValidationStateID=1,  FirstName="ÖZLEM ",LastName="YILMAZER"},
                new Personnel { ValidationStateID=1,  FirstName="PINAR ",LastName="AKYÜREK"},
                new Personnel { ValidationStateID=1,  FirstName="PINAR ",LastName="KARADAĞ"},
                new Personnel { ValidationStateID=1,  FirstName="PINAR ",LastName="SOYSAL"},
                new Personnel { ValidationStateID=1,  FirstName="PUNCH ",LastName="(METİN BAŞAR)"},
                new Personnel { ValidationStateID=1,  FirstName="RABİA ",LastName="RAHİMBAYEVA"},
                new Personnel { ValidationStateID=1,  FirstName="RAMAZAN ",LastName="ULUĞ"},
                new Personnel { ValidationStateID=1,  FirstName="SALİH ",LastName="AĞAÇHAN"},
                new Personnel { ValidationStateID=1,  FirstName="SALİH ",LastName="YİNESOR"},
                new Personnel { ValidationStateID=1,  FirstName="SARP ",LastName="CANKUL"},
                new Personnel { ValidationStateID=1,  FirstName="SEÇKİN ",LastName="GÖREN"},
                new Personnel { ValidationStateID=1,  FirstName="SEDAT ",LastName="KOLAT"},
                new Personnel { ValidationStateID=1,  FirstName="SEDEN ",LastName="KESEBİR"},
                new Personnel { ValidationStateID=1,  FirstName="SEGÂH ",LastName="ÇAVUŞLAR CALLCENTER1"},
                new Personnel { ValidationStateID=1,  FirstName="SELÇUK ",LastName="DEMİR"},
                new Personnel { ValidationStateID=1,  FirstName="SELÇUK ",LastName="KARADOĞAN"},
                new Personnel { ValidationStateID=1,  FirstName="SELDA ",LastName="ARAPOĞLU"},
                new Personnel { ValidationStateID=1,  FirstName="SELMA ",LastName="TANRIVERİO"},
                new Personnel { ValidationStateID=1,  FirstName="SEMİH ",LastName="KAZMA"},
                new Personnel { ValidationStateID=1,  FirstName="SENCER ",LastName="RIZA BAŞARIR"},
                new Personnel { ValidationStateID=1,  FirstName="SERDAR ",LastName="GÜLŞAN"},
                new Personnel { ValidationStateID=1,  FirstName="SERDAR ",LastName="ÖZKAN"},
                new Personnel { ValidationStateID=1,  FirstName="SERHAN ",LastName="BELCİLER"},
                new Personnel { ValidationStateID=1,  FirstName="SERKAN ",LastName="SEYRAN"},
                new Personnel { ValidationStateID=1,  FirstName="SERKAN ",LastName="UYANIK"},
                new Personnel { ValidationStateID=1,  FirstName="SERPİL ",LastName="ATLI"},
                new Personnel { ValidationStateID=1,  FirstName="SEZGİN ",LastName="KONUR"},
                new Personnel { ValidationStateID=1,  FirstName="SİDAL ",LastName="TUNÇER"},
                new Personnel { ValidationStateID=1,  FirstName="SUAT ",LastName="ATEŞ"},
                new Personnel { ValidationStateID=1,  FirstName="SUBHİ ",LastName="TOPLU"},
                new Personnel { ValidationStateID=1,  FirstName="ŞAFAK ",LastName="DAĞLAN"},
                new Personnel { ValidationStateID=1,  FirstName="ŞENER ",LastName="TABAK"},
                new Personnel { ValidationStateID=1,  FirstName="TANSEL ",LastName="POLAT"},
                new Personnel { ValidationStateID=1,  FirstName="TİMUÇİN ",LastName="HIRÇIN"},
                new Personnel { ValidationStateID=1,  FirstName="TUNCAY ",LastName="ÖZKAN"},
                new Personnel { ValidationStateID=1,  FirstName="TÜLAY ",LastName="SAĞLAM"},
                new Personnel { ValidationStateID=1,  FirstName="TÜRKER ",LastName="DAŞTI"},
                new Personnel { ValidationStateID=1,  FirstName="UĞUR ",LastName="ERBAŞOL"},
                new Personnel { ValidationStateID=1,  FirstName="UĞUR ",LastName="HIZAL"},
                new Personnel { ValidationStateID=1,  FirstName="UTKU ",LastName="ÖZEL"},
                new Personnel { ValidationStateID=1,  FirstName="VİLDAN ",LastName="GÖRGEN"},
                new Personnel { ValidationStateID=1,  FirstName="VOLKAN ",LastName="KORKMAZ"},
                new Personnel { ValidationStateID=1,  FirstName="YALINÇ ",LastName="ALTAY"},
                new Personnel { ValidationStateID=1,  FirstName="YAŞAR ",LastName="ZİNİ"},
                new Personnel { ValidationStateID=1,  FirstName="YEŞİM ",LastName="GÜRLEYEN"},
                new Personnel { ValidationStateID=1,  FirstName="YİĞİT ",LastName="DİNMEZ"},
                new Personnel { ValidationStateID=1,  FirstName="YÜCEL ",LastName="DENGEL"},
                new Personnel { ValidationStateID=1,  FirstName="YÜCEL ",LastName="MEŞELİ"},
                new Personnel { ValidationStateID=1,  FirstName="MUSA ",LastName="FEDAKAR"},
                new Personnel { ValidationStateID=1,  FirstName="ONUR ",LastName="DANIŞKAN"}
            };

            persons.ForEach(s => context.Personnels.Add(s));
            context.SaveChanges();

            var SurvNodes = new List<SurveyNode>
            {
                new SurveyNode { Description="Talebiniz Tamamlandı mı?" },
                new SurveyNode { Description ="Bu sorunu daha önce yaşamış mıydınız?" },
                new SurveyNode { Description="Memnuniyetiniz" }
            };
            SurvNodes.ForEach(s => context.SurveyNodes.Add(s));
            context.SaveChanges();

            var survrectypes = new List<SurveyRecordType>
            {
                new SurveyRecordType { Description="Puanlama" },
                new SurveyRecordType {Description = "Bool" }
            };
            survrectypes.ForEach(s => context.SurveyRecordTypes.Add(s));
            context.SaveChanges();

            var survrecs = new List<SurveyRecord>
            {
                new SurveyRecord { SurveyRecordTypeID = 2, SurveyNodeID= 1, OrderNum=0},
                new SurveyRecord { SurveyRecordTypeID = 2, SurveyNodeID= 2, OrderNum=1 },
                new SurveyRecord { SurveyRecordTypeID = 1, SurveyNodeID= 3, OrderNum=0 }
            };
            survrecs.ForEach(s => context.SurveyRecords.Add(s));
            context.SaveChanges();

            var survtemps = new List<SurveyTemplate>
            {
                new SurveyTemplate { Description="Donanım Arızası Anketi", PreDefined=true, RequestTypeID=5, SurveyRecords= new List<SurveyRecord>()},
                new SurveyTemplate { Description="Yazılım Arızası Anketi", PreDefined=true, RequestTypeID=1,SurveyRecords= new List<SurveyRecord>()}
            };
            survtemps.ForEach(s => context.SurveyTemplates.Add(s));
            context.SaveChanges();

            survtemps[0].SurveyRecords.Add(context.SurveyRecords.Find(1));
            survtemps[0].SurveyRecords.Add(context.SurveyRecords.Find(3));
            survtemps[1].SurveyRecords.Add(context.SurveyRecords.Find(1));
            survtemps[1].SurveyRecords.Add(context.SurveyRecords.Find(2));
            survtemps[1].SurveyRecords.Add(context.SurveyRecords.Find(3));

            context.SaveChanges();

            var paramsets = new List<ParameterSetting>
            {
                new ParameterSetting { Description="Mail Hesap Adı", ParameterValue="musa.fedakar@arelektronik.com"},
                new ParameterSetting { Description="Mail Hesap Şifre", ParameterValue="M123456."},
                new ParameterSetting { Description="Mail Sunucusu", ParameterValue="mail.arelektronik.com"},
                new ParameterSetting { Description="Mail Sunucusu Port", ParameterValue="587"},
                new ParameterSetting { Description="İş Tamamlandı Durum ID", ParameterValue="5"},
                new ParameterSetting { Description="İş Olumsuz Kapatıldı Durum ID", ParameterValue="6"},
                new ParameterSetting { Description="Anket Sistemi Aktif", ParameterValue="0"},
                new ParameterSetting { Description="Varsayılan Taslak Anket ID", ParameterValue="14"},
                new ParameterSetting { Description="EBA Kullanıcı ID", ParameterValue="7"},
                new ParameterSetting { Description="Ret Edilmiş Talep Anket Sorusu ID", ParameterValue="6"},
                new ParameterSetting { Description="Banner üst yazı", ParameterValue="AR Elektronik - 2013"},
                new ParameterSetting { Description="Banner resim dosyasi", ParameterValue="klimasan_banner.png"},
                new ParameterSetting { Description="Hakkinda Kayan Yazi", ParameterValue="ArElektronik - Intrawebs"},
                new ParameterSetting { Description="Yazılım Adı", ParameterValue="ArElektronik - Intrawebs"},
                new ParameterSetting { Description="Varsayilan Gonderen Mail Adresi", ParameterValue="musa.fedakar@arelektronik.com"},
                new ParameterSetting { Description="Çoklu Atölye Aktif", ParameterValue="1"},
                new ParameterSetting { Description="Proje Ekibini Email listesine Al", ParameterValue="1"}
            };

            paramsets.ForEach(s => context.ParameterSettings.Add(s));
            context.SaveChanges();

        }
    }
}