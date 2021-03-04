namespace PassionProjectTwo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstmigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WoWClasses",
                c => new
                    {
                        ClassID = c.Int(nullable: false, identity: true),
                        ClassName = c.String(),
                        ClassSpec = c.String(),
                        ClassPic = c.Boolean(nullable: false),
                        PicExtension = c.String(),
                        CompID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClassID)
                .ForeignKey("dbo.WoWComps", t => t.CompID, cascadeDelete: true)
                .Index(t => t.CompID);
            
            CreateTable(
                "dbo.WoWComps",
                c => new
                    {
                        CompID = c.Int(nullable: false, identity: true),
                        CompName = c.String(),
                        CompClass1 = c.String(),
                        CompClass2 = c.String(),
                        CompClass3 = c.String(),
                    })
                .PrimaryKey(t => t.CompID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WoWClasses", "CompID", "dbo.WoWComps");
            DropIndex("dbo.WoWClasses", new[] { "CompID" });
            DropTable("dbo.WoWComps");
            DropTable("dbo.WoWClasses");
        }
    }
}
