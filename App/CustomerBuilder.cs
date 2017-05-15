using App.ModelValidator;
using System;

namespace App
{
    public class CustomerBuilder : ICustomerBuilder
    {
        private static Customer _customer;
        private ICustomerValidator _customerValidator;

        public CustomerBuilder(ICustomerValidator customerValidator)
        {
            _customer = new Customer();
            _customerValidator = customerValidator;
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

        public ICustomerBuilder AddCompany(Company company)
        {
            _customer.Company = company;
            return this;
        }

        public ICustomerBuilder AddCreditLimit(int creditLimit)
        {
            _customer.HasCreditLimit = true;
            switch (_customer.Company.Name)
            {
                case "VeryImportantClient":
                    _customer.HasCreditLimit = false;
                    break;
                case "ImportantClient":
                    _customer.CreditLimit = 2 * creditLimit;
                    break;
                default:
                    _customer.CreditLimit = creditLimit;
                    break;
            }

            if (_customer.HasCreditLimit && _customer.CreditLimit < 500)
            {
                throw new ArgumentException("Invalid Credit Limit");
            }
            return this;
        }

        public Customer Build()
        {
            return _customer;
        }
    }
}
