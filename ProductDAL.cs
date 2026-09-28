using System.Data;
using Npgsql;
using System.Text.Json;
class ProductDAL
{
    private string connectionString =
        "Host=localhost;Port=5432;Database=productdb;Username=postgres;Password=postgres123";

    public void addProduct(Product product)
    {
        string insertQuery =
            "insert into Products(name,price,quantityinstock,category)" + " values(@pname,@pr,@qty,@ctg)";
        
        NpgsqlConnection connection =  new NpgsqlConnection(connectionString);

        connection.Open();
        NpgsqlCommand command = new NpgsqlCommand(insertQuery, connection);
        command.Parameters.AddWithValue("@pname", product.name);
        command.Parameters.AddWithValue("@pr", product.price);
        command.Parameters.AddWithValue("@qty", product.quantityInStock);
        command.Parameters.AddWithValue("@ctg", product.category);
        
        int reader = command.ExecuteNonQuery();
        connection.Close();

    }

    public List<Product> getAllProduct()
    {
        NpgsqlConnection connection =  new NpgsqlConnection(connectionString);

        connection.Open();

        string query = "SELECT * FROM Products";

        NpgsqlCommand command = new NpgsqlCommand(query, connection);
        
        NpgsqlDataReader reader = command.ExecuteReader();
        List<Product> products = new List<Product>();
        while (reader.Read())
        {
            Product pr = new Product(reader.GetInt32(0),reader.GetString(1),reader.GetInt32(2), reader.GetInt32(3), reader.GetString(4));
            products.Add(pr);
        }
        connection.Close();
        

        return products;

    }
    public Product getProductById(int productId)
    {
        NpgsqlConnection connection =  new NpgsqlConnection(connectionString);

        connection.Open();

        string query = "SELECT * FROM Products WHERE productid=@pid";

        NpgsqlCommand command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@pid", productId);
        NpgsqlDataReader reader = command.ExecuteReader();
        reader.Read(); 
        Product pr = new Product(reader.GetInt32(0),reader.GetString(1),reader.GetInt32(2), reader.GetInt32(3), reader.GetString(4));
        connection.Close();
        return pr;

    }
    public void updateProduct(Product product)
    {
        const string updateQuery =
            "UPDATE Products SET name = @name, price = @price, " +
            "quantityinstock = @qty, category = @category WHERE productid = @id";
        
        NpgsqlConnection connection =  new NpgsqlConnection(connectionString);

        connection.Open();
        NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection);
        command.Parameters.AddWithValue("@name", product.name);
        command.Parameters.AddWithValue("@price", product.price);
        command.Parameters.AddWithValue("@qty", product.quantityInStock);
        command.Parameters.AddWithValue("@category", product.category);
        command.Parameters.AddWithValue("@id", product.productId);

        int reader = command.ExecuteNonQuery();
        connection.Close();

    }
    public void deleteProduct(int productId)
    {
        NpgsqlConnection connection =  new NpgsqlConnection(connectionString);

        connection.Open();

        string query = "DELETE FROM Products WHERE productid=@pid";

        NpgsqlCommand command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@pid",productId);
        int reader = command.ExecuteNonQuery();
        connection.Close();

    }

    public List<Product> SearchProductsByName(string keyword)
    {
        NpgsqlConnection connection =  new NpgsqlConnection(connectionString);

        connection.Open();

        string query = "SELECT * FROM Products where name like @kw";

        NpgsqlCommand command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@kw","%" + keyword + "%");
        NpgsqlDataReader reader = command.ExecuteReader();
        List<Product> products = new List<Product>();
        while (reader.Read())
        {
            Product pr = new Product(reader.GetInt32(0),reader.GetString(1),reader.GetInt32(2), reader.GetInt32(3), reader.GetString(4));
            products.Add(pr);
        }
        connection.Close();
        return products;
        
    }

    public void ExportProductsToJson(string filePath)
    {
        List<Product> products=getAllProduct();
        foreach (var product in products)
        {
            var jsonString = JsonSerializer.Serialize(product);
            FileStream fs = new FileStream(filePath, FileMode.Append);
        StreamWriter sw = new StreamWriter(fs);
        sw.WriteLine(jsonString.ToString());
        }
        

    }

    public void ImportProductsFromJson(string filePath)
    {
        StreamReader sr = new StreamReader(filePath);
        while (sr.Read() != -1)
        {
            string jsonString = sr.ReadLine();
            Product product = JsonSerializer.Deserialize<Product>(jsonString);
            addProduct(product);
        }
    }

    public DataTable LoadProductsTable()
    {
        string selectQuery = "select * from products";
        NpgsqlConnection conn = new NpgsqlConnection(connectionString);
        DataTable dataTable = new DataTable();
        NpgsqlCommand selectCommand = new NpgsqlCommand(selectQuery, conn);
        NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
        adapter.SelectCommand = selectCommand;
        adapter.Fill(dataTable);
        return dataTable;
    }

    public void UpdatePriceInTable(DataTable table, NpgsqlDataAdapter adapter, int productId, decimal newPrice)
    {
        DataRow[] matchingRows = table.Select("productid = " + productId);
        foreach (var row in matchingRows)
        {
            row["price"] = newPrice;
        }
        adapter.Update(table);
    }
 
    
    
}