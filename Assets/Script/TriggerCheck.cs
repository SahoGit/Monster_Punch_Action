using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheck : MonoBehaviour
{
    public GameObject[] Enemy;
    public GameObject hitEffectPrefab; 
    private bool hasBeenHit = false;
    public float forceMagnitude = 10f; 
    public levelManager levelManager;
    public float deactivateDelay = 2f;

    private void Start()
    {
        deactivateDelay = 2f;
        forceMagnitude = 100f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasBeenHit && collision.gameObject.CompareTag("Enemy"))
        {
            hasBeenHit = true;

            Animator enemyAnimator = collision.gameObject.GetComponent<Animator>();
            if (enemyAnimator != null)
            {
                AudioManager.instance.PlaySFX("HitSound");
                enemyAnimator.SetTrigger("Hit");
            }

            Rigidbody [] ragdollRigidbodies = collision.gameObject.GetComponentsInChildren<Rigidbody>();

            if (ragdollRigidbodies.Length > 0)
            {
                Vector3 forceDirection = (collision.contacts[0].point - transform.position).normalized;
                forceDirection = new Vector3(0, 0, 1);
                foreach (Rigidbody rb in ragdollRigidbodies)
                {
                    rb.constraints = RigidbodyConstraints.None;
                    //rb.constraints = RigidbodyConstraints.FreezePositionX;
                    //rb.constraints = RigidbodyConstraints.FreezePositionY;
                    rb.constraints = RigidbodyConstraints.FreezePositionY;
                    rb.AddForce(forceDirection * forceMagnitude);
                }

                //Debug.Log("Hit " + hitTransform.name + " and applied force to its ragdoll parts.");
            }

            //if (enemyRigidbody != null)
            //{
            //    Vector3 forceDirection = collision.contacts[0].point - transform.position;
            //    forceDirection = forceDirection.normalized;
            //    forceDirection = new Vector3(0, 0, 1);

            //    collision.gameObject.transform.SetParent(null);

            //    //enemyRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

            //    enemyRigidbody.constraints = RigidbodyConstraints.None;
            //    enemyRigidbody.AddForce(forceDirection*forceMagnitude, ForceMode.Impulse);


            //    //enemyRigidbody.constraints = RigidbodyConstraints.None;
            //}

            Vector3 hitPoint = collision.contacts[0].point;
            Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);


            if (levelManager != null)
            {
                levelManager.EnemyKilled();
            }
            StartCoroutine(DeactivateEnemyAfterDelay(collision.gameObject));
            StartCoroutine(ResetHitFlag());
        }
    }

    private IEnumerator ResetHitFlag()
    {
        yield return new WaitForSeconds(0.5f);
        hasBeenHit = false;
    }
    private IEnumerator DeactivateEnemyAfterDelay(GameObject enemy)
    {
        yield return new WaitForSeconds(deactivateDelay);

        enemy.SetActive(false);
    }
}
