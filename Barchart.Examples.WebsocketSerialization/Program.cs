using System.Net.WebSockets;
using Barchart.BinarySerializer.Schemas;
using Barchart.SerializationData;
using Org.Openfeed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();
app.UseWebSockets();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.Map("/ws", async context => {

    if (context.WebSockets.IsWebSocketRequest)
    {
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        Schema<MarketData> marketDataSchema = SchemaFactory.GetSchema<MarketData>();

        while (true)
        {
            Random random = new();

            string[] symbols = { "AAPL", "TSLA", "GOOGL", "MSFT", "AMZN" };
            string[] descriptions = { "Apple Inc.", "Tesla, Inc.", "Alphabet Inc.", "Microsoft Corporation", "Amazon.com, Inc." };

            MarketData marketData = new()
            {
                SubscriptionResponse = new Barchart.SerializationData.SubscriptionResponse
                {
                    Symbol = symbols[random.Next(symbols.Length)],
                    CorrelationId = random.Next(),
                    MarketId = random.Next(),
                    Exchange = "NYSE",
                    ChannelId = random.Next(),
                    NumberOfDefinitions = random.Next(),
                    Unsubscribe = random.Next(0, 2) == 0 ? false : true,
                    SnapshotIntervalSeconds = random.Next(1, 3600)
                },
                InstrumentDefinition = new Barchart.SerializationData.InstrumentDefinition
                {
                    MarketId = random.Next(),
                    BookDepth = random.Next(),
                    VendorId = "XYZ123",
                    Symbol = symbols[random.Next(symbols.Length)],
                    Description = descriptions[random.Next(descriptions.Length)]
                },
                MarketSnapshot = new Barchart.SerializationData.MarketSnapshot
                {
                    MarketId = random.Next(),
                    TransactionTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    MarketSequence = random.Next(),
                    TradeDate = random.Next(20240101, 20250101)
                },
                MarketUpdate = new Barchart.SerializationData.MarketUpdate
                {
                    MarketId = random.Next(),
                    Symbol = symbols[random.Next(symbols.Length)],
                    TransactionTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    DistributionTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    MarketSequence = random.Next(),
                    SourceSequence = random.Next()
                }
            };

            byte[] serializedData = marketDataSchema.Serialize(marketData);
            var arraySegment = new ArraySegment<byte>(serializedData, 0, serializedData.Length);
            if (ws.State == WebSocketState.Open)
            {
                await ws.SendAsync(arraySegment, WebSocketMessageType.Binary, true, CancellationToken.None);
            }
            else if (ws.State == WebSocketState.Closed || ws.State == WebSocketState.Aborted)
            {
                break;
            }

            Thread.Sleep(1000);
        }
    }
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

await app.RunAsync();

