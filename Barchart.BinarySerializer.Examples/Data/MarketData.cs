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

    [Serialize]
    public SubscriptionResponse? SubscriptionResponse { get; set; }

    [Serialize]
    public InstrumentDefinition? InstrumentDefinition { get; set; }

    [Serialize]
    public MarketState? MarketState { get; set; }

    #endregion
}

public class SubscriptionResponse
{
    #region Properties

    [Serialize(true)]
    public string? Symbol { get; set; }

    [Serialize]
    public long? CorrelationId { get; set; }

    [Serialize]
    public Status? Status { get; set; }

    [Serialize(true)]
    public long? MarketId { get; set; }

    [Serialize]
    public string? Exchange { get; set; }

    [Serialize]
    public int? ChannelId { get; set; }

    [Serialize]
    public int? NumberOfDefinitions { get; set; }

    [Serialize]
    public SubscriptionType? SubscriptionType { get; set; }

    [Serialize]
    public bool? Unsubscribe { get; set; }

    [Serialize]
    public int? SnapshotIntervalSeconds { get; set; }

    #endregion
}

public class InstrumentDefinition
{
    #region Properties

    [Serialize(true)]
    public long? MarketId { get; set; }

    [Serialize]
    public InstrumentType? InstrumentType { get; set; }

    [Serialize]
    public List<BookType>? SupportBookTypes { get; set; }

    [Serialize]
    public int? BookDepth { get; set; }

    [Serialize]
    public string? VendorId { get; set; }

    [Serialize(true)]
    public string? Symbol { get; set; }

    [Serialize]
    public string? Description { get; set; }

    [Serialize]
    public string? CfiCode { get; set; }

    [Serialize]
    public string? CurrencyCode { get; set; }

    [Serialize]
    public string? ExchangeCode { get; set; }

    [Serialize]
    public float? MinimumPriceIncrement { get; set; }

    [Serialize]
    public float? ContractPointValue { get; set; }

    [Serialize]
    public Schedule? Schedule { get; set; }

    [Serialize]
    public Calendar? Calendar { get; set; }

    [Serialize]
    public long? RecordCreateTime { get; set; }

    [Serialize]
    public long? RecordUpdateTime { get; set; }

    [Serialize]
    public string? TimeZoneName { get; set; }

    [Serialize]
    public string? InstrumentGroup { get; set; }

    [Serialize]
    public MaturityDate? SymbolExpiration { get; set; }

    [Serialize]
    public State? State { get; set; }

    [Serialize]
    public int? Channel { get; set; }

    [Serialize]
    public long? UnderlyingMarketId { get; set; }

    [Serialize]
    public PriceFormat? PriceFormat { get; set; }

    [Serialize]
    public PriceFormat? OptionStrikePriceFormat { get; set; }

    [Serialize]
    public int? PriceDenominator { get; set; }

    [Serialize]
    public int? QuantityDenominator { get; set; }

    [Serialize]
    public bool? IsTradable { get; set; }

    [Serialize]
    public long? TransactionTime { get; set; }

    [Serialize]
    public string? AuxiliaryData { get; set; }

    [Serialize]
    public List<Symbol>? Symbols { get; set; }

    [Serialize]
    public long? OptionStrike { get; set; }

    [Serialize]
    public OptionType? OptionType { get; set; }

    [Serialize]
    public OptionStyle? OptionStyle { get; set; }

    [Serialize]
    public int? OptionStrikeDenominator { get; set; }

    [Serialize]
    public string? SpreadCode { get; set; }

    [Serialize]
    public List<SpreadLeg>? SpreadLeg { get; set; }

    [Serialize]
    public bool? UserDefinedSpread { get; set; }

    [Serialize]
    public string? MarketTier { get; set; }

    [Serialize]
    public string? FinancialStatusIndicator { get; set; }

    [Serialize]
    public string? Isin { get; set; }

    [Serialize]
    public CurrencyPair? CurrencyPair { get; set; }

    [Serialize]
    public bool? ExchangeSendsVolume { get; set; }

    [Serialize]
    public bool? ExchangeSendsHigh { get; set; }

    [Serialize]
    public bool? ExchangeSendsLow { get; set; }

    [Serialize]
    public bool? ExchangeSendsOpen { get; set; }

    [Serialize]
    public bool? ConsolidatedFeedInstrument { get; set; }

    [Serialize]
    public bool? OpenOutcryInstrument { get; set; }

    [Serialize]
    public bool? SyntheticAmericanOptionInstrument { get; set; }

    [Serialize]
    public string? BarchartExchangeCode { get; set; }

    [Serialize]
    public string? BarchartBaseCode { get; set; }

    [Serialize]
    public int? VolumeDenominator { get; set; }

    [Serialize]
    public int? BidOfferQuantityDenominator { get; set; }

    [Serialize]
    public string? PrimaryListingMarketParticipantId { get; set; }

    [Serialize]
    public string? SubscriptionSymbol { get; set; }

    [Serialize]
    public MaturityDate? ContractMaturity { get; set; }

    [Serialize]
    public string? Underlying { get; set; }

    [Serialize]
    public string? Commodity { get; set; }

    [Serialize]
    public long? UnderlyingOpenfeedMarketId { get; set; }

    #endregion
}

public class MarketState
{
    #region Properties

    [Serialize]
    public int? TradeDate { get; set; }

    [Serialize]
    public string? Symbol { get; set; }

    [Serialize]
    public int? PriceDenominator { get; set; }

    [Serialize]
    public InstrumentStatus? InstrumentStatus { get; set; }

    [Serialize]
    public BestBidOffer? BBO { get; set; }

    [Serialize]
    public IndexValue? Index { get; set; }

    [Serialize]
    public Open? Open { get; set; }

    [Serialize]
    public High? High { get; set; }

    [Serialize]
    public Low? Low { get; set; }

    [Serialize]
    public Close? Close { get; set; }

    [Serialize]
    public PrevClose? PrevClose { get; set; }

    [Serialize]
    public Last? Last { get; set; }

    [Serialize]
    public YearHigh? YearHigh { get; set; }

    [Serialize]
    public YearLow? YearLow { get; set; }

    [Serialize]
    public Volume? Volume { get; set; }

    [Serialize]
    public Vwap? Vwap { get; set; }

    [Serialize]
    public NumberOfTrades? NumberOfTrades { get; set; }

    [Serialize]
    public MarketSession? PreviousSession { get; set; }

    [Serialize]
    public MarketSession? TSession { get; set; }

    [Serialize]
    public VolumeAtPrice? VolumeAtPrice { get; set; }

    [Serialize]
    public HighRolling? HighRolling { get; set; }

    [Serialize]
    public LowRolling? LowRolling { get; set; }

    [Serialize]
    public MarketSession? ZSession { get; set; }

    [Serialize]
    public MarketSession? Session { get; set; }

    [Serialize]
    public MarketSummary? MarketSummary { get; set; }

    [Serialize]
    public long? TransactionTime { get; set; }

    [Serialize(true)]
    public long? MarketId { get; set; }

    #endregion
}

public class BestBidOffer 
{
    #region Properties

    [Serialize]
    public long? TransactionTime { get; set; }

    [Serialize]
    public long? BidPrice { get; set; }

    [Serialize]
    public long? BidQuantity { get; set; }

    [Serialize]
    public int? BidOrderCount { get; set; }

    [Serialize]
    public string? BidOriginator { get; set; }

    [Serialize]
    public string? BidQuoteCondition { get; set; }

    [Serialize]
    public long? OfferPrice { get; set; }

    [Serialize]
    public long? OfferQuantity { get; set; }

    [Serialize]
    public int? OfferOrderCount { get; set; }

    [Serialize]
    public string? OfferOriginator { get; set; }
    
    [Serialize]
    public string? OfferQuoteCondition { get; set; }
    
    [Serialize]
    public string? QuoteCondition { get; set; }

    [Serialize]
    public bool? Regional { get; set; }

    [Serialize]
    public bool? Transient { get; set; }

    [Serialize]
    public long? MarketId { get; set; }

    #endregion
}

public class VolumeAtPrice 
{
    #region Properties

    [Serialize]
    public long? MarketId { get; set; }
    
    [Serialize]
    public string? Symbol { get; set; }
    
    [Serialize]
    public long? TransactionTime { get; set; }
    
    [Serialize]
    public long? LastPrice { get; set; }
    
    [Serialize]
    public long? LastQuantity { get; set; }
    
    [Serialize]
    public long? LastCumulativeVolume { get; set; }
    
    [Serialize]
    public int? TradeDate { get; set; } 
    
    [Serialize]
    public List<PriceLevelVolume>? PriceVolumes { get; set; }
    
    #endregion
}

public class MarketSummary 
{
    #region Properties

    [Serialize]
    public long? TransactionTime { get; set; }
    
    [Serialize]
    public int? TradingDate { get; set; } 
    
    [Serialize]
    public bool? StartOfDay { get; set; }
    
    [Serialize]
    public bool? EndOfDay { get; set; }
    
    [Serialize]
    public ClearSet? Clear { get; set; }
    
    [Serialize]
    public InstrumentStatus? InstrumentStatus { get; set; }
    
    [Serialize]
    public BestBidOffer? Bbo { get; set; }
    
    [Serialize]
    public Open? Open { get; set; }
    
    [Serialize]
    public High? High { get; set; }
    
    [Serialize]
    public Low? Low { get; set; }
    
    [Serialize]
    public Close? Close { get; set; }
    
    [Serialize]
    public PrevClose? PrevClose { get; set; }
    
    [Serialize]
    public Last? Last { get; set; }
    
    [Serialize]
    public Volume? Volume { get; set; }
    
    [Serialize]
    public Settlement? Settlement { get; set; }
    
    [Serialize]
    public OpenInterest? OpenInterest { get; set; }
    
    [Serialize]
    public Vwap? Vwap { get; set; }
    
    [Serialize]
    public string? Session { get; set; }
    
    [Serialize]
    public SummaryType? SummaryType { get; set; }
    
    [Serialize]
    public Volume? PrevVolume { get; set; }
    
    [Serialize]
    public bool? Transient { get; set; }

    #endregion
}