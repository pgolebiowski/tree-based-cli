[![][nuget-img]][nuget]

[nuget]:     https://www.nuget.org/packages/TreeBasedCli
[nuget-img]: https://badge.fury.io/nu/TreeBasedCli.svg

# Tree-based command-line arguments parser

## About

This library offers a framework for building a user interface with arbitrarily nested subcommands. Here is an example of a command-line interface you could build:

```
$ cryptokit cryptographic-algorithms aes-gcm-256 encrypt \
    --input in \
    --output out
```

## Goals

* Asynchronous command execution
* Trivial command-tree structure modifications
* Documentation generated automatically
* Intuitive navigation

## Automatically generated documentation

The framework generates documentation on the fly. If your customers use your application incorrectly or just want to explore what is available, the library will present them with concise contextual guidelines.

For example, if a user knows absolutely nothing about a program and invokes it without any arguments or with `-h`, `--help`, or `help`, the outcome would be similar to this:

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

When a customer correctly specifies a subcommand, the library takes care of invoking the right function, and feeding it with pre-parsed arguments.

```csharp
private static void Decrypt(CommandArguments args)
{
    // ...
}
```

## How do I use it?

You have to create an object `ArgumentHandlerSettings`, feed it to `ArgumentHandler`, and run it on program arguments:

```csharp
private static void Main(string[] arguments)
{
    var argumentHandlerSettings = new ArgumentHandlerSettings
    {
        Name = "Animal Kingdom",
        Version = "v1.0",
        CommandTree = new CommandTree
        {
            Root = BuildCommandTree()
        }
    };

    var argumentHandler = new ArgumentHandler(argumentHandlerSettings);
    argumentHandler.Handle(arguments);
}
```

The tree of commands could be for example:

```csharp
private static Command BuildCommandTree()
    => new BranchCommand
    {
        Label = "animal-kingdom",
        Description = new[]
        {
            "Prints animals."
        },
        ChildCommands = new Command[]
        {
            BuildDogCommand(),
            BuildCatCommand()
        }
    };

private static Command BuildDogCommand()
    => new LeafCommand
    {
        Label = "dog",
        Description = new[] { "Print a dog." },
        TaskToRun = x =>
        {
            Console.WriteLine("🐩");
            return Task.CompletedTask;
        }
    };

private static Command BuildCatCommand()
    => new LeafCommand
    {
        Label = "cat",
        Description = new[] { "Print a cat." },
        TaskToRun = x =>
        {
            Console.WriteLine("🐈");
            return Task.CompletedTask;
        }
    };
```

If the program is invoked without arguments, the library generates:

```
$ animal-kingdom

                                                    Animal Kingdom
                                                         v1.0


    Command description:

        Prints animals.



    Usage:

        animal-kingdom <child command>



    Child commands:

        dog    Print a dog.


        cat    Print a cat.



    For more details on a particular child command, run:

        animal-kingdom help <child command>
```

If a help on a command is requested:

```
$ animal-kingdom help cat

                                                    Animal Kingdom
                                                         v1.0


    Command description:

        Print a cat.



    Usage:

        animal-kingdom cat
```

And if a particular subcommand is selected:

```
$ animal-kingdom dog
🐩
```
