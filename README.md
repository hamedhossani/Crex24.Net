Crex24.Net  
This is forked from 
<a target="_blank" rel="noopener noreferrer" href="https://github.com/JKorf/CoinEx.Net"><img src="https://github.com/JKorf/CoinEx.Net/raw/master/CoinEx.Net/Icon/icon.png?raw=true" alt="Icon" style="max-width:100%;"></a>
CoinEx.Net Version 3.2.6 - 04 mei 2021
 
A .Net wrapper for the Crex24 API as described on [Crex24](https://docs.crex24.com/trade-api/v2/), including all features the API provides using clear and readable objects.  

**If you think something is broken, something is missing or have any questions, please open an [Issue](https://github.com/hamedhossani/Crex24.Net/issues)**

## CryptoExchange.Net
Implementation is build upon the CryptoExchange.Net library, make sure to also check out the documentation on that: [docs](https://github.com/JKorf/CryptoExchange.Net)


## Donations
Donations are greatly appreciated and a motivation to keep improving.

**Btc**:       3DKwCEf1vzeByKTa8MYmBASzoEh4Gnc2oH  
**DogeCoin**:  DRVRkLj4j7zNgqFFQK7dEA5zFjpMgwS5vU  
**LTC**:       MQd5NcV9D1Yq2fTY5wCoiDdsTTBcQrJLCu 

## Discord
A Discord server is available [here](https://discord.gg/MSpeEtSY8t). For discussion and/or questions around the CryptoExchange.Net and implementation libraries, feel free to join.

To get started with Crex24.Net first you will need to get the library itself. 

## Getting started
After Downloaded it's time to actually use it. To get started you have to add the Crex24.Net namespace: `using Crex24.Net;`.

Crex24.Net provides two clients to interact with the Crex24 API. The  `Crex24Client`  provides all rest API calls. The `Crex24SocketClient` provides functions to interact with the websocket provided by the Crex24 API. Both clients are disposable and as such can be used in a `using` statement.
