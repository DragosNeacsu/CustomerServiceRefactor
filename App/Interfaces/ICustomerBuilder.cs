using App.ModelValidator;
using App.Repositories;
using System;

namespace App
{
    public interface ICustomerBuilder
    {
        ICustomerBuilder Add(string firstName, string surname, string email, DateTime dateOfBirth);
        ICustomerBuilder AddCompany(Company company);
        ICustomerBuilder AddCreditLimit(int creditLimit);
        Customer Build();
    }
}
