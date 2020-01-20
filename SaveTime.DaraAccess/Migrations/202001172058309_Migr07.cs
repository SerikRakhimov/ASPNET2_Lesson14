namespace SaveTime.DaraAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migr07 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Branches", "CompanyId", "dbo.Companies");
            AddForeignKey("dbo.Branches", "CompanyId", "dbo.Companies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Branches", "CompanyId", "dbo.Companies");
            AddForeignKey("dbo.Branches", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
    }
}
