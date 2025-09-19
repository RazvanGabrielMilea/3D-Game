using UnityEngine;

// Succeeds if healthPercent is strictly above the threshold.
public class HealthAboveNode : BTNode
{
    private EnemyBlackboard bb;
    private float threshold;

    public HealthAboveNode(EnemyBlackboard blackboard, float thresholdValue)
    {
        bb = blackboard;
        threshold = thresholdValue;
    }

    public override NodeState Tick()
    {
        return bb.healthPercent > threshold
            ? NodeState.Success
            : NodeState.Failure;
    }
}
