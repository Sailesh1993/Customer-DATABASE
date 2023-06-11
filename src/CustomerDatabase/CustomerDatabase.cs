using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class CustomerDatabase
{
    /* private List<Customer> customers;
    private HashSet<string> emails;
    private Stack<Tuple<string, Customer>> undoStack;
    private Stack<Tuple<string, Customer>> redoStack;
    private string DatabaseFilePath = "customers.csv";

    public CustomerDatabase()
    {
        customers = new List<Customer>();
        emails = new HashSet<string>();
        undoStack = new Stack<Tuple<string, Customer>>();
        redoStack = new Stack<Tuple<string, Customer>>();

        ReadDataFromFile();
    }
    
    public void AddCustomer(Customer customer)
    {
        if(emails.Contains(customer.Email))
        {
            Console.WriteLine("Email already exists. Customer not added.");
            return;
        }
        customers.Add(customer);
        emails.Add(customer.Email);
        undoStack.Push(new Tuple<string, Customer>("add",customer));
        WriteDataToFile();
    }

    public void DeleteCustomer (int customerID)
{
    Customer? customerToDelete = customers.FirstOrDefault(c => c.Id == customerID);
        if (customerToDelete != null)
        {
            customers.Remove(customerToDelete);
            emails.Remove(customerToDelete.Email);
            undoStack.Push(new Tuple<string, Customer>("delete", customerToDelete));

            WriteDataToFile();
        }
        else
        {
            Console.WriteLine("Customer not found.");
        }
}
  
    public void UpdateCustomer(int customerId, Customer updatedCustomer)
    {
        Customer? customerToUpdate = customers.FirstOrDefault(c => c.Id == customerId);
        if (customerToUpdate != null)
        {
            undoStack.Push(new Tuple<string, Customer>("update", new Customer
            {
                Id = customerToUpdate.Id,
                FirstName = customerToUpdate.FirstName,
                LastName = customerToUpdate.LastName,
                Email = customerToUpdate.Email,
                Address = customerToUpdate.Address
            }));

            customerToUpdate.FirstName = updatedCustomer.FirstName;
            customerToUpdate.LastName = updatedCustomer.LastName;
            customerToUpdate.Email = updatedCustomer.Email;
            customerToUpdate.Address = updatedCustomer.Address;

            WriteDataToFile();
        }
        else
        {
            Console.WriteLine("Customer not found.");
        }
    }
    
    public void Undo()
    {
        if (undoStack.Count > 0)
        {
            var action = undoStack.Pop();

            if (action.Item1 == "add")
            {
                var customerToRemove = customers.FirstOrDefault(c => c.Id == action.Item2.Id);
                if (customerToRemove != null)
                {
                    customers.Remove(customerToRemove);
                    emails.Remove(customerToRemove.Email);
                    redoStack.Push(new Tuple<string, Customer>("add", customerToRemove));
                }
            }
            else if (action.Item1 == "delete")
            {
                customers.Add(action.Item2);
                emails.Add(action.Item2.Email);
                redoStack.Push(new Tuple<string, Customer>("delete", action.Item2));
            }
            else if (action.Item1 == "update")
            {
                var customerToUpdate = customers.FirstOrDefault(c => c.Id == action.Item2.Id);
                if (customerToUpdate != null)
                {
                    redoStack.Push(new Tuple<string, Customer>("update", new Customer
                    {
                        Id = customerToUpdate.Id,
                        FirstName = customerToUpdate.FirstName,
                        LastName = customerToUpdate.LastName,
                        Email = customerToUpdate.Email,
                        Address = customerToUpdate.Address
                    }));

                    customerToUpdate.FirstName = action.Item2.FirstName;
                    customerToUpdate.LastName = action.Item2.LastName;
                    customerToUpdate.Email = action.Item2.Email;
                    customerToUpdate.Address = action.Item2.Address;
                }
            }

            WriteDataToFile();
        }
        else
        {
            Console.WriteLine("No action to undo.");
        }
    }
    
    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            var action = redoStack.Pop();

            if (action.Item1 == "add")
            {
                customers.Add(action.Item2);
                emails.Add(action.Item2.Email);
                undoStack.Push(new Tuple<string, Customer>("add", action.Item2));
            }
            else if (action.Item1 == "delete")
            {
                var customerToRemove = customers.FirstOrDefault(c => c.Id == action.Item2.Id);
                if (customerToRemove != null)
                {
                    customers.Remove(customerToRemove);
                    emails.Remove(customerToRemove.Email);
                    undoStack.Push(new Tuple<string, Customer>("delete", customerToRemove));
                }
            }
            else if (action.Item1 == "update")
            {
                var customerToUpdate = customers.FirstOrDefault(c => c.Id == action.Item2.Id);
                if (customerToUpdate != null)
                {
                    undoStack.Push(new Tuple<string, Customer>("update", new Customer
                    {
                        Id = customerToUpdate.Id,
                        FirstName = customerToUpdate.FirstName,
                        LastName = customerToUpdate.LastName,
                        Email = customerToUpdate.Email,
                        Address = customerToUpdate.Address
                    }));

                    customerToUpdate.FirstName = action.Item2.FirstName;
                    customerToUpdate.LastName = action.Item2.LastName;
                    customerToUpdate.Email = action.Item2.Email;
                    customerToUpdate.Address = action.Item2.Address;
                }
            }

            WriteDataToFile();
        }
        else
        {
            Console.WriteLine("No action to redo.");
        }
    }

    public void SearchCustomerById(int customerId)
{
    var customer = customers.FirstOrDefault(c => c.Id == customerId);
    Console.WriteLine(customer);
    if (customer != null)
    {
        Console.WriteLine($"Customer ID: {customer.Id}");
        Console.WriteLine($"Name: {customer.FirstName} {customer.LastName}");
        Console.WriteLine($"Email: {customer.Email}");
        Console.WriteLine($"Address: {customer.Address}");
    }
    else
    {
        Console.WriteLine("Customer not found.");
    }
}

    
    private void ReadDataFromFile()
    {
         if (File.Exists(DatabaseFilePath))
        {
            using (var reader = new StreamReader(DatabaseFilePath))
            {
                using (var csvReader = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    customers = csvReader.GetRecords<Customer>().ToList();
                    emails = new HashSet<string>(customers.Select(c => c.Email));
                }
            }
        }
    }
    private void WriteDataToFile()
    {
         using (var writer = new StreamWriter(DatabaseFilePath))
        {
            using (var csvWriter = new CsvHelper.CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(customers);
            }
        }
    } */

    private List<Customer> customers;
    private Stack<CustomerAction> actions;
    private Stack<CustomerAction> redoActions;

    public CustomerDatabase()
    {
        customers = new List<Customer>();
        actions = new Stack<CustomerAction>();
        redoActions = new Stack<CustomerAction>();
    }

    public List<Customer> GetCustomers()
    {
        return customers;
    }
    public void AddCustomer(Customer customer)
    {
        if (customers.Any(c => c.Email == customer.Email))
        {
            throw new Exception("Email already exists.");
        }

        customers.Add(customer);
        actions.Push(new CustomerAction(ActionType.Add, customer));
        redoActions.Clear();
    }

    public void DeleteCustomer(int customerId)
    {
        Customer? customer = customers.FirstOrDefault(c => c.Id == customerId);
        if (customer != null)
        {
            customers.Remove(customer);
            actions.Push(new CustomerAction(ActionType.Delete, customer));
            redoActions.Clear();
        }
    }

    public void UpdateCustomer(int customerId, Customer newCustomer)
    {
        Customer? customer = customers.FirstOrDefault(c => c.Id == customerId);
        if (customer != null)
        {
            if (customers.Any(c => c.Email == newCustomer.Email && c.Id != customerId))
            {
                throw new Exception("Email already exists.");
            }

            CustomerAction action = new CustomerAction(ActionType.Update, customer);
            action.PreviousCustomer = new Customer
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Address = customer.Address
            };

            customer.FirstName = newCustomer.FirstName;
            customer.LastName = newCustomer.LastName;
            customer.Email = newCustomer.Email;
            customer.Address = newCustomer.Address;

            action.NewCustomer = new Customer
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Address = customer.Address
            };

            actions.Push(action);
            redoActions.Clear();
        }
    }

    public Customer? GetCustomerById(int customerId)
    {
        return customers.FirstOrDefault(c => c.Id == customerId);
    }

    public Customer? GetCustomerByEmail(string email)
    {
        return customers.FirstOrDefault(c => c.Email == email);
    }

    public void Undo()
    {
        if (actions.Count > 0)
        {
            CustomerAction action = actions.Pop();
            redoActions.Push(action);

            switch (action.Type)
            {
                case ActionType.Add:
                    customers.Remove(action.Customer);
                    break;
                case ActionType.Delete:
                    customers.Add(action.Customer);
                    break;
                case ActionType.Update:
                    Customer? customer = customers.FirstOrDefault(c => c.Id == action.Customer.Id);
                    if (customer != null)
                    {
                        customer.FirstName = action.PreviousCustomer.FirstName;
                        customer.LastName = action.PreviousCustomer.LastName;
                        customer.Email = action.PreviousCustomer.Email;
                        customer.Address = action.PreviousCustomer.Address;
                    }
                    break;
            }
        }
    }

    public void Redo()
    {
        if (redoActions.Count > 0)
        {
            CustomerAction action = redoActions.Pop();
            actions.Push(action);

            switch (action.Type)
            {
                case ActionType.Add:
                    customers.Add(action.Customer);
                    break;
                case ActionType.Delete:
                    customers.Remove(action.Customer);
                    break;
                case ActionType.Update:
                    Customer? customer = customers.FirstOrDefault(c => c.Id == action.Customer.Id);
                    if (customer != null)
                    {
                        customer.FirstName = action.NewCustomer.FirstName;
                        customer.LastName = action.NewCustomer.LastName;
                        customer.Email = action.NewCustomer.Email;
                        customer.Address = action.NewCustomer.Address;
                    }
                    break;
            }
        }
    }

    private enum ActionType
    {
        Add,
        Delete,
        Update
    }

    private class CustomerAction
    {
        public ActionType Type { get; }
        public Customer Customer { get; }
        public Customer? PreviousCustomer { get; set; }
        public Customer? NewCustomer { get; set; }

        public CustomerAction(ActionType type, Customer customer)
        {
            Type = type;
            Customer = customer;
        }
    }
}