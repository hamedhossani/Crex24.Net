using Crex24.Net.Converters;
using Crex24.Net.Objects;
using Crex24.Net.Objects.Websocket;
using CryptoExchange.Net;
using CryptoExchange.Net.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crex24.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;

namespace Crex24.Net
{
    /// <summary>
    /// Client for the Crex24 socket API
    /// </summary>
    public class Crex24SocketClient: SocketClient, ICrex24SocketClient
    {
        #region fields
        private static Crex24SocketClientOptions defaultOptions = new Crex24SocketClientOptions();
        private static Crex24SocketClientOptions DefaultOptions => defaultOptions.Copy<Crex24SocketClientOptions>();
        
        private const string ServerSubject = "server";
        private const string StateSubject = "state";
        private const string DepthSubject = "depth";
        private const string TransactionSubject = "deals";
        private const string KlineSubject = "kline";
        private const string BalanceSubject = "asset";
        private const string OrderSubject = "order";

        private const string SubscribeAction = "subscribe";
        private const string QueryAction = "query";
        private const string ServerTimeAction = "time";
        private const string PingAction = "ping";
        private const string AuthenticateAction = "sign";

        private const string SuccessString = "success";        
        #endregion

        #region ctor
        /// <summary>
        /// Create a new instance of Crex24SocketClient with default options
        /// </summary>
        public Crex24SocketClient() : this(DefaultOptions)
        {
        }

        /// <summary>
        /// Create a new instance of Crex24SocketClient using provided options
        /// </summary>
        /// <param name="options">The options to use for this client</param>
        public Crex24SocketClient(Crex24SocketClientOptions options) : base("Crex24", options, options.ApiCredentials == null ? null : new Crex24AuthenticationProvider(options.ApiCredentials))
        {
            AddGenericHandler("Pong", (connection, token) => { });
            SendPeriodic(TimeSpan.FromMinutes(1), con => new Crex24SocketRequest(NextId(), ServerSubject, PingAction));
        }
        #endregion

        #region methods
        #region public
        /// <summary>
        /// Set the default options to be used when creating new socket clients
        /// </summary>
        /// <param name="options">The options to use for new clients</param>
        public static void SetDefaultOptions(Crex24SocketClientOptions options)
        {
            defaultOptions = options;
        }

        /// <summary>
        /// Pings the server
        /// </summary>
        /// <returns>True if server responded, false otherwise</returns>
        public CallResult<bool> Ping() => PingAsync().Result;
        /// <summary>
        /// Pings the server
        /// </summary>
        /// <returns>True if server responded, false otherwise</returns>
        public async Task<CallResult<bool>> PingAsync()
        {
            var result = await Query<string>(new Crex24SocketRequest(NextId(), ServerSubject, PingAction), false).ConfigureAwait(false);
            return new CallResult<bool>(result.Success, result.Error);
        }

        /// <summary>
        /// Gets the server time
        /// </summary>
        /// <returns>The server time</returns>
        public CallResult<DateTime> GetServerTime() => GetServerTimeAsync().Result;
        /// <summary>
        /// Gets the server time
        /// </summary>
        /// <returns>The server time</returns>
        public async Task<CallResult<DateTime>> GetServerTimeAsync()
        {
            var result = await Query<long>(new Crex24SocketRequest(NextId(), ServerSubject, ServerTimeAction), false).ConfigureAwait(false);
            if (!result)
                return new CallResult<DateTime>(default, result.Error);
            return new CallResult<DateTime>(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(result.Data), null);
        }

        /// <summary>
        /// Get the symbol state
        /// </summary>
        /// <param name="symbol">The symbol to get the state for</param>
        /// <param name="cyclePeriod">The period to get data over, specified in seconds. i.e. one minute = 60, one day = 86400</param>
        /// <returns>Symbol state</returns>
        public CallResult<Crex24SocketSymbolState> GetSymbolState(string symbol, int cyclePeriod) => GetSymbolStateAsync(symbol, cyclePeriod).Result;
        /// <summary>
        /// Get the symbol state
        /// </summary>
        /// <param name="symbol">The symbol to get the state for</param>
        /// <param name="cyclePeriod">The period to get data over, specified in seconds. i.e. one minute = 60, one day = 86400</param>
        /// <returns>Symbol state</returns>
        public async Task<CallResult<Crex24SocketSymbolState>> GetSymbolStateAsync(string symbol, int cyclePeriod)
        {
            symbol.ValidateCrex24Symbol();
            return await Query<Crex24SocketSymbolState>(new Crex24SocketRequest(NextId(), StateSubject, QueryAction, symbol, cyclePeriod), false).ConfigureAwait(false);
        }

        /// <summary>
        /// Get an order book
        /// </summary>
        /// <param name="symbol">The symbol to get the order book for</param>
        /// <param name="limit">The limit of results returned</param>
        /// <param name="mergeDepth">The depth of merging, based on 8 decimals. 1 mergeDepth will merge the last decimals of all order in the book, 7 will merge the last 7 decimals of all orders together</param>
        /// <returns>Order book for a symbol</returns>
        public CallResult<Crex24SocketOrderBook> GetOrderBook(string symbol, int limit, int mergeDepth) => GetOrderBookAsync(symbol, limit, mergeDepth).Result;
        /// <summary>
        /// Get an order book
        /// </summary>
        /// <param name="symbol">The symbol to get the order book for</param>
        /// <param name="limit">The limit of results returned</param>
        /// <param name="mergeDepth">The depth of merging, based on 8 decimals. 1 mergeDepth will merge the last decimals of all order in the book, 7 will merge the last 7 decimals of all orders together</param>
        /// <returns>Order book of a symbol</returns>
        public async Task<CallResult<Crex24SocketOrderBook>> GetOrderBookAsync(string symbol, int limit, int mergeDepth)
        {
            symbol.ValidateCrex24Symbol();
            mergeDepth.ValidateIntBetween(nameof(mergeDepth), 0, 8);
            limit.ValidateIntValues(nameof(limit), 5, 10, 20);

            return await Query<Crex24SocketOrderBook>(new Crex24SocketRequest(NextId(), DepthSubject, QueryAction, symbol, limit, Crex24Helpers.MergeDepthIntToString(mergeDepth)), false).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the latest trades on a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get the trades for</param>
        /// <param name="limit">The limit of trades</param>
        /// <param name="fromId">Return trades since this id</param>
        /// <returns>List of trades</returns>
        public CallResult<IEnumerable<Crex24SocketSymbolTrade>> GetSymbolTrades(string symbol, int limit, int? fromId = null) => GetSymbolTradesAsync(symbol, limit, fromId).Result;
        /// <summary>
        /// Gets the latest trades on a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get the trades for</param>
        /// <param name="limit">The limit of trades</param>
        /// <param name="fromId">Return trades since this id</param>
        /// <returns>List of trades</returns>
        public async Task<CallResult<IEnumerable<Crex24SocketSymbolTrade>>> GetSymbolTradesAsync(string symbol, int limit, int? fromId = null)
        {
            symbol.ValidateCrex24Symbol();

            return await Query<IEnumerable<Crex24SocketSymbolTrade>>(new Crex24SocketRequest(NextId(), TransactionSubject, QueryAction, symbol, limit, fromId ?? 0), false).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets symbol kline data
        /// </summary>
        /// <param name="symbol">The symbol to get the data for</param>
        /// <param name="interval">The interval of the candles</param>
        /// <returns></returns>
        public CallResult<Crex24Kline> GetKlines(string symbol, KlineInterval interval) => GetKlinesAsync(symbol, interval).Result;
        /// <summary>
        /// Gets symbol kline data
        /// </summary>
        /// <param name="symbol">The symbol to get the data for</param>
        /// <param name="interval">The interval of the candles</param>
        /// <returns></returns>
        public async Task<CallResult<Crex24Kline>> GetKlinesAsync(string symbol, KlineInterval interval)
        {
            symbol.ValidateCrex24Symbol();

            return await Query<Crex24Kline>(new Crex24SocketRequest(NextId(), KlineSubject, QueryAction, symbol, interval.ToSeconds()), false).ConfigureAwait(false);
        }

        /// <summary>
        /// Get balances of coins. Requires API credentials
        /// </summary>
        /// <param name="coins">The coins to get the balances for, empty for all</param>
        /// <returns>Dictionary of coins and their balances</returns>
        public CallResult<Dictionary<string, Crex24Balance>> GetBalances(params string[] coins) => GetBalancesAsync(coins).Result;
        /// <summary>
        /// Get balances of coins. Requires API credentials
        /// </summary>
        /// <param name="coins">The coins to get the balances for, empty for all</param>
        /// <returns>Dictionary of coins and their balances</returns>
        public async Task<CallResult<Dictionary<string, Crex24Balance>>> GetBalancesAsync(params string[] coins)
        {
            return await Query<Dictionary<string, Crex24Balance>>(new Crex24SocketRequest(NextId(), BalanceSubject, QueryAction, coins), true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of open orders for a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get open orders for</param>
        /// <param name="type">The type of orders to get</param>
        /// <param name="offset">The offset in the list</param>
        /// <param name="limit">The limit of results</param>
        /// <returns>List of open orders</returns>
        public CallResult<Crex24SocketPagedResult<Crex24SocketOrder>> GetOpenOrders(string symbol, TransactionType type, int offset, int limit) => GetOpenOrdersAsync(symbol, type, offset, limit).Result;
        /// <summary>
        /// Gets a list of open orders for a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get open orders for</param>
        /// <param name="type">The type of orders to get</param>
        /// <param name="offset">The offset in the list</param>
        /// <param name="limit">The limit of results</param>
        /// <returns>List of open orders</returns>
        public async Task<CallResult<Crex24SocketPagedResult<Crex24SocketOrder>>> GetOpenOrdersAsync(string symbol, TransactionType type, int offset, int limit)
        {
            symbol.ValidateCrex24Symbol();
            return await Query<Crex24SocketPagedResult<Crex24SocketOrder>>(
                new Crex24SocketRequest(NextId(), OrderSubject, QueryAction, symbol, int.Parse(JsonConvert.SerializeObject(type, new TransactionTypeIntConverter(false))), offset, limit), true).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to symbol state updates for a specific symbol
        /// </summary>
        /// <param name="symbol">Symbol to receive updates for</param>
        /// <param name="onMessage">Data handler, receives Param 1[string]: the symbol name, Param 2[Crex24SocketSymbolState]: the symbol state update</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public CallResult<UpdateSubscription> SubscribeToSymbolStateUpdates(string symbol, Action<string, Crex24SocketSymbolState> onMessage) => SubscribeToSymbolStateUpdatesAsync(symbol, onMessage).Result;
        /// <summary>
        /// Subscribe to symbol state updates for a specific symbol
        /// </summary>
        /// <param name="symbol">Symbol to receive updates for</param>
        /// <param name="onMessage">Data handler, receives Param 1[string]: the symbol name, Param 2[Crex24SocketSymbolState]: the symbol state update</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolStateUpdatesAsync(string symbol, Action<string, Crex24SocketSymbolState> onMessage)
        {
            symbol.ValidateCrex24Symbol();
            var internalHandler = new Action<JToken[]>(data =>
            {
                var desResult = Deserialize<Dictionary<string, Crex24SocketSymbolState>>(data[0]);
                if (!desResult)
                {
                    log.Write(LogVerbosity.Warning, "Received invalid state update: " + desResult.Error);
                    return;
                }

                onMessage(symbol, desResult.Data.First().Value);
            });

            return await Subscribe(new Crex24SocketRequest(NextId(), StateSubject, SubscribeAction, symbol), null, false, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to symbol state updates for all symbols
        /// </summary>
        /// <param name="onMessage">Data handler, receives a dictionary of symbol name -> symbol state</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public CallResult<UpdateSubscription> SubscribeToSymbolStateUpdates(Action<Dictionary<string, Crex24SocketSymbolState>> onMessage) => SubscribeToSymbolStateUpdatesAsync(onMessage).Result;
        /// <summary>
        /// Subscribe to symbol state updates for all symbols
        /// </summary>
        /// <param name="onMessage">Data handler, receives a dictionary of symbol name -> symbol state</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolStateUpdatesAsync(Action<Dictionary<string, Crex24SocketSymbolState>> onMessage)
        {
            var internalHandler = new Action<JToken[]>(data =>
            {
                var desResult = Deserialize<Dictionary<string, Crex24SocketSymbolState>>(data[0]);
                if (!desResult)
                {
                    log.Write(LogVerbosity.Warning, "Received invalid state update: " + desResult.Error);
                    return;
                }

                onMessage(desResult.Data);
            });

            return await Subscribe(new Crex24SocketRequest(NextId(), StateSubject, SubscribeAction), null, false, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to order book updates
        /// </summary>
        /// <param name="symbol">The symbol to receive updates for</param>
        /// <param name="limit">The limit of results to receive in a update</param>
        /// <param name="mergeDepth">The depth of merging, based on 8 decimals. 1 mergeDepth will merge the last decimals of all order in the book, 7 will merge the last 7 decimals of all orders together</param>
        /// <param name="onMessage">Data handler, receives Param 1[string]: the symbol name, Param 2[bool]: whether this is a full update, or an update based on the last send data, Param 3[Crex24SocketOrderBook]: the update data</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public CallResult<UpdateSubscription> SubscribeToOrderBookUpdates(string symbol, int limit, int mergeDepth, Action<string, bool, Crex24SocketOrderBook> onMessage) => SubscribeToOrderBookUpdatesAsync(symbol, limit, mergeDepth, onMessage).Result;
        /// <summary>
        /// Subscribe to order book updates
        /// </summary>
        /// <param name="symbol">The symbol to receive updates for</param>
        /// <param name="limit">The limit of results to receive in a update</param>
        /// <param name="mergeDepth">The depth of merging, based on 8 decimals. 1 mergeDepth will merge the last decimals of all order in the book, 7 will merge the last 7 decimals of all orders together</param>
        /// <param name="onMessage">Data handler, receives Param 1[string]: the symbol name, Param 2[bool]: whether this is a full update, or an update based on the last send data, Param 3[Crex24SocketOrderBook]: the update data</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int limit, int mergeDepth, Action<string, bool, Crex24SocketOrderBook> onMessage)
        {
            symbol.ValidateCrex24Symbol();
            mergeDepth.ValidateIntBetween(nameof(mergeDepth), 0, 8);
            limit.ValidateIntValues(nameof(limit), 5, 10, 20);

            var internalHandler = new Action<JToken[]>(data =>
            {
                if (data.Length != 3)
                {
                    log.Write(LogVerbosity.Warning, $"Received unexpected data format for depth update. Expected 3 objects, received {data.Length}. Data: " + data);
                    return;
                }

                var fullUpdate = (bool)data[0];
                var desResult = Deserialize<Crex24SocketOrderBook>(data[1], false);
                if (!desResult)
                {
                    log.Write(LogVerbosity.Warning, "Received invalid depth update: " + desResult.Error);
                    return;
                }

                onMessage(symbol, fullUpdate, desResult.Data);
            });

            return await Subscribe(new Crex24SocketRequest(NextId(), DepthSubject, SubscribeAction, symbol, limit, Crex24Helpers.MergeDepthIntToString(mergeDepth)), null, false, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to symbol trade updates for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to receive updates from</param>
        /// <param name="onMessage">Data handler, receives Param 1[string]: the symbol name, Param 2[Crex24SocketSymbolTrade[]]: list of trades</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public CallResult<UpdateSubscription> SubscribeToSymbolTradeUpdates(string symbol, Action<string, IEnumerable<Crex24SocketSymbolTrade>> onMessage) => SubscribeToSymbolTradeUpdatesAsync(symbol, onMessage).Result;
        /// <summary>
        /// Subscribe to symbol trade updates for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to receive updates from</param>
        /// <param name="onMessage">Data handler, receives Param 1[string]: the symbol name, Param 2[Crex24SocketSymbolTrade[]]: list of trades</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolTradeUpdatesAsync(string symbol, Action<string, IEnumerable<Crex24SocketSymbolTrade>> onMessage)
        {
            symbol.ValidateCrex24Symbol();
            var internalHandler = new Action<JToken[]>(data =>
            {
                if (data.Length != 2)
                {
                    log.Write(LogVerbosity.Warning, $"Received unexpected data format for trade update. Expected 2 objects, received {data.Length}. Data: [{string.Join(",", data.Select(s => s.ToString()))}]");
                    return;
                }

                var desResult = Deserialize<IEnumerable<Crex24SocketSymbolTrade>>(data[1], false);
                if (!desResult)
                {
                    log.Write(LogVerbosity.Warning, "Received invalid trade update: " + desResult.Error);
                    return;
                }

                onMessage(symbol, desResult.Data);
            });

            return await Subscribe(new Crex24SocketRequest(NextId(), TransactionSubject, SubscribeAction, symbol), null, false, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to kline updates for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to receive updates for</param>
        /// <param name="interval">The interval of the candle to receive updates for</param>
        /// <param name="onMessage">Data handler, receives Param 1[string]: the symbol name, Param 2[Crex24Kline[]]: list of klines updated klines</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public CallResult<UpdateSubscription> SubscribeToKlineUpdates(string symbol, KlineInterval interval, Action<string, IEnumerable<Crex24Kline>> onMessage) => SubscribeToKlineUpdatesAsync(symbol, interval, onMessage).Result;
        /// <summary>
        /// Subscribe to kline updates for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to receive updates for</param>
        /// <param name="interval">The interval of the candle to receive updates for</param>
        /// <param name="onMessage">Data handler, receives Param 1[string]: the symbol name, Param 2[Crex24Kline[]]: list of klines updated klines</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<string, IEnumerable<Crex24Kline>> onMessage)
        {
            symbol.ValidateCrex24Symbol();
            var internalHandler = new Action<JToken[]>(data =>
            {
                if (data.Length > 2)
                {
                    log.Write(LogVerbosity.Warning, $"Received unexpected data format for kline update. Expected 1 or 2 objects, received {data.Length}. Data: [{string.Join(",", data.Select(s => s.ToString()))}]");
                    return;
                }

                var desResult = Deserialize<IEnumerable<Crex24Kline>>(new JArray(data), false);
                if (!desResult)
                {
                    log.Write(LogVerbosity.Warning, "Received invalid kline update: " + desResult.Error);
                    return;
                }

                onMessage(symbol, desResult.Data);
            });

            return await Subscribe(new Crex24SocketRequest(NextId(), KlineSubject, SubscribeAction, symbol, interval.ToSeconds()), null, false, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to updates of your balances, Receives updates whenever the balance for a coin changes
        /// </summary>
        /// <param name="onMessage">Data handler, receives a dictionary of coin name -> balance</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public CallResult<UpdateSubscription> SubscribeToBalanceUpdates(Action<Dictionary<string, Crex24Balance>> onMessage) => SubscribeToBalanceUpdatesAsync(onMessage).Result;
        /// <summary>
        /// Subscribe to updates of your balances, Receives updates whenever the balance for a coin changes
        /// </summary>
        /// <param name="onMessage">Data handler, receives a dictionary of coin name -> balance</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<Dictionary<string, Crex24Balance>> onMessage)
        {
            var internalHandler = new Action<JToken[]>(data =>
            {
                if (data.Length != 1)
                {
                    log.Write(LogVerbosity.Warning, $"Received unexpected data format for order update. Expected 1 objects, received {data.Length}. Data: [{string.Join(",", data.Select(s => s.ToString()))}]");
                    return;
                }

                var desResult = Deserialize<Dictionary<string, Crex24Balance>>(data[0], false);
                if (!desResult)
                {
                    log.Write(LogVerbosity.Warning, "Received invalid balance update: " + desResult.Error);
                    return;
                }

                onMessage(desResult.Data);
            });

            return await Subscribe(new Crex24SocketRequest(NextId(), BalanceSubject, SubscribeAction), null, true, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to updates of active orders. Receives updates whenever an order is placed, updated or finished
        /// </summary>
        /// <param name="symbols">The symbols to receive order updates from</param>
        /// <param name="onMessage">Data handler, receives Param 1[UpdateType]: the type of update, Param 2[Crex24SocketOrder]: the order that was updated</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public CallResult<UpdateSubscription> SubscribeToOrderUpdates(IEnumerable<string> symbols, Action<UpdateType, Crex24SocketOrder> onMessage) => SubscribeToOrderUpdatesAsync(symbols, onMessage).Result;
        /// <summary>
        /// Subscribe to updates of active orders. Receives updates whenever an order is placed, updated or finished
        /// </summary>
        /// <param name="symbols">The symbols to receive order updates from</param>
        /// <param name="onMessage">Data handler, receives Param 1[UpdateType]: the type of update, Param 2[Crex24SocketOrder]: the order that was updated</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(IEnumerable<string> symbols, Action<UpdateType, Crex24SocketOrder> onMessage)
        {
            var internalHandler = new Action<JToken[]>(data =>
            {
                if (data.Length != 2)
                {
                    log.Write(LogVerbosity.Warning, $"Received unexpected data format for order update. Expected 2 objects, received {data.Length}. Data: [{string.Join(",", data.Select(s => s.ToString()))}]");
                    return;
                }

                var updateResult = JsonConvert.DeserializeObject<UpdateType>((string)data[0], new UpdateTypeConverter(false));
                var desResult = Deserialize<Crex24SocketOrder>(data[1], false);
                if (!desResult)
                {
                    log.Write(LogVerbosity.Warning, "Received invalid order update: " + desResult.Error);
                    return;
                }

                onMessage(updateResult, desResult.Data);
            });

            return await Subscribe(new Crex24SocketRequest(NextId(), OrderSubject, SubscribeAction, symbols), null, true, internalHandler).ConfigureAwait(false);
        }
        #endregion

        #region private
        private object[] GetAuthParameters()
        {
            if(authProvider!.Credentials.Key == null || authProvider.Credentials.Secret == null)
                throw new ArgumentException("ApiKey/Secret not provided");

            var tonce = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            var parameterString = $"access_id={authProvider.Credentials.Key.GetString()}&tonce={tonce}&secret_key={authProvider.Credentials.Secret.GetString()}";
            var auth = authProvider.Sign(parameterString);
            return new object[] { authProvider.Credentials.Key.GetString(), auth, tonce };
        }
        #endregion
        #endregion

        /// <inheritdoc />
#pragma warning disable 8765
        protected override bool HandleQueryResponse<T>(SocketConnection s, object request, JToken data, out CallResult<T>? callResult)
#pragma warning disable 8765
        {
            callResult = null;
            var cRequest = (Crex24SocketRequest) request;
            var idField = data["id"];
            if (idField == null)
                return false;

            if ((int)idField != cRequest.Id)
                return false;

            if (data["error"].Type != JTokenType.Null)
            {
                callResult = new CallResult<T>(default, new ServerError((int)data["error"]["code"], (string)data["error"]["message"]));
                return true;
            }
            else
            {
                var desResult = Deserialize<T>(data["result"]);
                if (!desResult)
                {
                    callResult = new CallResult<T>(default, desResult.Error);
                    return true;
                }

                callResult = new CallResult<T>(desResult.Data, null);
                return true;
            }
        }

        /// <inheritdoc />
        protected override bool HandleSubscriptionResponse(SocketConnection s, SocketSubscription subscription, object request, JToken message, out CallResult<object>? callResult)
        {
            callResult = null;
            if (message.Type != JTokenType.Object)
                return false;

            var idField = message["id"];
            if (idField == null || idField.Type == JTokenType.Null)
                return false;

            var cRequest = (Crex24SocketRequest) request;
            if ((int) idField != cRequest.Id)
                return false;

            var subResponse = Deserialize<Crex24SocketRequestResponse<Crex24SocketRequestResponseMessage>>(message, false);
            if (!subResponse)
            {
                log.Write(LogVerbosity.Warning, "Subscription failed: " + subResponse.Error);
                callResult = new CallResult<object>(null, subResponse.Error);
                return true;
            }

            if (subResponse.Data.Error != null)
            {
                log.Write(LogVerbosity.Debug, $"Failed to subscribe: {subResponse.Data.Error.Code} {subResponse.Data.Error.Message}");
                callResult = new CallResult<object>(null, new ServerError(subResponse.Data.Error.Code, subResponse.Data.Error.Message));
                return true;
            }

            log.Write(LogVerbosity.Debug, "Subscription completed");
            callResult = new CallResult<object>(subResponse, null);
            return true;
        }

        /// <inheritdoc />
        protected override JToken ProcessTokenData(JToken data)
        {
            return data["params"];
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(JToken message, object request)
        {
            var cRequest = (Crex24SocketRequest)request;
            var method = message["method"];
            if (method == null)
                return false;

            var subject = ((string) method).Split(new [] { "." }, StringSplitOptions.RemoveEmptyEntries)[0];
            return cRequest.Subject == subject;
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(JToken message, string identifier)
        {
            if (message.Type != JTokenType.Object)
                return false;
            return identifier == "Pong" && (string) message["result"] == "pong";
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> AuthenticateSocket(SocketConnection s)
        {
            if (authProvider == null)
                return new CallResult<bool>(false, new NoApiCredentialsError());

            var request = new Crex24SocketRequest(NextId(), ServerSubject, AuthenticateAction, GetAuthParameters());
            var result = new CallResult<bool>(false, new ServerError("No response from server"));
            await s.SendAndWait(request, ResponseTimeout, data =>
            {
                var idField = data["id"];
                if (idField == null)
                    return false;

                if ((int)idField != request.Id)
                    return false; // Not for this request

                var authResponse = Deserialize<Crex24SocketRequestResponse<Crex24SocketRequestResponseMessage>>(data, false);
                if (!authResponse)
                {
                    log.Write(LogVerbosity.Warning, "Authorization failed: " + authResponse.Error);
                    result = new CallResult<bool>(false, authResponse.Error);
                    return true;
                }

                if (authResponse.Data.Error != null)
                {
                    var error = new ServerError(authResponse.Data.Error.Code, authResponse.Data.Error.Message);
                    log.Write(LogVerbosity.Debug, "Failed to authenticate: " + error);
                    result = new CallResult<bool>(false, error);
                    return true;
                }

                if (authResponse.Data.Result.Status != SuccessString)
                {
                    log.Write(LogVerbosity.Debug, "Failed to authenticate: " + authResponse.Data.Result.Status);
                    result = new CallResult<bool>(false, new ServerError(authResponse.Data.Result.Status));
                    return true;
                }

                log.Write(LogVerbosity.Debug, "Authorization completed");
                result = new CallResult<bool>(true, null);
                return true;
            });

            return result;
        }

        /// <inheritdoc />
        protected override Task<bool> Unsubscribe(SocketConnection connection, SocketSubscription s)
        {
            return Task.FromResult(true);
        }
    }
}
