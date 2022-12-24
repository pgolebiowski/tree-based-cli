namespace TreeBasedCli
{
    /// <summary>
    /// Represents a tree of commands. Leaf nodes are the most specific commands
    /// assigned to corresponding actions. Inner nodes are commands used for branching
    /// into more specific child commands.
    /// </summary>
    public class CommandTree
    {
        private Command root;

        public Command Root
        {
            get => this.root;
            set
            {
                this.root = value;
                this.SetParentReferences(this.root, parentNode: null);
            }
        }

        private void SetParentReferences(
            Command node,
            BranchCommand parentNode)
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
