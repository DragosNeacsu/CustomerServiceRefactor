using App.ModelValidator;
using NSubstitute;
using NUnit.Framework;
using System;

namespace App.Tests
{
    public class CustomerBuilderTest
    {
        private ICustomerBuilder _customerBuilder;
        private ICustomerValidator _customerValidator;

        [SetUp]
        public void SetUp()
        {
            _customerValidator = Substitute.For<ICustomerValidator>();
            _customerBuilder = new CustomerBuilder(_customerValidator);
        }

        [Test]
        public void Add_ShouldCallValidator()
        {
            // When
            _customerBuilder.Add("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27));

            // Then
            _customerValidator.Received(1).Validate(Arg.Any<Customer>());
        }

        [Test]
        public void Add_ValidCustomer_ShouldCreateCustomer()
        {
            // Given
            _customerBuilder.Add("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27));

            // When
            var result = _customerBuilder.Build();

            // Then
            Assert.NotNull(result);
            Assert.AreEqual("Joe", result.Firstname);
            Assert.AreEqual("Bloggs", result.Surname);
            Assert.AreEqual("joe.bloggs@adomain.com", result.EmailAddress);
            Assert.AreEqual(new DateTime(1980, 3, 27), result.DateOfBirth);
        }

        [Test]
        public void AddCompany_ValidCustomer_ShouldSetCompany()
        {
            // Given
            var company = new Company { Id = 4 };

            // When
            _customerBuilder.AddCompany(company);
            var result = _customerBuilder.Build();

            // Then
            Assert.NotNull(result);
            Assert.AreEqual(company, result.Company);
        }

        [Test]
        public void AddCreditLimit_VeryImportantClient_ShouldSetNoCreditLimit()
        {
            // Given
            var company = new Company { Name = "VeryImportantClient" };

            // When
            _customerBuilder.AddCompany(company);
            _customerBuilder.AddCreditLimit(10);
            var result = _customerBuilder.Build();

            // Then
            Assert.NotNull(result);
            Assert.False(result.HasCreditLimit);
            Assert.AreEqual(0, result.CreditLimit);
        }

        [Test]
        public void AddCreditLimit_ImportantClient_ShouldSetCreditLimit()
        {
            // Given
            var company = new Company { Name = "ImportantClient" };

            // When
            _customerBuilder.AddCompany(company);
            _customerBuilder.AddCreditLimit(500);
            var result = _customerBuilder.Build();

            // Then
            Assert.NotNull(result);
            Assert.True(result.HasCreditLimit);
            Assert.AreEqual(2 * 500, result.CreditLimit);
        }

        [Test]
        public void AddCreditLimit_DefaultClient_ShouldSetCreditLimit()
        {
            // When
            _customerBuilder.AddCompany(new Company());
            _customerBuilder.AddCreditLimit(500);
            var result = _customerBuilder.Build();

            // Then
            Assert.NotNull(result);
            Assert.True(result.HasCreditLimit);
            Assert.AreEqual(500, result.CreditLimit);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(499)]
        public void AddCreditLimit_InvalidCreditLimit_ShouldReturnthrowException(int creditLimit)
        {
            // Given
            _customerBuilder.AddCompany(new Company());

            // When Then
            var ex = Assert.Throws<ArgumentException>(
                () => _customerBuilder.AddCreditLimit(creditLimit));

            Assert.AreEqual("Invalid Credit Limit", ex.Message);
        }
    }
}
