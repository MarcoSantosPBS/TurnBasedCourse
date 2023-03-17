using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVFX;

    private Vector3 targetPosition;

    public void SetUp(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        float distanceBeforeMoving = Vector3.Distance(targetPosition, transform.position);

        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * Time.deltaTime * projectileSpeed;

        float distanceAfterMoving = Vector3.Distance(targetPosition, transform.position);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition;
            trailRenderer.transform.parent = null;
            Destroy(gameObject);
            Instantiate(bulletHitVFX, targetPosition, Quaternion.identity);
        }
    }
}
