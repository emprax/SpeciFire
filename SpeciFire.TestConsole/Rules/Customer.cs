using System;

namespace SpeciFire.TestConsole.Rules;

public class Customer
{
    public Customer(string name, DateTime birthday, bool hasAccount)
    {
        this.Name = name;
        this.Birthday = birthday;
        this.HasAccount = hasAccount;
    }

    public string Name { get; set; }

    public DateTime Birthday { get; set; }

    public bool HasAccount { get; set; }
}
