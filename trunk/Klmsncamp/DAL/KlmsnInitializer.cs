using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Klmsncamp.Models;

namespace Klmsncamp.DAL
{
    public class KlmsnInitializer :DropCreateDatabaseIfModelChanges<KlmsnContext>
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
                new RequestState { Description = "SERVİS BEKLİYOR" },
                new RequestState { Description = "FİRMA BEKLİYOR" },
                new RequestState { Description = "PARÇA BEKLİYOR" },
                new RequestState { Description = "PLANLANDI" }
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
            
            var workshops = new List<Workshop>
            {
                new Workshop { Description = "BİLGİ İŞLEM", ValidationStateID=1  }
                
            };
            workshops.ForEach(s => context.Workshops.Add(s));
            context.SaveChanges();

            var invowns = new List<InventoryOwnership>
            {
                new InventoryOwnership { Description="KLİMASAN" },
                new InventoryOwnership { Description = "DIŞ FİRMA"},
                new InventoryOwnership { Description = "DİĞER"}
            };
            invowns.ForEach(s=> context.InventoryOwnerships.Add(s));
            context.SaveChanges();

            var corptypes = new List<CorporateType>
            {
                new CorporateType { Description = "FİRMA" },
                new CorporateType { Description = "YETKİLİ SERVİS"},
                new CorporateType { Description = "ŞAHIS" },
                new CorporateType { Description = "İÇ KULLANICI"}
            };
            corptypes.ForEach(s=> context.CorporateTypes.Add(s));
            context.SaveChanges();

            var locations = new List<Location>
            {
                new Location { Description = "AMBAR", ValidationStateID=1},
                new Location { Description = "AR-GE LABORATUVAR", ValidationStateID=1},
                new Location { Description = "AR-GE TASARIM", ValidationStateID=1},
                new Location { Description = "BAKIM", ValidationStateID=1},
                new Location { Description = "BİLGİ İŞLEM", ValidationStateID=1},
                new Location { Description = "BOYAHANE", ValidationStateID=1},
                new Location { Description = "CALL CENTER", ValidationStateID=1},
                new Location { Description = "CEO", ValidationStateID=1},
                new Location { Description = "DEPO", ValidationStateID=1},
                new Location { Description = "FİNANSAL PLANLAMA", ValidationStateID=1},
                new Location { Description = "FİNANSMAN", ValidationStateID=1},
                new Location { Description = "GMY", ValidationStateID=1},
                new Location { Description = "GÜVENLİK", ValidationStateID=1},
                new Location { Description = "İÇ DENETÇİ", ValidationStateID=1},
                new Location { Description = "İNSAN KAYNAKLARI", ValidationStateID=1},
                new Location { Description = "İTHALAT", ValidationStateID=1},
                new Location { Description = "KALİTE", ValidationStateID=1},
                new Location { Description = "LOJİSTİK", ValidationStateID=1},
                new Location { Description = "MALİ İŞLER", ValidationStateID=1},
                new Location { Description = "METAL İŞLERİ", ValidationStateID=1},
                new Location { Description = "MODİFİKASYON", ValidationStateID=1},
                new Location { Description = "MUHASEBE", ValidationStateID=1},
                new Location { Description = "PERFORMANS", ValidationStateID=1},
                new Location { Description = "PERSONEL", ValidationStateID=1},
                new Location { Description = "PLANLAMA", ValidationStateID=1},
                new Location { Description = "REVIR", ValidationStateID=1},
                new Location { Description = "SANTRAL", ValidationStateID=1},
                new Location { Description = "SATIN ALMA", ValidationStateID=1},
                new Location { Description = "SATIŞ", ValidationStateID=1},
                new Location { Description = "SATIŞ DİREKTÖRÜ", ValidationStateID=1},
                new Location { Description = "SATIŞ SONRASI HİZMETLER", ValidationStateID=1},
                new Location { Description = "SERİGRAFİ", ValidationStateID=1},
                new Location { Description = "SEVKİYAT", ValidationStateID=1},
                new Location { Description = "TEKNİK DEPARTMAN", ValidationStateID=1},
                new Location { Description = "ÜRETİM", ValidationStateID=1},
                new Location { Description = "ÜRETİM OFİS", ValidationStateID=1},
                new Location { Description = "ÜRETİM VE TEKNİK İŞLER", ValidationStateID=1},
                new Location { Description = "YATIRIM", ValidationStateID=1},
                new Location { Description = "YEMEKHANE", ValidationStateID=1},
                new Location { Description = "YÖNETİCİ ASİSTANI", ValidationStateID=1},
                new Location { Description = "YÖNETİM", ValidationStateID=1},
                new Location { Description = "YURT DIŞI SATIŞ", ValidationStateID=1},
                new Location { Description = "YURT İÇİ SATIŞ", ValidationStateID=1},
                new Location { Description = "YURT İÇİ SERVİS", ValidationStateID=1},
                new Location { Description = "6 SIGMA", ValidationStateID=1}

            };
            locations.ForEach(s => context.Locations.Add(s));
            context.SaveChanges();

            UserRepository user_ = new UserRepository();
            var MemUser = user_.CreateUser("scully", "Musa","Fedakar","qwerty3", "musaf@klimasan.com.tr");
            


            var usergroups = new List<UserGroup>
            {
                new UserGroup { Name="administrators" },
                new UserGroup {Name = "Power Users" },
                new UserGroup {Name = "Users"}
            };
            usergroups.ForEach(s => context.UserGroups.Add(s));
            context.SaveChanges();

            var roles = new List<Role>
            {
                new Role { Description="administrators" , Users= new List<User>() },
                new Role { Description ="moderators" , Users= new List<User>()},
                new Role { Description = "users" , Users= new List<User>()}
            };

            roles.ForEach(s => context.Roles.Add(s));
            context.SaveChanges();

            roles[0].Users.Add(context.Users.Find(1));
            roles[1].Users.Add(context.Users.Find(1));

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
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=24,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6481ZTT",LocationID=3,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="TOSHIBA PORTEGE R500 U7700", SerialNumber="68050188H",LocationID=8,CorporateAccountID=11,BrandModel=" PORTEGE R500 U7700", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="--",LocationID=36,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538N3",LocationID=28,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFV2",LocationID=25,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6200", SerialNumber="CZC6294N4R",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6200", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=3,CorporateAccountID=null,BrandModel="", Notes="YÜCEL MEŞELİ SORUMLULUĞUNDA ORTAK KULLANIMDADIR. SEVCANB BİLGİSAYARINDAN ALINAN 256 MB RAM BU BİLGİSAYARA TAKILDI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVP",LocationID=7,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVN",LocationID=2,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=17,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6481ZF7",LocationID=22,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTD",LocationID=28,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=40,CorporateAccountID=null,BrandModel="", Notes="FEYZA CAKA'DAN KALAN PC. YERİNE GELEN ASLI ERDEN' E VERİLDİ.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NX 7010", SerialNumber="CNF33621PQ",LocationID=40,CorporateAccountID=4,BrandModel=" NX 7010", Notes="SUMUN YAPMAK İSTEYEN PERSONLİN KULLANIMI İÇİN VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTJ",LocationID=36,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="3 EKSENLİ PUNCH MAKİNASI İÇİN VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=36,CorporateAccountID=null,BrandModel="", Notes="TEBIS PROGRAMI İÇİN VEİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=9,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=3,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="MANİSA FABRİKA",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTR",LocationID=22,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538J0",LocationID=11,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=11,CorporateAccountID=null,BrandModel="", Notes="FOREKS KULLANIMI İÇİN VERİLMİŞTİR. DONANIM YETERSİZLİĞİNDEN DOLAYI BAŞKA BİR BİLGİSAYAR İLE DEĞİŞTİRİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=17,CorporateAccountID=null,BrandModel="", Notes="KALİTE ÇIKIŞ OFİSİ TEST BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=17,CorporateAccountID=null,BrandModel="", Notes="KALİTE ÇIKIŞ OFİSİ TEST BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="BİSS SİSTEM BİLGİSAYARI. LABORATUVARDA GEZİCİ OLARAK KULLANILIYOR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="BİSS SİSTEM BİLGİSAYARI. LABORATUVARDA GEZİCİ OLARAK KULLANILIYOR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="BİSS SİSTEM BİLGİSAYARI. LABORATUVARDA GEZİCİ OLARAK KULLANILIYOR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=22,CorporateAccountID=null,BrandModel="", Notes="SERHAT SARISAKAL'DAN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=6,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=6,CorporateAccountID=null,BrandModel="", Notes="BOYAHANE AMBARINDA KULLANILIYOR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTW",LocationID=11,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTN",LocationID=33,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTL",LocationID=36,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NX 7010", SerialNumber="--",LocationID=42,CorporateAccountID=4,BrandModel=" NX 7010", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538Q0",LocationID=33,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=22,CorporateAccountID=null,BrandModel="", Notes="GÖNÜL GÖRDÜK'TEN KALAN PC. PC DEĞİŞİKLĞİ YAPILARAK CAVİT KUMGEÇ'E ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFT9",LocationID=28,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="CEM KARTAL'DAN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NX 7010", SerialNumber="CND4440GL1",LocationID=42,CorporateAccountID=4,BrandModel=" NX 7010", Notes="BURÇİN TOKSABAY'DAN KALAN NOTEBOOK. CEMİL UYSALCI'NIN KULLANIMINA VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6120", SerialNumber="CNU540244B",LocationID=42,CorporateAccountID=4,BrandModel=" NC 6120", Notes="ERDİNÇ KURDOĞLU'NDAN ALINAN NOTEBBOK.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=23,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=23,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="SONY VAIO VGN-SZ71MN/B", SerialNumber="28271654 5000435",LocationID=42,CorporateAccountID=10,BrandModel=" VAIO VGN-SZ71MN/B", Notes="KULLANMAKTA OLDUĞU NOTEBOOK'TA DVD-RW ARIZALI OLDUĞUNDAN 09.05.2008 TARİHİNDE AYNI MODEL BİR NOTEBOOK İLE DEĞİŞTİRİLMİŞTİR. 28.05.2008 TARİHİNDE YENİ ALINAN SONY VAIO İLE DEĞİŞTİRİLDİ.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6400", SerialNumber="CZC7052SN9",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6400", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540w", SerialNumber="--",LocationID=5,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540w", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFT7",LocationID=24,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6200", SerialNumber="CZC5460BJ4",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6200", Notes="HALİT ESMERAY'IN KULLANDIĞI WORKSTATION. FATİH BZDOĞAN'DAN KALAN MAKİNA. YERİNE GELEN DUYGU KOÇ'A ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=34,CorporateAccountID=null,BrandModel="", Notes="GÜVENLİK CAMERALARINDAN ALINAN BİLGİSAYAR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFV8",LocationID=1,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVO",LocationID=25,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=27,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6481ZJY",LocationID=34,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=16,CorporateAccountID=null,BrandModel="", Notes="GEÇİCİ PERSONEL İÇİN VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVJ",LocationID=25,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVK",LocationID=33,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538J6",LocationID=43,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=7,CorporateAccountID=null,BrandModel="", Notes="SİNEM ÇELİK'TEN KALAN PC. ALPER URGENÇ'TEN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="TOSHIBA PORTEGE R500 U7700", SerialNumber="Y7085208H",LocationID=12,CorporateAccountID=11,BrandModel=" PORTEGE R500 U7700", Notes="KULLANMAKTA OLDUĞU SONY VAIO NOTEBOOK GERİ ALINMIŞTIR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540w", SerialNumber="--",LocationID=5,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540w", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="--",LocationID=5,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=1,CorporateAccountID=null,BrandModel="", Notes="KAAN GÜNDOĞAN'DAN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="--",LocationID=12,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=36,CorporateAccountID=null,BrandModel="", Notes="GÖREV DEĞİŞİKLİĞİ NEDENİYLE İLHAN KENT'TEN ALINAN PC FATMA ALTUNTAŞ'A VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=1,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. Z800", SerialNumber="CZC04556ZF",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. Z800", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=7,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=7,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="SONY VAIO VPCSB1Z9E", SerialNumber="27539266 5000689",LocationID=14,CorporateAccountID=10,BrandModel=" VAIO VPCSB1Z9E", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU641165P",LocationID=25,CorporateAccountID=4,BrandModel=" NC 6320", Notes="CND4440GL1 SERİ NOLU NX7010 GERİ ALINMIŞTIR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=34,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=34,CorporateAccountID=null,BrandModel="", Notes="PLIS TEST KURULARININ ARIZA TESPİTİNDE KULLANILIYOR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538KY",LocationID=28,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTS",LocationID=16,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=13,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=13,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP VECTRA VL 420 DT", SerialNumber="--",LocationID=13,CorporateAccountID=4,BrandModel=" VECTRA VL 420 DT", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538NF",LocationID=45,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="SONY VAIO VGN-SZ71MN/B", SerialNumber="28271654 5000407",LocationID=42,CorporateAccountID=10,BrandModel=" VAIO VGN-SZ71MN/B", Notes="28.05.2008 TARİHİNDE KULLANMAKTA OLDUĞU NC6320 GERİ ALINARAK SONY VAIO NOTEBOOK VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=34,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=43,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="POWER MAC G4", SerialNumber="CK32009HP96",LocationID=32,CorporateAccountID=7,BrandModel=" MAC G4", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="MACPRO", SerialNumber="CK644046UPZ",LocationID=32,CorporateAccountID=5, Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="MACPRO", SerialNumber="--",LocationID=32,CorporateAccountID=5, Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=32,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="ERHAN URAS'TAN KALAN PC. YERİNE GELEN G.DURMAZ'A ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6200", SerialNumber="CZC5460BJ2",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6200", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=34,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6200", SerialNumber="CZC5460BJI",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6200", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6481ZH6",LocationID=22,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVM",LocationID=16,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=6,CorporateAccountID=null,BrandModel="", Notes="SEMRA SOLMAZE'DEN KALAN PC. YERİNE GELEN H.DAĞTEKE'YE ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6200", SerialNumber="CZC5460BJ3",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6200", Notes="YÜCEL DENGEL'DEN ALINAN MAKİNA.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTZ",LocationID=22,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=34,CorporateAccountID=null,BrandModel="", Notes="BARIŞ YÖRÜK'TEN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=1,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="BUĞRA LÜLECİ'DEN KALAN PC. YERİNE GELEN H.OĞUZ'A ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVD",LocationID=3,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=21,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=20,CorporateAccountID=null,BrandModel="", Notes="Üzerinde LANTEK programı çalışıyor. Üretim makinası. (CNC). MEHMET DEMİREL'DEN KALAN PC. YERİNE GELEN İLHAN MICILI'YA ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="DELL PRECISION T1500", SerialNumber="--",LocationID=36,CorporateAccountID=3,BrandModel=" PRECISION T1500", Notes="SALVAGNINI BİLGİSAYARINA YEDEK OLARAK VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFV1",LocationID=36,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="SERVET YILDIRIM'DAN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 4100", SerialNumber="--",LocationID=20,CorporateAccountID=4,BrandModel=" WORKS. xw 4100", Notes="OFİS BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 4100", SerialNumber="FRB4040B6N",LocationID=20,CorporateAccountID=4,BrandModel=" WORKS. xw 4100", Notes="ÜRETİM HATTI BİLGİSAYARII.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVQ",LocationID=44,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFV9",LocationID=42,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="SIEMENS NOTEBOOK", SerialNumber="--",LocationID=4,CorporateAccountID=9,BrandModel=" NOTEBOOK", Notes="POLİÜRETAN MAKİNALARININ ARIZA TESPİTİNDE KULLANILIYOR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=4,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=4,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFV5",LocationID=1,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6200", SerialNumber="CZC6294N4S",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6200", Notes="ADİL TUNCER'DEN KALAN MAKİNA. YERİNE GELEN H. KOYUNCUOĞLU'NA VERİLMİŞTİR. HAKAN KOYUNCUOĞLUNUN İŞTEN AYRILMASI ÜZERİNE YERİNE GELEN K.O.GÖKGÖL'E ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=24,CorporateAccountID=null,BrandModel="", Notes="MANİSA FABRİKA",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=17,CorporateAccountID=null,BrandModel="", Notes="KALİTE ÇIKIŞ OFİSİ AUDIT BİLGİSAYARI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=17,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=17,CorporateAccountID=null,BrandModel="", Notes="MANİSA FABRİKA",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=17,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=17,CorporateAccountID=null,BrandModel="", Notes="MANİSA FABRİKA",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=17,CorporateAccountID=null,BrandModel="", Notes="GİRİŞ KALİTE OFİSİNDE STAJER KULLANIMINA VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=36,CorporateAccountID=null,BrandModel="", Notes="İLHAN MICILI'DAN ALINAN PC",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=17,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=17,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=35,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="ADVANTECH PPC-177T PANEL PC", SerialNumber="--",LocationID=17,CorporateAccountID=1,BrandModel=" PPC-177T PANEL PC", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="OFİS BİLGİSAYARI",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=23,CorporateAccountID=null,BrandModel="", Notes="MANİSA FABRİKA",InventoryOwnershipID=1,ValidationStateID=1},
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
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND115381Q",LocationID=38,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=25,CorporateAccountID=null,BrandModel="", Notes="KULAKLIĞINI İADE ETTİ.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=7,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=15,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=38,CorporateAccountID=null,BrandModel="", Notes="MEHMET URHAN'DAN ALINAN PC. GÖREV DEĞİŞİKLİĞİ NEDENİYLE YİĞİT ÇOKDUYAR'DAN ALINARAK MERTHAN SAYLAN'A ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="DELL WORKS. PRECISION 690", SerialNumber="--",LocationID=3,CorporateAccountID=3,BrandModel=" WORKS. PRECISION 690", Notes="LİDER METAL FRIO'DAN ALINAN WORKSTATION.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=1,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="SONY VAIO VGN-SZ2HP/B", SerialNumber="282443575000074",LocationID=30,CorporateAccountID=10,BrandModel=" VAIO VGN-SZ2HP/B", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 8600", SerialNumber="CZC8231BB5",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 8600", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6481ZDZ",LocationID=44,CorporateAccountID=4,BrandModel=" NC 6320", Notes="AYKUT TOKMAKOĞLU'NDAN KALAN NOTEBOOK.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=44,CorporateAccountID=null,BrandModel="", Notes="E-İMZA İÇİN VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538F5",LocationID=38,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. Z800", SerialNumber="CZC00551XW",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. Z800", Notes="KULLANMAKTA OLDUĞU HP WORKSTATION XW6200 DİĞER DONANIMLARI İLE BİRLİKTE GERİ ALINMIŞTIR. KULLANMAKTA OLDUĞU HP WORKS. XW8600 GERİ ALINMIŞTIR",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=22,CorporateAccountID=null,BrandModel="", Notes="ONUR SARÇIN'DAN KALAN PC. YERİNE GELEN G.AKMAN'A ZİMMETLENMİŞTİR. G.AKMAN'IN İŞTEN AYRILMASI NEDENİYLE YERİNE GELEN MERVE TİRİL'E ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTH",LocationID=44,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=42,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVL",LocationID=44,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6481ZF4",LocationID=24,CorporateAccountID=4,BrandModel=" NC 6320", Notes="MELİH YILDIRIM'DAN KALAN NOTEBOOK. YERİNE GELEN OGUN İSTANBUL'A ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 8600", SerialNumber="CZC8231BB3",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 8600", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVS",LocationID=33,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTM",LocationID=28,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTP",LocationID=13,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="ERDEM DEĞER'DEN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6400", SerialNumber="--",LocationID=5,CorporateAccountID=4,BrandModel=" WORKS. xw 6400", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 8430", SerialNumber="CNU6463FVT",LocationID=5,CorporateAccountID=4,BrandModel=" NC 8430", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU7322DKM",LocationID=25,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
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
                 new Inventory{Description="SONY VAIO VGN-SZ71MN/B", SerialNumber="28271654 5000353",LocationID=42,CorporateAccountID=10,BrandModel=" VAIO VGN-SZ71MN/B", Notes="28.05.2008 TARİHİNDE KULLANMAKTA OLDUĞU NO NAME PC GERİ ALINARAK  SONY VAIO NOTEBOOK VERİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVG",LocationID=32,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=26,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=1,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6200", SerialNumber="CZC6262D4L",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6200", Notes="SEMİH KAZMA'DAN KALAN WORKSTATION.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 6200", SerialNumber="CZC5460BJ0",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 6200", Notes="ÖZGÜR İLKUTLU'DAN KALAN MAKİNA. YÖNETİCİSİ HARUN İŞBİLİR'E ZİMMETLENDİ. KAMER ÇELİK'İN KULLANIMINA VERİLMİŞTİR. GÖREV DEĞİŞİKLİĞİ NEDENİYLE KAMER ÇELİK'TEN ALINARAK H.İŞBİLİR'E ZİMMETLENMİŞTİR. İŞE YENİ BAŞLAYAN SARP CANKUL'A VEİLMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 8200", SerialNumber="CZC62128XL",LocationID=32,CorporateAccountID=4,BrandModel=" WORKS. xw 8200", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFT8",LocationID=10,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=36,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFV7",LocationID=22,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="MİNE ŞAHİN'DEN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTB",LocationID=7,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTQ",LocationID=44,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538J7",LocationID=17,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVC",LocationID=16,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=2,CorporateAccountID=null,BrandModel="", Notes="BERKAN DERYA'DAN KALAN PC",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 8600", SerialNumber="CZC8335N7C",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 8600", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NX 7010", SerialNumber="CND4490PF1",LocationID=3,CorporateAccountID=4,BrandModel=" NX 7010", Notes="CND447291Z SERİ NOLU NX7010 ARIZASI NEDENİYLE GERİ ALINMIŞTIR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=34,CorporateAccountID=null,BrandModel="", Notes="GÜVENLİK CAMERALARINDAN ALINAN BİLGİSAYAR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=22,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. xw 8600", SerialNumber="CZC8335N7G",LocationID=3,CorporateAccountID=4,BrandModel=" WORKS. xw 8600", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=32,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFT6",LocationID=3,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538",LocationID=10,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6481ZF0",LocationID=22,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=3,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NX 7010", SerialNumber="--",LocationID=10,CorporateAccountID=4,BrandModel=" NX 7010", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=35,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=9,CorporateAccountID=null,BrandModel="", Notes="ERCAN KOÇ'TAN KALAN PC. İŞTEN AYRILMASI NEDENİYLE UĞUR HIZAL'A ZİMMETLENMİŞTİR. MUSTAFA ÜNLÜ'YE ZİMMETLENMİŞTİR.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=9,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTK",LocationID=1,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="TOSHIBA PORTEGE R500 U7700", SerialNumber="68052505H",LocationID=12,CorporateAccountID=11,BrandModel=" PORTEGE R500 U7700", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVD",LocationID=36,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6481ZDP",LocationID=17,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
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
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFV6",LocationID=25,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVH",LocationID=42,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTC",LocationID=34,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTT",LocationID=22,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6481ZNK",LocationID=17,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTV",LocationID=3,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="ESİN ALTIN'DAN KALAN PC.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NX 7010", SerialNumber="CND448127M",LocationID=9,CorporateAccountID=4,BrandModel=" NX 7010", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540p (WD919EA#AB8)", SerialNumber="CND11538MP",LocationID=38,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540p (WD919EA#AB8)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITEBOOK 8540w", SerialNumber="CND0221L1C",LocationID=5,CorporateAccountID=4,BrandModel=" ELITEBOOK 8540w", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTG",LocationID=7,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFVR",LocationID=44,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFV4",LocationID=28,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=39,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP ELITE 8100 SFF PC (AY032AV)", SerialNumber="CZC108CFTF",LocationID=43,CorporateAccountID=4,BrandModel=" ELITE 8100 SFF PC (AY032AV)", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=3,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 6320", SerialNumber="CNU6403SNS",LocationID=3,CorporateAccountID=4,BrandModel=" NC 6320", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NX 8220", SerialNumber="CNU6092C4N",LocationID=3,CorporateAccountID=4,BrandModel=" NX 8220", Notes="KULLANMAKTA OLDUĞU HP NC 6320 (CNU63726T0) NOTEBOOK EKRAN ARIZASI NEDENİYLE GERİ ALINDI.",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP NC 8430", SerialNumber="--",LocationID=5,CorporateAccountID=4,BrandModel=" NC 8430", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="HP WORKS. Z800", SerialNumber="--",LocationID=2,CorporateAccountID=4,BrandModel=" WORKS. Z800", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
                 new Inventory{Description="NO NAME PC", SerialNumber="--",LocationID=22,CorporateAccountID=null,BrandModel="", Notes="",InventoryOwnershipID=1,ValidationStateID=1},
    
            };

            invs.ForEach(s => context.Inventories.Add(s));
            context.SaveChanges();
        }

    }
}
