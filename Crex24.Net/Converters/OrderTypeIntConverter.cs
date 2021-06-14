using Crex24.Net.Objects;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace Crex24.Net.Converters
{
    internal class OrderTypeIntConverter : BaseConverter<OrderType>
    {
        public OrderTypeIntConverter() : this(true) { }
        public OrderTypeIntConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OrderType, string>> Mapping => new List<KeyValuePair<OrderType, string>>
        {
            new KeyValuePair<OrderType, string>(OrderType.Limit, "1"),
            new KeyValuePair<OrderType, string>(OrderType.Market, "2")
        };
    }
}
