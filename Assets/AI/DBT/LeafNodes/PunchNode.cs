using UnityEngine;

public class PunchNode : BTNode
{
    private EnemyBlackboard bb;
    private Animator anim;
    private float range;
    private bool hasPunched = false;

    public PunchNode(EnemyBlackboard blackboard, Animator animator, float punchRange)
    {
        bb    = blackboard;
        anim  = animator;
        range = punchRange;
    }

    public override NodeState Tick()
    {
        // 1) If the player is out of punch range, reset for next time:
        if (bb.playerDistance > range)
        {
            hasPunched = false;
            return NodeState.Failure;
        }

        // 2) If we haven't yet punched this time, do it now:
        if (!hasPunched)
        {
            // Face the player:
            Vector3 dir = (bb.playerPosition - anim.transform.position).normalized;
            if (dir.sqrMagnitude > 0.001f)
            {
                anim.transform.rotation = Quaternion.Slerp(
                    anim.transform.rotation,
                    Quaternion.LookRotation(dir),
                    Time.deltaTime * 10f
                );
            }

            // Trigger the punch animation:
            anim.SetTrigger("Punch");
            hasPunched = true;
        }

        // 3) Return Running so the Sequence never completes/restarts.
        return NodeState.Running;
    }

    public override void Abort()
    {
        // On abort (e.g. fleeing away), allow punching again next time
        hasPunched = false;
    }
}
