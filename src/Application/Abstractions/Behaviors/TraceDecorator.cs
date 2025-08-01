using Application.Abstractions.Messaging;
using OpenTelemetry.Trace;
using SharedKernel;

namespace Application.Abstractions.Behaviors;

internal static class TraceDecorator
{
    public const string SpanName = "APPLICATION cqrs handler";
    public const string HandlerName = "handler.name";
    public const string HandlerType = "handler.type";
    public const string HandlerResult = "handler.result";
    public const string HandlerError = "handler.error";

    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        TracerProvider tracerProvider)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            using TelemetrySpan span = tracerProvider.GetTracer("Web.Api")  // consider using options pattern to give application name to this layer
                .StartActiveSpan(SpanName);

            span.SetAttribute(HandlerName, typeof(TCommand).Name);
            span.SetAttribute(HandlerType, "command");

            Result<TResponse> result = await innerHandler.Handle(command, cancellationToken);

            result.Match(
                onSuccess: response => span.SetAttribute(HandlerResult, System.Text.Json.JsonSerializer.Serialize(response)),
                onFailure: error =>
                {
                    span.SetAttribute(HandlerResult, false);
                    span.SetAttribute(HandlerError, error.ToString());
                });

            return result;
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        TracerProvider tracerProvider)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            using TelemetrySpan span = tracerProvider.GetTracer("Web.Api")  // consider using options pattern to give application name to this layer
                .StartActiveSpan(SpanName);

            span.SetAttribute(HandlerName, typeof(TCommand).Name);
            span.SetAttribute(HandlerType, "command");

            Result result = await innerHandler.Handle(command, cancellationToken);

            result.Match(
                onSuccess: () => span.SetAttribute(HandlerResult, false),
                onFailure: error =>
                {
                    span.SetAttribute(HandlerResult, false);
                    span.SetAttribute(HandlerError, error.ToString());
                });

            return result;
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        TracerProvider tracerProvider)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
        {            
            using TelemetrySpan span = tracerProvider.GetTracer("Web.Api")  // consider using options pattern to give application name to this layer
                .StartActiveSpan(SpanName);

            span.SetAttribute(HandlerName, typeof(TQuery).Name);
            span.SetAttribute(HandlerType, "query");

            Result<TResponse> result = await innerHandler.Handle(query, cancellationToken);

            result.Match(
                onSuccess: response => span.SetAttribute(HandlerResult, System.Text.Json.JsonSerializer.Serialize(response)),
                onFailure: error =>
                {
                    span.SetAttribute(HandlerResult, false);
                    span.SetAttribute(HandlerError, error.ToString());
                });

            return result;
        }
    }
}
