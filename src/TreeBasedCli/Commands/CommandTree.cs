namespace TreeBasedCli
{
    /// <summary>
    /// A hierarchical structure of commands in a command-line interface (CLI), with each level
    /// representing a different level of specificity or functionality. The root command
    /// is typically a branch command, but can also be a single leaf command in simple CLIs.
    /// The tree branches out into subcommands, each representing a group of related commands
    /// or options, and can further branch out or end in leaf commands representing specific actions.
    /// The command tree organizes and structures the CLI's functionality, making it easier
    /// for users to navigate and developers to maintain the code.
    /// </summary>
    public class CommandTree
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandTree"/> class
        /// with a root command and an optional dependency injection service.
        /// </summary>
        /// <param name="root">The root command of the tree.</param>
        /// <param name="dependencyInjectionService">
        /// An optional dependency injection service to be used with the command tree.
        /// </param>
        public CommandTree(
            Command root,
            IDependencyInjectionService? dependencyInjectionService = null)
        {
            this.Root = root;
            this.DependencyInjectionService = dependencyInjectionService;
            this.SetParentReferences(this.Root, parentNode: null);
        }

        /// <summary>
        /// Gets the root command of the tree. This will typically be
        /// a branch command, but for a simple CLI, this
        /// may also just be a single leaf command.
        /// </summary>
        public Command Root { get; }

        internal IDependencyInjectionService? DependencyInjectionService { get; }

        private void SetParentReferences(
            Command node,
            BranchCommand? parentNode)
        {
            if (node == null)
            {
                return;
            }

            node.Parent = parentNode;
            node.Tree = this;

            if (node is BranchCommand branchNode)
            {
                if (branchNode.ChildCommands != null)
                {
                    foreach (Command childNode in branchNode.ChildCommands)
                    {
                        this.SetParentReferences(childNode, branchNode);
                    }
                }
            }
        }
    }
}
