using System.Net.WebSockets;
using Barchart.BinarySerializer.Schemas;
using Barchart.SerializationData;

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

        MarketData oldMarketData = new();

        while (true)
        {
            Random random = new();

            string[] symbols = { "AAPL", "TSLA", "GOOGL", "MSFT", "AMZN" };
            string[] descriptions = { "Apple Inc.", "Tesla, Inc.", "Alphabet Inc.", "Microsoft Corporation", "Amazon.com, Inc." };

            MarketData newMarketData = new()
            {
                SubscriptionResponse = new SubscriptionResponse
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
                InstrumentDefinition = new InstrumentDefinition
                {
                    MarketId = random.Next(),
                    BookDepth = random.Next(),
                    VendorId = "XYZ123",
                    Symbol = symbols[random.Next(symbols.Length)],
                    Description = descriptions[random.Next(descriptions.Length)]
                },
                MarketState = new MarketState
                {
                    TradeDate = random.Next(20240101, 20250101),
                    Symbol = symbols[random.Next(symbols.Length)]
                }
            };

            byte[] serializedData = marketDataSchema.Serialize(oldMarketData, newMarketData);
            var arraySegment = new ArraySegment<byte>(serializedData, 0, serializedData.Length);
            if (ws.State == WebSocketState.Open)
            {
                await ws.SendAsync(arraySegment, WebSocketMessageType.Binary, true, CancellationToken.None);
            }
            else if (ws.State == WebSocketState.Closed || ws.State == WebSocketState.Aborted)
            {
                break;
            }

            CopyMarketData(newMarketData, oldMarketData);

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

void CopyMarketData(MarketData source, MarketData target)
{
    target.SubscriptionResponse = new SubscriptionResponse
    {
        Symbol = source.SubscriptionResponse?.Symbol,
        CorrelationId = source.SubscriptionResponse?.CorrelationId,
        MarketId = source.SubscriptionResponse?.MarketId,
        Exchange = source.SubscriptionResponse?.Exchange,
        ChannelId = source.SubscriptionResponse?.ChannelId,
        NumberOfDefinitions = source.SubscriptionResponse?.NumberOfDefinitions,
        Unsubscribe = source.SubscriptionResponse?.Unsubscribe,
        SnapshotIntervalSeconds = source.SubscriptionResponse?.SnapshotIntervalSeconds
    };

    target.InstrumentDefinition = new InstrumentDefinition
    {
        MarketId = source.InstrumentDefinition?.MarketId,
        BookDepth = source.InstrumentDefinition?.BookDepth,
        VendorId = source.InstrumentDefinition?.VendorId,
        Symbol = source.InstrumentDefinition?.Symbol,
        Description = source.InstrumentDefinition?.Description
    };

    target.MarketState = new MarketState
    {
        TradeDate = source.MarketState?.TradeDate,
        Symbol = source.MarketState?.Symbol
    };
}