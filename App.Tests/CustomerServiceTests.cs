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
            _customerService = new CustomerService(_customerBuilder, _customerRepository);

            _customerBuilder.Add(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>()).Returns(_customerBuilder);
            _customerBuilder.AddCompany(Arg.Any<int>()).Returns(_customerBuilder);
            _customerBuilder.AddCreditLimit().Returns(_customerBuilder);
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
        public void AddCustomer_ShouldCallCustomerBuilderAddCompany()
        {
            // When
            _customerService.AddCustomer("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27), 4);

            // Then
            _customerBuilder.Received(1).AddCompany(4);
        }

        [Test]
        public void AddCustomer_ShouldCallCustomerBuilderAddCreditLimity()
        {
            // When
            _customerService.AddCustomer("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27), 4);

            // Then
            _customerBuilder.Received(1).AddCreditLimit();
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
