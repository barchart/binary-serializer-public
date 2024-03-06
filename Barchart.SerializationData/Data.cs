using Barchart.BinarySerializer.Schemas;

namespace Barchart.SerializationData;

public class StockData
{
    [BinarySerialize(include: true, key: true)]
    public string? Symbol { get; set; }

    [BinarySerialize(include: true, key: false)]
    public bool? Synthetic { get; set; }

    [BinarySerialize(include: true, key: false)]
    public bool? Online { get; set; }

    [BinarySerialize(include: true, key: false)]
    public bool? Active { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? Sequence { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? Mode { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? DayNum { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? BidPrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? BidSize { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? AskPrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? AskSize { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? TradePrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? TradeSize { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? NumberOfTrades { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? Vwap1 { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? OpenPrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? VolumeSpecial { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? OpenInterest { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? PriceChange { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? PercentChange { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? LastPriceDirection { get; set; }

    [BinarySerialize(include: true, key: false)]
    public DateTime? Time { get; set; }

    [BinarySerialize(include: true, key: false)]
    public long? TimeActual { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? TimeDateDisplayLong { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? SessionDateDisplayLong { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? PreviousTimeDateDisplayLong { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? YesterdayDateDisplay { get; set; }

    [BinarySerialize(include: true, key: false)]
    public DateTime? TradeTime { get; set; }

    [BinarySerialize(include: true, key: false)]
    public long? TradeTimeActual { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? TradeTimeDisplay { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? TradeDateDisplay { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? PreviousOpenPrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? PreviousHighPrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? PreviousLowPrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? LastPriceHigh { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? LastPriceLow { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? PremarketPriceChange { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? PremarketPercentChange { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? PostmarketPriceChange { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? PostmarketPercentChange { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? YesterdayPriceChange { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? YesterdayPercentChange { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? PreviousPriceChange { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? PreviousPercentChange { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? Day { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? Flag { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? State { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? Session { get; set; }

    [BinarySerialize(include: true, key: false)]
    public bool? SessionT { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? TodayPrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? PreviousPrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? PreviousPricePreview { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? SettlementPrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? YesterdayPrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? PremarketPrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? PostmarketPrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? LastPrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? LastPriceT { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? HighPrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? LowPrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? PreviousPreviousPrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? PreviousSettlementPrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? TimeDateDisplay { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? TimeDisplay { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? SessionDateDisplay { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? PreviousTimeDateDisplay { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? Volume { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? VolumePrevious { get; set; }

    [BinarySerialize(include: true, key: false)]
    public double? PriceVol { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? PreviousTime { get; set; }
}