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

            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                Vector3 forceDirection = collision.contacts[0].point - transform.position;
                forceDirection = forceDirection.normalized;


                enemyRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;


                enemyRigidbody.AddForce(new Vector3(forceDirection.x, 0, 0) * forceMagnitude, ForceMode.Impulse);


                enemyRigidbody.constraints = RigidbodyConstraints.None;
            }

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
