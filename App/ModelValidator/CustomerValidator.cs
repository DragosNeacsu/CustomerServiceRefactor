using System;

namespace App.ModelValidator
{
    public class CustomerValidator : ICustomerValidator
    {
        public bool Validate(Customer customer)
        {
            if (string.IsNullOrEmpty(customer.Firstname))
            {
                throw new ArgumentException("Firstname must be specified");
            }
            if (string.IsNullOrEmpty(customer.Surname))
            {
                throw new ArgumentException("Surname must be specified");
            }
            if (string.IsNullOrEmpty(customer.EmailAddress) ||
                !customer.EmailAddress.Contains("@") ||
                !customer.EmailAddress.Contains("."))
            {
                throw new ArgumentException("EmailAddress is not valid");
            }
            if (!IsValidAge(customer.DateOfBirth))
            {
                throw new ArgumentException("DateOfBirth is not valid");
            }
            return true;
        }

        private bool IsValidAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            var age = now.Year - dateOfBirth.Year;

            if (now.Month < dateOfBirth.Month ||
                (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            {
                age--;
            }

            return age >= 21;
        }
    }
}
