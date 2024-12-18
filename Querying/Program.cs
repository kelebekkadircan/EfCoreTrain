



using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

Console.WriteLine(" ");
// global context tanımlaması
ECommerceDbContext context = new();

#region Adding data
//Customers customers1 = new() { Name = "Mehmet", Address = "İstanbul" };
//Customers customers2 = new() { Name = "Ahmet", Address = "Ankara" };
//Customers customers3 = new() { Name = "Ayşe", Address = "İzmir" };
//await context.Customers.AddRangeAsync(customers1, customers2, customers3);
//await context.SaveChangesAsync();

//Product product1 = new() { Name = "Laptop", Price = 5000 };
//Product product2 = new() { Name = "Mouse", Price = 50 };
//Product product3 = new() { Name = "Keyboard", Price = 100 };
//await context.Products.AddRangeAsync(product1, product2, product3);
//await context.SaveChangesAsync();

//Orders order1 = new() { CustomerId = 1, ProductId = 1, Quantity = 2, OrderDate = DateTime.Now };
//Orders order2 = new() { CustomerId = 2, ProductId = 2, Quantity = 1, OrderDate = DateTime.Now };
//Orders order3 = new() { CustomerId = 3, ProductId = 3, Quantity = 3, OrderDate = DateTime.Now };
//await context.Orders.AddRangeAsync(order1, order2, order3);
//await context.SaveChangesAsync();


#endregion

#region Querying
#region Method Syntax

//var querys1 = await context.Products.ToListAsync();
//foreach (var item in query1)
//{
//    Console.WriteLine($"Product Name: {item.Name} - Price: {item.Price}");
//}
#endregion
#region Querying Syntax
// var querys2 =    await (from product in context.Products
//           select product).ToListAsync();
//foreach (var item in query2)
//    {
//    Console.WriteLine($"Product Name: {item.Name} - Price: {item.Price}");
//}
#endregion
#region IQueryable
// sorguya karşılık gelir
// ef core üzerinde yapılmış olan  sorgunun  execute edilmemiş halini ifade eder.
//var queryable1 = context.Orders;

#endregion
#region IEnumerable
//bellekteki verileri temsil eder kolleksyionel olanları ifade eder
//sorgunun çalışıp/execute edilip verilerin in memorye yüklenmiş halini ifader eder.
//var enumerable1 = context.Orders.ToList(); // sorguyu exec etmek için to list async kullanırız.

#endregion
#region foreach
//var orders1 = context.Orders;

//foreach ( var item in orders1)
//{
//    Console.WriteLine($"OrderID:{item.Id} - Quantity: {item.Quantity} - Order Date: {item.OrderDate} - CustomerID :{item.CustomerId} - ProductID :{item.ProductId} ");
//}

#endregion
#region Deffered Execution
// IQuerable çalışmalarada ilgili kod yazıldığı yerde execute edilmez. sorgu çalıştırılmadan önce bekler.
// execute edildiği noktada tetiklenir. buna da ertelenmiş çalışma denir.
#endregion

#endregion

#region Multiple Querying
#region ToListAsync
// ToListAsync() => Asenkron olarak execute ettirmemizi sağlayan fonksiyondur.
    //var querableData = context.Orders;
    //var ıenumerableData = await querableData.ToListAsync(); // sorguyu çalıştırır ve verileri getirir.
#endregion

#region Where
// x orderı temsil eder
//var data = await context.Orders.Where(x => x.Quantity >= 2).ToListAsync(); 
//foreach (var item in data)
//{
//    Console.WriteLine($"OrderID:{item.Id} - Quantity: {item.Quantity} - Order Date: {item.OrderDate} - CustomerID :{item.CustomerId} - ProductID :{item.ProductId} ");
//}
#endregion

#region OrderBy
// sıralama fonksiyonudur
// burada küçükten büyüğe sıralıyor ve quantity miktarına göre sıralıyor
//var datasAscending = await context.Orders.OrderBy(x => x.Quantity).ToListAsync();
//foreach (var item in datasAscending)
//{
//    Console.WriteLine($"OrderID:{item.Id} - Quantity: {item.Quantity} - Order Date: {item.OrderDate} - CustomerID :{item.CustomerId} - ProductID :{item.ProductId} ");
//}

#endregion

#region ThenBy
// eğer orderby ile sıralama yaptıysak ardından thenby ile ikinci bir sıralama yapabiliriz.
//var datasThenBy = await context.Orders.OrderBy(x => x.Quantity).ThenBy(x => x.Id).ToListAsync();

//foreach (var item in datasThenBy)
//{
//    Console.WriteLine($"OrderID:{item.Id} - Quantity: {item.Quantity} - Order Date: {item.OrderDate} - CustomerID :{item.CustomerId} - ProductID :{item.ProductId} ");
//}


#endregion

#region OrderByDescending
// descending sıralama yapar yani azalan sıralama yapar
//var datasDescending = await context.Orders.OrderByDescending(x => x.Quantity).ToListAsync();

#endregion

#region ThenByDescending
// descending sıralama yapar yani azalan sıralama yapar ardından thenby ile ikinci bir sıralama yapabiliriz.
//var datasThenByDescending = await context.Orders.OrderBy(x => x.Quantity).ThenByDescending(x => x.Id).ToListAsync();

#endregion


#endregion 




public class ECommerceDbContext : DbContext
{
    public DbSet<Customers> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Orders> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Connection stringinizi buraya ekleyin
             optionsBuilder.UseSqlServer("Server=localhost;Database=ECommerceDbEfTrain;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Orders tablosu için ilişkiler
        modelBuilder.Entity<Orders>()
            .HasKey(o => o.Id); // Primary Key

        modelBuilder.Entity<Orders>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId); // Foreign Key

        modelBuilder.Entity<Orders>()
            .HasOne(o => o.Product)
            .WithMany(p => p.Orders)
            .HasForeignKey(o => o.ProductId); // Foreign Key
    }
}







// Customers sınıfı
public class Customers
{
    public int Id { get; set; } // Primary key

    public string Name { get; set; } // Müşteri adı
    public string Address { get; set; } // Müşteri adresi

    // Navigation Property: Bir müşteri birden fazla sipariş verebilir
    public ICollection<Orders> Orders { get; set; }
}

// Product sınıfı
public class Product
{
    public int Id { get; set; } // Primary key

    public string Name { get; set; } // Ürün adı
    public decimal Price { get; set; } // Ürün fiyatı

    // Navigation Property: Bir ürün birden fazla siparişin parçası olabilir
    public ICollection<Orders> Orders { get; set; }
}

// Orders sınıfı (Ara tablo)
public class Orders
{
    public int Id { get; set; } // Primary key

    public int CustomerId { get; set; } // Foreign key
    public Customers Customer { get; set; } // Navigation Property

    public int ProductId { get; set; } // Foreign key
    public Product Product { get; set; } // Navigation Property

    public int Quantity { get; set; } // Sipariş edilen ürün adedi
    public DateTime OrderDate { get; set; } // Sipariş tarihi
}



//public class ECommerceDbContext : DbContext
//{
//    //DESKTOP-KQ16HG1
//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//    {
//        // connection string araştırması yapabilirsin
//        optionsBuilder.UseSqlServer("Server=localhost;Database=ECommerceDbEfTrain;Trusted_Connection=True;TrustServerCertificate=True;");
//    }

//    public DbSet<Product> Products { get; set; }

//    public DbSet<Customers> Customers { get; set; }


//    // get-migration => migrationları getirir
//    // add-migration => migration ekler
//    // update-database => migrationları database'e uygular
//    // down fonksionu => update-database [migName] => geri alır example : update-database mig2

//    //// bu sayede kod ile de migrate işlemi yapılabilir
//    // ECommerceDbContext context = new();
//    //await context.Database.MigrateAsync();


//}

//public class Customers
//{
//    //bunlardan birini yapmamız primary key için yeterli olacaktır
//    ////public int CustomersId { get; set; }
//    ////public int ID { get; set; }
//    ////public int CustomersID { get; set; }
//    public int Id { get; set; }


//    public string Name { get; set; }
//    public string Address { get; set; }
//}

//public class Product
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public decimal Price { get; set; }
//}



