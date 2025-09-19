// Assets/AI/DBT/LeafNodes/CheckPlayerDistanceNode.cs
using UnityEngine;

public class CheckPlayerDistanceNode : BTNode
{
    private EnemyBlackboard bb;
    private float range;
    public CheckPlayerDistanceNode(EnemyBlackboard blackboard, float detectRange)
    {
        bb = blackboard;
        range = detectRange;
    }

    public override NodeState Tick()
    {
        // purely distance-based
        return bb.playerDistance <= range
            ? NodeState.Success
            : NodeState.Failure;
    }
}
