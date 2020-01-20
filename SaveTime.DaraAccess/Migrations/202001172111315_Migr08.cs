namespace SaveTime.DaraAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migr08 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Employees", new[] { "AccountId" });
            DropIndex("dbo.Employees", new[] { "BranchId" });
            AlterColumn("dbo.Employees", "AccountId", c => c.Int(nullable: false));
            AlterColumn("dbo.Employees", "BranchId", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "AccountId");
            CreateIndex("dbo.Employees", "BranchId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Employees", new[] { "BranchId" });
            DropIndex("dbo.Employees", new[] { "AccountId" });
            AlterColumn("dbo.Employees", "BranchId", c => c.Int());
            AlterColumn("dbo.Employees", "AccountId", c => c.Int());
            CreateIndex("dbo.Employees", "BranchId");
            CreateIndex("dbo.Employees", "AccountId");
        }
    }
}
