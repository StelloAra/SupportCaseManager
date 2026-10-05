public sealed class Name
{
    public string Value { get; }

    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Name cannot be empty.");

        Value = value.Trim();
    }

    public override string ToString() => Value;
}
