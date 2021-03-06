namespace PassionProjectTwo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fourthmig : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WoWClasses", "WoWComp_CompID", "dbo.WoWComps");
            DropIndex("dbo.WoWClasses", new[] { "WoWComp_CompID" });
            RenameColumn(table: "dbo.WoWClasses", name: "WoWComp_CompID", newName: "CompID");
            AlterColumn("dbo.WoWClasses", "CompID", c => c.Int(nullable: true));
            CreateIndex("dbo.WoWClasses", "CompID");
            AddForeignKey("dbo.WoWClasses", "CompID", "dbo.WoWComps", "CompID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WoWClasses", "CompID", "dbo.WoWComps");
            DropIndex("dbo.WoWClasses", new[] { "CompID" });
            AlterColumn("dbo.WoWClasses", "CompID", c => c.Int());
            RenameColumn(table: "dbo.WoWClasses", name: "CompID", newName: "WoWComp_CompID");
            CreateIndex("dbo.WoWClasses", "WoWComp_CompID");
            AddForeignKey("dbo.WoWClasses", "WoWComp_CompID", "dbo.WoWComps", "CompID");
        }
    }
}
