using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using Turtle.Maps;

namespace Turtle.TelegramBot;

public class TelegramBotHandler
{
    private readonly TurtleProvider _turtleProvider;

    public TelegramBotHandler(TurtleProvider turtleProvider)
    {
        _turtleProvider = turtleProvider;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        
        if (update.Type != UpdateType.Message)
            return;
        if (update.Message!.Type != MessageType.Text)
            return;
        
        var chatId = update.Message.Chat.Id;
        var messageText = (update.Message.Text ?? "").ToLower();

        var mfPattern = "^mf ?([0-9]{0,3})$";
        var putWallPattern = "^pw ?(-?[0-9]{1,3})[,| ] ?(-?[0-9]{1,3})$";
        var outMessage = "";
        var processedFlag = false;

        var turtle = _turtleProvider.GetTurtle(chatId);
            
        //Trying to find out whether it was a command
        var match = Regex.Match(messageText, mfPattern);
        if (match.Success)
        {
            var distance = match.Groups[1].Value == ""?"1":match.Groups[1].Value;
            turtle.MoveForward(int.Parse(distance));
            outMessage = $"Moved by {distance}";
            processedFlag = true;
        }
        else
        {
            match = Regex.Match(messageText, putWallPattern);
            if (match.Success)
            {
                var x = int.Parse(match.Groups[1].Value);
                var y = int.Parse(match.Groups[2].Value);
                turtle.WhatIsMyState().Map.Add(new Wall(x, y));
                outMessage = $"Wall was put at x={x} y={y}";
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
                    case "help":
                        outMessage = "Commands:" +
                                     "\ntr - turn right" +
                                     "\ntl - turn left" +
                                     "\ntb - turn back" +
                                     "\nmf N- move forward N cells (move 1 if no N as default)" +
                                     "\npw x y - build a wall at coordinate (x, y)" +
                                     "\nhelp - this help";

                        processedFlag = true;
                        break;
                }
            }

        }
        var turtlePainter = new TurtlePainter();
        var picture = turtlePainter.DrawField(turtle.WhatIsMyState());

        if (!processedFlag)
        {
            outMessage = "It's not a command!!!";
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: outMessage,
                cancellationToken: cancellationToken);
            return;
        }

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: outMessage,
            cancellationToken: cancellationToken);

        await using var inMemoryStream = new MemoryStream();
        picture.Save(inMemoryStream, ImageFormat.Jpeg);
        //File.WriteAllBytes("qwe.jpg",inMemoryStream.ToArray());
        inMemoryStream.Position = 0;
        var file = new InputOnlineFile(inMemoryStream);
        KeyboardButton[] buttons = new[]
        {
            new KeyboardButton("mf"),
            new KeyboardButton("tr"),
            new KeyboardButton("tl"),
            new KeyboardButton("tb"),
            new KeyboardButton("pw"),
        };
        
        var replyKeyboardMarkup = new ReplyKeyboardMarkup(buttons);
        
        // Send back the message
        await botClient.SendPhotoAsync(
            chatId: chatId,
            caption: "Current state",
            photo: file,
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);

        
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
    
}