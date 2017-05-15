using App.ModelValidator;
using NUnit.Framework;
using System;

namespace App.Tests
{
    public class CustomerValidatorTest
    {
        private ICustomerValidator _customerValidator;

        [SetUp]
        public void Setup()
        {
            _customerValidator = new CustomerValidator();
        }

        [Test]
        public void Validate_CustomerIsNull_ShouldReturnNegativeResult()
        {
            // Given
            var customer = new Customer();

            // When Then
            var ex = Assert.Throws<ArgumentException>(
                () => _customerValidator.Validate(customer));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void Validate_InvalidFirstName_ShouldReturnNegativeResultWithProperErrorMessage(string firstName)
        {
            // Given
            var customer = GetValidCustomer();
            customer.Firstname = firstName;

            // When Then
            var ex = Assert.Throws<ArgumentException>(
                () => _customerValidator.Validate(customer));

            Assert.AreEqual("Firstname must be specified", ex.Message);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void Validate_InvalidSurname_ShouldReturnNegativeResultWithProperErrorMessage(string surname)
        {
            // Given
            var customer = GetValidCustomer();
            customer.Surname = surname;

            // When Then
            var ex = Assert.Throws<ArgumentException>(
                () => _customerValidator.Validate(customer));

            Assert.AreEqual("Surname must be specified", ex.Message);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("inavlidemail")]
        [TestCase("inavlidemail.com")]
        [TestCase("inavlid@emailcom")]
        public void Validate_InvalidEmail_ShouldReturnNegativeResultWithProperErrorMessage(string email)
        {
            // Given
            var customer = GetValidCustomer();
            customer.EmailAddress = email;

            // When Then
            var ex = Assert.Throws<ArgumentException>(
                () => _customerValidator.Validate(customer));

            Assert.AreEqual("EmailAddress is not valid", ex.Message);
        }

        [Test]
        [TestCase("01/01/2017")]
        [TestCase("01/06/1996")]
        public void Validate_InvalidAge_ShouldReturnNegativeResultWithProperErrorMessage(string date)
        {
            // Given
            var customer = GetValidCustomer();
            customer.DateOfBirth = Convert.ToDateTime(date);

            // When Then
            var ex = Assert.Throws<ArgumentException>(
                () => _customerValidator.Validate(customer));

            Assert.AreEqual("DateOfBirth is not valid", ex.Message);
        }

        [Test]
        public void Validate_ValidCustomer_ShouldReturnPositiveResult()
        {
            // Given
            var customer = GetValidCustomer();

            // When
            var result = _customerValidator.Validate(customer);

            // Then
            Assert.True(result);
        }

        private Customer GetValidCustomer()
        {
            return new Customer
            {
                Firstname = Guid.NewGuid().ToString(),
                Surname = Guid.NewGuid().ToString(),
                EmailAddress = "valid@email.com",
                DateOfBirth = DateTime.Now.AddYears(-30)
            };
        }
    }
}
