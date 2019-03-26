# Change Log

## master

## 1.1.1 (2019-03-26)

**Bug fixes:**
- [#23](https://github.com/castle/castle-dotnet/pull/23) Include xml docs in all build configurations.

## 1.1.0 (2019-03-20)

**Features:**
- [#11](https://github.com/castle/castle-dotnet/pull/11) Allow optional `clientId` parameter in the call to GetDevices(), which will set each device's `IsCurrentDevice` property accordingly.
- [#16](https://github.com/castle/castle-dotnet/pull/16) Can get IP address from request headers when using `Castle.Context.FromHttpRequest()`, by providing the helper with a list of header names to look for.