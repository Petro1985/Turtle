using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Turtle;
using Turtle.TelegramBot;

public class TurtleHostService : IHostedService
{
    private string _apiKey;
    private readonly ILogger<TurtleHostService> _logger;
    private TurtleProvider _turtleProvider;  

    public TurtleHostService(ILogger<TurtleHostService> logger, IConfiguration config, TurtleProvider turtleProvider)
    {
        _logger = logger;
        _turtleProvider = turtleProvider;
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

        var turtle = _turtleProvider.GetTurtle(chatId);
            
        //Trying to find out whether it was a command
        var match = Regex.Match(messageText, mfPattern);
        if (match.Success)
        {
            var distance = match.Groups[1].Value;
            turtle.MoveForward(int.Parse(distance));
            outMessage = $"Moved by {distance}";
            processedFlag = true;
        }
        else
        {
            switch (messageText)
            {
                case "tl": 
                    turtle.Turn(TurnDirections.Left);
                    outMessage = "Turned left";
                    processedFlag = true;
                    break; 
                case "tr": 
                    turtle.Turn(TurnDirections.Right);
                    processedFlag = true;
                    outMessage = "Turned right";
                    break; 
                case "tb":
                    processedFlag = true;
                    outMessage = "Turned back";
                    turtle.Turn(TurnDirections.Back);
                    break;
                case "wims": outMessage = turtle.WhatIsMyState().ToString();
                    processedFlag = true;
                    break;
            }

        }
        var turtlePainter = new TurtlePainter();
        var picture = turtlePainter.DrawField(turtle.WhatIsMyState());

        if (!processedFlag)
        {
            outMessage = "It's not a command!!!";
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: outMessage,
                cancellationToken: cancellationToken);
            return;
        }

        using (MemoryStream inMemoryStream = new MemoryStream())
        {
            picture.Save(inMemoryStream, ImageFormat.Jpeg);
            //File.WriteAllBytes("qwe.jpg",inMemoryStream.ToArray());
            inMemoryStream.Position = 0;
            var file = new InputOnlineFile(inMemoryStream);
            
            
            // Send back the message
            Message sentMessage = await botClient.SendPhotoAsync(
                chatId: chatId,
                caption: "Current state",
                photo: file,
                cancellationToken: cancellationToken);
        }

        
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