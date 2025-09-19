using UnityEngine;

// Returns Success if the enemy’s healthPercent is below the given threshold.
public class HealthBelowNode : BTNode
{
    private EnemyBlackboard bb;
    private float threshold;

    public HealthBelowNode(EnemyBlackboard blackboard, float thresholdValue)
    {
        bb = blackboard;
        threshold = thresholdValue;
    }

    public override NodeState Tick()
    {
        return bb.healthPercent < threshold
            ? NodeState.Success
            : NodeState.Failure;
    }
}
