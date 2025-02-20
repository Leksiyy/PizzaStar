using Microsoft.AspNetCore.Mvc;
using PizzaStar.Interfaces;

namespace PizzaStar.ViewComponents;

public class RelatedProductsViewComponent : ViewComponent
{
    private readonly IProduct _product;

    public RelatedProductsViewComponent(IProduct product)
    {
        _product = product;
    }

    public async Task<IViewComponentResult> InvokeAsync(int productId)
    {
        return View("RelatedProducts", await _product.GetEightRandomProductsAsync(productId));
    }
}