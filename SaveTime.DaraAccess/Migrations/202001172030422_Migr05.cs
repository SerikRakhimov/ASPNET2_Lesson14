namespace SaveTime.DaraAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migr05 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Employees", new[] { "BranchId" });
            AlterColumn("dbo.Employees", "BranchId", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "BranchId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Employees", new[] { "BranchId" });
            AlterColumn("dbo.Employees", "BranchId", c => c.Int());
            CreateIndex("dbo.Employees", "BranchId");
        }
    }
}
