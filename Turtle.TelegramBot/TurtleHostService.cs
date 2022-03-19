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
    // private TurtleProvider _turtleProvider;  
    private TelegramBotHandler _telegramBotHandler;  

    public TurtleHostService(ILogger<TurtleHostService> logger, IConfiguration config, TelegramBotHandler telegramBotHandler)
    {
        _logger = logger;
        // _turtleProvider = turtleProvider;
        _telegramBotHandler = telegramBotHandler;
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
            _telegramBotHandler.HandleUpdateAsync,
            _telegramBotHandler.HandleErrorAsync,
            receiverOptions,
            cancellationToken);

        while (true)
        {
            await Task.Delay(1000);
        }
    }
    
    public async Task StopAsync(CancellationToken cancellationToken) {}
}