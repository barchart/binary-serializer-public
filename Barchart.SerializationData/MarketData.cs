using Barchart.BinarySerializer.Schemas;
using Google.Protobuf;
using Org.Openfeed;
using static Org.Openfeed.InstrumentDefinition.Types;

namespace Barchart.SerializationData;

public class MarketData
{
    [BinarySerialize(include: true, key: false)]
    public SubscriptionResponse? SubscriptionResponse { get; set; }

    [BinarySerialize(include: true, key: false)]
    public InstrumentDefinition? InstrumentDefinition { get; set; }

    [BinarySerialize(include: true, key: false)]
    public MarketSnapshot? MarketSnapshot { get; set; }

    [BinarySerialize(include: true, key: false)]
    public MarketUpdate? MarketUpdate{ get; set; }
}

public class SubscriptionResponse
{
    [BinarySerialize(include: true, key: true)]
    public string? Symbol { get; set; }

    [BinarySerialize(include: true, key: false)]
    public long? CorrelationId { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Status? Status { get; set; }

    [BinarySerialize(include: true, key: false)]
    public long? MarketId { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? Exchange { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? ChannelId { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? NumberOfDefinitions { get; set; }

    [BinarySerialize(include: true, key: false)]
    public SubscriptionType? SubscriptionType { get; set; }

    [BinarySerialize(include: true, key: false)]
    public bool? Unsubscribe { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? SnapshotIntervalSeconds { get; set; }
}

public class InstrumentDefinition
{
    [BinarySerialize(include: true, key: false)]
    public long? MarketId { get; set; }

    [BinarySerialize(include: true, key: false)]
    public InstrumentType? InstrumentType { get; set; }

    [BinarySerialize(include: true, key: false)]
    public List<BookType>? SupportBookTypes { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? BookDepth { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? VendorId { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? Symbol { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? Description { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? CfiCode { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? CurrencyCode { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? ExchangeCode { get; set; }

    [BinarySerialize(include: true, key: false)]
    public float? MinimumPriceIncrement { get; set; }

    [BinarySerialize(include: true, key: false)]
    public float? ContractPointValue { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Schedule? Schedule { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Calendar? Calendar { get; set; }

    [BinarySerialize(include: true, key: false)]
    public long? RecordCreateTime { get; set; }

    [BinarySerialize(include: true, key: false)]
    public long? RecordUpdateTime { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? TimeZoneName { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? InstrumentGroup { get; set; }

    [BinarySerialize(include: true, key: false)]
    public MaturityDate? SymbolExpiration { get; set; }

    [BinarySerialize(include: true, key: false)]
    public State? State { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? Channel { get; set; }

    [BinarySerialize(include: true, key: false)]
    public long? UnderlyingMarketId { get; set; }

    [BinarySerialize(include: true, key: false)]
    public PriceFormat? PriceFormat { get; set; }

    [BinarySerialize(include: true, key: false)]
    public PriceFormat? OptionStrikePriceFormat { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? PriceDenominator { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? QuantityDenominator { get; set; }

    [BinarySerialize(include: true, key: false)]
    public bool? IsTradable { get; set; }

    [BinarySerialize(include: true, key: false)]
    public long? TransactionTime { get; set; }

    [BinarySerialize(include: true, key: false)]
    public ByteString? AuxiliaryData { get; set; }

    [BinarySerialize(include: true, key: false)]
    public List<Symbol>? Symbols { get; set; }

    [BinarySerialize(include: true, key: false)]
    public long? OptionStrike { get; set; }

    [BinarySerialize(include: true, key: false)]
    public OptionType? OptionType { get; set; }

    [BinarySerialize(include: true, key: false)]
    public OptionStyle? OptionStyle { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? OptionStrikeDenominator { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? SpreadCode { get; set; }

    [BinarySerialize(include: true, key: false)]
    public List<SpreadLeg>? SpreadLeg { get; set; }

    [BinarySerialize(include: true, key: false)]
    public bool? UserDefinedSpread { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? MarketTier { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? FinancialStatusIndicator { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? Isin { get; set; }

    [BinarySerialize(include: true, key: false)]
    public CurrencyPair? CurrencyPair { get; set; }

    [BinarySerialize(include: true, key: false)]
    public bool? ExchangeSendsVolume { get; set; }

    [BinarySerialize(include: true, key: false)]
    public bool? ExchangeSendsHigh { get; set; }

    [BinarySerialize(include: true, key: false)]
    public bool? ExchangeSendsLow { get; set; }

    [BinarySerialize(include: true, key: false)]
    public bool? ExchangeSendsOpen { get; set; }

    [BinarySerialize(include: true, key: false)]
    public bool? ConsolidatedFeedInstrument { get; set; }

    [BinarySerialize(include: true, key: false)]
    public bool? OpenOutcryInstrument { get; set; }

    [BinarySerialize(include: true, key: false)]
    public bool? SyntheticAmericanOptionInstrument { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? BarchartExchangeCode { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? BarchartBaseCode { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? VolumeDenominator { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? BidOfferQuantityDenominator { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? PrimaryListingMarketParticipantId { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? SubscriptionSymbol { get; set; }

    [BinarySerialize(include: true, key: false)]
    public MaturityDate? ContractMaturity { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? Underlying { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? Commodity { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? ExchangeId { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? PriceScalingExponent { get; set; }

    [BinarySerialize(include: true, key: false)]
    public long? UnderlyingOpenfeedMarketId { get; set; }
}

public class MarketSnapshot
{
    [BinarySerialize(include: true, key: false)]
    public long? MarketId { get; set; }

    [BinarySerialize(include: true, key: false)]
    public long? TransactionTime { get; set; }

    [BinarySerialize(include: true, key: false)]
    public long? MarketSequence { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? TradeDate { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? TotalChunks { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? CurrentChunk { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? Symbol { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? PriceDenominator { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Service? Service { get; set; }

    [BinarySerialize(include: true, key: false)]
    public InstrumentStatus? InstrumentStatus { get; set; }

    [BinarySerialize(include: true, key: false)]
    public BestBidOffer? BBO { get; set; }

    [BinarySerialize(include: true, key: false)]
    public IndexValue? Index { get; set; }

    [BinarySerialize(include: true, key: false)]
    public List<AddPriceLevel>? PriceLevels { get; set; }

    [BinarySerialize(include: true, key: false)]
    public List<AddOrder>? Orders { get; set; }

    [BinarySerialize(include: true, key: false)]
    public News? News { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Open? Open { get; set; }

    [BinarySerialize(include: true, key: false)]
    public High? High { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Low? Low { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Close? Close { get; set; }

    [BinarySerialize(include: true, key: false)]
    public PrevClose? PrevClose { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Last? Last { get; set; }

    [BinarySerialize(include: true, key: false)]
    public YearHigh? YearHigh { get; set; }

    [BinarySerialize(include: true, key: false)]
    public YearLow? YearLow { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Volume? Volume { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Settlement? Settlement { get; set; }

    [BinarySerialize(include: true, key: false)]
    public OpenInterest? OpenInterest { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Vwap? Vwap { get; set; }

    [BinarySerialize(include: true, key: false)]
    public DividendsIncomeDistributions? DividendsIncomeDistributions { get; set; }

    [BinarySerialize(include: true, key: false)]
    public NumberOfTrades? NumberOfTrades { get; set; }

    [BinarySerialize(include: true, key: false)]
    public MonetaryValue? MonetaryValue { get; set; }

    [BinarySerialize(include: true, key: false)]
    public CapitalDistributions? CapitalDistributions { get; set; }

    [BinarySerialize(include: true, key: false)]
    public SharesOutstanding? SharesOutstanding { get; set; }

    [BinarySerialize(include: true, key: false)]
    public NetAssetValue? NetAssetValue { get; set; }

    [BinarySerialize(include: true, key: false)]
    public MarketSession? PreviousSession { get; set; }

    [BinarySerialize(include: true, key: false)]
    public MarketSession? TSession { get; set; }

    [BinarySerialize(include: true, key: false)]
    public VolumeAtPrice? VolumeAtPrice { get; set; }

    [BinarySerialize(include: true, key: false)]
    public HighRolling? HighRolling { get; set; }

    [BinarySerialize(include: true, key: false)]
    public LowRolling? LowRolling { get; set; }

    [BinarySerialize(include: true, key: false)]
    public MarketSession? ZSession { get; set; }
}

public class MarketUpdate
{
    [BinarySerialize(include: true, key: false)]
    public long? MarketId { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? Symbol { get; set; }

    [BinarySerialize(include: true, key: false)]
    public long? TransactionTime { get; set; }

    [BinarySerialize(include: true, key: false)]
    public long? DistributionTime { get; set; }

    [BinarySerialize(include: true, key: false)]
    public long? MarketSequence { get; set; }

    [BinarySerialize(include: true, key: false)]
    public long? SourceSequence { get; set; }

    [BinarySerialize(include: true, key: false)]
    public ByteString? OriginatorId { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? PriceDenominator { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Context? Context { get; set; }

    [BinarySerialize(include: true, key: false)]
    public MarketSession? Session { get; set; }

    [BinarySerialize(include: true, key: false)]
    public MarketSession? TSession { get; set; }

    [BinarySerialize(include: true, key: false)]
    public MarketSession? PreviousSession { get; set; }

    [BinarySerialize(include: true, key: false)]
    public bool? Regional { get; set; }

    [BinarySerialize(include: true, key: false)]
    public MarketSession? ZSession { get; set; }

    [BinarySerialize(include: true, key: false)]
    public News? News { get; set; }

    [BinarySerialize(include: true, key: false)]
    public ClearBook? ClearBook { get; set; }

    [BinarySerialize(include: true, key: false)]
    public InstrumentStatus? InstrumentStatus { get; set; }

    [BinarySerialize(include: true, key: false)]
    public BestBidOffer? BBO { get; set; }

    [BinarySerialize(include: true, key: false)]
    public DepthPriceLevel? DepthPriceLevel { get; set; }

    [BinarySerialize(include: true, key: false)]
    public DepthOrder? DepthOrder { get; set; }

    [BinarySerialize(include: true, key: false)]
    public IndexValue? Index { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Trades? Trades { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Open? Open { get; set; }

    [BinarySerialize(include: true, key: false)]
    public High? High { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Low? Low { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Close? Close { get; set; }

    [BinarySerialize(include: true, key: false)]
    public PrevClose? PrevClose { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Last? Last { get; set; }

    [BinarySerialize(include: true, key: false)]
    public YearHigh? YearHigh { get; set; }

    [BinarySerialize(include: true, key: false)]
    public YearLow? YearLow { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Volume? Volume { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Settlement? Settlement { get; set; }

    [BinarySerialize(include: true, key: false)]
    public OpenInterest? OpenInterest { get; set; }

    [BinarySerialize(include: true, key: false)]
    public Vwap? Vwap { get; set; }

    [BinarySerialize(include: true, key: false)]
    public DividendsIncomeDistributions? DividendsIncomeDistributions { get; set; }

    [BinarySerialize(include: true, key: false)]
    public NumberOfTrades? NumberOfTrades { get; set; }

    [BinarySerialize(include: true, key: false)]
    public MonetaryValue? MonetaryValue { get; set; }

    [BinarySerialize(include: true, key: false)]
    public CapitalDistributions? CapitalDistributions { get; set; }

    [BinarySerialize(include: true, key: false)]
    public SharesOutstanding? SharesOutstanding { get; set; }

    [BinarySerialize(include: true, key: false)]
    public NetAssetValue? NetAssetValue { get; set; }

    [BinarySerialize(include: true, key: false)]
    public MarketSummary? MarketSummary { get; set; }

    [BinarySerialize(include: true, key: false)]
    public HighRolling? HighRolling { get; set; }

    [BinarySerialize(include: true, key: false)]
    public LowRolling? LowRolling { get; set; }

    [BinarySerialize(include: true, key: false)]
    public RequestForQuote? RequestForQuote { get; set; }
}

public class RequestForQuote
{
    [BinarySerialize(include: true, key: false)]
    public string? QuoteRequestId { get; set; }

    [BinarySerialize(include: true, key: false)]
    public string? Symbol { get; set; }

    [BinarySerialize(include: true, key: false)]
    public long? SecurityId { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? OrderQuantity { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? QuoteType { get; set; }

    [BinarySerialize(include: true, key: false)]
    public int? Side { get; set; }
}