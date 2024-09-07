// See https://aka.ms/new-console-template for more information
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

internal class Program
{
    private static async Task Main(string[] args)
    {



        var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", false, true);

        var config = builder.Build();



        var keyVaultUri = config["KeyVaultURL"] ?? "";
        var tenantId = config["TenantId"] ?? "";
        var clientId = config["ClientId"] ?? "";
        var clientSecret = config["ClientSecret"] ?? "";

        var clientCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
        var defaultCredential = new DefaultAzureCredential();

        var keyClient = new KeyClient(new Uri(keyVaultUri), new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            ManagedIdentityClientId = clientId
        }));

        var secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            ManagedIdentityClientId = clientId
        }));

        var key = keyClient.CreateKey("myKey", KeyType.Rsa);
        Console.WriteLine($"Create a key with name {key.Value.Properties.Name} CreatedOn: {key.Value.Properties.CreatedOn}");

        var getKey = keyClient.GetKey("myKey");
        Console.WriteLine($"Get a key with name {getKey.Value.Properties.Name} CreatedOn: {getKey.Value.Properties.CreatedOn}");


        var secret = secretClient.GetSecret("mySecret");
        Console.WriteLine("Your secret is '" + secret.Value.Value + "'.");

        Console.Write("Input the value of your secret > ");
        string secretValue = Console.ReadLine();
        secretClient.SetSecret("mySecret", secretValue);


        Console.WriteLine("Hello, World!");
    }
}