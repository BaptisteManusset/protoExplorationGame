using UnityEngine;
using System.Collections;

public class Levitation : MonoBehaviour
{
    public float levitationHeight = 1.0f;
    public AnimationCurve forceCurve;
    public float directionForce = 10.0f;

    private Rigidbody rb;
    private RaycastHit hitInfo;

    public float value;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // if (Physics.Raycast(transform.position, Vector3.down, out hitInfo))
        // {
        //     float distanceToGround = hitInfo.distance;
        //
        //     value = distanceToGround.Remap(0, levitationHeight, levitationHeight, 0);
        //
        //     rb.MovePosition(Vector3.up * -value);
        // }
        rb.MovePosition(Vector3.up * NewMethod() * Time.deltaTime);
    }

    void AddLevitationForce(RaycastHit hitInfo)
    {
        Vector3 targetPosition = transform.position - Vector3.down * forceCurve.Evaluate(hitInfo.distance);
        rb.MovePosition(targetPosition);
    }

    void AddDirectionForce()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(transform.forward * directionForce, ForceMode.Acceleration);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.AddForce(-transform.forward * directionForce, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(-rb.velocity * directionForce, ForceMode.VelocityChange);
        }
    }

    void OnDrawGizmos()
    {
        // NewMethod();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * levitationHeight);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * value);
        Gizmos.DrawSphere(transform.position + Vector3.up * value, value);
    }

    private float NewMethod()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo))
        {
            float distanceToGround = hitInfo.distance;

            value = distanceToGround.Remap(0, levitationHeight, levitationHeight, 0);

            if (value < 0)
            {
                value *= .2f;
                return value;
            }
        }

        return 0;
    }
}