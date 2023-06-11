
public class FileHelper
{
    public static List<Customer> ReadCustomersFromFile(string filePath)
    {
        List<Customer> customers = new List<Customer>();
        using (StreamReader reader = new StreamReader(filePath))
        {
            reader.ReadLine(); // Skip header line
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] data = line.Split(',');
                int id = int.Parse(data[0]);
                string firstName = data[1];
                string lastName = data[2];
                string email = data[3];
                string address = data[4];
                Customer customer = new Customer
                {
                    Id = id,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Address = address
                };
                customers.Add(customer);
            }
        }
        return customers;
    }

    public static void WriteCustomersToFile(List<Customer> customers, string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Id,FirstName,LastName,Email,Address");
            foreach (Customer customer in customers)
            {
                writer.WriteLine($"{customer.Id},{customer.FirstName},{customer.LastName},{customer.Email},{customer.Address}");
            }
        }
    }
}