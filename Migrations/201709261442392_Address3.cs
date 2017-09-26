namespace ShauliProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Address3 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.FanClubs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FanClubs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Seniority = c.Int(nullable: false),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
