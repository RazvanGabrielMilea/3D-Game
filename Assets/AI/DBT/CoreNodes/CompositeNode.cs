using System.Collections.Generic;

// A CompositeNode holds and manages multiple children.
public abstract class CompositeNode : BTNode
{
    // List of child nodes
    protected List<BTNode> children = new List<BTNode>();

    // Add a child to this composite
    public void AddChild(BTNode child)
    {
        children.Add(child);
    }
}
