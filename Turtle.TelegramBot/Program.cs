// See https://aka.ms/new-console-template for more information

using Turtle;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using File = System.IO.File;
using Turtle = Turtle.Turtle;


var host = Host.CreateDefaultBuilder()
    .ConfigureServices(collection =>
    {
        collection.AddHostedService<TurtleHostService>();
        collection.AddTurtle();
    }).Build();

await host.StartAsync();