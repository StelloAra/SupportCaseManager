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

    public void UpdateContactInfo(Name name, Email email)
    {
        Name = name;
        Email = email;
    }
}