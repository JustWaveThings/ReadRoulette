namespace ReadRoulette.Test;

public partial class Broker
{
    public async Task<object> AuthUser(string email, string password)
        => await _brokerClient.AuthUser(email, password);
}
