using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform ragdollPrefab;
    [SerializeField] private Transform originalRootBone;
    
    private HealthSystem healthSystem;

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead()
    {
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        ragdollTransform.GetComponent<UnitRagdoll>().SetUp(originalRootBone);
    }
}
