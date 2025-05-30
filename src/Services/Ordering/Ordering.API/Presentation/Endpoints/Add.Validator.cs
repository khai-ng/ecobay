namespace Ordering.API.Presentation.Endpoint
{
    public class AddOrderValidator : Validator<CreateOrderCommand>
    {
        public AddOrderValidator() 
        {
            RuleFor(x => x.Country)
                .MaximumLength(255);

            RuleFor(x => x.City)
                .MaximumLength(255);

            RuleFor(x => x.District)
                .MaximumLength(255);

            RuleFor(x => x.Street)
                .MaximumLength(255);
        }
    }
}
