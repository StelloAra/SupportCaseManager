using SupportCaseManager.Domain;

namespace SupportCaseManager.Application.Contracts;

public interface ICustomerRepository
{
    IReadOnlyList<Customer> GetAll();

    Customer? GetById(Guid id);

    void Add(Customer customer);

}
