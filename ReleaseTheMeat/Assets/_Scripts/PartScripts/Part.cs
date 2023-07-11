using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    public float breakForce;
    public PartTypes.Type partType;
    public bool canBreak = true;
    public bool partOfCamera = true;

    public List<Collider2D> ignoredColliders = new List<Collider2D>();
}
