using SupportCaseManager.Application.Contracts;
using SupportCaseManager.Domain;
using SupportCaseManager.Infrastructure.Data.FileModels;
using System.Text.Json;
using SupportCaseManager.Domain.ValueObjects;

namespace SupportCaseManager.Infrastructure.Repositories;

public class JsonCustomerRepository : ICustomerRepository
{
    private readonly string _filePath;

    public JsonCustomerRepository()
    {
        _filePath = Path.Combine(
            AppContext.BaseDirectory,
            "Data",
            "customers.json");
    }
    public void Add(Customer customer)
    {
        var customers = GetAll();

        var fileModels = customers
            .Select(existingCustomer => new CustomerFileModel
            {
                Id = existingCustomer.Id,
                Name = existingCustomer.Name.Value,
                Email = existingCustomer.Email.Value
            })
            .ToList();

        fileModels.Add(new CustomerFileModel
        {
            Id = customer.Id,
            Name = customer.Name.Value,
            Email = customer.Email.Value
        });

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(fileModels, options);

        File.WriteAllText(_filePath, json);

    }

    public IReadOnlyList<Customer> GetAll()
    {
        if (!File.Exists(_filePath))
            return new List<Customer>();

        var json = File.ReadAllText(_filePath);

        List<CustomerFileModel>? fileModels;

        try
        {
            fileModels = JsonSerializer.Deserialize<List<CustomerFileModel>>(json);
        }
        catch (JsonException)
        {
            return new List<Customer>();
        }

        if (fileModels == null)
            return new List<Customer>();

        return fileModels
            .Select(fileModel =>
                Customer.Rehydrate(
                    fileModel.Id,
                    new Name(fileModel.Name),
                    new Email(fileModel.Email)))
            .ToList();
    }

    public Customer? GetById(Guid id)
    {
        return GetAll()
            .FirstOrDefault(customer => customer.Id == id);
    }
}
