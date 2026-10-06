using SupportCaseManager.Application.Contracts;
using SupportCaseManager.Domain;
using SupportCaseManager.Domain.ValueObjects;

namespace SupportCaseManager.Application.Services;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public IReadOnlyList<Customer> GetAllCustomers()
    {
        return _customerRepository.GetAll();
    }
    public Customer? GetCustomer(Guid id)
    {
        return _customerRepository.GetById(id);
    }
    public Customer CreateCustomer(string name, string email)
    {
        var customer = new Customer(
            new Name(name),
            new Email(email));

        _customerRepository.Add(customer);

        return customer;
    }

}
