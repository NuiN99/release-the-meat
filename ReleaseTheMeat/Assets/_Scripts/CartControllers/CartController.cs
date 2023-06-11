using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Remove unused using statements

//namespace? use one
public class CartController : MonoBehaviour
{
    //This doesn't need to be a singleton. But if you do want it to be a singleton then you need to make this work with monobehaviour
    //Example
    /*
        public static Singleton Instance { get; private set; }
        private void Awake() 
        { 
            // If there is an instance, and it's not me, delete myself.
    
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this; 
            } 
        }
     */
    //A MonoBehaviour singleton needs to be able to destroy objects



    public static CartController instance;


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
    void Awake()
    {
        if (instance == null)
            instance = this;
    }

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

    //If you aren't using Update, then remove it because Unity will still call it
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        ShowJointStress();
    }

    void ShowJointStress()
    {
        if(debugStressMode)
        //POSSIBLY SHOW AN INSTANTIATED CIRCLE WHERE JOINT IS AND THAT IS WHAT CHANGES COLOR
        if (GamePhase.instance.currentPhase == GamePhase.Phase.LEVEL && partPositions != null)
        {
            foreach (Transform part in partPositions)
            {
                part.GetComponent<SpriteRenderer>().color = Color.black;

                if (part.TryGetComponent(out HingeJoint2D joint) && joint.connectedBody != null)
                {
                    if (part.GetComponent<Plank>())
                    {
                            //I'm seeing a lot of repeat code here. Make these code blocks into a method so you can easily read this
                            // and change things. You can easily set the float breakforce as a parameter
                        Vector2 reactionForce = joint.GetReactionForce(Time.deltaTime);
                        part.GetComponent<SpriteRenderer>().color = new Color((reactionForce.x + reactionForce.y) / plankBreakForce, 0, 0, 1);;
                        if (reactionForce.x + reactionForce.y >= plankBreakForce - 50) //No magic numbers, replace 50 with a variable
                        {
                            part.GetComponent<SpriteRenderer>().color = new Color(0, 0, 255, 1);
                        }
                    }
                    if (part.GetComponent<Rod>())
                    {
                        Vector2 reactionForce = joint.GetReactionForce(Time.deltaTime);
                        part.GetComponent<SpriteRenderer>().color = new Color((reactionForce.x + reactionForce.y) / rodBreakForce, 0, 0, 1);
                        if (reactionForce.x + reactionForce.y >= rodBreakForce - 50)
                        {
                            part.GetComponent<SpriteRenderer>().color = new Color(0, 0, 255, 1);
                        }
                    } 
                }

                if (part.TryGetComponent(out WheelJoint2D wheelJoint) && wheelJoint.connectedBody != null)
                {
                    if (part.GetComponent<Wheel>())
                    {
                        Vector2 reactionForce = wheelJoint.GetReactionForce(Time.deltaTime);
                        part.GetComponent<SpriteRenderer>().color = new Color((reactionForce.x + reactionForce.y) / plankBreakForce, 0, 0, 1); ;
                        if (reactionForce.x + reactionForce.y >= wheelBreakForce - 50)
                        {
                            part.GetComponent<SpriteRenderer>().color = new Color(0, 0, 255, 1);
                        }
                    }
                }
            }
        }
    }

    //Try to use callbacks instead of using a FindObjectsOfType. I left an example on CartCompletor
    //You can even have one object hold the references of all the parts and then call it from there
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
            sum += partPos.position;
        }

        //Grab the main camera reference in start
        if (partPositions.Count == 0) return Camera.main.transform.position;
        print(partPositions.Count);
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
            //No magic numbers, make 25 a variable so you can adjust it later if need be
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

        float distX = maxX - minX;
        float distY = maxY - minY;

        float cartSize = (distX + distY) / sizeDivider;

        if (cartSize < minSize) return minSize;
        else if (cartSize > maxSize) return maxSize;

        else return cartSize;
    }
}
