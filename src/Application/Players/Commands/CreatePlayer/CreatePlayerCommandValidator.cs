namespace PipAndIvory.Application.Players.Commands.CreatePlayer;

public class CreatePlayerCommandValidator : AbstractValidator<CreatePlayerCommand>
{
    public CreatePlayerCommandValidator()
    {
        RuleFor(v => v.DisplayName).MaximumLength(70);
    }
}
