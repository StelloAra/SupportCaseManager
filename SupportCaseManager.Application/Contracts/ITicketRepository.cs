using SupportCaseManager.Domain;

namespace SupportCaseManager.Application.Contracts;

public interface ITicketRepository
{
    IReadOnlyList<SupportTicket> GetAll();
    SupportTicket? GetById(Guid id);
    void Add(SupportTicket ticket);
    void Update(SupportTicket ticket);
}
