namespace PassionProjectTwo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thirdmig : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WoWClasses", "CompID", "dbo.WoWComps");
            DropIndex("dbo.WoWClasses", new[] { "CompID" });
            RenameColumn(table: "dbo.WoWClasses", name: "CompID", newName: "WoWComp_CompID");
            AlterColumn("dbo.WoWClasses", "WoWComp_CompID", c => c.Int());
            CreateIndex("dbo.WoWClasses", "WoWComp_CompID");
            AddForeignKey("dbo.WoWClasses", "WoWComp_CompID", "dbo.WoWComps", "CompID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WoWClasses", "WoWComp_CompID", "dbo.WoWComps");
            DropIndex("dbo.WoWClasses", new[] { "WoWComp_CompID" });
            AlterColumn("dbo.WoWClasses", "WoWComp_CompID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.WoWClasses", name: "WoWComp_CompID", newName: "CompID");
            CreateIndex("dbo.WoWClasses", "CompID");
            AddForeignKey("dbo.WoWClasses", "CompID", "dbo.WoWComps", "CompID", cascadeDelete: true);
        }
    }
}
