using SupportCaseManager.Domain.Enums;

namespace SupportCaseManager.Domain;

public class SupportTicket
{
    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public Guid CustomerId { get; private set; }

    public TicketPriority Priority { get; private set; }

    public TicketStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public Guid? TechnicianId { get; private set; }

    public IReadOnlyList<Comment> Comments => _comments;

    private readonly List<Comment> _comments = new();

    public SupportTicket(
        string title,
        string description,
        Guid customerId,
        TicketPriority priority)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.");

        if (customerId == Guid.Empty)
            throw new ArgumentException("Customer must be specified.");

        Id = Guid.NewGuid();
        Title = title.Trim();
        Description = description.Trim();
        CustomerId = customerId;
        Priority = priority;
        Status = TicketStatus.New;
        CreatedAt = DateTime.Now;
    }

    public void AssignTechnician(Guid technicianId)
    {
        if (technicianId == Guid.Empty)
            throw new ArgumentException("Technician must be specified.");

        TechnicianId = technicianId;
    }

    public void ChangeStatus(TicketStatus status)
    {
        if (status != TicketStatus.New && TechnicianId == null)
            throw new InvalidOperationException(
                "A technician is required for this status.");

        Status = status;
    }

    public void AddComment(string text)
    {
        var comment = new Comment(text);
        _comments.Add(comment);
    }

    public void UpdateDetails(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.");

        Title = title.Trim();
        Description = description.Trim();
    }

    public void ChangePriority(TicketPriority priority)
    {
        Priority = priority;
    }
}