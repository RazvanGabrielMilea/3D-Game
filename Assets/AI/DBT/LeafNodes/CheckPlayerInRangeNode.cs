using UnityEngine;

public class CheckPlayerInRangeNode : BTNode
{
    private EnemyBlackboard bb;
    private float range;

    public CheckPlayerInRangeNode(EnemyBlackboard blackboard, float detectRange)
    {
        bb = blackboard;
        range = detectRange;
    }

    public override NodeState Tick()
    {
        return bb.playerDistance <= range 
            ? NodeState.Success 
            : NodeState.Failure;
    }
}
