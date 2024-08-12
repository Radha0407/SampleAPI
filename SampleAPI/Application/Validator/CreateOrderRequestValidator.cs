using FluentValidation;
using SampleAPI.Application.Command;

namespace SampleAPI.Application.Validator;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequestCommand>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().NotNull();
        RuleFor(x => x.Products).NotEmpty().NotNull();
        RuleFor(x => x.OrderDate).NotEmpty().NotNull();
        RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(100)
            .WithMessage(AppConstants.InvalidDescription);
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100)
            .WithMessage(AppConstants.InvalidName);
        RuleForEach(x => x.Products).SetValidator(new ProductRequestValidator());
    }
}

public class ProductRequestValidator : AbstractValidator<ProductRequestCommand>
{
    public ProductRequestValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().NotNull();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}