using BookStore.Models.Configuration.Initializers;
using BookStore.Models.Configuration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Configuration
{
    public class EntityInitializer
    {
        private readonly List<ITypeInitializer> typeInitializers;
        //private readonly UserManager<User> userManager;
        //private readonly RoleManager<IdentityRole<int>> roleManager;
        private readonly ApplicationContext context;

        public EntityInitializer(ApplicationContext _context
                                 //UserManager<User> _userManager,
                                 //RoleManager<IdentityRole<int>> _roleManager
                                 )
        {
            typeInitializers = new List<ITypeInitializer>();
            //userManager = _userManager;
            //roleManager = _roleManager;
            context = _context;

            //add concrete initializers
            //order is IMPORTANT (for example, you first need to add categories and only then products that are category-dependent)
            this.AddConfig(new CategoriesInitializer());

        }

        public void AddConfig(ITypeInitializer typeInitializer)
        {
            typeInitializers.Add(typeInitializer);
        }

        public async Task SeedData()
        {
            //always delete and recreate database with seeded data
            bool deleted = await context.Database.EnsureDeletedAsync();
            bool created = await context.Database.EnsureCreatedAsync();

            //create test users and admins
            //go through all the initializers and seed them all
            foreach (var initializer in typeInitializers)
            {
                await initializer.Init(context);
                await context.SaveChangesAsync();
            }
        }
    }
}
