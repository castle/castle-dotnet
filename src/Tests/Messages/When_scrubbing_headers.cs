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
        public void Should_scrub_all_in_blacklist_to_truestring_regardless_of_casing(
            string[] allowed, 
            string[] blacklist)
        {
            var headers = new Dictionary<string, string>(
                allowed.Select(ToDictionaryEntry)
                    .Union(blacklist.Select(x => ToDictionaryEntry(x.ToUpper()))));

            var result = HeaderScrubber.Scrub(headers, new string[] { }, blacklist);

            result
                .Where(x => x.Value == "true")
                .Select(x => x.Key)
                .Should()
                .Equal(blacklist, (s1, s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase));
        }

        [Theory, AutoData]
        public void Should_scrub_all_not_in_whitelist_to_truestring_regardless_of_casing(
            string[] unallowed,
            string[] whitelist)
        {
            var headers = new Dictionary<string, string>(
                unallowed.Select(ToDictionaryEntry)
                    .Union(whitelist.Select(x => ToDictionaryEntry(x.ToUpper()))));

            var result = HeaderScrubber.Scrub(headers, whitelist, new string[] { });

            result
                .Where(x => x.Value == "true")
                .Select(x => x.Key)
                .Should()
                .Equal(unallowed, (s1, s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Should_always_allow_default_whitelist()
        {
            var whitelist = new string[] { "Only-This" };
            var headers = new Dictionary<string, string> {
                {
                    "User-Agent", "something"
                }
            };

            var result = HeaderScrubber.Scrub(headers, whitelist, new string[] { });

            result.Should().Equal(headers);
        }

        [Fact]
        public void Should_always_apply_default_blacklist()
        {
            var headers = new Dictionary<string, string> {
                {
                    "Authorization", "secret"
                },
                {
                    "Cookie", "secret"
                }
            };

            var result = HeaderScrubber.Scrub(headers, new string[] { }, new string[] { });

            result
                .Select(x => x.Value)
                .Should()
                .Equal(new string[] { "true", "true" });
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
