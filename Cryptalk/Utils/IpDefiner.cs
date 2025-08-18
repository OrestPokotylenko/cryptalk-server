namespace Cryptalk.Utils
{
    public static class IpDefiner
    {
        public static async Task<string?> GetPublicIpAddressAsync()
        {
            using var httpClient = new HttpClient();
            return await httpClient.GetStringAsync("https://api.ipify.org");
        }
    }
}