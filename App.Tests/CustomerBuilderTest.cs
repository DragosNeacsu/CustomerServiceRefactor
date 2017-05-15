using App.ModelValidator;
using App.Repositories;
using NSubstitute;
using NUnit.Framework;
using System;

namespace App.Tests
{
    public class CustomerBuilderTest
    {
        private ICustomerBuilder _customerBuilder;
        private ICustomerValidator _customerValidator;
        private ICompanyRepository _companyRepository;
        private ICustomerCreditService _customerCreditService;

        [SetUp]
        public void SetUp()
        {
            _customerBuilder = new CustomerBuilder();
            _customerValidator = Substitute.For<ICustomerValidator>();
            _companyRepository = Substitute.For<ICompanyRepository>();
            _customerCreditService = Substitute.For<ICustomerCreditService>();

            _companyRepository.GetById(Arg.Any<int>()).Returns(x => new Company { Id = x.ArgAt<int>(0) });
            _customerCreditService.GetCreditLimit(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>()).Returns(500);
        }

        [Test]
        public void Add_ShouldCallValidator()
        {
            // When
            _customerBuilder.Add("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27), _customerValidator);

            // Then
            _customerValidator.Received(1).Validate(Arg.Any<Customer>());
        }

        [Test]
        public void Add_ValidCustomer_ShouldCreateCustomer()
        {
            // Given
            _customerBuilder.Add("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27), _customerValidator);

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
        public void AddCompany_ValidCustomer_ShouldCallGetCompany()
        {
            // Given
            var companyId = 4;

            // When
            _customerBuilder.AddCompany(companyId, _companyRepository);
            var result = _customerBuilder.Build();

            // Then
            _companyRepository.Received(1).GetById(companyId);
            Assert.NotNull(result);
            Assert.AreEqual(companyId, result.Company.Id);
        }

        [Test]
        public void AddCreditLimit_VeryImportantClient_ShouldSetNoCreditLimit()
        {
            // Given
            _companyRepository.GetById(Arg.Any<int>()).Returns(new Company { Name = "VeryImportantClient" });

            // When
            _customerBuilder.AddCompany(4, _companyRepository);
            _customerBuilder.AddCreditLimit(_customerCreditService);
            var result = _customerBuilder.Build();

            // Then
            _customerCreditService.Received(0).GetCreditLimit(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>());
            Assert.NotNull(result);
            Assert.False(result.HasCreditLimit);
            Assert.AreEqual(0, result.CreditLimit);
        }

        [Test]
        public void AddCreditLimit_ImportantClient_ShouldSetCreditLimit()
        {
            // Given
            _companyRepository.GetById(Arg.Any<int>()).Returns(new Company { Name = "ImportantClient" });

            // When
            _customerBuilder.AddCompany(4, _companyRepository);
            _customerBuilder.AddCreditLimit(_customerCreditService);
            var result = _customerBuilder.Build();

            // Then
            _customerCreditService.Received(1).GetCreditLimit(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>());
            Assert.NotNull(result);
            Assert.True(result.HasCreditLimit);
            Assert.AreEqual(2 * 500, result.CreditLimit);
        }

        [Test]
        public void AddCreditLimit_DefaultClient_ShouldSetCreditLimit()
        {
            // When
            _customerBuilder.AddCompany(4, _companyRepository);
            _customerBuilder.AddCreditLimit(_customerCreditService);
            var result = _customerBuilder.Build();

            // Then
            _customerCreditService.Received(1).GetCreditLimit(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>());
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
            _customerBuilder.AddCompany(4, _companyRepository);
            _customerCreditService.GetCreditLimit(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>()).Returns(creditLimit);

            // When Then
            var ex = Assert.Throws<ArgumentException>(
                () => _customerBuilder.AddCreditLimit(_customerCreditService));

            Assert.AreEqual("Invalid Credit Limit", ex.Message);
        }
    }
}
