using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathfindingDebugObject : GridDebugVisual
{
    [SerializeField] private TextMeshPro gCost;
    [SerializeField] private TextMeshPro hCost;
    [SerializeField] private TextMeshPro fCost;

    private PathNode pathNode;

    public  override void Init(object gridObject)
    {
        base.Init(gridObject);

        pathNode = gridObject as PathNode;
    }

    protected override void Update()
    {
        base.Update();

        gCost.text = pathNode.GetGCost().ToString();
        hCost.text = pathNode.GetHCost().ToString();
        fCost.text = pathNode.GetFCost().ToString();
    }
}
