using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugVisual : MonoBehaviour
{
    [SerializeField] TextMeshPro textMeshPro;

    protected object gridObject;

    protected virtual void Update()
    {
        textMeshPro.text = gridObject.ToString();
    }

    public virtual void Init(object gridObject)
    {
        this.gridObject = gridObject;
    }

}
