namespace ReadRoulette.Test;

public partial class Broker
{
    public async Task AuthUser(string email, string password)
        => await _brokerClient.AuthUser(email, password);

    public async Task RegisterUser(string email, string password)
        => await _brokerClient.RegisterUser(email, password);

    public void Logout() => _brokerClient.ClearAllHeaders();
}
