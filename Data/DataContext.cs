using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        //DbSet of type Charatacter class and table Name is Characters
        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Weapon> Weapons {get; set;}
        public DbSet<Skill> Skills {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
                new Skill {Id=1 , Name="FireBall", Damage = 30},
                new Skill {Id=2 , Name="Frenzy", Damage = 20},
                new Skill {Id=3 , Name="Blizzard", Damage = 50}
            );
        }
        
    }
}

/*
    Code First Approch

    1. Create a Modal Class with Appropriate Fields 
    2. Add Entry of Dataset in DataContext file about Table.
    3. Add Migrations
        a. dotnet ef migrations add _Name-to-Migration  
    4. Update Database    
        a. dotnet ef database update
    
    After Successfull Update we can see the Tables in our database.
*/  
