using System.ComponentModel.DataAnnotations;

namespace PizzaStar.ViewModels;

public class ContactFormViewModel
{
    [Display(Name = "Имя")]
    [Required(ErrorMessage = "Пожалуйста, введите Ваше имя.")]
    public string? Name { get; set; }
    
    [Display(Name = "Элестронный адресс")]
    [Required(ErrorMessage = "Пожалуйста, введите Ваш эл. адресс.")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    
    [Display(Name = "Номер телефона")]
    [Required(ErrorMessage = "Пожалуйста, введите Ваш номер телефона.")]
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }
    
    [Display(Name = "Описание")]
    [Required(ErrorMessage = "Пожалуйста, введите описание.")]
    public string? Enquiry { get; set; }
    
    public string? ReturnUrl { get; set; }
}