# TestPerformance

Quick test of multi threading performance using .net core / net462.

NOTE: If you run this directly on Visual Studio DISABLE DIAGNOSTICS when running on .Net core. It is 100x slower.

Overall tests results:

|Total: 6000 reads of Account balance|
|---|
|.Net core, Parity, HTTP / RPC, Windows = ~1.8 seconds|
|.Net core, Parity, HTTP / RPC, Linux Ubuntu = ~1.8 seconds|
|.Net core, Parity, IPC, Windows = ~1.5 seconds|
| |
|.Net core, Geth, HTTP / RPC, Windows = ~2 seconds|
|.Net core, Geth, HTTP / RPC, Linux Ubuntu = ~2 seconds|
|.Net core, Geth, IPC, Windows = ~1.8 seconds|
| |
|.Net 462, Geth, HTTP / RPC, Windows = ~1.5 seconds|
|.Net 462, Geth, IPC, Windows = ~1.8 seconds|
| |
|.Net 462, Parity, HTTP / RPC, Windows = ~2 seconds|
|.Net 462, Parity, IPC, Windows = ~1.3 seconds|




