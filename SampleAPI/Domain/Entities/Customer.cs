namespace SampleAPI.Domain.Entities;

public class Customer
{
    public string? CustomerId { get; set; }
    public string? Name { get; set; }
    public string? EmailId { get; set; }
    public Address? BillingAddress { get; set; }

}

public class Address
{
    public string? City  { get; set; }
    public int PinCode  { get; set; }
    public string? State  { get; set; }
    public string? Street  { get; set; }
    public AddressType Type  { get; set; }
}

public enum AddressType
{
    Home,
    Office
}