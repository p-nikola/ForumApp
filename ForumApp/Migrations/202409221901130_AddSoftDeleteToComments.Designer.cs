﻿// <auto-generated />
namespace ForumApp.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.4.4")]
    public sealed partial class AddSoftDeleteToComments : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddSoftDeleteToComments));
        
        string IMigrationMetadata.Id
        {
            get { return "202409221901130_AddSoftDeleteToComments"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
