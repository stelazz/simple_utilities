using UnityEngine;
using UnityEngine.InputSystem;

public class SU_Crosshair : MonoBehaviour
{
    [SerializeField] private GameObject humanoid;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private bool crosshairMovable;
    [SerializeField] private Camera camera;
    private Rigidbody2D rb;


    public Vector3 MouseWorldPosition { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
        {
            rb = rigidbody;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = camera.nearClipPlane;
        MouseWorldPosition = mousePosition;
        Vector3 humanoidPosition = humanoid.transform.position;
        if (rb)
        {
            humanoidPosition = rb.position;
        }

        Vector3 angle = mousePosition - humanoidPosition;
        if (crosshairMovable)
        {
            angle.Normalize();
            angle *= 10;
        }

        crosshair.transform.position = angle + humanoidPosition;
    }
}