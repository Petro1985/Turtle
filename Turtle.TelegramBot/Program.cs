// See https://aka.ms/new-console-template for more information

using Turtle;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Turtle.TelegramBot;


var host = Host.CreateDefaultBuilder()
    .ConfigureServices(collection =>
    {
        collection.AddHostedService<TurtleHostService>();
        collection.AddTurtle(".\\Resources");
        collection.AddSingleton<TelegramBotHandler>();
    }).Build();

await host.StartAsync();