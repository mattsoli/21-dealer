using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalCard : MonoBehaviour
{
    public Card cardObject;
    public bool isInHand = false;

    private bool isDragging = false;

    private Rigidbody rb;
    private Vector3 mOffset; // Offset of the mouse
    private Vector3 lastMousePosition; // To track the last mouse position for velocity calculation
    private Vector3 currentVelocity;   // Velocity of the card for the throwing effect

    private void Start()
    {
        mOffset = this.transform.position - GetMouseWorldPosition(); // Calculate the offset between the card's position and the mouse position
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

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
        // Create a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Plane is set to XZ (Y = fixedY), which matches the table height
        Plane plane = new(Vector3.up, new Vector3(0, 1f, 0));

        if (plane.Raycast(ray, out float distanceToPlane))
        {
            // Return the point where the ray intersects the XZ plane
            return ray.GetPoint(distanceToPlane);
        }

        return Vector3.zero;  // Default return value (shouldn't happen if the plane is properly set up)
    }

    private void OnMouseDown()
    {
        if (isInHand)
            return; // If the PhysicalCard is in an Hand, can't be moved again

        isDragging = true;

        if (rb != null)
            rb.isKinematic = true; // Make the card kinematic while dragging

        // Calculate the offset between the mouse click position and the card's position on the XZ plane
        mOffset = transform.position - GetMouseWorldPosition();

        // Store the last mouse position to calculate velocity later
        lastMousePosition = GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        if (!isDragging)
            return;

        // Update the card's position based on the mouse movement, using the offset
        Vector3 newPosition = GetMouseWorldPosition() + mOffset;
        // Keep the Y position fixed to the height of the table
        newPosition.y = 1f;
        transform.position = newPosition;

        // Calculate the current velocity based on the difference between the last and current mouse positions
        currentVelocity = (GetMouseWorldPosition() - lastMousePosition) / Time.deltaTime;

        lastMousePosition = GetMouseWorldPosition();
    }

    private void OnMouseUp()
    {
        isDragging = false;
        FindObjectOfType<Deck>().StopDraggingCard();

        // Apply the throwing effect (velocity) when the mouse button is released
        if (rb != null)
        {
            rb.isKinematic = false; // Enable physics after releasing
            rb.velocity = currentVelocity; // Apply the velocity calculated during dragging
        }
    }


}
