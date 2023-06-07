using UnityEngine;

public class DamageArea : MonoBehaviour
{
    private Hit _hit;

    public void Initialize(Hit hit)
    {
        _hit = hit;
        CheckExistingEntities();
    }

    private void CheckExistingEntities()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.x / 2f);
        foreach (Collider collider in colliders)
        {
            Strike(collider);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Strike(other);
    }

    private void Strike(Collider collider)
    {
        ICombatable combatable = collider.GetComponent<ICombatable>();
        if (combatable != null)
        {
            //Debug.Log(_hit.ToString());
            combatable.TakeHit(_hit);
        }
    }
}
