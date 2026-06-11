# Fern scaffold (proposal)

This directory is a proposal for generating the Castle .NET SDK surface with
[Fern](https://buildwithfern.com) from an OpenAPI specification.

## Layout

- `fern.config.json` — organization name and pinned Fern CLI version.
- `generators.yml` — declares the API spec and the `fern-csharp-sdk` generator group.
- `openapi/openapi.yml` — OpenAPI spec for the Castle scoring, Lists, Privacy
  and Events endpoints used as the generator input.

## Usage

```bash
npm install -g fern-api
fern check
fern generate --group csharp-sdk --local
```

Generated code is written to `../generated/csharp` and is not committed.
