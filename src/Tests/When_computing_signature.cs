using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using AutoFixture.Xunit2;
using Xunit;

namespace Tests
{
    public class When_computing_signature
    {
        [Theory, AutoData]
        public void Should_create_valid_hex_string(string key, string message)
        {
            var result = Castle.Signature.Compute(key, message);

            var numberGroups = Regex.Matches(result, @".{2}");
            var numbers = numberGroups.Select(s => int.Parse(s.Value, NumberStyles.HexNumber)).ToList();
        }
    }
}
