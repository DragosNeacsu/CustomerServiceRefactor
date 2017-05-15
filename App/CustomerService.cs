using App.ModelValidator;
using App.Repositories;
using System;

namespace App
{
    public class CustomerService : ICustomerService
    {
        private ICustomerBuilder _customerBuilder;
        private ICompanyRepository _companyRepository;
        private ICustomerRepository _customerRepository;
        private ICustomerCreditService _customerCreditService;
        private ICustomerValidator _customerValidator;
        
        public CustomerService()
        {
            _companyRepository = new CompanyRepository();
            _customerRepository = new CustomerRepository();
            _customerCreditService = new CustomerCreditServiceClient();
            _customerValidator = new CustomerValidator();
        }

        public CustomerService(ICustomerBuilder customerBuilder,
            ICompanyRepository companyRepository,
            ICustomerRepository customerRepository,
            ICustomerCreditService customerCreditService,
            ICustomerValidator customerValidator)
        {
            _customerBuilder = customerBuilder;
            _companyRepository = companyRepository;
            _customerRepository = customerRepository;
            _customerCreditService = customerCreditService;
            _customerValidator = customerValidator;
        }
        public bool AddCustomer(string firname, string surname, string email, DateTime dateOfBirth, int companyId)
        {
            try
            {
                var customer = _customerBuilder
                        .Add(firname, surname, email, dateOfBirth, _customerValidator)
                        .AddCompany(companyId, _companyRepository)
                        .AddCreditLimit(_customerCreditService)
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
