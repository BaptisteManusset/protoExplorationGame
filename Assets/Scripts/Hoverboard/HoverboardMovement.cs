using UnityEngine;

public class HoverboardMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float turnSpeed = 5f;
    public float tiltAngle = 20f;
    [Range(0, 1f)] public float fallSpeed = .1f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = transform.forward * vertical * moveSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, horizontal * turnSpeed * Time.deltaTime, 0f);

        rb.AddTorque(transform.up * horizontal * tiltAngle);
        rb.MoveRotation(rb.rotation * turnRotation);
        rb.MovePosition(rb.position + movement);

        // Calculate hover height and apply upward force
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 1f))
        {
            float hoverHeight = 1f - (hit.distance / 1f);
            rb.AddForce(transform.up * hoverHeight * Physics.gravity.magnitude * rb.mass, ForceMode.Force);
        }
        else
        {
            // Apply maximum downward force if not hovering over anything
            rb.AddForce(-transform.up * Physics.gravity.magnitude * rb.mass * fallSpeed, ForceMode.Force);
        }
    }
}