﻿namespace PizzaStar.Models.Cart;

public class ShopCartItem
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public string ShopCartId { get; set; }
}