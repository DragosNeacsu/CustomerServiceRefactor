namespace App.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public void Add(Customer customer)
        {
            CustomerDataAccess.AddCustomer(customer);
        }
    }
}
