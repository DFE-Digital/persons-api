using FluentValidation;

namespace Dfe.PersonsApi.Application.Constituencies.Queries.SearchMembersOfParliament
{
    public class SearchMembersOfParliamentValidator : AbstractValidator<SearchMembersOfParliamentQuery>
    {
        public SearchMembersOfParliamentValidator()
        {
            RuleFor(x => x.SearchTerm)
                .NotNull().WithMessage("Search term cannot be null.")
                .NotEmpty().WithMessage("Search term cannot be empty.");
        }
    }
}
