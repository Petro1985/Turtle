// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;
using Telegram.Bot;
//
// var botClient = new TelegramBotClient("{YOUR_ACCESS_TOKEN_HERE}");
//
// var me = await botClient.GetMeAsync();
// Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using TurtleApp;


var host = Host.CreateDefaultBuilder()
    .ConfigureServices(collection =>
    {
        collection.AddHostedService<TurtleHostService>();
        collection.AddTurtle();
    }).Build();

await host.StartAsync();

public class TurtleHostService : IHostedService
{
    private string _apiKey;
    private readonly ILogger<TurtleHostService> _logger;
    private Turtle _turtle;

    public TurtleHostService(ILogger<TurtleHostService> logger, IConfiguration config, Turtle turtle)
    {
        _logger = logger;
        _turtle = turtle;
        _apiKey = config["Telegram:apiKey"]
                  ?? throw new Exception("No API exception. Add API key in configuration file.");
    }

    public async Task StartAsync(CancellationToken cancellationToken) {
        var botClient = new TelegramBotClient(_apiKey);
        _logger.LogInformation("Telegram bot connection established!!!");

        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { } // receive all update types
        };
        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken);

        while (true)
        {
            await Task.Delay(1000);
        }
    }
    
    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message)
            return;
        if (update.Message!.Type != MessageType.Text)
            return;
        
        var chatId = update.Message.Chat.Id;
        var messageText = update.Message.Text.ToLower();

        var mfPattern = "^mf ?([0-9]{0,3})$";
        var outMessage = "";
        var processedFlag = false;
        
        //Trying to find out whether it was a command
        var match = Regex.Match(messageText, mfPattern);
        if (match.Success)
        {
            var distance = match.Groups[1].Value;
            _turtle.MoveForward(int.Parse(distance));
            outMessage = $"Moved by {distance}";
            processedFlag = true;
        }
        else
        {
            switch (messageText)
            {
                case "tl": 
                    _turtle.Turn(TurnDirections.Left);
                    outMessage = "Turned left";
                    processedFlag = true;
                    break; 
                case "tr": 
                    _turtle.Turn(TurnDirections.Right);
                    processedFlag = true;
                    outMessage = "Turned right";
                    break; 
                case "tb":
                    processedFlag = true;
                    outMessage = "Turned back";
                    _turtle.Turn(TurnDirections.Back);
                    break;
                case "wims": outMessage = _turtle.WhatIsMyState().ToString();
                    processedFlag = true;
                    break;
            }
        }

        if (!processedFlag) outMessage = "It's not a command!!!";

        // Send back the message
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: outMessage,
            cancellationToken: cancellationToken);
        
    }

    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken) {}
}


