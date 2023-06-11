using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class CustomerDatabase
{
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
        public ActionType? Type { get; }
        public Customer? Customer { get; }
        public Customer? PreviousCustomer { get; set; }
        public Customer? NewCustomer { get; set; }

        public CustomerAction(ActionType type, Customer customer)
        {
            Type = type;
            Customer = customer;
        }
    }
}