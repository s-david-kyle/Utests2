using NetworkUtility.DomainNameService;
using System.Net.NetworkInformation;

namespace NetworkUtility.Ping;

public class NetworkService
{
    private readonly IDns _dns;


    public NetworkService(IDns dns)
    {
        _dns = dns;
    }

    public string SendPing()
    {
        var denSuccess = _dns.SendDNS();
        if (denSuccess)
        {
            return "Ping Sent";
        }
        else
        {
            return "Ping not sent";
        }
    }

    public int PingTimeout(int a, int b)
    {
        return a + b;
    }

    public DateTime LastPingDate()
    {

        return DateTime.Now;
    }

    public PingOptions GetPingOptions()
    {
        return new PingOptions()
        {
            DontFragment = true,
            Ttl = 1
        };
    }

    public IEnumerable<PingOptions> MostRecentPings()
    {
        return new List<PingOptions>
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
    }
}


