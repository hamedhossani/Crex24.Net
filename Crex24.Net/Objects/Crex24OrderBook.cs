using System.Collections.Generic;
using Crex24.Net.Converters;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.ExchangeInterfaces;
using CryptoExchange.Net.Interfaces;
using Newtonsoft.Json;

namespace Crex24.Net.Objects
{
    /// <summary>
    /// Order book
    /// </summary>
    public class Crex24OrderBook: ICommonOrderBook
    {
        /// <summary>
        /// The price of the last transaction
        /// </summary>
        [JsonConverter(typeof(DecimalConverter))]
        public decimal Last { get; set; }
        /// <summary>
        /// The asks on this symbol
        /// </summary>
        public IEnumerable<Crex24DepthEntry> Asks { get; set; } = new List<Crex24DepthEntry>();
        /// <summary>
        /// The bids on this symbol
        /// </summary>
        public IEnumerable<Crex24DepthEntry> Bids { get; set; } = new List<Crex24DepthEntry>();

        IEnumerable<ISymbolOrderBookEntry> ICommonOrderBook.CommonBids => Bids;
        IEnumerable<ISymbolOrderBookEntry> ICommonOrderBook.CommonAsks => Asks;
    }

    /// <summary>
    /// Depth info
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public class Crex24DepthEntry: ISymbolOrderBookEntry
    {
        /// <summary>
        /// The price per unit of the entry
        /// </summary>
        [ArrayProperty(0)]
        public decimal Price { get; set; }
        /// <summary>
        /// The quantity of the entry
        /// </summary>
        [ArrayProperty(1)]
        public decimal Quantity { get; set; }
    }
}
