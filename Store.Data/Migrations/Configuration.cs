namespace Store.Data.Migrations
{
    using Store.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<Store.Data.StoreContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Store.Data.StoreContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            string username = "test_admin";
            string displayName = "Admin";
            string authCode = "7110eda4d09e062aa5e4a390b0a572ac0d2c0220";

            var admin = context.Admins.FirstOrDefault(a => a.Username == username);
            if (admin != null)
            {
                context.Admins.Add(
                    new Admin 
                    { 
                        Username = username,
                        DisplayName = displayName,
                        AuthCode = authCode
                    });

                context.SaveChanges();
            }
        }
    }
}
