using MediatR;
using Telegram.Bot.Types;

namespace CheapChic.Infrastructure.Handlers.Telegram.Commands;

public static class HandleUpdate
{
    public class Command : IRequest<Unit>
    {
        public Command(string token, Update update)
        {
            Token = token;
            Update = update;
        }

        public string Token { get; }
        public Update Update { get; }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            return Unit.Value;
        }
    }
}