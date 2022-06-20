// using Microsoft.EntityFrameworkCore;
// using EcAPI.Entity;
// namespace EcAPI.Entities
// {
//     public class MyDBContext : DbContext
//     {
//        public DbSet<Product> Products { get; set; }
//         private string _conn = "Data Source=C:/SQLite/ECommerce.db;";
//         //public MyDBContext()
//         //{
//         //    //_conn = conn;
//         //}dotnet ef database update -c MyDBContext
//         public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) { }

// 		public MyDBContext()
// 		{
// 		}

// 		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//         {
//             //optionsBuilder.Configurations.Add(new UserAttachmentConfiguration());

//             optionsBuilder.UseSqlite(_conn);
//         }
//         //public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
//         //{
            
//         //}
        
//         //      public MyDBContext()
//         //{
//         //}
//         private static DbContextOptions GetOptions(string connectionString)
//         {
//             return SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder(), "Data Source=C:/SQLite/mydb.db;").Options;
//         }
//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             // Use Fluent API to c
//             // onfigure  
//             //optionsBuilder.Configurations.Add(new UserAttachmentConfiguration());

//             // Map entities to tables  
//         }
//     }
// }
