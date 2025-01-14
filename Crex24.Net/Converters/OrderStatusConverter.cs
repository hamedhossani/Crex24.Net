﻿using Crex24.Net.Objects;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace Crex24.Net.Converters
{
    internal class OrderStatusConverter: BaseConverter<OrderStatus>
    {
        public OrderStatusConverter() : this(true) { }
        public OrderStatusConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OrderStatus, string>> Mapping => new List<KeyValuePair<OrderStatus, string>>
        {
            new KeyValuePair<OrderStatus, string>(OrderStatus.Executed, "done"),
            new KeyValuePair<OrderStatus, string>(OrderStatus.PartiallyExecuted, "part_deal"),
            new KeyValuePair<OrderStatus, string>(OrderStatus.UnExecuted, "not_deal"),
            new KeyValuePair<OrderStatus, string>(OrderStatus.Canceled, "cancel")
        };
    }
}
