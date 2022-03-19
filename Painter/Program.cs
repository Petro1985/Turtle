// See https://aka.ms/new-console-template for more information

using System.Drawing.Imaging;
using Turtle;
using Turtle.Handlers;
using Turtle.Maps;
using Turtle.TelegramBot;

var map = new Map();
map.Add(new Wall(-2, -3));
map.Add(new Wall(-2, -2));
map.Add(new Wall(-2, -1));
map.Add(new Wall(-2, 0));
map.Add(new Wall(-2, 1));
map.Add(new Wall(-2, 2));
map.Add(new Wall(-2, 3));

var turtle = new Turtle.Turtle(map, new List<CrushHandlerBase>());

turtle.MoveForward(2);
turtle.Turn(TurnDirections.Right);
turtle.MoveForward(2);

var painter = new TurtlePainter();

var bitmap = painter.DrawField(turtle.WhatIsMyState());

bitmap.Save("qwe.jpg", ImageFormat.Jpeg);
