using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] CartController cartController;
    [SerializeField] GamePhase gamePhase;

    Camera mainCamera;
   
    [SerializeField] float minSize, maxSize;

    [SerializeField] float sizeDivider;
    [SerializeField] float verticalOffsetPercent;

    private void Start()
    {
        mainCamera = Camera.main;   
    }

    private void Update()
    {
        if (gamePhase.currentPhase == GamePhase.Phase.LEVEL)
        {
            MoveCamToCart(cartController.MiddleOfCart());
            SizeCamToCart(cartController.CartSize(sizeDivider, minSize, maxSize));
        }
    }

    void MoveCamToCart(Vector3 pos)
    {
        mainCamera.transform.position = new Vector3(pos.x, pos.y, mainCamera.transform.position.z);
    }

    void SizeCamToCart(float cartSize)
    {
        mainCamera.orthographicSize = cartSize;
    }
}
