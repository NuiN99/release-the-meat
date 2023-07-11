using System.Collections.Generic;
using UnityEngine;

public class CartController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] GamePhase gamePhase;

    [Header("Debug Stress Mode")]
    [SerializeField] bool debugStressMode;

    List<Part> parts = new List<Part>();

    private void OnEnable()
    {
        GamePhase.OnLevel += GetPartPositions;
        GamePhase.OnLevel += SetBreakForces;
    }
    private void OnDisable()
    {
        GamePhase.OnLevel -= GetPartPositions;
        GamePhase.OnLevel -= SetBreakForces;
    }


    private void Start()
    {
        CheckDisabledBreakForces();
    }

    void CheckDisabledBreakForces()
    {
        foreach (Part part in FindObjectsOfType<Part>())
        {
            if(!part.canBreak)
            {
                part.breakForce = Mathf.Infinity;
            }
        }
    }

    void Update()
    {
        ShowJointStress();
    }

    void ShowJointStress()
    {
        if (debugStressMode && gamePhase.currentPhase == GamePhase.Phase.LEVEL)
        {
            foreach (Part part in parts)
            {
                if (part == null) continue;

                SpriteRenderer spriteRenderer = part.GetComponent<SpriteRenderer>();
                if (spriteRenderer == null) continue;

                Color baseColor = Color.black;

                spriteRenderer.color = baseColor;

                Joint2D[] joints = part.GetComponents<Joint2D>();
                if (joints == null) continue;

                foreach(Joint2D joint in joints) 
                {
                    if(joint.connectedBody != null)
                    {
                        CheckStressAndSetColor(spriteRenderer, joint.GetReactionForce(Time.deltaTime), part.breakForce, baseColor);
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

    void SetBreakForces()
    {
        foreach(Part part in parts)
        {
            Joint2D[] joints = part.GetComponents<Joint2D>();

            foreach (Joint2D joint in joints) 
            {
                joint.breakForce = part.breakForce;
            }
        }
    }

    void GetPartPositions()
    {
        parts.Clear();
        foreach(Part part in FindObjectsOfType<Part>())
        {
            if (part.partOfCamera)
            {
                parts.Add(part);
            }
        } 
    }

    public Vector3 MiddleOfCart()
    {
        Vector3 sum = Vector3.zero;
        foreach (Part part in parts)
        {
            sum += part.transform.position;
        }

        if (parts.Count == 0) return Camera.main.transform.position;
        return sum / parts.Count;
    }

    public float CartSize(float sizeDivider, float minSize, float maxSize)
    {
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;

        foreach (Part part in parts)
        {
            if (Vector2.Distance(MiddleOfCart(), part.transform.position) > 25)
            {
                parts.Remove(part);
                break;
            }
            Vector3 position = part.transform.position;

            minX = Mathf.Min(minX, position.x);
            minY = Mathf.Min(minY, position.y);
            maxX = Mathf.Max(maxX, position.x);
            maxY = Mathf.Max(maxY, position.y);
        }

        float distX = maxX - minX;
        float distY = maxY - minY;

        float cartSize = (distX + distY) / sizeDivider;

        if (cartSize < minSize) return minSize;
        else if (cartSize > maxSize) return maxSize;

        else return cartSize;
    }
}
