using System;

namespace App
{
    public interface ICustomerService
    {
        bool AddCustomer(string firname, string surname, string email, DateTime dateOfBirth, int companyId);
    }
}
