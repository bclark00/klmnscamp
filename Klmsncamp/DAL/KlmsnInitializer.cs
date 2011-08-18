using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Klmsncamp.Models;

namespace Klmsncamp.DAL
{
    public class KlmsnInitializer : DropCreateDatabaseIfModelChanges<KlmsnContext>
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
                new Workshop { Description = "BİLGİ İŞLEM", ValidationStateID=1  },
                new Workshop{ Description = "TEKNİK SERVİS" ,ValidationStateID=1}
                
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
                new Location { Description= "RESEPSİYON", ValidationStateID=1},
                new Location { Description = "AMBAR", ValidationStateID = 1},
                new Location { Description = "SATIŞ", ValidationStateID= 1},
                new Location { Description = "LOJİSTİK", ValidationStateID=1}
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

            var corpaccs = new List<CorporateAccount>
            {
                new CorporateAccount{ Title="MUSA FEDAKAR", Address="KLIMASAN AŞ", Phone1="164", CorporateTypeID=4, ValidationStateID=1, UserID=1 }
            };

            corpaccs.ForEach(s => context.CorporateAccounts.Add(s));
            context.SaveChanges();


        }

    }
}
