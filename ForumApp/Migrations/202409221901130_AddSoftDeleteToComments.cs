namespace ForumApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSoftDeleteToComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "IsSoftDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "IsSoftDeleted");
        }
    }
}
