class Product
{
    public Product()
    {
        
    }

    public int productId { get; set; } = 0;
    public string name { get; set; }
    public int price { get; set; }
    public int quantityInStock { get; set; }
    public string category { get; set; }
    
    public Product(int pid, string nm, int pr, int qis, string ct)
    {
        productId = pid;
        name = nm;
        price = pr;
        quantityInStock = qis;
        category = ct;

    }
    
}


