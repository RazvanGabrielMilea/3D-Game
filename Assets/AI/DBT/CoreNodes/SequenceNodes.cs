using System.Collections.Generic;

// A Sequence runs each child in order until one fails.
// If a child returns Running, Sequence returns Running.
// If all children return Success, Sequence returns Success.
// If any child returns Failure, Sequence returns Failure and resets.
public class SequenceNode : CompositeNode
{
    private int currentChild = 0;

    public override NodeState Tick()
    {
        // Loop through children starting at currentChild
        while (currentChild < children.Count)
        {
            NodeState state = children[currentChild].Tick();

            // If child is still running, return Running now
            if (state == NodeState.Running)
                return NodeState.Running;

            // If child failed, reset and return Failure
            if (state == NodeState.Failure)
            {
                currentChild = 0;
                return NodeState.Failure;
            }

            // Child succeeded → move to next child
            currentChild++;
        }

        // All children succeeded
        currentChild = 0;
        return NodeState.Success;
    }

    public override void Abort()
    {
        // When interrupted, reset index and abort any running child
        if (currentChild < children.Count)
        {
            children[currentChild].Abort();
        }
        currentChild = 0;
    }
}
