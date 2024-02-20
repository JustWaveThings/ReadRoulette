using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ReadRoulette.Persistence;

namespace ReadRoulette.Test;

public partial class Broker
{
    private readonly WebApplicationFactory<Program> _webAppFactory;
    private readonly HttpClient _baseClient;
    private readonly BrokerHttpClient _brokerClient;

    public AppDbContext DbContext => _webAppFactory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

    public Broker()
    {
        _webAppFactory = new TestWebApplicationFactory<Program>();
        _baseClient = _webAppFactory.CreateClient();
        _brokerClient = new BrokerHttpClient(_baseClient);
    }
}
