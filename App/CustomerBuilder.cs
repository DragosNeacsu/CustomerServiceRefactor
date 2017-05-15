using App.ModelValidator;
using App.Repositories;
using System;

namespace App
{
    public class CustomerBuilder : ICustomerBuilder
    {
        private static Customer _customer;
        public CustomerBuilder()
        {
            _customer = new Customer();
        }
        public ICustomerBuilder Add(string firstName, string surname, string email, DateTime dateOfBirth, ICustomerValidator customerValidator)
        {
            _customer.DateOfBirth = dateOfBirth;
            _customer.EmailAddress = email;
            _customer.Firstname = firstName;
            _customer.Surname = surname;

            customerValidator.Validate(_customer);
            return this;
        }

        public ICustomerBuilder AddCompany(int companyId, ICompanyRepository companyRepository)
        {
            if (companyRepository == null)
                throw new ArgumentException("Repository cannot be null");

            _customer.Company = companyRepository.GetById(companyId);
            return this;
        }

        public ICustomerBuilder AddCreditLimit(ICustomerCreditService creditService)
        {
            if (creditService == null)
                throw new ArgumentException("Service cannot be null");

            _customer.HasCreditLimit = true;
            switch (_customer.Company.Name)
            {
                case "VeryImportantClient":
                    _customer.HasCreditLimit = false;
                    break;
                case "ImportantClient":
                    _customer.CreditLimit = 2 * GetCreditLimit(creditService);
                    break;
                default:
                    _customer.CreditLimit = GetCreditLimit(creditService);
                    break;
            }

            if (_customer.HasCreditLimit && _customer.CreditLimit < 500)
            {
                throw new ArgumentException("Invalid Credit Limit");
            }
            return this;
        }

        private int GetCreditLimit(ICustomerCreditService creditService)
        {
            return creditService.GetCreditLimit(_customer.Firstname, _customer.Surname, _customer.DateOfBirth);
        }
        public Customer Build()
        {
            return _customer;
        }
    }
}
