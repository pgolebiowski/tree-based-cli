[![][nuget-img]][nuget]

[nuget]:     https://www.nuget.org/packages/TreeBasedCli
[nuget-img]: https://badge.fury.io/nu/TreeBasedCli.svg

# TreeBasedCli

<img src="image.jpg" alt="mascot ^_^" width=30% height=30%>

## 1. Project mission 🤗

TreeBasedCli is a C# library that simplifies the process of creating command-line interfaces (CLIs) with nested subcommands. It offers intuitive navigation, clear documentation, and actionable error messages to guide users through the command tree. With TreeBasedCli, you can easily organize and structure your CLI's functionality, and take advantage of native support for asynchronous command execution and customizable dependency injection.

It is a powerful choice for building CLIs of any size or complexity, designed to offer an intuitive and enjoyable experience for both developers and users.

* [Key benefits 🌟](#2-key-benefits-)
* [Key concepts 💡](#3-key-concepts-)
* [Getting started 🚀](#4-getting-started-)
   * [Leaf commands 🌱](#41-leaf-commands-)
   * [Branch commands 🌳](#42-branch-commands-)
   * [Wrapping it all up! 🎁](#43-wrapping-it-all-up-)

## 2. Key benefits 🌟

TreeBasedCli is a powerful and user-friendly choice, designed to provide both developers and users with a superb usage experience. Here are some of the key benefits of using TreeBasedCli:

### 2.1. For Users

* **Intuitive navigation.** TreeBasedCli allows you to easily organize and structure your CLI's functionality, making it easy for users to find the commands they need, even in larger and more complex CLIs.
* **Clear and concise documentation.** TreeBasedCli generates documentation at runtime, providing context-specific guidelines for using your application. This helps users understand how to use your CLI and navigate the command tree, even in complex scenarios with many subcommands and a complex command tree structure.
* **Actionable error messages.** If a user makes a mistake, TreeBasedCli provides clear and specific error messages to help them correct their input and continue using the application.

### 2.2. For Developers

* **Modular structure.** Each leaf command in TreeBasedCli has its own class with the command definition, input parser, and asynchronous handler. This makes it easy to test and maintain your code, even for large and complex CLIs.
* **Asynchronous command execution.** TreeBasedCli natively supports Task-based command execution, making it easy to build asynchronous CLIs.
* **Custom dependency injection.** TreeBasedCli includes a lightweight Dependency Injection (DI) interface, allowing you to use your preferred method of DI type resolution.

## 3. Key concepts 💡

The library is structured around the following key concepts:

* **Leaf command.** A leaf command is a terminal command in the command tree, representing a specific action that can be performed. Leaf commands are implemented as individual classes, with the command definition, input parser, and asynchronous handler contained within.
* **Branch command.** A branch command is a non-terminal command in the command tree, representing a group of subcommands (leaf of branch commands) that can be accessed by the user. Branch commands do not have an associated action, and are used to organize and structure the CLI's functionality.
* **Command tree.** The command tree is the hierarchical structure of commands in a CLI, with branch commands serving as nodes and leaf commands serving as leaves. The command tree can have multiple levels of nesting, allowing you to organize your CLI's functionality in a flexible and intuitive way.

## 4. Getting started 🚀

To get started with TreeBasedCli, you'll need to install the library using the following command:

```
dotnet add package TreeBasedCli
```

### 4.1. Leaf commands 🌱

There are 3 ways to create a leaf command, depending on your requirements.

#### 4.1.1. A leaf command that has no options or dependencies on existing objects

We will use `SimpleLeafCommand`. It represents a leaf command that comes with no options, and thus also no arguments or parser. Further, it does not come with a handler, and thus does not leverage dependency injection. Derive from this class for the simplest kinds of leaf commands, which require no parameters, and the task logic does not have a dependency on other existing objects.

```csharp
public class CreateRandomAnimalCommand : SimpleLeafCommand
{
    public CreateRandomAnimalCommand() : base(
        label: "create-random-animal",
        description: new[]
        {
            "Prints out a random animal."
        })
    { }

    public override Task TaskToRun()
    {
        var animals = new[]
        {
            "🦔",
            "🐝",
            "🐘"
        };

        string animal = animals[Random.Shared.Next(0, 3)];
        Console.WriteLine(animal);

        return Task.CompletedTask;
    }
}
```

#### 4.1.2. A leaf command that has no options, but has a dependency on existing objects

Building on top of the previous example, let's say that you would like to leverage dependency injection here, and have modular logic for the command handler.

We will use `LeafCommand<THandler>`. It represents a leaf command that comes with no options, and thus also no arguments or parser. However, it comes with a handler that leverages dependency injection. Derive from this class for the simplest kinds of leaf commands, which require no parameters, but the task logic does have a dependency on other existing objects.

```csharp
public class CreateRandomAnimalCommand :
    LeafCommand<CreateRandomAnimalCommand.Handler>
{
    public CreateRandomAnimalCommand() : base(
        label: "create-random-animal",
        description: new[]
        {
            "Prints out a random animal."
        },
        DependencyInjectionService.Instance)
    { }

    public class Handler : ILeafCommandHandler
    {
        private readonly IUserInterface userInterface;

        public Handler(IUserInterface userInterface)
        {
            this.userInterface = userInterface;
        }

        public Task HandleAsync(LeafCommand _)
        {
            var animals = new[]
            {
                "🦔",
                "🐝",
                "🐘"
            };

            string animal = animals[Random.Shared.Next(0, 3)];
            this.userInterface.WriteLine(animal);

            return Task.CompletedTask;
        }
    }
}
```

#### 4.1.3. A leaf command with options

In a general case, your commmands have options, and there will be user input to parse.

We will use `LeafCommand<TArguments, TParser, THandler>`. It represents a leaf command in a TreeBasedCli command tree. It is designed to be the most flexible and powerful way to create leaf commands, and is intended for use in scenarios where you need to specify custom options and/or inject custom dependencies.

* `TArguments` is a custom class that represents the already-parsed arguments to your command options.
* `TParser` is a custom class that parses user input into an instance of `TArguments`.
* `THandler` is a custom class that handles the execution of your command asynchronously.

To use `LeafCommand<TArguments, TParser, THandler>`, you will need to derive from it and provide implementations for `TArguments`, `TParser`, and `THandler` that suit your specific needs. This allows you to tailor your leaf commands to the unique requirements of your CLI and create a highly customized and user-friendly experience for your users.

```csharp
public class CreateCatCommand :
    LeafCommand<
        CreateCatCommand.Arguments,
        CreateCatCommand.Parser,
        CreateCatCommand.Handler>
{
    private const string NameLabel = "--name";

    public CreateCatCommand() : base(
        label: "create-cat",
        description: new[]
        {
            "Prints out a cat."
        },
        options: new[]
        {
            new CommandOption(
                label: NameLabel,
                description: new[]
                {
                    "Required. The name of the cat to print."
                }
            ),
        },
        DependencyInjectionService.Instance)
    { }

    public record Arguments(string CatName) : IParsedCommandArguments;

    public class Parser : ICommandArgumentParser<Arguments>
    {
        public IParseResult<Arguments> Parse(CommandArguments arguments)
        {
            string name = arguments.GetArgument(NameLabel).ExpectedAsSingleValue();

            var result = new Arguments(
                CatName: name
            );

            return new SuccessfulParseResult<Arguments>(result);
        }
    }

    public class Handler : ICommandHandler<Arguments>
    {
        private readonly IUserInterface userInterface;

        public Handler(IUserInterface userInterface)
        {
            this.userInterface = userInterface;
        }

        public Task HandleAsync(Arguments arguments, LeafCommand _)
        {
            this.userInterface.WriteLine($"I am a cat 😸 with the name {arguments.CatName}!");
            return Task.CompletedTask;
        }
    }
}
```

### 4.2. Branch commands 🌳

A branch command is a non-terminal command in the command tree that groups related subcommands (either leaf or branch commands) together and serves as a logical structure for your CLI. Branch commands do not have an associated action themselves, and are used solely to organize and structure the command tree. By using branch commands, you can create a hierarchical structure for your CLI that is easy to navigate and understand for users.

To create a branch command, you can use the `BranchCommand` class, which takes a `label`, a `description`, and a list of `childCommands` as arguments in its constructor. Alternatively, you can use the `BranchCommandBuilder` class to build a `BranchCommand` instance using a fluent interface.

Here is an example of how you can create a branch command using the `BranchCommand` class:

```csharp
new BranchCommand(
    label: "branch-label",
    description: new string[]
    {
        "Branch command description.",
        "Another long paragraph."
    },
    childCommands: new List<Command> { leafCommand1, leafCommand2, branchCommand1 });
```

And here is an example of how you can create a branch command using the `BranchCommandBuilder` class:

```csharp
BranchCommand branchCommand = new BranchCommandBuilder("branch-label")
    .WithDescription(new string[] { /* ... */ })
    .WithChildCommand(leafCommand1)
    .WithChildCommand(leafCommand2)
    .WithChildCommand(branchCommand1)
    .Build();
```

Once you have created your branch commands, you can nest them within other branch commands to create a hierarchy of commands within your CLI. This allows you to easily organize and structure your CLI's functionality in a way that is intuitive and user-friendly.

### 4.3. Wrapping it all up! 🎁

To wrap up your CLI application, you will need to create an instance of the `ArgumentHandler` class, which is responsible for parsing the user's input and executing the right command. You can do this by creating an instance of `ArgumentHandlerSettings`, which specifies the name, version, and command tree of your application.

To create the `ArgumentHandlerSettings` instance, you'll need to provide the name and version of your application, as well as the root of your command tree. Here is an example:

```csharp
Command rootOfYourCommandTree = branchCommand;

var settings = new ArgumentHandlerSettings
(
    name: "Animal Kingdom",
    version: "1.0",
    commandTree: new CommandTree(
        root: rootOfYourCommandTree)
);
```

Once you have your `ArgumentHandlerSettings` object, you can create an instance of the `ArgumentHandler` class and call the `HandleAsync` method, passing in the user's input as arguments. This will parse the input and execute the corresponding command in the command tree.

```csharp
internal class Program
{
    private static async Task Main(string[] args)
    {
        var settings = new ArgumentHandlerSettings
        (
            name: "Animal Kingdom",
            version: "1.0",
            commandTree: new CommandTree(
                root: CreateCommandTreeRoot())
        );

        var argumentHandler = new ArgumentHandler(settings);
        await argumentHandler.HandleAsync(args);
    }

    private static Command CreateCommandTreeRoot()
    {
        /* your command tree */
    }
}
```

## 5. Do you have more code examples? 👩‍💻

Yes, see the sample applications in this repository:

* [CryptoKit](src/samples/Samples.CryptoKit)
* [Animal Kingdom](src/samples/Samples.AnimalKingdom)

## 6. How does the CLI interface look like from the user perspective? 👀

### 6.1. Example command

Here is an example of a command-line interface you could build:

```
$ cryptokit cryptographic-algorithms aes-gcm-256 encrypt \
    --input in \
    --output out
```

### 6.2. Automatically generated documentation

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

### 6.3. Exploring subcommands

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

### 6.4. Reporting wrong command usage errors

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

## 7. Contributing 🤝

We welcome contributions to TreeBasedCli! Whether you're interested in adding new features or simply improving the documentation, there are many ways to get involved in the project.

If you'd like to contribute, please follow these steps:

1. Open a new issue to start a discussion.
2. Fork the repository and create a new branch for your changes.
3. Make your changes, and don't forget to add tests to ensure that your code is working as expected.
4. Submit a pull request with a brief description of your changes.

Thank you for considering contributing to the library! Your help is greatly appreciated and will help make this library even better for everyone.

