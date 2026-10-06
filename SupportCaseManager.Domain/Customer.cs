using SupportCaseManager.Domain.ValueObjects;

namespace SupportCaseManager.Domain;


public class Customer
{
    public Guid Id { get; private set; }
    public Name Name { get; private set; }
    public Email Email { get; private set; }

    public Customer(Name name, Email email)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
    }

    public static Customer Rehydrate(Guid id, Name name, Email email)
    {
        var customer = new Customer(name, email);
        customer.Id = id;

        return customer;
    }

    public void UpdateContactInfo(Name name, Email email)
    {
        Name = name;
        Email = email;
    }
}