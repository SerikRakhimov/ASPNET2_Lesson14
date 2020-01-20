namespace SaveTime.DaraAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migr04 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Employees", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Employees", new[] { "AccountId" });
            DropIndex("dbo.Employees", new[] { "BranchId" });
            AlterColumn("dbo.Employees", "AccountId", c => c.Int());
            AlterColumn("dbo.Employees", "BranchId", c => c.Int());
            CreateIndex("dbo.Employees", "AccountId");
            CreateIndex("dbo.Employees", "BranchId");
            AddForeignKey("dbo.Employees", "BranchId", "dbo.Branches", "Id");
            AddForeignKey("dbo.Employees", "AccountId", "dbo.Accounts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Employees", "BranchId", "dbo.Branches");
            DropIndex("dbo.Employees", new[] { "BranchId" });
            DropIndex("dbo.Employees", new[] { "AccountId" });
            AlterColumn("dbo.Employees", "BranchId", c => c.Int(nullable: false));
            AlterColumn("dbo.Employees", "AccountId", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "BranchId");
            CreateIndex("dbo.Employees", "AccountId");
            AddForeignKey("dbo.Employees", "AccountId", "dbo.Accounts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Employees", "BranchId", "dbo.Branches", "Id", cascadeDelete: true);
        }
    }
}
