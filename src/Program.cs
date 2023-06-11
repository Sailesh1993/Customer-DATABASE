class Program
{
    static void Main( string[] args)
    {
       /* CustomerDatabase customerDatabase = new CustomerDatabase();

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

        Console.ReadLine(); */
    
     // File path for storing customer data
        string filePath = "customers.csv";

        // Create a customer database and initialize it with data from the file
        CustomerDatabase database = new CustomerDatabase();
        try
        {
            var customers = FileHelper.ReadCustomersFromFile(filePath);
            foreach (var customer in customers)
            {
                database.AddCustomer(customer);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException(ex);
        }

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine();
            Console.WriteLine("Customer Database Management");
            Console.WriteLine("1. Add Customer");
            Console.WriteLine("2. Update Customer");
            Console.WriteLine("3. Delete Customer");
            Console.WriteLine("4. Search Customer by ID");
            Console.WriteLine("5. Undo");
            Console.WriteLine("6. Redo");
            Console.WriteLine("0. Exit");
            Console.WriteLine();

            Console.Write("Enter your choice: ");
            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    AddCustomer(database);
                    break;
                case "2":
                    UpdateCustomer(database);
                    break;
                case "3":
                    DeleteCustomer(database);
                    break;
                case "4":
                    SearchCustomerById(database);
                    break;
                case "5":
                    database.Undo();
                    Console.WriteLine("Undo performed.");
                    break;
                case "6":
                    database.Redo();
                    Console.WriteLine("Redo performed.");
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            try
            {
                FileHelper.WriteCustomersToFile(database.GetCustomers(), filePath);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
    }

    static void AddCustomer(CustomerDatabase database)
    {
        Console.WriteLine("Add Customer");
        Console.WriteLine();

        Console.Write("Enter ID: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Enter First Name: ");
        string firstName = Console.ReadLine();

        Console.Write("Enter Last Name: ");
        string lastName = Console.ReadLine();

        Console.Write("Enter Email: ");
        string email = Console.ReadLine();

        Console.Write("Enter Address: ");
        string address = Console.ReadLine();

        try
        {
            Customer newCustomer = new Customer
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Address = address
            };
            database.AddCustomer(newCustomer);
            Console.WriteLine("Customer added successfully.");
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException(ex);
        }
    }

    static void UpdateCustomer(CustomerDatabase database)
    {
        Console.WriteLine("Update Customer");
        Console.WriteLine();

        Console.Write("Enter ID of the customer to update: ");
        int id = int.Parse(Console.ReadLine());

        Customer existingCustomer = database.GetCustomerById(id);
        if (existingCustomer != null)
        {
            Console.Write("Enter New First Name: ");
            string newFirstName = Console.ReadLine();

            Console.Write("Enter New Last Name: ");
            string newLastName = Console.ReadLine();

            Console.Write("Enter New Email: ");
            string newEmail = Console.ReadLine();

            Console.Write("Enter New Address: ");
            string newAddress = Console.ReadLine();

            try
            {
                Customer updatedCustomer = new Customer
                {
                    Id = existingCustomer.Id,
                    FirstName = newFirstName,
                    LastName = newLastName,
                    Email = newEmail,
                    Address = newAddress
                };
                database.UpdateCustomer(id, updatedCustomer);
                Console.WriteLine("Customer updated successfully.");
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        else
        {
            Console.WriteLine("Customer not found.");
        }
    }

    static void DeleteCustomer(CustomerDatabase database)
    {
        Console.WriteLine("Delete Customer");
        Console.WriteLine();

        Console.Write("Enter ID of the customer to delete: ");
        int id = int.Parse(Console.ReadLine());

        Customer existingCustomer = database.GetCustomerById(id);
        if (existingCustomer != null)
        {
            database.DeleteCustomer(id);
            Console.WriteLine("Customer deleted successfully.");
        }
        else
        {
            Console.WriteLine("Customer not found.");
        }
    }

    static void SearchCustomerById(CustomerDatabase database)
    {
        Console.WriteLine("Search Customer by ID");
        Console.WriteLine();

        Console.Write("Enter ID of the customer to search: ");
        int id = int.Parse(Console.ReadLine());

        Customer customer = database.GetCustomerById(id);
        if (customer != null)
        {
            Console.WriteLine("Customer Found:");
            Console.WriteLine($"ID: {customer.Id}");
            Console.WriteLine($"First Name: {customer.FirstName}");
            Console.WriteLine($"Last Name: {customer.LastName}");
            Console.WriteLine($"Email: {customer.Email}");
            Console.WriteLine($"Address: {customer.Address}");
        }
        else
        {
            Console.WriteLine("Customer not found.");
        }
    }
}