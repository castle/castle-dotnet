using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Xunit2;
using Castle.Infrastructure;
using FluentAssertions;
using Xunit;

namespace Tests.Messages
{
    public class When_scrubbing_headers
    {
        [Theory, AutoData]
        public void Should_scrub_all_in_denylist_to_truestring_regardless_of_casing(
            string[] allowed,
            string[] denyList)
        {
            var headers = new Dictionary<string, string>(
                allowed.Select(ToDictionaryEntry)
                    .Union(denyList.Select(x => ToDictionaryEntry(x.ToUpper()))));

            var result = HeaderScrubber.Scrub(headers, new string[] { }, denyList);

            result
                .Where(x => x.Value == "true")
                .Select(x => x.Key)
                .Should()
                .Equal(denyList, (s1, s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase));
        }

        [Theory, AutoData]
        public void Should_scrub_all_not_in_allowlist_to_truestring_regardless_of_casing(
            string[] unallowed,
            string[] allowList)
        {
            var headers = new Dictionary<string, string>(
                unallowed.Select(ToDictionaryEntry)
                    .Union(allowList.Select(x => ToDictionaryEntry(x.ToUpper()))));

            var result = HeaderScrubber.Scrub(headers, allowList, new string[] { });

            result
                .Where(x => x.Value == "true")
                .Select(x => x.Key)
                .Should()
                .Equal(unallowed, (s1, s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Should_always_allow_default_allowlist()
        {
            var allowList = new string[] { "Only-This" };
            var headers = new Dictionary<string, string> {
                {
                    "User-Agent", "something"
                }
            };

            var result = HeaderScrubber.Scrub(headers, allowList, new string[] { });

            result.Should().Equal(headers);
        }

        [Fact]
        public void Should_always_apply_default_denylist()
        {
            var headers = new Dictionary<string, string> {
                {
                    "Authorization", "secret"
                },
                {
                    "Cookie", "secret"
                },
                {
                    "Other", "secret"
                }
            };

            var result = HeaderScrubber.Scrub(headers, new string[] { }, new string[] { "Other" });

            result
                .Select(x => x.Value)
                .Should()
                .Equal(new string[] { "true", "true", "true" });
        }

        [Fact]
        public void Should_scrub_http_from_header()
        {
            var headers = new Dictionary<string, string> {
                {
                    "HTTP_Authorization", "secret"
                },
                {
                    "Cookie", "secret"
                },
                {
                    "HTTP_OK", "test"
                },
                {
                    "http_Another", "test"
                }
            };

            var result = HeaderScrubber.Scrub(headers, new string[] { }, new string[] { });

            result
                .Select(x => x.Value)
                .Should()
                .Equal(new string[] { "true", "true", "test", "test" });
        }

        [Fact]
        public void Should_apply_allow_and_deny_list_when_passed()
        {
            var headers = new Dictionary<string, string> {
                {
                    "Accept", "test"
                },
                {
                    "Authorization", "secret"
                },
                {
                    "Cookie", "secret"
                },
                {
                    "Content-Length", "0"
                },
                {
                    "Ok", "OK"
                },
                {
                    "User-Agent", "Mozilla 1234"
                },
                {
                    "X-Forwarded-For", "1.2.3.4"
                }
            };

            var result = HeaderScrubber.Scrub(headers, new [] { "Content-Length" }, new [] { "Ok" });

            result
                .Select(x => x.Value)
                .Should()
                .Equal(new string[] { "true", "true", "true", "0", "true", "Mozilla 1234", "true" });
        }

        [Theory, AutoData]
        public void Should_not_throw_exception_if_lists_are_null(Dictionary<string, string> headers)
        {
            var result = HeaderScrubber.Scrub(headers, null, null);
        }

        private static KeyValuePair<string, string> ToDictionaryEntry(string header)
        {
            return new KeyValuePair<string, string>(header, $"{header}_value");
        }
    }
}
