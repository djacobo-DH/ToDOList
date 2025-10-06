using Microsoft.EntityFrameworkCore;
using toDoList.ApiService.Models;

namespace toDoList.ApiService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> ToDoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

   
            modelBuilder.Entity<TodoItem>().ToTable("TodoItem");
        }
    }
}