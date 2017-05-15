using App.ModelValidator;
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

        [SetUp]
        public void Setup()
        {
            _customerBuilder = Substitute.For<ICustomerBuilder>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _customerService = new CustomerService(
                _customerBuilder,
                Substitute.For<ICompanyRepository>(),
                _customerRepository,
                Substitute.For<ICustomerCreditService>(),
                Substitute.For<ICustomerValidator>());

            _customerBuilder.Add(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<ICustomerValidator>()).Returns(_customerBuilder);
            _customerBuilder.AddCompany(Arg.Any<int>(), Arg.Any<ICompanyRepository>()).Returns(_customerBuilder);
            _customerBuilder.AddCreditLimit(Arg.Any<ICustomerCreditService>()).Returns(_customerBuilder);
        }

        [Test]
        public void AddCustomer_ShouldCallCustomerBuilderAdd()
        {
            // When
            _customerService.AddCustomer("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27), 4);

            // Then
            _customerBuilder.Received(1).Add("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27), Arg.Any<ICustomerValidator>());
        }

        [Test]
        public void AddCustomer_ShouldCallCustomerBuilderAddCompany()
        {
            // When
            _customerService.AddCustomer("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27), 4);

            // Then
            _customerBuilder.Received(1).AddCompany(4, Arg.Any<ICompanyRepository>());
        }

        [Test]
        public void AddCustomer_ShouldCallCustomerBuilderAddCreditLimity()
        {
            // When
            _customerService.AddCustomer("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27), 4);

            // Then
            _customerBuilder.Received(1).AddCreditLimit(Arg.Any<ICustomerCreditService>());
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
