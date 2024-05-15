# Changelog

## master/develop

## 2.2.0 (2024-05-15)

Added support for `skip_request_token_validation` and `skip_context_validation` options,
Added .net8 support

## 2.1.1 (2022-04-01)

Added support for transaction and changeset objects

## 2.1.0 (2022-03-21)

Better error handling, removed X-Castle-\* headers from allowlist, added policy to the failover for risk and filter

## 2.0.2 (2022-02-10)

Added device fingerprint to risk/filter response

## 2.0.1 (2022-01-21)

Added support for name property

## 2.0.0 (2022-01-13)

**Features:**

- [#56](https://github.com/castle/castle-dotnet/pull/56) Support for Log action

**BREAKING CHANGES:**

- [#57](https://github.com/castle/castle-dotnet/pull/57) Dropped Events list - please use [recognized events](https://docs.castle.io/docs/events) instead

## 1.6.0 (2022-01-13)

**Features:**

- [#54](https://github.com/castle/castle-dotnet/pull/54) Support for Risk / Filter actions, updated headers

## 1.5.0 (2020-09-30)

**Features:**

- [#43](https://github.com/castle/castle-dotnet/pull/43) Add `IpHeaders`, `TrustedProxies`, `TrustedProxyDepth` and `TrustProxyChain` configuration options

**Enhancement**

- [#48](https://github.com/castle/castle-dotnet/pull/48) Update the readme with new config options
- [#47](https://github.com/castle/castle-dotnet/pull/47) Add more tests for the context section

## 1.4.0 (2020-08-21)

**Features:**

- [#40](https://github.com/castle/castle-dotnet/pull/40) Support the correct basic list of allowed headers

**Enhancement**

- [#39](https://github.com/castle/castle-dotnet/pull/39) Tests project references upgraded as well as the target itself
- [#38](https://github.com/castle/castle-dotnet/pull/38) Targets list updated (.NET 48 and .NET Core 3.1)

## 1.3.0 (2019-11-20)

**Enhancement**

- [#34](https://github.com/castle/castle-dotnet/pull/34) Update recommended whitelist, add auto blacklist header (`Authorization`)

## 1.2.0 (2019-03-28)

**Features:**

- [#25](https://github.com/castle/castle-dotnet/pull/25) Adds `CastleClient.ArchiveDevices`

**Enhancement:**

- [#26](https://github.com/castle/castle-dotnet/pull/26) Guard against missing client method arguments and API secret
- [#27](https://github.com/castle/castle-dotnet/pull/27) Return null for requests caught in exception guard

**Bug fixes:**

- [#28](https://github.com/castle/castle-dotnet/pull/28) Include request/response content in info logging

## 1.1.1 (2019-03-26)

**Bug fixes:**

- [#23](https://github.com/castle/castle-dotnet/pull/23) Include xml docs in all build configurations.

## 1.1.0 (2019-03-20)

**Features:**

- [#11](https://github.com/castle/castle-dotnet/pull/11) Allow optional `clientId` parameter in the call to GetDevices(), which will set each device's `IsCurrentDevice` property accordingly.
- [#16](https://github.com/castle/castle-dotnet/pull/16) Can get IP address from request headers when using `Castle.Context.FromHttpRequest()`, by providing the helper with a list of header names to look for.
