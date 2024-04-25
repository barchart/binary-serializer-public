using Barchart.BinarySerializer.Schemas;
using Org.Openfeed;
using static Org.Openfeed.InstrumentDefinition.Types;

namespace Barchart.SerializationData;

public class MarketData
{
    [BinarySerialize(key: false)]
    public SubscriptionResponse? SubscriptionResponse { get; set; }

    [BinarySerialize(key: false)]
    public InstrumentDefinition? InstrumentDefinition { get; set; }

    [BinarySerialize(key: false)]
    public MarketState? MarketState { get; set; }
}

public class SubscriptionResponse
{
    [BinarySerialize(key: true)]
    public string? Symbol { get; set; }

    [BinarySerialize(key: false)]
    public long? CorrelationId { get; set; }

    [BinarySerialize(key: false)]
    public Status? Status { get; set; }

    [BinarySerialize(key: false)]
    public long? MarketId { get; set; }

    [BinarySerialize(key: false)]
    public string? Exchange { get; set; }

    [BinarySerialize(key: false)]
    public int? ChannelId { get; set; }

    [BinarySerialize(key: false)]
    public int? NumberOfDefinitions { get; set; }

    [BinarySerialize(key: false)]
    public SubscriptionType? SubscriptionType { get; set; }

    [BinarySerialize(key: false)]
    public bool? Unsubscribe { get; set; }

    [BinarySerialize(key: false)]
    public int? SnapshotIntervalSeconds { get; set; }
}

public class InstrumentDefinition
{
    [BinarySerialize(key: false)]
    public long? MarketId { get; set; }

    [BinarySerialize(key: false)]
    public InstrumentType? InstrumentType { get; set; }

    [BinarySerialize(key: false)]
    public List<BookType>? SupportBookTypes { get; set; }

    [BinarySerialize(key: false)]
    public int? BookDepth { get; set; }

    [BinarySerialize(key: false)]
    public string? VendorId { get; set; }

    [BinarySerialize(key: false)]
    public string? Symbol { get; set; }

    [BinarySerialize(key: false)]
    public string? Description { get; set; }

    [BinarySerialize(key: false)]
    public string? CfiCode { get; set; }

    [BinarySerialize(key: false)]
    public string? CurrencyCode { get; set; }

    [BinarySerialize(key: false)]
    public string? ExchangeCode { get; set; }

    [BinarySerialize(key: false)]
    public float? MinimumPriceIncrement { get; set; }

    [BinarySerialize(key: false)]
    public float? ContractPointValue { get; set; }

    [BinarySerialize(key: false)]
    public Schedule? Schedule { get; set; }

    [BinarySerialize(key: false)]
    public Calendar? Calendar { get; set; }

    [BinarySerialize(key: false)]
    public long? RecordCreateTime { get; set; }

    [BinarySerialize(key: false)]
    public long? RecordUpdateTime { get; set; }

    [BinarySerialize(key: false)]
    public string? TimeZoneName { get; set; }

    [BinarySerialize(key: false)]
    public string? InstrumentGroup { get; set; }

    [BinarySerialize(key: false)]
    public MaturityDate? SymbolExpiration { get; set; }

    [BinarySerialize(key: false)]
    public State? State { get; set; }

    [BinarySerialize(key: false)]
    public int? Channel { get; set; }

    [BinarySerialize(key: false)]
    public long? UnderlyingMarketId { get; set; }

    [BinarySerialize(key: false)]
    public PriceFormat? PriceFormat { get; set; }

    [BinarySerialize(key: false)]
    public PriceFormat? OptionStrikePriceFormat { get; set; }

    [BinarySerialize(key: false)]
    public int? PriceDenominator { get; set; }

    [BinarySerialize(key: false)]
    public int? QuantityDenominator { get; set; }

    [BinarySerialize(key: false)]
    public bool? IsTradable { get; set; }

    [BinarySerialize(key: false)]
    public long? TransactionTime { get; set; }

    [BinarySerialize(key: false)]
    public string? AuxiliaryData { get; set; }

    [BinarySerialize(key: false)]
    public List<Symbol>? Symbols { get; set; }

    [BinarySerialize(key: false)]
    public long? OptionStrike { get; set; }

    [BinarySerialize(key: false)]
    public OptionType? OptionType { get; set; }

    [BinarySerialize(key: false)]
    public OptionStyle? OptionStyle { get; set; }

    [BinarySerialize(key: false)]
    public int? OptionStrikeDenominator { get; set; }

    [BinarySerialize(key: false)]
    public string? SpreadCode { get; set; }

    [BinarySerialize(key: false)]
    public List<SpreadLeg>? SpreadLeg { get; set; }

    [BinarySerialize(key: false)]
    public bool? UserDefinedSpread { get; set; }

    [BinarySerialize(key: false)]
    public string? MarketTier { get; set; }

    [BinarySerialize(key: false)]
    public string? FinancialStatusIndicator { get; set; }

    [BinarySerialize(key: false)]
    public string? Isin { get; set; }

    [BinarySerialize(key: false)]
    public CurrencyPair? CurrencyPair { get; set; }

    [BinarySerialize(key: false)]
    public bool? ExchangeSendsVolume { get; set; }

    [BinarySerialize(key: false)]
    public bool? ExchangeSendsHigh { get; set; }

    [BinarySerialize(key: false)]
    public bool? ExchangeSendsLow { get; set; }

    [BinarySerialize(key: false)]
    public bool? ExchangeSendsOpen { get; set; }

    [BinarySerialize(key: false)]
    public bool? ConsolidatedFeedInstrument { get; set; }

    [BinarySerialize(key: false)]
    public bool? OpenOutcryInstrument { get; set; }

    [BinarySerialize(key: false)]
    public bool? SyntheticAmericanOptionInstrument { get; set; }

    [BinarySerialize(key: false)]
    public string? BarchartExchangeCode { get; set; }

    [BinarySerialize(key: false)]
    public string? BarchartBaseCode { get; set; }

    [BinarySerialize(key: false)]
    public int? VolumeDenominator { get; set; }

    [BinarySerialize(key: false)]
    public int? BidOfferQuantityDenominator { get; set; }

    [BinarySerialize(key: false)]
    public string? PrimaryListingMarketParticipantId { get; set; }

    [BinarySerialize(key: false)]
    public string? SubscriptionSymbol { get; set; }

    [BinarySerialize(key: false)]
    public MaturityDate? ContractMaturity { get; set; }

    [BinarySerialize(key: false)]
    public string? Underlying { get; set; }

    [BinarySerialize(key: false)]
    public string? Commodity { get; set; }

    [BinarySerialize(key: false)]
    public long? UnderlyingOpenfeedMarketId { get; set; }
}

public class MarketState
{
    [BinarySerialize(key: false)]
    public int? TradeDate { get; set; }

    [BinarySerialize(key: false)]
    public string? Symbol { get; set; }

    [BinarySerialize(key: false)]
    public int? PriceDenominator { get; set; }

    [BinarySerialize(key: false)]
    public BestBidOffer? BBO { get; set; }

    [BinarySerialize(key: false)]
    public IndexValue? Index { get; set; }

    [BinarySerialize(key: false)]
    public Open? Open { get; set; }

    [BinarySerialize(key: false)]
    public High? High { get; set; }

    [BinarySerialize(key: false)]
    public Low? Low { get; set; }

    [BinarySerialize(key: false)]
    public Close? Close { get; set; }

    [BinarySerialize(key: false)]
    public PrevClose? PrevClose { get; set; }

    [BinarySerialize(key: false)]
    public Last? Last { get; set; }

    [BinarySerialize(key: false)]
    public YearHigh? YearHigh { get; set; }

    [BinarySerialize(key: false)]
    public YearLow? YearLow { get; set; }

    [BinarySerialize(key: false)]
    public Volume? Volume { get; set; }

    [BinarySerialize(key: false)]
    public Vwap? Vwap { get; set; }

    [BinarySerialize(key: false)]
    public NumberOfTrades? NumberOfTrades { get; set; }

    [BinarySerialize(key: false)]
    public MarketSession? PreviousSession { get; set; }

    [BinarySerialize(key: false)]
    public MarketSession? TSession { get; set; }

    [BinarySerialize(key: false)]
    public VolumeAtPrice? VolumeAtPrice { get; set; }

    [BinarySerialize(key: false)]
    public HighRolling? HighRolling { get; set; }

    [BinarySerialize(key: false)]
    public LowRolling? LowRolling { get; set; }

    [BinarySerialize(key: false)]
    public MarketSession? ZSession { get; set; }

    [BinarySerialize(key: false)]
    public MarketSession? Session { get; set; }

    [BinarySerialize(key: false)]
    public MarketSummary? MarketSummary { get; set; }
}