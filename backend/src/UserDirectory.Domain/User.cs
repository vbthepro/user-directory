namespace UserDirectory.Domain;

public sealed class User
{
    private User() { }

    public User(string name, int age, string city, string state, string pincode)
    {
        Id = Guid.NewGuid();
        Update(name, age, city, state, pincode);
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Age { get; private set; }
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string Pincode { get; private set; } = string.Empty;

    public void Update(string name, int age, string city, string state, string pincode)
    {
        Name = name.Trim();
        Age = age;
        City = city.Trim();
        State = state.Trim();
        Pincode = pincode.Trim();
    }
}
