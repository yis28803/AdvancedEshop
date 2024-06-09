using Microsoft.EntityFrameworkCore;

namespace Duanmau.Web.API.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Khai báo các DbSet để tương tác với các bảng trong cơ sở dữ liệu
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillInfo> BillInfos { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Food_Ingredient> Food_Ingredients { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<Tablefood> Tablefoods { get; set; }
        public DbSet<DanhMucFood> DanhMucFoods { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Food>()
                .HasOne(a => a.DanhMucFoods)
                .WithMany()
                .HasForeignKey(s => s.DanhMucFoodId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bill>()
                .HasOne(a => a.KhachHangs)
                .WithMany()
                .HasForeignKey(s => s.IdKhachHang)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Bill>()
                .HasOne(a => a.NhanViens)
                .WithMany()
                .HasForeignKey(s => s.IdNhanVien)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Bill>()
                .HasOne(a => a.Tablefoods)
                .WithMany()
                .HasForeignKey(s => s.IdTableFood)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BillInfo>()
               .HasOne(a => a.Foods)
               .WithMany()
               .HasForeignKey(s => s.IdFood)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<BillInfo>()
               .HasOne(a => a.Bills)
               .WithMany()
               .HasForeignKey(s => s.IdBill)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Food_Ingredient>()
               .HasOne(a => a.Foods)
               .WithMany()
               .HasForeignKey(s => s.FoodId)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Food_Ingredient>()
               .HasOne(a => a.Ingredients)
               .WithMany()
               .HasForeignKey(s => s.IngredientId)
               .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

        // Khai báo các DbSet khác nếu cần thiết
    }
}
