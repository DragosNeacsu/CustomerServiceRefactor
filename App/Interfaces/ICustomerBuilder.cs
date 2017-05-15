using App.ModelValidator;
using App.Repositories;
using System;

namespace App
{
    public interface ICustomerBuilder
    {
        ICustomerBuilder Add(string firstName, string surname, string email, DateTime dateOfBirth, ICustomerValidator customerValidator);
        ICustomerBuilder AddCompany(int companyId, ICompanyRepository companyRepository);
        ICustomerBuilder AddCreditLimit(ICustomerCreditService service);
        Customer Build();
    }
}
