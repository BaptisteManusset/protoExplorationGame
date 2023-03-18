using UnityEngine;

public class HoverboardLevitation : MonoBehaviour
{
    public Transform detectionZone;
    public float levitationForce = 10f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(detectionZone.position, Vector3.down, out hit))
        {
            float distance = Vector3.Distance(detectionZone.position, hit.point);
            float force = levitationForce * (1f - distance / detectionZone.localScale.y);
            rb.AddForce(Vector3.up * force, ForceMode.Acceleration);
        }
    }
}