# TgBotFramework
[![package](https://img.shields.io/nuget/v/TgBotFramework)](https://www.nuget.org/packages/TgBotFramework/)
[![licence](https://img.shields.io/github/license/TgBotFramework/TgBotFramework)]()
![Lines of code](https://img.shields.io/tokei/lines/github/TgBotFramework/TgBotFramework)
[![telegram chat](https://img.shields.io/badge/Support_Chat-Telegram-blue.svg?style=flat-square)](https://t.me/+QKud8BwGzeM1YWMy)

Make [Telegram.Bot.Framework](https://github.com/TelegramBots/Telegram.Bot.Framework) great again!

read [Wiki](https://github.com/TgBotFramework/TgBotFramework/wiki) for more details

# Early state
There is nothing that is guarantied to work, but something definitely works ;)

# Whats new?
This project targets .NET 6.0+ and there won't be any support for old .Net Framework. (it is available only in VS 2022 or rider 2021.3)
(I recommend using rider, cuz it can generate code for handlers)
In this implementation you can get (at least) same pipeline experience as in [Telegram.Bot.Framework](https://github.com/TelegramBots/Telegram.Bot.Framework) (but with blackjack and... )

# How to start
This framework uses Dependency Injection as hard as it gets, so:
1) Create default console/asp.net core project:
2) Add TgBotFramework nuget package
3) Add usings (on the top of file):
```C#
using TgBotFramework;
using TgBotFramework.WrapperExtensions;
```
4) if you have console app add:
```C#
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.
}).RunConsoleAsync(); 
```
"services" is your IServiceCollection that we'll use. 
5) if you created asp core project. You should locate your IServiceCollection yourself. In default empty example its builder.Services
6) Now lets add simple bot. here services is IServiceCollection.
```C# 
services.AddBotService<ExampleContext>("<token>", builder => builder
        .UseLongPolling()
        .SetPipeline(pipeBuilder => pipeBuilder
                .UseCommand<StartCommandExample>("start")
                .Use<ConsoleEchoHandler>()
                )
    );
services.AddSingleton<ConsoleEchoHandler>();
services.AddSingleton<StartCommandExample>();
```
1st line adds our bot service to our DI. ExampleContext - class that you should create:
```C#
public class ExampleContext : UpdateContext { } 
```
You can use UpdateContext directly, but its better to use your own inherited class.
line after that states that you will use LongPolling. 
and after that comes example of actual pipeline that will process your updates.
Add next 2 classes:
```c#
public class ConsoleEchoHandler : IUpdateHandler<ExampleContext>
{
    public async Task HandleAsync(ExampleContext context, UpdateDelegate<ExampleContext> next, CancellationToken cancellationToken)
    {
        Console.WriteLine(context.Update.ToJsonString());
    }
}

public class StartCommandExample : CommandBase<ExampleContext>
{
    public override async Task HandleAsync(ExampleContext context, UpdateDelegate<ExampleContext> next, string[] args, CancellationToken cancellationToken)
    {
        await context.Client.SendTextMessageAsync(context.ChatId, "Hello");
    }
}
```
ConsoleEchoHandler - example of any update handler in pipeline. It implements generic interface IUpdateHandler<TContext>, where TContext is your context type.
It should contain HandleAsync method, that will actually handle update. 1st param -> your context object that contains Telegram.Bot client, update and few other properties.
2nd one is method that will call next pipeline handler. Normally you'll just call it this way: 
```C#
await next(context, cancellationToken);
```
but in this case there is no reason for that.

StartCommandExample - example of command handler. Should implement CommandBase<TContext> which is almost same as IUpdateHandler<> but has additional checks and param string[] args which is comes in hand when you use commands with params:
/command someParam1 someParam2


ALL HANDLERS, such as ConsoleEchoHandler or StartCommandExample should be added in DI.