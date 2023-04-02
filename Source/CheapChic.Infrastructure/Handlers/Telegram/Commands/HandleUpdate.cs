using CheapChic.Infrastructure.UpdateHandlers.CallbackQuery;
using CheapChic.Infrastructure.UpdateHandlers.Message;
using CheapChic.Infrastructure.UpdateHandlers.MyChatMember;
using MediatR;
using Prometheus;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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
        private static readonly Counter ReceivedMessages = Metrics
            .CreateCounter("received_messages", "Number of received messages.");

        private readonly ICallbackQueryHandler _callbackQueryHandler;
        private readonly IMessageHandler _messageHandler;
        private readonly IMyChatMemberHandler _myChatMemberHandler;


        public Handler(IMessageHandler messageHandler, ICallbackQueryHandler callbackQueryHandler,
            IMyChatMemberHandler myChatMemberHandler)
        {
            _messageHandler = messageHandler;
            _callbackQueryHandler = callbackQueryHandler;
            _myChatMemberHandler = myChatMemberHandler;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            ReceivedMessages.Inc();
            var update = request.Update;
            switch (update.Type)
            {
                case UpdateType.Message:
                    await _messageHandler.HandleMessage(request.Token, update.Message!,
                        cancellationToken);
                    break;
                case UpdateType.CallbackQuery:
                    await _callbackQueryHandler.HandleCallbackQuery(request.Token, update.CallbackQuery!,
                        cancellationToken);
                    break;
                case UpdateType.MyChatMember:
                    await _myChatMemberHandler.HandleMyChatMember(request.Token, update.MyChatMember!,
                        cancellationToken);
                    break;
                case UpdateType.Unknown:
                case UpdateType.InlineQuery:
                case UpdateType.ChosenInlineResult:
                case UpdateType.EditedMessage:
                case UpdateType.ChannelPost:
                case UpdateType.EditedChannelPost:
                case UpdateType.ShippingQuery:
                case UpdateType.PreCheckoutQuery:
                case UpdateType.Poll:
                case UpdateType.PollAnswer:
                case UpdateType.ChatMember:
                case UpdateType.ChatJoinRequest:
                default:
                    break;
            }

            return Unit.Value;
        }
    }
}