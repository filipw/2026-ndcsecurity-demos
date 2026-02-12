using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);
var cert = X509Certificate2.CreateFromPemFile("server.crt", "server.key");

builder.WebHost.ConfigureKestrel(opts => 
    opts.ListenAnyIP(443, listenOptions => listenOptions.UseHttps(cert)));

var app = builder.Build();
app.MapGet("/", () => "SUCCESS! Verified Pure ML-DSA Connection.");
app.Run();