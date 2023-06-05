class Program
{
    static void Main( string[] args)
    {
       CustomerDatabase customerDatabase = new CustomerDatabase();

        // Test the functionality
        Customer customer1 = new Customer
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Address = "123 Main St"
        };
        customerDatabase.AddCustomer(customer1);

        Customer customer2 = new Customer
        {
            Id = 2,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            Address = "456 Elm St"
        };
        customerDatabase.AddCustomer(customer2);

        customerDatabase.SearchCustomerById(1);

        Customer updatedCustomer = new Customer
        {
            Id = 2,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            Address = "789 Oak St"
        };
        customerDatabase.UpdateCustomer(2, updatedCustomer);

        customerDatabase.SearchCustomerById(2);

        customerDatabase.DeleteCustomer(2);

        customerDatabase.Undo();
        customerDatabase.SearchCustomerById(2);

        customerDatabase.Redo();
        customerDatabase.SearchCustomerById(1);

        Console.ReadLine();
    }
}