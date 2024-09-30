using UnityEngine;

public class PhysicalCard : MonoBehaviour
{
    public Card cardObject;
    private bool isInHand = false;
    public bool IsInHand { get; set; }

    private bool isDragging = false;

    private Rigidbody rb;
    private Vector3 mOffset; // Offset of the mouse
    private Vector3 lastMousePosition; // To track the last mouse position for velocity calculation
    private Vector3 currentVelocity;   // Velocity of the card for the throwing effect

    private float minVelocityMagnitude = 0.1f;
    public float maxVelocityMagnitude = 10f;
    public float smoothingFactor = 0.1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        mOffset = this.transform.position - GetMouseWorldPosition(); // Calculate the offset between the card's position and the mouse position

        mOffset = transform.position - GetMouseWorldPosition();
        lastMousePosition = GetMouseWorldPosition();
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 newPosition = GetMouseWorldPosition() + mOffset;
            newPosition.y = 1f;
            transform.position = newPosition;

            Vector3 currentMousePosition = GetMouseWorldPosition();
            Vector3 frameVelocity = (currentMousePosition - lastMousePosition) / Time.deltaTime;

            currentVelocity = Vector3.Lerp(currentVelocity, frameVelocity, smoothingFactor);
            currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxVelocityMagnitude);

            lastMousePosition = currentMousePosition;

            if (Input.GetMouseButtonUp(0))
            {
                StopDragging();
            }
        }
    }

    public void StartDragging()
    {
        if (isInHand) return;

        isDragging = true;
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        mOffset = transform.position - GetMouseWorldPosition();
        lastMousePosition = GetMouseWorldPosition();
        currentVelocity = Vector3.zero;
    }

    private void StopDragging()
    {
        if (!isDragging) return;

        isDragging = false;
        FindObjectOfType<Deck>().StopDraggingCard();

        if (rb != null)
        {
            rb.isKinematic = false;

            if (currentVelocity.magnitude > minVelocityMagnitude) rb.velocity = currentVelocity;
            else rb.velocity = Vector3.zero;
        }
        else
        {
            Debug.LogError("Rigidbody is null on stop dragging!");
        }

        currentVelocity = Vector3.zero;
    }



    public void CardRenderer()
    {
        // Get the Renderer component from the prefab object and this one
        Renderer sourceRenderer = cardObject.model.GetComponent<Renderer>();
        Renderer thisRenderer = this.GetComponent<Renderer>();

        if (sourceRenderer != null && thisRenderer != null)
        {
            // Copy the material from the source object to this object
            thisRenderer.material = sourceRenderer.sharedMaterial;
        }
    }

    private Vector3 GetMouseWorldPosition()  // Return the mouse position in world coordinates
    {
        // Using a raycast on a virtual plane where the card can be dragged
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0, 1f, 0));

        if (plane.Raycast(ray, out float distanceToPlane))
        {
            return ray.GetPoint(distanceToPlane);
        }

        Debug.LogWarning("Failed to get mouse world position");
        return Vector3.zero;
    }

    private void OnMouseDown()
    {
        if (!isDragging && !isInHand)
        {
            StartDragging();
        }
    }

    //private void OnMouseDown()
    //{
    //    if (isInHand) return; // If the card in in hand it can't be dragged anymore

    //    isDragging = true;
    //    rb.isKinematic = true;
    //    mOffset = transform.position - GetMouseWorldPosition();
    //    lastMousePosition = GetMouseWorldPosition();

    //    currentVelocity = Vector3.zero;
    //}

    //private void OnMouseDrag()
    //{
    //    if (!isDragging) return;

    //    Vector3 newPosition = GetMouseWorldPosition() + mOffset;
    //    newPosition.y = 1f; // The height is fixed in the scene
    //    transform.position = newPosition;

    //    Vector3 currentMousePosition = GetMouseWorldPosition();
    //    Vector3 frameVelocity = (currentMousePosition - lastMousePosition) / Time.deltaTime;

    //    // Smooth the velocity
    //    currentVelocity = Vector3.Lerp(currentVelocity, frameVelocity, smoothingFactor);

    //    // Clamp the velocity magnitude
    //    currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxVelocityMagnitude);

    //    lastMousePosition = currentMousePosition;
    //}

    //private void OnMouseUp()
    //{
    //    if (!isDragging) return;

    //    isDragging = false;
    //    FindObjectOfType<Deck>().StopDraggingCard();

    //    if (rb != null)
    //    {
    //        rb.isKinematic = false;

    //        if (currentVelocity.magnitude > minVelocityMagnitude) rb.velocity = currentVelocity;
    //        else rb.velocity = Vector3.zero;
    //    }
    //    else
    //    {
    //        Debug.LogError("Rigidbody is null on mouse up!");
    //    }

    //    currentVelocity = Vector3.zero;
    //}


}
