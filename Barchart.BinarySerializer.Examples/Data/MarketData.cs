#region Using Statements

using Barchart.BinarySerializer.Attributes;

using Org.Openfeed;
using static Org.Openfeed.InstrumentDefinition.Types;
using static Org.Openfeed.MarketSummary.Types;
using static Org.Openfeed.VolumeAtPrice.Types;

#endregion

namespace Barchart.BinarySerializer.Examples.Data;

public class MarketData
{
    #region Properties

    [Serialize(key: false)]
    public SubscriptionResponse? SubscriptionResponse { get; set; }

    [Serialize(key: false)]
    public InstrumentDefinition? InstrumentDefinition { get; set; }

    [Serialize(key: false)]
    public MarketState? MarketState { get; set; }

    #endregion
}

public class SubscriptionResponse
{
    #region Properties

    [Serialize(key: true)]
    public string? Symbol { get; set; }

    [Serialize(key: false)]
    public long? CorrelationId { get; set; }

    [Serialize(key: false)]
    public Status? Status { get; set; }

    [Serialize(key: false)]
    public long? MarketId { get; set; }

    [Serialize(key: false)]
    public string? Exchange { get; set; }

    [Serialize(key: false)]
    public int? ChannelId { get; set; }

    [Serialize(key: false)]
    public int? NumberOfDefinitions { get; set; }

    [Serialize(key: false)]
    public SubscriptionType? SubscriptionType { get; set; }

    [Serialize(key: false)]
    public bool? Unsubscribe { get; set; }

    [Serialize(key: false)]
    public int? SnapshotIntervalSeconds { get; set; }

    #endregion
}

public class InstrumentDefinition
{
    #region Properties

    [Serialize(key: false)]
    public long? MarketId { get; set; }

    [Serialize(key: false)]
    public InstrumentType? InstrumentType { get; set; }

    [Serialize(key: false)]
    public List<BookType>? SupportBookTypes { get; set; }

    [Serialize(key: false)]
    public int? BookDepth { get; set; }

    [Serialize(key: false)]
    public string? VendorId { get; set; }

    [Serialize(key: true)]
    public string? Symbol { get; set; }

    [Serialize(key: false)]
    public string? Description { get; set; }

    [Serialize(key: false)]
    public string? CfiCode { get; set; }

    [Serialize(key: false)]
    public string? CurrencyCode { get; set; }

    [Serialize(key: false)]
    public string? ExchangeCode { get; set; }

    [Serialize(key: false)]
    public float? MinimumPriceIncrement { get; set; }

    [Serialize(key: false)]
    public float? ContractPointValue { get; set; }

    [Serialize(key: false)]
    public Schedule? Schedule { get; set; }

    [Serialize(key: false)]
    public Calendar? Calendar { get; set; }

    [Serialize(key: false)]
    public long? RecordCreateTime { get; set; }

    [Serialize(key: false)]
    public long? RecordUpdateTime { get; set; }

    [Serialize(key: false)]
    public string? TimeZoneName { get; set; }

    [Serialize(key: false)]
    public string? InstrumentGroup { get; set; }

    [Serialize(key: false)]
    public MaturityDate? SymbolExpiration { get; set; }

    [Serialize(key: false)]
    public State? State { get; set; }

    [Serialize(key: false)]
    public int? Channel { get; set; }

    [Serialize(key: false)]
    public long? UnderlyingMarketId { get; set; }

    [Serialize(key: false)]
    public PriceFormat? PriceFormat { get; set; }

    [Serialize(key: false)]
    public PriceFormat? OptionStrikePriceFormat { get; set; }

    [Serialize(key: false)]
    public int? PriceDenominator { get; set; }

    [Serialize(key: false)]
    public int? QuantityDenominator { get; set; }

    [Serialize(key: false)]
    public bool? IsTradable { get; set; }

    [Serialize(key: false)]
    public long? TransactionTime { get; set; }

    [Serialize(key: false)]
    public string? AuxiliaryData { get; set; }

    [Serialize(key: false)]
    public List<Symbol>? Symbols { get; set; }

    [Serialize(key: false)]
    public long? OptionStrike { get; set; }

    [Serialize(key: false)]
    public OptionType? OptionType { get; set; }

    [Serialize(key: false)]
    public OptionStyle? OptionStyle { get; set; }

    [Serialize(key: false)]
    public int? OptionStrikeDenominator { get; set; }

    [Serialize(key: false)]
    public string? SpreadCode { get; set; }

    [Serialize(key: false)]
    public List<SpreadLeg>? SpreadLeg { get; set; }

    [Serialize(key: false)]
    public bool? UserDefinedSpread { get; set; }

    [Serialize(key: false)]
    public string? MarketTier { get; set; }

    [Serialize(key: false)]
    public string? FinancialStatusIndicator { get; set; }

    [Serialize(key: false)]
    public string? Isin { get; set; }

    [Serialize(key: false)]
    public CurrencyPair? CurrencyPair { get; set; }

    [Serialize(key: false)]
    public bool? ExchangeSendsVolume { get; set; }

    [Serialize(key: false)]
    public bool? ExchangeSendsHigh { get; set; }

    [Serialize(key: false)]
    public bool? ExchangeSendsLow { get; set; }

    [Serialize(key: false)]
    public bool? ExchangeSendsOpen { get; set; }

    [Serialize(key: false)]
    public bool? ConsolidatedFeedInstrument { get; set; }

    [Serialize(key: false)]
    public bool? OpenOutcryInstrument { get; set; }

    [Serialize(key: false)]
    public bool? SyntheticAmericanOptionInstrument { get; set; }

    [Serialize(key: false)]
    public string? BarchartExchangeCode { get; set; }

    [Serialize(key: false)]
    public string? BarchartBaseCode { get; set; }

    [Serialize(key: false)]
    public int? VolumeDenominator { get; set; }

    [Serialize(key: false)]
    public int? BidOfferQuantityDenominator { get; set; }

    [Serialize(key: false)]
    public string? PrimaryListingMarketParticipantId { get; set; }

    [Serialize(key: false)]
    public string? SubscriptionSymbol { get; set; }

    [Serialize(key: false)]
    public MaturityDate? ContractMaturity { get; set; }

    [Serialize(key: false)]
    public string? Underlying { get; set; }

    [Serialize(key: false)]
    public string? Commodity { get; set; }

    [Serialize(key: false)]
    public long? UnderlyingOpenfeedMarketId { get; set; }

    #endregion
}

public class MarketState
{
    #region Properties

    [Serialize(key: false)]
    public int? TradeDate { get; set; }

    [Serialize(key: true)]
    public string? Symbol { get; set; }

    [Serialize(key: false)]
    public int? PriceDenominator { get; set; }

    [Serialize(key: false)]
    public InstrumentStatus? InstrumentStatus { get; set; }

    [Serialize(key: false)]
    public BestBidOffer? BBO { get; set; }

    [Serialize(key: false)]
    public IndexValue? Index { get; set; }

    [Serialize(key: false)]
    public Open? Open { get; set; }

    [Serialize(key: false)]
    public High? High { get; set; }

    [Serialize(key: false)]
    public Low? Low { get; set; }

    [Serialize(key: false)]
    public Close? Close { get; set; }

    [Serialize(key: false)]
    public PrevClose? PrevClose { get; set; }

    [Serialize(key: false)]
    public Last? Last { get; set; }

    [Serialize(key: false)]
    public YearHigh? YearHigh { get; set; }

    [Serialize(key: false)]
    public YearLow? YearLow { get; set; }

    [Serialize(key: false)]
    public Volume? Volume { get; set; }

    [Serialize(key: false)]
    public Vwap? Vwap { get; set; }

    [Serialize(key: false)]
    public NumberOfTrades? NumberOfTrades { get; set; }

    [Serialize(key: false)]
    public MarketSession? PreviousSession { get; set; }

    [Serialize(key: false)]
    public MarketSession? TSession { get; set; }

    [Serialize(key: false)]
    public VolumeAtPrice? VolumeAtPrice { get; set; }

    [Serialize(key: false)]
    public HighRolling? HighRolling { get; set; }

    [Serialize(key: false)]
    public LowRolling? LowRolling { get; set; }

    [Serialize(key: false)]
    public MarketSession? ZSession { get; set; }

    [Serialize(key: false)]
    public MarketSession? Session { get; set; }

    [Serialize(key: false)]
    public MarketSummary? MarketSummary { get; set; }

    [Serialize(key: false)]
    public long? TransactionTime { get; set; }

    #endregion
}

public class BestBidOffer {

    #region Properties

    [Serialize(key: false)]
    public long? TransactionTime { get; set; }

    [Serialize(key: false)]
    public long? BidPrice { get; set; }

    [Serialize(key: false)]
    public long? BidQuantity { get; set; }

    [Serialize(key: false)]
    public int? BidOrderCount { get; set; }

    [Serialize(key: false)]
    public string? BidOriginator { get; set; }

    [Serialize(key: false)]
    public string? BidQuoteCondition { get; set; }

    [Serialize(key: false)]
    public long? OfferPrice { get; set; }

    [Serialize(key: false)]
    public long? OfferQuantity { get; set; }

    [Serialize(key: false)]
    public int? OfferOrderCount { get; set; }

    [Serialize(key: false)]
    public string? OfferOriginator { get; set; }
    
    [Serialize(key: false)]
    public string? OfferQuoteCondition { get; set; }
    
    [Serialize(key: false)]
    public string? QuoteCondition { get; set; }

    [Serialize(key: false)]
    public bool? Regional { get; set; }

    [Serialize(key: false)]
    public bool? Transient { get; set; }

    #endregion
}

public class VolumeAtPrice {

    #region Properties

    [Serialize(key: false)]
    public long? MarketId { get; set; }
    
    [Serialize(key: false)]
    public string? Symbol { get; set; }
    
    [Serialize(key: false)]
    public long? TransactionTime { get; set; }
    
    [Serialize(key: false)]
    public long? LastPrice { get; set; }
    
    [Serialize(key: false)]
    public long? LastQuantity { get; set; }
    
    [Serialize(key: false)]
    public long? LastCumulativeVolume { get; set; }
    
    [Serialize(key: false)]
    public int? TradeDate { get; set; } 
    
    [Serialize(key: false)]
    public List<PriceLevelVolume>? PriceVolumes { get; set; }
    
    #endregion
}

public class MarketSummary {

    #region Properties

    [Serialize(key: false)]
    public long? TransactionTime { get; set; }
    
    [Serialize(key: false)]
    public int? TradingDate { get; set; } 
    
    [Serialize(key: false)]
    public bool? StartOfDay { get; set; }
    
    [Serialize(key: false)]
    public bool? EndOfDay { get; set; }
    
    [Serialize(key: false)]
    public ClearSet? Clear { get; set; }
    
    [Serialize(key: false)]
    public InstrumentStatus? InstrumentStatus { get; set; }
    
    [Serialize(key: false)]
    public BestBidOffer? Bbo { get; set; }
    
    [Serialize(key: false)]
    public Open? Open { get; set; }
    
    [Serialize(key: false)]
    public High? High { get; set; }
    
    [Serialize(key: false)]
    public Low? Low { get; set; }
    
    [Serialize(key: false)]
    public Close? Close { get; set; }
    
    [Serialize(key: false)]
    public PrevClose? PrevClose { get; set; }
    
    [Serialize(key: false)]
    public Last? Last { get; set; }
    
    [Serialize(key: false)]
    public Volume? Volume { get; set; }
    
    [Serialize(key: false)]
    public Settlement? Settlement { get; set; }
    
    [Serialize(key: false)]
    public OpenInterest? OpenInterest { get; set; }
    
    [Serialize(key: false)]
    public Vwap? Vwap { get; set; }
    
    [Serialize(key: false)]
    public string? Session { get; set; }
    
    [Serialize(key: false)]
    public SummaryType? SummaryType { get; set; }
    
    [Serialize(key: false)]
    public Volume? PrevVolume { get; set; }
    
    [Serialize(key: false)]
    public bool? Transient { get; set; }

    #endregion
}