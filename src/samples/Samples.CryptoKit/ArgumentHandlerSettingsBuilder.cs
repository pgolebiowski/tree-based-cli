using TreeBasedCli;

namespace Samples.CryptoKit
{
    internal static class ArgumentHandlerSettingsBuilder
    {
        public static ArgumentHandlerSettings Build()
            => new ArgumentHandlerSettings
            (
                name: "Crypto Kit",
                version: "1.0",
                commandTree: new CommandTree(
                    root: BuildCommandTree(),
                    dependencyInjectionService: DependencyInjectionService.Instance)
            );

        private static Command BuildCommandTree()
            => new BranchCommandBuilder(label: "cryptokit")
                .WithDesription(new[]
                {
                    "CryptoKit is a small program facilitating confidential data handling.",
                    "Unless explicitly stated otherwise, every command interprets standard input and output as text encoded in UTF-8."
                })
                .WithChildCommand(BuildCheckAlgorithmCommand())
                .WithChildCommand(BuildCryptographicAlgorithmsCommand())
                .WithChildCommand(BuildExperimentalCommand())
                .WithChildCommand(new GenerateFishShellCompletionScriptCommand())
                .Build();

        private static Command BuildCheckAlgorithmCommand()
            => new BranchCommandBuilder(label: "check-algorithm")
                .WithDesription(new[]
                {
                    "Check the algorithm that has been used to create a particular file. This applies only to binary files generated using this program."
                })
                .Build();

        private static Command BuildCryptographicAlgorithmsCommand()
            => new BranchCommandBuilder(label: "cryptographic-algorithms")
                .WithDesription(new[]
                {
                    "Invoke a cryptographic algorithm."
                })
                .WithChildCommand(BuildAesGcm256Command())
                .Build();

        private static Command BuildAesGcm256Command()
            => new BranchCommandBuilder(label: "aes-gcm-256")
                .WithDesription(new[]
                {
                    "Use AES in the GCM mode, with 256-bit cryptographic keys."
                })
                .WithChildCommand(new EncryptWithAesGcm256Command())
                .WithChildCommand(new DecryptWithAesGcm256Command())
                .WithChildCommand(new GenerateKeyForAesGcm256Command())
                .Build();

        private static Command BuildExperimentalCommand()
            => new BranchCommandBuilder(label: "experimental")
                .WithDesription(new[]
                {
                    "Playground for new functionalities."
                })
                .Build();
    }
}
