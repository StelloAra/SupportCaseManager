namespace SupportCaseManager.Domain;

public class Comment
{
    public string Text { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Comment(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Comment cannot be empty.");

        Text = text.Trim();
        CreatedAt = DateTime.Now;
    }
}