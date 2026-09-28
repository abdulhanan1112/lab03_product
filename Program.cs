using System;
using System.Data;
using System.Data.Common;
using Microsoft.VisualBasic.FileIO;
using Npgsql;

namespace MyNamespace
{
    internal class Program
    {
        static void Main()
        {
           
            ProductDAL dal = new ProductDAL();
            bool flag = true;

            while (flag)
            {
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. View All Products");
                Console.WriteLine("3. View Product by Id");
                Console.WriteLine("4. Update Product");
                Console.WriteLine("5. Delete Product");
                Console.WriteLine("6. Search Products by Name");
                Console.WriteLine("7. Export Products to JSON ");
                Console.WriteLine("8. Import Products from JSON ");
                Console.WriteLine("9. Update Product Price (Disconnected Model)");
                Console.WriteLine("10. Exit");

                int input = int.Parse(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        Product product = new Product();
                        

                        Console.Write("Enter Product Name: ");
                        product.name = Console.ReadLine();

                        Console.Write("Enter Price: ");
                        product.price = int.Parse(Console.ReadLine());

                        Console.Write("Enter Quantity In Stock: ");
                        product.quantityInStock = int.Parse(Console.ReadLine());

                        Console.Write("Enter Category: ");
                        product.category = Console.ReadLine();

                        dal.addProduct(product);

                        Console.WriteLine("Product Added Successfully.");
                        break;

                    case 2:
                        List<Product> products = dal.getAllProduct();

                        foreach (Product p in products)
                        {
                            Console.WriteLine(
                                $"Id: {p.productId}, Name: {p.name}, Price: {p.price}, Quantity: {p.quantityInStock}, Category: {p.category}"
                            );
                        }

                        break;

                    case 3:
                        Console.Write("Enter Product Id: ");
                        int productId = int.Parse(Console.ReadLine());

                        Product productById = dal.getProductById(productId);

                        Console.WriteLine(
                            $"Id: {productById.productId}, Name: {productById.name}, Price: {productById.price}, Quantity: {productById.quantityInStock}, Category: {productById.category}"
                        );

                        break;

                    case 4:
                        Product updateProduct = new Product();

                        Console.Write("Enter Product Id: ");
                        updateProduct.productId = int.Parse(Console.ReadLine());

                        Console.Write("Enter Product Name: ");
                        updateProduct.name = Console.ReadLine();

                        Console.Write("Enter Price: ");
                        updateProduct.price = int.Parse(Console.ReadLine());

                        Console.Write("Enter Quantity In Stock: ");
                        updateProduct.quantityInStock = int.Parse(Console.ReadLine());

                        Console.Write("Enter Category: ");
                        updateProduct.category = Console.ReadLine();

                        dal.updateProduct(updateProduct);

                        Console.WriteLine("Product Updated Successfully.");
                        break;

                    case 5:
                        Console.Write("Enter Product Id: ");
                        int deleteId = int.Parse(Console.ReadLine());

                        dal.deleteProduct(deleteId);

                        Console.WriteLine("Product Deleted Successfully.");
                        break;
                    case 6:
                        string keyword;
                        Console.WriteLine("Enter the keyword : ");
                        keyword = Console.ReadLine();
                        List<Product> prs = dal.SearchProductsByName(keyword);

                        foreach (Product p in prs)
                        {
                            Console.WriteLine(
                                $"Id: {p.productId}, Name: {p.name}, Price: {p.price}, Quantity: {p.quantityInStock}, Category: {p.category}"
                            );
                        }
                        break;
                    
                    case 7:
                        string filePath1;
                        Console.WriteLine("Write the path of file : ");
                        filePath1 = Console.ReadLine();
                        dal.ExportProductsToJson(filePath1);
                        break;
                    case 8:
                        string filePath2;
                        Console.WriteLine("Write the path of file : ");
                        filePath2 = Console.ReadLine();
                        dal.ImportProductsFromJson(filePath2);
                        break;
                    case 9:
                        NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
                        DataTable table = dal.LoadProductsTable();
                        int id;
                        decimal newPrice;
                        Console.WriteLine("Enter the productId : ");
                        id = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter the new price : ");
                        newPrice = decimal.Parse(Console.ReadLine());
                        dal.UpdatePriceInTable(table,adapter,id,newPrice);
                        List<Product> products2 = dal.getAllProduct();
                        foreach (Product p in products2)
                        {
                            Console.WriteLine(
                                $"Id: {p.productId}, Name: {p.name}, Price: {p.price}, Quantity: {p.quantityInStock}, Category: {p.category}"
                            );
                        }
                        break;
                    case 10:
                        flag = false;
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid Input.");
                        break;
                }
            }
        }
    }
}