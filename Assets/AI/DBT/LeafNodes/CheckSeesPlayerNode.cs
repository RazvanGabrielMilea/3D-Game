using UnityEngine;

public class CheckSeesPlayerNode : BTNode
{
    private EnemyBlackboard bb;

    public CheckSeesPlayerNode(EnemyBlackboard blackboard)
    {
        bb = blackboard;
    }

    public override NodeState Tick()
    {
        // Returns Success if seesPlayer is true, else Failure.
        return bb.seesPlayer 
            ? NodeState.Success 
            : NodeState.Failure;
    }
}
