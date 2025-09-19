using System;
using System.Collections.Generic;
using UnityEngine;

// Very simple test: Two “leaf” nodes that always return fixed states.
public class TestLeafNode : BTNode
{
    private NodeState result;

    public TestLeafNode(NodeState fixedResult)
    {
        result = fixedResult;
    }

    public override NodeState Tick()
    {
        Debug.Log($"TestLeafNode returning {result}");
        return result;
    }
}

public class DBTTester : MonoBehaviour
{
    private BTNode root;

    void Start()
    {
        // Create two static leaves:
        var leafA = new TestLeafNode(NodeState.Running);
        var leafB = new TestLeafNode(NodeState.Success);

        // Wrap them in a Sequence:
        var seq = new SequenceNode();
        seq.AddChild(leafA);
        seq.AddChild(leafB);

        // Put that Sequence under a DynamicSelector with dummy weight functions:
        var dyn = new DynamicSelectorNode();
        dyn.AddChild(seq);
        dyn.weightFunctions.Add(() => 1f); // constant weight

        root = dyn;
    }

    void Update()
    {
        if (root != null)
            root.Tick();
    }
}
