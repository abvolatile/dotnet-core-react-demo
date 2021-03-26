using Domain;
using FluentValidation;

namespace Application.Activities
{
    public class ActivityValidator : AbstractValidator<Activity>
    {
        public ActivityValidator()
        {
            //you can easily chain on after NotEmpty().____ with other validation rules (check it out)
            RuleFor(a => a.Title).NotEmpty();
            RuleFor(a => a.Description).NotEmpty();
            RuleFor(a => a.Date).NotEmpty();
            RuleFor(a => a.Category).NotEmpty();
            RuleFor(a => a.City).NotEmpty();
            RuleFor(a => a.Venue).NotEmpty();
        }
    }
}