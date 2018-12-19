﻿using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Router;

class ForwardSubscribeGatewayRule : IRule<ForwardSubscribeContext, ForwardSubscribeContext>
{
    string localAddress;
    string localEndpoint;

    public ForwardSubscribeGatewayRule(string localAddress, string localEndpoint)
    {
        this.localAddress = localAddress;
        this.localEndpoint = localEndpoint;
    }

    public async Task Invoke(ForwardSubscribeContext context, Func<ForwardSubscribeContext, Task> next)
    {
        var forwardedSubscribes = context.Routes.Where(r => r.Gateway != null);
        var forkContexts = forwardedSubscribes.Select(r =>
            new AnycastContext(r.Gateway,
                MessageDrivenPubSub.CreateMessage(r.Destination, context.MessageType, localAddress, localEndpoint, MessageIntentEnum.Subscribe),
                DistributionStrategyScope.Send,
                context));

        var chain = context.Chains.Get<AnycastContext>();
        var forkTasks = forkContexts.Select(c => chain.Invoke(c));
        await Task.WhenAll(forkTasks).ConfigureAwait(false);
        await next(context);
    }
}
