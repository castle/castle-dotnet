using System.Collections.Generic;
using System.Linq;
using AutoFixture.Xunit2;
using Castle.Net.Infrastructure;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class When_scrubbing_headers
    {
        [Theory, AutoData]
        public void Should_remove_all_in_blacklist_regardless_of_casing(
            string[] allowed, 
            string[] blacklist)
        {
            var allowedPairs = allowed.Select(ToDictionaryEntry);
            var expected = new Dictionary<string, string>(allowedPairs.ToList());
            var headers = new Dictionary<string, string>(allowedPairs.Union(blacklist.Select(x => ToDictionaryEntry(x.ToUpper()))));

            var result = HeaderScrubber.Scrub(headers, new string[] { }, blacklist);

            result.Should().Equal(expected);
        }

        [Theory, AutoData]
        public void Should_remove_all_not_in_whitelist_regardless_of_casing(
            string[] unallowed,
            string[] whitelist)
        {
            var whitelistPairs = whitelist.Select(x => ToDictionaryEntry(x.ToUpper()));
            var expected = new Dictionary<string, string>(whitelistPairs);
            var headers = new Dictionary<string, string>(unallowed.Select(ToDictionaryEntry).Union(whitelistPairs));

            var result = HeaderScrubber.Scrub(headers, whitelist, new string[] { });

            result.Should().Equal(expected);
        }

        private static KeyValuePair<string, string> ToDictionaryEntry(string header)
        {
            return new KeyValuePair<string, string>(header, $"{header}_value");
        }
    }
}
