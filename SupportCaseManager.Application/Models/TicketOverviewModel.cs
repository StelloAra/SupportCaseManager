using SupportCaseManager.Domain.Enums;

namespace SupportCaseManager.Application.Models;
public class TicketOverviewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string CustomerName { get; set; }
    public TicketPriority Priority { get; set; }
    public TicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
