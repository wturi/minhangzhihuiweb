﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MinHangWisdomParkWeb.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class ajIIPdbEntities1 : DbContext
    {
        public ajIIPdbEntities1()
            : base("name=ajIIPdbEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Functions> Functions { get; set; }
        public DbSet<mtActor> mtActor { get; set; }
        public DbSet<mtCamera> mtCamera { get; set; }
        public DbSet<mtConfirmAuto> mtConfirmAuto { get; set; }
        public DbSet<mtConfirmFlow> mtConfirmFlow { get; set; }
        public DbSet<mtConfirmLevel> mtConfirmLevel { get; set; }
        public DbSet<mtFunction> mtFunction { get; set; }
        public DbSet<mtOwner> mtOwner { get; set; }
        public DbSet<mtPower> mtPower { get; set; }
        public DbSet<mtRegistrant> mtRegistrant { get; set; }
        public DbSet<mtRfidCode> mtRfidCode { get; set; }
        public DbSet<mtUniversalCode> mtUniversalCode { get; set; }
        public DbSet<mtUser> mtUser { get; set; }
        public DbSet<mtUserActor> mtUserActor { get; set; }
        public DbSet<sysdiagrams> sysdiagrams { get; set; }
        public DbSet<tbActorPower> tbActorPower { get; set; }
        public DbSet<tbApplyBill> tbApplyBill { get; set; }
        public DbSet<tbBuniess> tbBuniess { get; set; }
        public DbSet<tbCar> tbCar { get; set; }
        public DbSet<tbCheckInOut> tbCheckInOut { get; set; }
        public DbSet<tbCheckInOuts> tbCheckInOuts { get; set; }
        public DbSet<tbConfirmState> tbConfirmState { get; set; }
        public DbSet<tbFiles> tbFiles { get; set; }
        public DbSet<tbIORecord> tbIORecord { get; set; }
        public DbSet<tbOwnerCamera> tbOwnerCamera { get; set; }
        public DbSet<tbPeblish> tbPeblish { get; set; }
        public DbSet<tbPeople> tbPeople { get; set; }
        public DbSet<tbProduct> tbProduct { get; set; }
        public DbSet<tbReceive> tbReceive { get; set; }
        public DbSet<tbRepair> tbRepair { get; set; }
        public DbSet<tbResourceFiles> tbResourceFiles { get; set; }
        public DbSet<tbTemp> tbTemp { get; set; }
        public DbSet<tbWeight> tbWeight { get; set; }
        public DbSet<mtDevices> mtDevices { get; set; }
        public DbSet<tbOwnerRfid> tbOwnerRfid { get; set; }
        public DbSet<mdlPeblish> mdlPeblish { get; set; }
        public DbSet<twCarLastVersion> twCarLastVersion { get; set; }
        public DbSet<twIORecord> twIORecord { get; set; }
        public DbSet<twOwnerRfidStatus> twOwnerRfidStatus { get; set; }
        public DbSet<twPeopleLastVersion> twPeopleLastVersion { get; set; }
        public DbSet<twProductLastVersion> twProductLastVersion { get; set; }
    
        public virtual int Proc_RegistrantID(string registrantType)
        {
            var registrantTypeParameter = registrantType != null ?
                new ObjectParameter("RegistrantType", registrantType) :
                new ObjectParameter("RegistrantType", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Proc_RegistrantID", registrantTypeParameter);
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    }
}