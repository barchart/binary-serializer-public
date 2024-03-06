using System.Globalization;
using System.Net.WebSockets;
using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.SerializationData;

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
        Schema<StockData> stockDataSchema = SchemaFactory.GetSchema<StockData>();

        StockData oldStockData = new StockData();

        while (true)
        {
            Random random = new Random();

            StockData newStockData = new StockData()
            {
                Symbol = "TSLA",
                Synthetic = random.Next(2) == 0,
                Online = true,
                Active = true,
                Sequence = random.Next(),
                Mode = random.NextDouble() < 0.5 ? "I" : "II",
                DayNum = random.Next(1, 100),
                BidPrice = random.NextDouble() * 200,
                BidSize = random.Next(1, 100),
                AskPrice = random.NextDouble() * 200,
                AskSize = random.Next(1, 100),
                TradePrice = random.NextDouble() * 200,
                TradeSize = random.Next(1, 1000),
                NumberOfTrades = random.Next(10000),
                Vwap1 = random.NextDouble() * 200,
                OpenPrice = random.NextDouble() * 200,
                VolumeSpecial = random.Next(),
                PriceChange = random.NextDouble() * 10,
                PercentChange = random.NextDouble(),
                LastPriceDirection = random.NextDouble() < 0.5 ? "up" : "down",
                Time = DateTime.UtcNow,
                TimeActual = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                TimeDateDisplayLong = DateTime.UtcNow.ToString("ddd, MMM d, yyyy", CultureInfo.InvariantCulture),
                SessionDateDisplayLong = DateTime.UtcNow.ToString("ddd, MMM d, yyyy", CultureInfo.InvariantCulture),
                PreviousTimeDateDisplayLong = DateTime.UtcNow.ToString("ddd, MMM d, yyyy", CultureInfo.InvariantCulture),
                YesterdayDateDisplay = null,
                TradeTime = DateTime.UtcNow,
                TradeTimeActual = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                TradeTimeDisplay = DateTime.UtcNow.ToString("HH:mm tt", CultureInfo.InvariantCulture),
                TradeDateDisplay = DateTime.UtcNow.ToString("MM/dd/yy", CultureInfo.InvariantCulture),
                PreviousOpenPrice = random.NextDouble() * 200,
                PreviousHighPrice = random.NextDouble() * 200,
                PreviousLowPrice = random.NextDouble() * 200,
                LastPriceHigh = random.NextDouble() * 200,
                LastPriceLow = random.NextDouble() * 200,
                Day = random.NextDouble() < 0.5 ? "K" : "P",
                Session = random.NextDouble() < 0.5 ? "@" : "#",
                TodayPrice = random.NextDouble() * 200,
                PreviousPrice = random.NextDouble() * 200,
                LastPrice = random.NextDouble() * 200,
                LastPriceT = random.NextDouble() * 200,
                HighPrice = random.NextDouble() * 200,
                LowPrice = random.NextDouble() * 200,
                TimeDateDisplay = DateTime.UtcNow.ToString("MM/dd/yy", CultureInfo.InvariantCulture),
                TimeDisplay = DateTime.UtcNow.ToString("HH:mm tt", CultureInfo.InvariantCulture),
                Volume = random.Next(),
                VolumePrevious = random.Next(),
                PriceVol = random.NextDouble() * 1_000_000_000,
                PreviousTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture)
            };

            byte[] serializedData = stockDataSchema.Serialize(oldStockData, newStockData);
            var arraySegment = new ArraySegment<byte>(serializedData, 0, serializedData.Length);
            if (ws.State == WebSocketState.Open)
            {
                await ws.SendAsync(arraySegment, WebSocketMessageType.Binary, true, CancellationToken.None);
            }
            else if (ws.State == WebSocketState.Closed || ws.State == WebSocketState.Aborted)
            {
                break;
            }

            CopyStockData(newStockData, old);

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

void CopyStockData(StockData source, StockData target)
{
    target.Symbol = source.Symbol;
    target.Synthetic = source.Synthetic;
    target.Online = source.Online;
    target.Active = source.Active;
    target.Sequence = source.Sequence;
    target.Mode = source.Mode;
    target.DayNum = source.DayNum;
    target.BidPrice = source.BidPrice;
    target.BidSize = source.BidSize;
    target.AskPrice = source.AskPrice;
    target.AskSize = source.AskSize;
    target.TradePrice = source.TradePrice;
    target.TradeSize = source.TradeSize;
    target.NumberOfTrades = source.NumberOfTrades;
    target.Vwap1 = source.Vwap1;
    target.OpenPrice = source.OpenPrice;
    target.VolumeSpecial = source.VolumeSpecial;
    target.PriceChange = source.PriceChange;
    target.PercentChange = source.PercentChange;
    target.LastPriceDirection = source.LastPriceDirection;
    target.Time = source.Time;
    target.TimeActual = source.TimeActual;
    target.TimeDateDisplayLong = source.TimeDateDisplayLong;
    target.SessionDateDisplayLong = source.SessionDateDisplayLong;
    target.PreviousTimeDateDisplayLong = source.PreviousTimeDateDisplayLong;
    target.YesterdayDateDisplay = source.YesterdayDateDisplay;
    target.TradeTime = source.TradeTime;
    target.TradeTimeActual = source.TradeTimeActual;
    target.TradeTimeDisplay = source.TradeTimeDisplay;
    target.TradeDateDisplay = source.TradeDateDisplay;
    target.PreviousOpenPrice = source.PreviousOpenPrice;
    target.PreviousHighPrice = source.PreviousHighPrice;
    target.PreviousLowPrice = source.PreviousLowPrice;
    target.LastPriceHigh = source.LastPriceHigh;
    target.LastPriceLow = source.LastPriceLow;
    target.Day = source.Day;
    target.Session = source.Session;
    target.TodayPrice = source.TodayPrice;
    target.PreviousPrice = source.PreviousPrice;
    target.LastPrice = source.LastPrice;
    target.LastPriceT = source.LastPriceT;
    target.HighPrice = source.HighPrice;
    target.LowPrice = source.LowPrice;
    target.TimeDateDisplay = source.TimeDateDisplay;
    target.TimeDisplay = source.TimeDisplay;
    target.Volume = source.Volume;
    target.VolumePrevious = source.VolumePrevious;
    target.PriceVol = source.PriceVol;
    target.PreviousTime = source.PreviousTime;
}