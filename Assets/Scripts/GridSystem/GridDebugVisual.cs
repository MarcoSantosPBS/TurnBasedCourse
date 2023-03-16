using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugVisual : MonoBehaviour
{
    [SerializeField] TextMeshPro textMeshPro;
    private GridObject gridObject;

    private void Update()
    {
        textMeshPro.text = gridObject.ToString();
    }

    public void Init(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

}
