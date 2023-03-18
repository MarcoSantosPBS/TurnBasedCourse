using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;

    public void SetUp(Transform originalRootBone)
    {
        //MatchAllChildTransforms(originalRootBone, ragdollRootBone);

        ApplyExplosionToRagdoll(ragdollRootBone, 300f, transform.position, 10f);
    }

    private void MatchAllChildTransforms(Transform originalRoot, Transform clone)
    {
        foreach (Transform child in originalRoot)
        {
            Transform cloneChild = clone.Find(child.name);

            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MatchAllChildTransforms(child, cloneChild);
            }
        }
    }

    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
                ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
            }
        }
    }
}
