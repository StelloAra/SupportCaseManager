using SupportCaseManager.Application.Contracts;
using SupportCaseManager.Application.Models;
using SupportCaseManager.Domain;
using SupportCaseManager.Domain.Enums;

namespace SupportCaseManager.Application.Services;

public class TicketQueryService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ICustomerRepository _customerRepository;

    public TicketQueryService(
        ITicketRepository ticketRepository,
        ICustomerRepository customerRepository)
    {
        _ticketRepository = ticketRepository;
        _customerRepository = customerRepository;
    }
    public IReadOnlyList<TicketOverviewModel> GetTicketOverview()
    {
        var tickets = _ticketRepository.GetAll();

        var result = new List<TicketOverviewModel>();

        foreach (var ticket in tickets)
        {
            var customer = _customerRepository.GetById(ticket.CustomerId);

            if (customer == null)
                continue;

            result.Add(new TicketOverviewModel
            {
                Id = ticket.Id,
                Title = ticket.Title,
                CustomerName = customer.Name.Value,
                Priority = ticket.Priority,
                Status = ticket.Status,
                CreatedAt = ticket.CreatedAt
            });
        }

        return result
            .OrderByDescending(ticket => ticket.CreatedAt)
            .ToList();
    }
    public IReadOnlyList<TicketOverviewModel> SearchTickets(string searchTerm)
    {
        var tickets = GetTicketOverview();

        if (string.IsNullOrWhiteSpace(searchTerm))
            return tickets;

        searchTerm = searchTerm.Trim();

        return tickets
            .Where(ticket =>
                ticket.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                ticket.CustomerName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
    public IReadOnlyList<TicketOverviewModel> FilterTicketsByStatus(
TicketStatus status)
    {
        var tickets = GetTicketOverview();

        return tickets
            .Where(ticket => ticket.Status == status)
            .ToList();
    }
    public IReadOnlyList<TicketOverviewModel> SearchAndFilterTickets(
    string searchTerm,
    TicketStatus status)
    {
        var tickets = SearchTickets(searchTerm);

        return tickets
            .Where(ticket => ticket.Status == status)
            .ToList();
    }
}
