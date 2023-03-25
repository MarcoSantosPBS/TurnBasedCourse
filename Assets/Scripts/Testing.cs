using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] LayerMask layerMask;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(Raycast(GetRay(), layerMask).point);
            GridPosition startGridPosition = new GridPosition(0, 0);

            List<GridPosition> path = Pathfinding.Instance.FindNode(startGridPosition, mouseGridPosition);

            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(
                    LevelGrid.Instance.GetWorldPosition(path[i]),
                    LevelGrid.Instance.GetWorldPosition(path[i + 1]),
                    Color.red,
                    10f
                    );
            }
        }
    }

    private RaycastHit Raycast(Ray ray, LayerMask layerMask)
    {
        Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask);
        return hit;
    }

    private Ray GetRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}
