using System.Collections.Generic;
using UnityEngine;

public class CartController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] GamePhase gamePhase;

    [Header("Debug Stress Mode")]
    [SerializeField] bool debugStressMode;

    [Header("Toggle Break Forces")]
    [SerializeField] bool plankBreakEnabled;
    [SerializeField] bool rodBreakEnabled;
    [SerializeField] bool wheelBreakEnabled;

    [Header("Break Forces")]
    public float plankBreakForce;
    public float rodBreakForce;
    public float wheelBreakForce;

    List<Transform> partPositions = new List<Transform>();

    private void Start()
    {
        CheckDisabledBreakForces();
    }

    void CheckDisabledBreakForces()
    {
        if(!plankBreakEnabled) plankBreakForce = Mathf.Infinity;

        if (!rodBreakEnabled) rodBreakForce = Mathf.Infinity;

        if (!wheelBreakEnabled) wheelBreakForce = Mathf.Infinity;
    }

    void Update()
    {
        ShowJointStress();
    }

    void ShowJointStress()
    {
        if (debugStressMode && gamePhase.currentPhase == GamePhase.Phase.LEVEL && partPositions != null)
        {
            foreach (Transform part in partPositions)
            {
                if (part == null) continue;

                SpriteRenderer spriteRenderer = part.GetComponent<SpriteRenderer>();
                Color baseColor = Color.black;

                spriteRenderer.color = baseColor;

                if (part.TryGetComponent(out Joint2D joint) && joint.connectedBody != null)
                {
                    if (part.GetComponent<Plank>())
                    {
                        CheckStressAndSetColor(spriteRenderer, joint.GetReactionForce(Time.deltaTime), plankBreakForce, baseColor);
                    }
                    else if (part.GetComponent<Rod>())
                    {
                        CheckStressAndSetColor(spriteRenderer, joint.GetReactionForce(Time.deltaTime), rodBreakForce, baseColor);
                    }
                    if (part.GetComponent<Wheel>())
                    {
                        CheckStressAndSetColor(spriteRenderer, joint.GetReactionForce(Time.deltaTime), wheelBreakForce, baseColor);
                    }
                }
            }
        }
    }

    void CheckStressAndSetColor(SpriteRenderer spriteRenderer, Vector2 reactionForce, float breakForce, Color baseColor)
    {
        spriteRenderer.color = new Color((reactionForce.x + reactionForce.y) / breakForce, 0, 0, 1);
        if (reactionForce.x + reactionForce.y >= breakForce - 50)
        {
            spriteRenderer.color = new Color(0, 0, 255, 1);
        }
    }


    public void GetPartPositions()
    {
        Part[] parts = FindObjectsOfType<Part>();

        foreach (Part part in parts)
        {
            partPositions.Add(part.gameObject.transform);
        }
    }

    public Vector3 MiddleOfCart()
    {
        Vector3 sum = Vector3.zero;
        foreach (Transform partPos in partPositions)
        {
            if(partPos != null)
            sum += partPos.position;
        }

        if (partPositions.Count == 0) return Camera.main.transform.position;
        return sum / partPositions.Count;
    }

    public float CartSize(float sizeDivider, float minSize, float maxSize)
    {
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;

        foreach (Transform partPos in partPositions)
        {
            if(partPos != null)
            {
                if (Vector2.Distance(MiddleOfCart(), partPos.position) > 25)
                {
                    partPositions.Remove(partPos);
                    break;
                }

                Vector3 position = partPos.position;

                minX = Mathf.Min(minX, position.x);
                minY = Mathf.Min(minY, position.y);
                maxX = Mathf.Max(maxX, position.x);
                maxY = Mathf.Max(maxY, position.y);
            }
        }

        float distX = maxX - minX;
        float distY = maxY - minY;

        float cartSize = (distX + distY) / sizeDivider;

        if (cartSize < minSize) return minSize;
        else if (cartSize > maxSize) return maxSize;

        else return cartSize;
    }
}
