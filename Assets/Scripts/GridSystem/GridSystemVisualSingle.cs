using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;

    public void Show(Material gridVisualTypeMaterial)
    {
        meshRenderer.enabled = true;
        meshRenderer.material = gridVisualTypeMaterial;
    }

    public void Hide()
    {
        meshRenderer.enabled = false;
    }

}
