﻿using Crex24.Net.Objects;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace Crex24.Net.Converters
{
    internal class UpdateTypeConverter: BaseConverter<UpdateType>
    {
        public UpdateTypeConverter() : this(true) { }
        public UpdateTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<UpdateType, string>> Mapping => new List<KeyValuePair<UpdateType, string>>
        {
            new KeyValuePair<UpdateType, string>(UpdateType.New, "1"),
            new KeyValuePair<UpdateType, string>(UpdateType.Update, "2"),
            new KeyValuePair<UpdateType, string>(UpdateType.Done, "3")
        };
    }
}
