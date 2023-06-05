using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class CustomerDatabase
{
    private List<Customer> customers;
    private HashSet<string> emails;
    private Stack<Tuple<string, Customer>> undoStack;
    private Stack<Tuple<string, Customer>> redoStack;
    private const string DatabaseFilePath = "customer.csv";

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
    }

}