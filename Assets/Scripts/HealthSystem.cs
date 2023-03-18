using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int health = 80;

    private float maxHealth;
    public event Action OnDead;
    public event Action OnDamage;

    private void Awake()
    {
        maxHealth = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        
        if (health < 0)
        {
            health = 0;
        }

        OnDamage?.Invoke();

        if (health == 0)
        {
            OnDead?.Invoke();
        }
    }

    public float GetHealthNormalized()
    {
        return (float)health / maxHealth;
    }
}
