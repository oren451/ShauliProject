namespace ShauliProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Address : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FanClubs", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FanClubs", "Address");
        }
    }
}
