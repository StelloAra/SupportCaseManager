using SupportCaseManager.Application.Contracts;
using SupportCaseManager.Domain;
using SupportCaseManager.Domain.Enums;

namespace SupportCaseManager.Application.Services;
public class TicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ICustomerRepository _customerRepository;

    public TicketService(ITicketRepository ticketRepository, ICustomerRepository customerRepository)
    {
        _ticketRepository = ticketRepository;
        _customerRepository = customerRepository;
    }
    public IReadOnlyList<SupportTicket> GetAllTickets()
    {
        return _ticketRepository.GetAll();
    }
    public SupportTicket? GetTicket(Guid id)
    {
        return _ticketRepository.GetById(id);
    }
    public SupportTicket CreateTicket(
    string title,
    string description,
    Guid customerId,
    TicketPriority priority)
    {
        var customer = _customerRepository.GetById(customerId);

        if (customer == null)
            throw new InvalidOperationException("Customer does not exist.");

        var ticket = new SupportTicket(
            title,
            description,
            customerId,
            priority);

        _ticketRepository.Add(ticket);

        return ticket;
    }
    public void UpdateTicketDetails(
    Guid ticketId,
    string title,
    string description)
    {
        var ticket = _ticketRepository.GetById(ticketId);

        if (ticket == null)
            throw new InvalidOperationException("Ticket does not exist.");

        ticket.UpdateDetails(title, description);

        _ticketRepository.Update(ticket);
    }
    public void ChangeTicketPriority(
    Guid ticketId,
    TicketPriority priority)
    {
        var ticket = _ticketRepository.GetById(ticketId);

        if (ticket == null)
            throw new InvalidOperationException("Ticket does not exist.");

        ticket.ChangePriority(priority);

        _ticketRepository.Update(ticket);
    }
    public void AssignTicketTechnician(
    Guid ticketId,
    Guid technicianId)
    {
        var ticket = _ticketRepository.GetById(ticketId);

        if (ticket == null)
            throw new InvalidOperationException("Ticket does not exist.");

        ticket.AssignTechnician(technicianId);

        _ticketRepository.Update(ticket);
    }
    public void ChangeTicketStatus(
    Guid ticketId,
    TicketStatus status)
    {
        var ticket = _ticketRepository.GetById(ticketId);

        if (ticket == null)
            throw new InvalidOperationException("Ticket does not exist.");

        ticket.ChangeStatus(status);

        _ticketRepository.Update(ticket);
    }
    public void AddTicketComment(
    Guid ticketId,
    string text)
    {
        var ticket = _ticketRepository.GetById(ticketId);

        if (ticket == null)
            throw new InvalidOperationException("Ticket does not exist.");

        ticket.AddComment(text);

        _ticketRepository.Update(ticket);
    }

}
