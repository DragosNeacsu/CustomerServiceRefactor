using App.Repositories;
using NSubstitute;
using NUnit.Framework;
using System;

namespace App.Tests
{
    public class CustomerServiceTests
    {
        private ICustomerBuilder _customerBuilder;
        private ICustomerService _customerService;
        private ICustomerRepository _customerRepository;
        private ICompanyRepository _companyRepository;
        private ICustomerCreditService _customerCreditService;

        [SetUp]
        public void Setup()
        {
            _customerBuilder = Substitute.For<ICustomerBuilder>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _companyRepository = Substitute.For<ICompanyRepository>();
            _customerCreditService = Substitute.For<ICustomerCreditService>();
            _customerService = new CustomerService(_customerBuilder, _customerRepository, _companyRepository, _customerCreditService);

            _customerBuilder.Add(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>()).Returns(_customerBuilder);
            _customerBuilder.AddCompany(Arg.Any<Company>()).Returns(_customerBuilder);
            _customerBuilder.AddCreditLimit(Arg.Any<int>()).Returns(_customerBuilder);
        }

        [Test]
        public void AddCustomer_ShouldCallCustomerBuilderAdd()
        {
            // When
            _customerService.AddCustomer("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27), 4);

            // Then
            _customerBuilder.Received(1).Add("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27));
        }

        [Test]
        public void AddCustomer_ShouldCallGetCompanyById()
        {
            // When
            _customerService.AddCustomer("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27), 4);

            // Then
            _companyRepository.Received(1).GetById(4);
        }

        [Test]
        public void AddCustomer_ShouldCallCustomerBuilderAddCompany()
        {
            // When
            _customerService.AddCustomer("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27), 4);

            // Then
            _customerBuilder.Received(1).AddCompany(Arg.Any<Company>());
        }

        [Test]
        public void AddCustomer_ShouldCallGetCreditLimit()
        {
            // When
            _customerService.AddCustomer("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27), 4);

            // Then
            _customerCreditService.Received(1).GetCreditLimit(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>());
        }

        [Test]
        public void AddCustomer_ShouldCallCustomerBuilderAddCreditLimit()
        {
            // Given
            _customerCreditService.GetCreditLimit(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>()).Returns(100);

            // When
            _customerService.AddCustomer("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27), 4);

            // Then
            _customerBuilder.Received(1).AddCreditLimit(100);
        }

        [Test]
        public void AddCustomer_ShouldCallCustomerBuilderBuild()
        {
            // When
            _customerService.AddCustomer("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27), 4);

            // Then
            _customerBuilder.Received(1).Build();
        }

        [Test]
        public void AddCustomer_ShouldCallCustomerRepositoryAdd()
        {
            // When
            _customerService.AddCustomer("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27), 4);

            // Then
            _customerRepository.Received(1).Add(Arg.Any<Customer>());
        }
    }
}
