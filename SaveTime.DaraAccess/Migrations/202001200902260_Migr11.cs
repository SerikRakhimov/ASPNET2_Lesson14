namespace SaveTime.DaraAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migr11 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Branches", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Branches", new[] { "CompanyId" });
            AlterColumn("dbo.Branches", "CompanyId", c => c.Int());
            CreateIndex("dbo.Branches", "CompanyId");
            AddForeignKey("dbo.Branches", "CompanyId", "dbo.Companies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Branches", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Branches", new[] { "CompanyId" });
            AlterColumn("dbo.Branches", "CompanyId", c => c.Int(nullable: false));
            CreateIndex("dbo.Branches", "CompanyId");
            AddForeignKey("dbo.Branches", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
    }
}
