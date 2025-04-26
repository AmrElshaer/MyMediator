using FluentValidation;
using MyMediator.Core;

namespace MyMediator.UseCases;

public interface IUserRepository
{
    Task<int> AddAsync(User user, CancellationToken cancellationToken);
}

public class UserRepository:IUserRepository
{
    public Task<int> AddAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(1);
    }
}
public class CreateUserCommand : IRequest<int>
{
    public string Name { get; set; }
    public string Email { get; set; }

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email address is required")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");
        }
    }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IUserRepository _repository;
    private readonly InMemoryMessageQueue _mediator;

    public CreateUserCommandHandler(IUserRepository repository,InMemoryMessageQueue mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }
    
    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User { Id= Guid.NewGuid(), Name = request.Name, Email = request.Email };
        var userCreatedNotification = new UserCreatedNotification()
        {
            Name = user.Name,
            UserId = user.Id,
        };
         await _mediator.Writer.WriteAsync(userCreatedNotification,cancellationToken);
        return await _repository.AddAsync(user, cancellationToken);
    }
}