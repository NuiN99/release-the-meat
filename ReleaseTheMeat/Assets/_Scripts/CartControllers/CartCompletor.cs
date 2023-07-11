using UnityEngine;

public class CartCompletor : MonoBehaviour
{
    [SerializeField] float collisionIgnoreRadius = 0.25f;
    private void OnEnable()
    {
        GamePhase.OnLevel += CompleteCart;
    }

    private void OnDisable()
    {
        GamePhase.OnLevel -= CompleteCart;
    }

    public void CompleteCart()
    {
        Part[] parts = FindObjectsOfType<Part>();
        foreach (Part part in parts)
        {
            if(part.TryGetComponent(out Rigidbody2D partRB))
            {
                partRB.bodyType = RigidbodyType2D.Dynamic;
            }
            CheckPartsAndIgnore(part.gameObject);
        }                         
    }

    void CheckPartsAndIgnore(GameObject part)
    {
        if(part.TryGetComponent(out ExtendablePart extendablePart))
        {
            if (extendablePart.objAttachedToStart != null)
            {
                IgnoreCollisionForCloseParts(part, extendablePart.startPoint, collisionIgnoreRadius);
            }
            if (extendablePart.objAttachedToEnd != null)
            {
                IgnoreCollisionForCloseParts(part, extendablePart.endPoint, collisionIgnoreRadius);
            }
        }
        else if(part.TryGetComponent(out SimplePart simplePart) && simplePart.attached)
        {
            //replace with variable later
            IgnoreCollisionForCloseParts(part, part.transform.position, 1.1f); 
        }
    }

    void IgnoreCollisionForCloseParts(GameObject part, Vector3 raycastPos, float radius)
    {
        RaycastHit2D[] collisionDisabler = Physics2D.CircleCastAll(raycastPos, radius, Vector3.zero, 0);
        foreach (RaycastHit2D hit in collisionDisabler)
        {
            if (hit.collider.gameObject.GetComponent<Part>() == null) continue;
            if (part.TryGetComponent(out Collider2D partCollider))
            {
                if(part.TryGetComponent(out Part partScript))
                {
                    partScript.ignoredColliders.Add(hit.collider);
                }

                Physics2D.IgnoreCollision(hit.collider, partCollider, true);
            }
        }
    }
}
