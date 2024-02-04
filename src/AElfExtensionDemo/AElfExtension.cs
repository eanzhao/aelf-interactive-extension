using System.CommandLine;
using AElf.Client;
using Microsoft.AspNetCore.Html;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Formatting;
using static Microsoft.DotNet.Interactive.Formatting.PocketViewTags;

namespace AElfExtensionDemo;

public static class AElfKernelExtension
{
    public static void Load(Kernel kernel)
    {
        var chainOption = new Option<string>(["-i", "--chain"],
            "Query chain information.");
        var txOption = new Option<string>(["-t", "--tx"],
            "Query transaction information.");
        var contractOption = new Option<string>(["-c", "--contract"],
            "Query contract information.");

        var aelfCommand = new Command("#!aelf", "Query information from aelf blockchain.")
        {
            chainOption,
            txOption,
            contractOption
        };

        aelfCommand.SetHandler(
            (chain, tx, contract) => KernelInvocationContext.Current.Display(Query(chain, tx, contract)),
            chainOption,
            txOption,
            contractOption);

        kernel.AddDirective(aelfCommand);

        PocketView view = div(
            code(nameof(AElfExtensionDemo)),
            " is loaded. It helps you query information from aelf node",
            ". Try it by running: ",
            code("#!aelf -c"),
            " or ",
            code("#!aelf -t {txId}")
        );

        KernelInvocationContext.Current?.Display(view);
    }

    private static IHtmlContent Query(string chain, string txId, string contractAddress)
    {
        var client = new AElfClientBuilder().UseEndpoint("https://tdvw-test-node.aelf.io").Build();
        var chainStatus = client.GetChainStatusAsync().Result;
        return div($"ChainId: {chainStatus.ChainId}\nHeight: {chainStatus.BestChainHeight}\n" +
                   $"Hash: {chainStatus.BestChainHash}");
    }
}