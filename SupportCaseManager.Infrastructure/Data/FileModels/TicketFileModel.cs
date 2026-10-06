using SupportCaseManager.Domain.Enums;

namespace SupportCaseManager.Infrastructure.Data.FileModels;

public class TicketFileModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid CustomerId { get; set; }
    public TicketPriority Priority { get; set; }
    public TicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? TechnicianId { get; set; }
    public List<CommentFileModel> Comments { get; set; } = new();
}
