public sealed class Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty.");

        value = value.Trim();

        if (!value.Contains('@'))
            throw new ArgumentException("Email is not valid.");

        Value = value;
    }

    public override string ToString() => Value;
}

