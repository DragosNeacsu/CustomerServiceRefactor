using App.Repositories;
using System;

namespace App
{
    public class CustomerService : ICustomerService
    {
        private ICustomerBuilder _customerBuilder;
        private ICustomerRepository _customerRepository;
        private ICompanyRepository _companyRepository;
        private ICustomerCreditService _customerCreditService;

        public CustomerService()
        {
            _customerRepository = new CustomerRepository();
            _companyRepository = new CompanyRepository();
            _customerCreditService = new CustomerCreditServiceClient();
        }

        public CustomerService(ICustomerBuilder customerBuilder,
            ICustomerRepository customerRepository,
            ICompanyRepository companyRepository,
            ICustomerCreditService customerCreditService)
        {
            _customerBuilder = customerBuilder;
            _customerRepository = customerRepository;
            _companyRepository = companyRepository;
            _customerCreditService = customerCreditService;
        }
        public bool AddCustomer(string firstname, string surname, string email, DateTime dateOfBirth, int companyId)
        {
            try
            {
                var customer = _customerBuilder
                        .Add(firstname, surname, email, dateOfBirth)
                        .AddCompany(_companyRepository.GetById(companyId))
                        .AddCreditLimit(_customerCreditService.GetCreditLimit(firstname, surname, dateOfBirth))
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
