using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crex24.Net.Objects;
using Crex24.Net.Objects.Websocket;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;

namespace Crex24.Net.Interfaces
{
    /// <summary>
    /// Interface for the Crex24 socket client
    /// </summary>
    public interface ICrex24SocketClient: ISocketClient
    {
        /// <summary>
        /// Pings the server
        /// </summary>
        /// <returns>True if server responded, false otherwise</returns>
        CallResult<bool> Ping();

        /// <summary>
        /// Pings the server
        /// </summary>
        /// <returns>True if server responded, false otherwise</returns>
        Task<CallResult<bool>> PingAsync();

        /// <summary>
        /// Gets the server time
        /// </summary>
        /// <returns>The server time</returns>
        CallResult<DateTime> GetServerTime();

        /// <summary>
        /// Gets the server time
        /// </summary>
        /// <returns>The server time</returns>
        Task<CallResult<DateTime>> GetServerTimeAsync();

        /// <summary>
        /// Get the symbol state
        /// </summary>
        /// <param name="symbol">The symbol to get the state for</param>
        /// <param name="cyclePeriod">The period to get data over, specified in seconds. i.e. one minute = 60, one day = 86400</param>
        /// <returns>Symbol state</returns>
        CallResult<Crex24SocketSymbolState> GetSymbolState(string symbol, int cyclePeriod);

        /// <summary>
        /// Get the symbol state
        /// </summary>
        /// <param name="symbol">The symbol to get the state for</param>
        /// <param name="cyclePeriod">The period to get data over, specified in seconds. i.e. one minute = 60, one day = 86400</param>
        /// <returns>Symbol state</returns>
        Task<CallResult<Crex24SocketSymbolState>> GetSymbolStateAsync(string symbol, int cyclePeriod);

        /// <summary>
        /// Get an order book
        /// </summary>
        /// <param name="symbol">The symbol to get the order book for</param>
        /// <param name="limit">The limit of results returned</param>
        /// <param name="mergeDepth">The depth of merging, based on 8 decimals. 1 mergeDepth will merge the last decimals of all order in the book, 7 will merge the last 7 decimals of all orders together</param>
        /// <returns>Order book for a symbol</returns>
        CallResult<Crex24SocketOrderBook> GetOrderBook(string symbol, int limit, int mergeDepth);

        /// <summary>
        /// Get an order book
        /// </summary>
        /// <param name="symbol">The symbol to get the order book for</param>
        /// <param name="limit">The limit of results returned</param>
        /// <param name="mergeDepth">The depth of merging, based on 8 decimals. 1 mergeDepth will merge the last decimals of all order in the book, 7 will merge the last 7 decimals of all orders together</param>
        /// <returns>Order book of a symbol</returns>
        Task<CallResult<Crex24SocketOrderBook>> GetOrderBookAsync(string symbol, int limit, int mergeDepth);

        /// <summary>
        /// Gets the latest trades on a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get the trades for</param>
        /// <param name="limit">The limit of trades</param>
        /// <param name="fromId">Return trades since this id</param>
        /// <returns>List of trades</returns>
        CallResult<IEnumerable<Crex24SocketSymbolTrade>> GetSymbolTrades(string symbol, int limit, int? fromId = null);

        /// <summary>
        /// Gets the latest trades on a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get the trades for</param>
        /// <param name="limit">The limit of trades</param>
        /// <param name="fromId">Return trades since this id</param>
        /// <returns>List of trades</returns>
        Task<CallResult<IEnumerable<Crex24SocketSymbolTrade>>> GetSymbolTradesAsync(string symbol, int limit, int? fromId = null);

        /// <summary>
        /// Gets symbol kline data
        /// </summary>
        /// <param name="symbol">The symbol to get the data for</param>
        /// <param name="interval">The interval of the candles</param>
        /// <returns></returns>
        CallResult<Crex24Kline> GetKlines(string symbol, KlineInterval interval);

        /// <summary>
        /// Gets symbol kline data
        /// </summary>
        /// <param name="symbol">The symbol to get the data for</param>
        /// <param name="interval">The interval of the candles</param>
        /// <returns></returns>
        Task<CallResult<Crex24Kline>> GetKlinesAsync(string symbol, KlineInterval interval);

        /// <summary>
        /// Get balances of coins. Requires API credentials
        /// </summary>
        /// <param name="coins">The coins to get the balances for, empty for all</param>
        /// <returns>Dictionary of coins and their balances</returns>
        CallResult<Dictionary<string, Crex24Balance>> GetBalances(params string[] coins);

        /// <summary>
        /// Get balances of coins. Requires API credentials
        /// </summary>
        /// <param name="coins">The coins to get the balances for, empty for all</param>
        /// <returns>Dictionary of coins and their balances</returns>
        Task<CallResult<Dictionary<string, Crex24Balance>>> GetBalancesAsync(params string[] coins);

        /// <summary>
        /// Gets a list of open orders for a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get open orders for</param>
        /// <param name="type">The type of orders to get</param>
        /// <param name="offset">The offset in the list</param>
        /// <param name="limit">The limit of results</param>
        /// <returns>List of open orders</returns>
        CallResult<Crex24SocketPagedResult<Crex24SocketOrder>> GetOpenOrders(string symbol, TransactionType type, int offset, int limit);

        /// <summary>
        /// Gets a list of open orders for a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get open orders for</param>
        /// <param name="type">The type of orders to get</param>
        /// <param name="offset">The offset in the list</param>
        /// <param name="limit">The limit of results</param>
        /// <returns>List of open orders</returns>
        Task<CallResult<Crex24SocketPagedResult<Crex24SocketOrder>>> GetOpenOrdersAsync(string symbol, TransactionType type, int offset, int limit);

        /// <summary>
        /// Subscribe to symbol state updates for a specific symbol
        /// </summary>
        /// <param name="symbol">Symbol to receive updates for</param>
        /// <param name="onMessage">Data handler, receives Param 1[string]: the symbol name, Param 2[CoinExSocketSymbolState]: the symbol state update</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToSymbolStateUpdates(string symbol, Action<string, Crex24SocketSymbolState> onMessage);

        /// <summary>
        /// Subscribe to symbol state updates for a specific symbol
        /// </summary>
        /// <param name="symbol">Symbol to receive updates for</param>
        /// <param name="onMessage">Data handler, receives Param 1[string]: the symbol name, Param 2[CoinExSocketSymbolState]: the symbol state update</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolStateUpdatesAsync(string symbol, Action<string, Crex24SocketSymbolState> onMessage);

        /// <summary>
        /// Subscribe to symbol state updates for all symbols
        /// </summary>
        /// <param name="onMessage">Data handler, receives a dictionary of symbol name -> symbol state</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToSymbolStateUpdates(Action<Dictionary<string, Crex24SocketSymbolState>> onMessage);

        /// <summary>
        /// Subscribe to symbol state updates for all symbols
        /// </summary>
        /// <param name="onMessage">Data handler, receives a dictionary of symbol name -> symbol state</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolStateUpdatesAsync(Action<Dictionary<string, Crex24SocketSymbolState>> onMessage);

        /// <summary>
        /// Subscribe to order book updates
        /// </summary>
        /// <param name="symbol">The symbol to receive updates for</param>
        /// <param name="limit">The limit of results to receive in a update</param>
        /// <param name="mergeDepth">The depth of merging, based on 8 decimals. 1 mergeDepth will merge the last decimals of all order in the book, 7 will merge the last 7 decimals of all orders together</param>
        /// <param name="onMessage">Data handler, receives Param 1[string]: the symbol name, Param 2[bool]: whether this is a full update, or an update based on the last send data, Param 3[CoinExSocketOrderBook]: the update data</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToOrderBookUpdates(string symbol, int limit, int mergeDepth, Action<string, bool, Crex24SocketOrderBook> onMessage);

        /// <summary>
        /// Subscribe to order book updates
        /// </summary>
        /// <param name="symbol">The symbol to receive updates for</param>
        /// <param name="limit">The limit of results to receive in a update</param>
        /// <param name="mergeDepth">The depth of merging, based on 8 decimals. 1 mergeDepth will merge the last decimals of all order in the book, 7 will merge the last 7 decimals of all orders together</param>
        /// <param name="onMessage">Data handler, receives Param 1[string]: the symbol name, Param 2[bool]: whether this is a full update, or an update based on the last send data, Param 3[CoinExSocketOrderBook]: the update data</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int limit, int mergeDepth, Action<string, bool, Crex24SocketOrderBook> onMessage);

        /// <summary>
        /// Subscribe to symbol trade updates for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to receive updates from</param>
        /// <param name="onMessage">Data handler, receives Param 1[string]: the symbol name, Param 2[CoinExSocketSymbolTrade[]]: list of trades</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToSymbolTradeUpdates(string symbol, Action<string, IEnumerable<Crex24SocketSymbolTrade>> onMessage);

        /// <summary>
        /// Subscribe to symbol trade updates for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to receive updates from</param>
        /// <param name="onMessage">Data handler, receives Param 1[string]: the symbol name, Param 2[CoinExSocketSymbolTrade[]]: list of trades</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolTradeUpdatesAsync(string symbol, Action<string, IEnumerable<Crex24SocketSymbolTrade>> onMessage);

        /// <summary>
        /// Subscribe to kline updates for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to receive updates for</param>
        /// <param name="interval">The interval of the candle to receive updates for</param>
        /// <param name="onMessage">Data handler, receives Param 1[string]: the symbol name, Param 2[CoinExKline[]]: list of klines updated klines</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToKlineUpdates(string symbol, KlineInterval interval, Action<string, IEnumerable<Crex24Kline>> onMessage);

        /// <summary>
        /// Subscribe to kline updates for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to receive updates for</param>
        /// <param name="interval">The interval of the candle to receive updates for</param>
        /// <param name="onMessage">Data handler, receives Param 1[string]: the symbol name, Param 2[CoinExKline[]]: list of klines updated klines</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<string, IEnumerable<Crex24Kline>> onMessage);

        /// <summary>
        /// Subscribe to updates of your balances, Receives updates whenever the balance for a coin changes
        /// </summary>
        /// <param name="onMessage">Data handler, receives a dictionary of coin name -> balance</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToBalanceUpdates(Action<Dictionary<string, Crex24Balance>> onMessage);

        /// <summary>
        /// Subscribe to updates of your balances, Receives updates whenever the balance for a coin changes
        /// </summary>
        /// <param name="onMessage">Data handler, receives a dictionary of coin name -> balance</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<Dictionary<string, Crex24Balance>> onMessage);

        /// <summary>
        /// Subscribe to updates of active orders. Receives updates whenever an order is placed, updated or finished
        /// </summary>
        /// <param name="symbols">The symbols to receive order updates from</param>
        /// <param name="onMessage">Data handler, receives Param 1[UpdateType]: the type of update, Param 2[CoinExSocketOrder]: the order that was updated</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToOrderUpdates(IEnumerable<string> symbols, Action<UpdateType, Crex24SocketOrder> onMessage);

        /// <summary>
        /// Subscribe to updates of active orders. Receives updates whenever an order is placed, updated or finished
        /// </summary>
        /// <param name="symbols">The symbols to receive order updates from</param>
        /// <param name="onMessage">Data handler, receives Param 1[UpdateType]: the type of update, Param 2[CoinExSocketOrder]: the order that was updated</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(IEnumerable<string> symbols, Action<UpdateType, Crex24SocketOrder> onMessage);

    }
}