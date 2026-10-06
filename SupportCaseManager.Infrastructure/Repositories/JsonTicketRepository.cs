using SupportCaseManager.Application.Contracts;
using SupportCaseManager.Domain;
using SupportCaseManager.Infrastructure.Data.FileModels;
using System.Text.Json;

namespace SupportCaseManager.Infrastructure.Repositories;

public class JsonTicketRepository : ITicketRepository
{
    private readonly string _filePath;

    public JsonTicketRepository()
    {
        _filePath = Path.Combine(
            AppContext.BaseDirectory,
            "Data",
            "tickets.json");
    }

    public void Add(SupportTicket ticket)
    {
        var tickets = GetAll();

        var fileModels = tickets
            .Select(existingTicket => new TicketFileModel
            {
                Id = existingTicket.Id,
                Title = existingTicket.Title,
                Description = existingTicket.Description,
                CustomerId = existingTicket.CustomerId,
                Priority = existingTicket.Priority,
                Status = existingTicket.Status,
                CreatedAt = existingTicket.CreatedAt,
                TechnicianId = existingTicket.TechnicianId
            })
            .ToList();

        var fileModel = new TicketFileModel
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            CustomerId = ticket.CustomerId,
            Priority = ticket.Priority,
            Status = ticket.Status,
            CreatedAt = ticket.CreatedAt,
            TechnicianId = ticket.TechnicianId
        };

        foreach (var comment in ticket.Comments)
        {
            fileModel.Comments.Add(new CommentFileModel
            {
                Text = comment.Text,
                CreatedAt = comment.CreatedAt
            });
        }

        fileModels.Add(fileModel);

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(fileModels, options);

        File.WriteAllText(_filePath, json);
    }

    public IReadOnlyList<SupportTicket> GetAll()
    {
        if (!File.Exists(_filePath))
            return new List<SupportTicket>();

        var json = File.ReadAllText(_filePath);

        List<TicketFileModel>? fileModels;

        try
        {
            fileModels =
                JsonSerializer.Deserialize<List<TicketFileModel>>(json);
        }
        catch (JsonException)
        {
            return new List<SupportTicket>();
        }

        if (fileModels == null)
            return new List<SupportTicket>();

        return fileModels
            .Select(fileModel =>
            {
                var ticket = SupportTicket.Rehydrate(
                    fileModel.Id,
                    fileModel.Title,
                    fileModel.Description,
                    fileModel.CustomerId,
                    fileModel.Priority,
                    fileModel.Status,
                    fileModel.CreatedAt,
                    fileModel.TechnicianId);

                foreach (var comment in fileModel.Comments)
                {
                    ticket.RestoreComment(
                        comment.Text,
                        comment.CreatedAt);
                }

                return ticket;
            })
            .ToList();
    }

    public SupportTicket? GetById(Guid id)
    {
        return GetAll()
            .FirstOrDefault(ticket => ticket.Id == id);
    }

    public void Update(SupportTicket ticket)
    {
        var tickets = GetAll();

        var existingTicket = tickets
            .FirstOrDefault(existing => existing.Id == ticket.Id);

        if (existingTicket == null)
            throw new InvalidOperationException("Ticket does not exist.");

        var fileModels = tickets
            .Select(existingTicket => new TicketFileModel
            {
                Id = existingTicket.Id,
                Title = existingTicket.Title,
                Description = existingTicket.Description,
                CustomerId = existingTicket.CustomerId,
                Priority = existingTicket.Priority,
                Status = existingTicket.Status,
                CreatedAt = existingTicket.CreatedAt,
                TechnicianId = existingTicket.TechnicianId
            })
            .ToList();

        var updatedFileModel = new TicketFileModel
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            CustomerId = ticket.CustomerId,
            Priority = ticket.Priority,
            Status = ticket.Status,
            CreatedAt = ticket.CreatedAt,
            TechnicianId = ticket.TechnicianId
        };

        foreach (var comment in ticket.Comments)
        {
            updatedFileModel.Comments.Add(new CommentFileModel
            {
                Text = comment.Text,
                CreatedAt = comment.CreatedAt
            });
        }
        var index = fileModels.FindIndex(
            fileModel => fileModel.Id == ticket.Id);

        fileModels[index] = updatedFileModel;

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(fileModels, options);

        File.WriteAllText(_filePath, json);
    }
}