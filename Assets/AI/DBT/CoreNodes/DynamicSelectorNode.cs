using System;
using System.Collections.Generic;
using UnityEngine;
// A DynamicSelector picks exactly one child to tick each frame based on weights.
// It recomputes weights every Tick, chooses the highest‐weight child, and ticks it.
// If the chosen child differs from the previous frame, it aborts the old child.
public class DynamicSelectorNode : CompositeNode
{
    // One weight‐function per child. Each returns a float based on blackboard data.
    public List<Func<float>> weightFunctions = new List<Func<float>>();

    // Index of the child that was ticking last frame; -1 if none.
    private int currentIndex = -1;

    public override NodeState Tick()
    {
        if (children.Count == 0 || weightFunctions.Count != children.Count)
        {
            Debug.LogError("DynamicSelector: children.Count must equal weightFunctions.Count and be > 0.");
            return NodeState.Failure;
        }

        // 1) Recompute all weights & pick the child with the highest weight.
        float bestWeight = float.MinValue;
        int bestIndex = 0;
        for (int i = 0; i < children.Count; i++)
        {
            float w = weightFunctions[i]();
            if (w > bestWeight)
            {
                bestWeight = w;
                bestIndex = i;
            }
        }

        // 2) If we switched from a different child last frame, abort that old child.
        if (bestIndex != currentIndex && currentIndex >= 0)
        {
            children[currentIndex].Abort();
        }
        currentIndex = bestIndex;

        // 3) Tick the “winning” child and return its state.
        return children[currentIndex].Tick();
    }

    public override void Abort()
    {
        // If a child is currently running, abort it and clear currentIndex.
        if (currentIndex >= 0 && currentIndex < children.Count)
        {
            children[currentIndex].Abort();
        }
        currentIndex = -1;
    }
}
