using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Box;
    public float forceMagnitude = 10f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boxes"))
        {
            // Get the Rigidbody component of the collided box
            Rigidbody boxRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            if (boxRigidbody != null)
            {
                // Calculate the direction of the force based on the collision point
                Vector3 forceDirection = collision.contacts[0].point - transform.position;
                forceDirection = forceDirection.normalized;

                // Apply a force to the box Rigidbody
                boxRigidbody.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
            }
        }
    }
}
