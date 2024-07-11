using FastEndpoints;
using FluentValidation;
using Identity.Application.Services;

namespace Identity.API.Endpoint
{
    public class SignInValidator : Validator<SignInRequest>
    {
        public SignInValidator() 
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("UserName is required");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required");
        }
    }
}
