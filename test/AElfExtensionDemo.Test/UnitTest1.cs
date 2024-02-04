using AElf.Client;

namespace AElfExtensionDemo.Test;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var client = new AElfClientBuilder().UseEndpoint("https://tdvw-test-node.aelf.io").Build();
        var chainStatus = client.GetChainStatusAsync().Result;
        var chainId = chainStatus.ChainId;
    }
}