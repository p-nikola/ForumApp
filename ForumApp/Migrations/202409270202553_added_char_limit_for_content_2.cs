namespace ForumApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_char_limit_for_content_2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comments", "Content", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Posts", "Content", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "Content", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Comments", "Content", c => c.String(nullable: false, maxLength: 250));
        }
    }
}
