namespace TreeBasedCli
{
    /// <summary>
    /// Represents a tree of commands. Leaf nodes are the most specific commands
    /// assigned to corresponding actions. Inner nodes are commands used for branching
    /// into more specific child commands.
    /// </summary>
    public class CommandTree
    {
        public CommandTree(
            Command root,
            IDependencyInjectionService? dependencyInjectionService = null)
        {
            this.Root = root;
            this.DependencyInjectionService = dependencyInjectionService;
            this.SetParentReferences(this.Root, parentNode: null);
        }

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
