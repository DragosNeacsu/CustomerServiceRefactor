using App.Repositories;
using System;

namespace App
{
    public class CustomerService : ICustomerService
    {
        private ICustomerBuilder _customerBuilder;
        private ICustomerRepository _customerRepository;

        public CustomerService()
        {
            _customerRepository = new CustomerRepository();
        }

        public CustomerService(ICustomerBuilder customerBuilder, ICustomerRepository customerRepository)
        {
            _customerBuilder = customerBuilder;
            _customerRepository = customerRepository;
        }
        public bool AddCustomer(string firname, string surname, string email, DateTime dateOfBirth, int companyId)
        {
            try
            {
                var customer = _customerBuilder
                        .Add(firname, surname, email, dateOfBirth)
                        .AddCompany(companyId)
                        .AddCreditLimit()
                        .Build();

                _customerRepository.Add(customer);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
