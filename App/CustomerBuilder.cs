using App.ModelValidator;
using App.Repositories;
using System;

namespace App
{
    public class CustomerBuilder : ICustomerBuilder
    {
        private static Customer _customer;
        private ICustomerValidator _customerValidator;
        private ICustomerCreditService _customerCreditService;
        private ICompanyRepository _companyRepository;

        public CustomerBuilder(ICustomerValidator customerValidator, ICustomerCreditService customerCreditService, ICompanyRepository companyRepository)
        {
            _customer = new Customer();
            _customerValidator = customerValidator;
            _customerCreditService = customerCreditService;
            _companyRepository = companyRepository;
        }
        public ICustomerBuilder Add(string firstName, string surname, string email, DateTime dateOfBirth)
        {
            _customer.DateOfBirth = dateOfBirth;
            _customer.EmailAddress = email;
            _customer.Firstname = firstName;
            _customer.Surname = surname;

            _customerValidator.Validate(_customer);
            return this;
        }

        public ICustomerBuilder AddCompany(int companyId)
        {
            _customer.Company = _companyRepository.GetById(companyId);
            return this;
        }

        public ICustomerBuilder AddCreditLimit()
        {
            _customer.HasCreditLimit = true;
            switch (_customer.Company.Name)
            {
                case "VeryImportantClient":
                    _customer.HasCreditLimit = false;
                    break;
                case "ImportantClient":
                    _customer.CreditLimit = 2 * GetCreditLimit();
                    break;
                default:
                    _customer.CreditLimit = GetCreditLimit();
                    break;
            }

            if (_customer.HasCreditLimit && _customer.CreditLimit < 500)
            {
                throw new ArgumentException("Invalid Credit Limit");
            }
            return this;
        }

        private int GetCreditLimit()
        {
            return _customerCreditService.GetCreditLimit(_customer.Firstname, _customer.Surname, _customer.DateOfBirth);
        }
        public Customer Build()
        {
            return _customer;
        }
    }
}
