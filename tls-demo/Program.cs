using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Connections.Features;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(443, listenOptions =>
    {
        listenOptions.UseHttps(httpsOptions =>
        {
            var cert = X509Certificate2.CreateFromPemFile("server.crt", "server.key");
            httpsOptions.ServerCertificate = cert;
        });
    });
});

var app = builder.Build();

app.MapGet("/", (HttpContext ctx) => 
{
    var tlsFeature = ctx.Features.Get<ITlsHandshakeFeature>();
    
    var protocol = ctx.Request.Protocol;
    var cipherSuite = tlsFeature?.NegotiatedCipherSuite.ToString() ?? "Unknown";
    var tlsVersion = tlsFeature?.Protocol.ToString() ?? "Unknown";

    Console.WriteLine($"[Connected] Protocol: {tlsVersion}");
    Console.WriteLine($"[Crypto] Cipher Suite: {cipherSuite}");

    return Results.Text($"""
        Secure Connection Established.
        
        TLS Version: {tlsVersion}
        Cipher: {cipherSuite}
        """);
});

app.Run();