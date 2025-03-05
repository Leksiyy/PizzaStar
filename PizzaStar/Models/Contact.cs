using System.ComponentModel.DataAnnotations;

namespace PizzaStar.Models;

public class Contact
{
    public int Id { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}