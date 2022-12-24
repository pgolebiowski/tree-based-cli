[![][nuget-img]][nuget]

[nuget]:     https://www.nuget.org/packages/TreeBasedCli
[nuget-img]: https://badge.fury.io/nu/TreeBasedCli.svg

# TreeBasedCli

## 1. What is this?

Welcome to TreeBasedCli, a C# library that allows you to create a command-line interface with nested subcommands. With this library, you can easily modify the structure of your command tree and provide automatically generated documentation to help your users navigate your application.

## 2. Key features

* **Nested subcommands.** Create a command-line interface with multiple levels of subcommands to easily organize and structure your application's functionality.
* **Asynchronous command execution.** Native support for `Task`-based command execution.
* **Intuitive documentation.** The library generates clear and concise documentation on the fly, providing users with context-specific guidelines for using your application, in particular if they use your application incorrectly or just want to explore what is available.
* **Individual class for each command.** Each leaf command has its own class with the command definition, input parser, and asynchronous handler. The library also includes a lightweight dependency injection interface for custom DI type resolution.

## 3. Getting started

To install TreeBasedCli, use the following command:

```
dotnet add package TreeBasedCli
```

## 4. Contributions

We welcome contributions to TreeBasedCli! If you have an idea for a new feature or a bug fix, please open an issue or create a pull request.

## 5. Show me the code!

### 5.1. Example command

Here is an example of a command-line interface you could build:

```
$ cryptokit cryptographic-algorithms aes-gcm-256 encrypt \
    --input in \
    --output out
```

### 5.2. Automatically generated documentation

If a user knows absolutely nothing about a program and invokes it without any arguments or with `-h`, `--help`, or `help`, the outcome would be similar to this:

```
$ cryptokit

                                    CryptoKit
                                       v1.0


    Command description:

        CryptoKit is a small program facilitating confidential data handling.

        Unless explicitly stated otherwise, every command interprets standard
        input and output as text encoded in UTF-8.



    Usage:

        cryptokit <child command>



    Child commands:

        check-algorithm             Check the algorithm that has been used to
                                    created a particular file. This applies only
                                    to binary files generated using this program.


        cryptographic-algorithms    Invoke a cryptographic algorithm.


        experimental                Playground for new functionalities.



    For more details on a particular child command, run:

        cryptokit help <child command>
```

### 5.3. Exploring subcommands

A user may want to explore a particular command and see what subcommands are assigned to it. For example:

```
$ cryptokit cryptographic-algorithms aes-gcm-256

                                   Security Kit
                                       v1.0


    Command description:

        Use AES in the GCM mode, with 256-bit cryptographic keys.



    Usage:

        cryptokit cryptographic-algorithms aes-gcm-256 <child command>



    Child commands:

        encrypt         Encrypts the specified file using a cryptographic key and
                        additional authenticated data.


        decrypt         Decrypts the specified file using a cryptographic key and
                        additional authenticated data.


        generate-key    Generate a 256-bit cryptographic key and print it in
                        base64.



    For more details on a particular child command, run:

        cryptokit help cryptographic-algorithms aes-gcm-256 <child command>
```

### 5.4. Reporting wrong command usage errors

If an error occurs, for example if required options are not provided:

```
                                   Security Kit
                                       v1.0


    Error:

        The command 'cryptokit cryptographic-algorithms aes-gcm-256 decrypt'
        requires the option '--input' to be specified.



    Command description:

        Decrypts the specified file using a cryptographic key.



    Usage:

        cryptokit cryptographic-algorithms aes-gcm-256 decrypt <options>



    Options:

        --input     The path to the input file that is to be decrypted.


        --output    The path to the output file where the decrypted data is to be
                    written.
```

### 5.5. How to code the handler for an individual command

When a customer correctly specifies a subcommand, the library takes care of invoking the right function, and feeding it with pre-parsed arguments.

Here is the class for the command we saw above (you can find it in the sample CLI applications):

```csharp
public class DecryptWithAesGcm256Command :
    LeafCommand<
        DecryptWithAesGcm256Command.Arguments,
        DecryptWithAesGcm256Command.Parser,
        DecryptWithAesGcm256Command.Handler>
{
    private const string InputLabel = "--input";
    private const string OutputLabel = "--output";

    public DecryptWithAesGcm256Command() : base(
        label: "decrypt",
        description: new[]
        {
            "Decrypts the specified file using a cryptographic key and additional authenticated data."
        },
        options: new[]
        {
            new CommandOption(
                label: InputLabel,
                description: new[]
                {
                    "The path to the input file that is to be decrypted."
                }
            ),
            new CommandOption(
                label: OutputLabel,
                description: new[]
                {
                    "The path to the output file where the decrypted data is to be written."
                }
            ),
        },
        DependencyInjectionService.Instance)
    { }

    public record Arguments(string InputPath, string OutputPath) : IParsedCommandArguments;

    public class Parser : ICommandArgumentParser<Arguments>
    {
        public IParseResult<Arguments> Parse(CommandArguments arguments)
        {
            string inputPath = arguments.GetArgument(InputLabel).ExpectedAsSinglePathToExistingFile();
            string outputPath = arguments.GetArgument(OutputLabel).ExpectedAsSingleValue();

            var result = new Arguments(
                InputPath: inputPath,
                OutputPath: outputPath
            );

            return new SuccessfulParseResult<Arguments>(result);
        }
    }

    public class Handler : ICommandHandler<Arguments>
    {
        public Task HandleAsync(Arguments arguments, LeafCommand _)
        {
            Console.WriteLine($"decrypting file {arguments.InputPath}");
            Console.WriteLine($"writing output to {arguments.OutputPath}");
            return Task.CompletedTask;
        }
    }
}
```

### 5.6. How do I get started with the code?

See the sample applications: `CryptoKit` and `AnimalKingdom`.

