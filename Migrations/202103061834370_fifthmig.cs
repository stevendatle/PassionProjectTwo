namespace PassionProjectTwo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fifthmig : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WoWClasses", "CompID", "dbo.WoWComps");
            DropIndex("dbo.WoWClasses", new[] { "CompID" });
            AlterColumn("dbo.WoWClasses", "CompID", c => c.Int());
            CreateIndex("dbo.WoWClasses", "CompID");
            AddForeignKey("dbo.WoWClasses", "CompID", "dbo.WoWComps", "CompID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WoWClasses", "CompID", "dbo.WoWComps");
            DropIndex("dbo.WoWClasses", new[] { "CompID" });
            AlterColumn("dbo.WoWClasses", "CompID", c => c.Int(nullable: false));
            CreateIndex("dbo.WoWClasses", "CompID");
            AddForeignKey("dbo.WoWClasses", "CompID", "dbo.WoWComps", "CompID", cascadeDelete: true);
        }
    }
}
