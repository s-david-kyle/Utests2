using FakeItEasy;
using FluentAssertions;
using NetworkUtility.DomainNameService;
using NetworkUtility.Ping;
using System.Net.NetworkInformation;

namespace NetworkUtility.Tests.PingTests;

public class NetworkServiceTests
{
    private readonly NetworkService networkService;
    private readonly IDns _dns;

    public NetworkServiceTests()
    {
        _dns = A.Fake<IDns>();
        networkService = new NetworkService(_dns);
    }

    [Fact]
    public void MostRecentPingsTest()
    {
        //Arrange
        var expected = new List<PingOptions>
        {
            new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            },
            new PingOptions()
            {
                DontFragment = false,
                Ttl = 2
            }
        };
        // Act
        var result = networkService.MostRecentPings();
        // Assert
        result.ElementAt(0).DontFragment.Should().BeTrue();
        result.ElementAt(1).DontFragment.Should().BeFalse();
        result.ElementAt(0).Ttl.Should().Be(1);

    }

    [Fact]
    public void GetPingOptionsTest()
    {
        //Arrange
        var expected = new PingOptions()
        {
            DontFragment = true,
            Ttl = 1
        };
        // Act
        var result = networkService.GetPingOptions();
        // Assert
        result.Should().BeOfType<PingOptions>();
        result.DontFragment.Should().BeTrue();
        result.Ttl.Should().Be(1);
        result.Should().BeEquivalentTo(expected);

    }

    [Fact]
    public void LastPingTest()
    {
        // Act
        var result = networkService.LastPingDate();
        // Assert
        result.Should().BeAfter(DateTime.Now.AddSeconds(-1));
        result.Should().BeBefore(DateTime.Now.AddSeconds(1));

    }

    [Fact]
    public void SendPingTest()
    {
        // Arrange
        A.CallTo(() => _dns.SendDNS()).Returns(true);
        // Act
        var result = networkService.SendPing();
        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("Ping Sent");
    }

    [Theory]
    [InlineData(2, 3, 5)]
    [InlineData(5, 5, 10)]
    [InlineData(10, 10, 20)]
    [InlineData(0, 0, 0)]
    public void PingTimeoutTest(int a, int b, int expected)
    {
        // Act
        var result = networkService.PingTimeout(a, b);
        // Assert
        result.Should().Be(expected);
    }
}
