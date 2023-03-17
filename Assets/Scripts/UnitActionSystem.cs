using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] Unit selectedUnit;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] LayerMask unitLayerMask;

    private bool isBusy;
    private BaseAction selectedAction;
    public static UnitActionSystem Instance;

    public event Action OnSelectedUnitChange;
    public event Action OnSelectedActionChange;
    public event Action OnActionStarted;
    public event Action<bool> OnBusyChange;

    private void Awake()
    {
        if (Instance != null)
        {
            print($"There's already an instance of Unit Action System: {gameObject.name}");
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        if (isBusy) { return; }
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
        if (!TurnSystem.Instance.IsPlayerTurn()) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            if (HandleSelection()) { return; }
            HandleTakeAction();
        }
    }

    private bool HandleSelection()
    {
        Ray ray = GetRay();
        RaycastHit hit = Raycast(ray, unitLayerMask);
        if (hit.transform != null && hit.transform.TryGetComponent<Unit>(out Unit unit))
        {
            if (unit == selectedUnit) { return false; }
            if (unit.IsEnemy()) { return false; }

            SetSelectedUnit(unit);
            return true;
        }

        return false;
    }

    private void HandleTakeAction()
    {
        Ray ray = GetRay();
        RaycastHit hit = Raycast(ray, groundLayerMask);
        GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(hit.point);

        if (selectedAction.IsValidActionGridPosition(mouseGridPosition))
        {
            if (selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
            {
                SetBusy();
                selectedAction.TakeAction(mouseGridPosition, ClearBusy);
                OnActionStarted?.Invoke();
            }
        }
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChange?.Invoke(isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChange?.Invoke(isBusy);
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        OnSelectedUnitChange?.Invoke();
        SetSelectedAction(selectedUnit.GetMoveAction());
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

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        OnSelectedActionChange?.Invoke();
    }

    public Unit GetSelectedUnit() => selectedUnit;
    public BaseAction GetSelectedAction() => selectedAction; 
    
    
}
