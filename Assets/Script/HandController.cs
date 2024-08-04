using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour
{
    public GameObject handObject;
    public GameObject playerObject;
    public GameObject aimObject;
    public float stretchSpeed = 10f;
    public float maxStretchDistance = 5f;
    public LayerMask hitLayers;

    private Vector3 originalPosition;
    private bool isReturning = false;
    private Quaternion originalRotation;
    private Coroutine moveHandCoroutine;

    void Start()
    {
        originalPosition = handObject.transform.position;
        originalRotation = handObject.transform.rotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isReturning)
        {
            Vector3 targetPosition = GetMouseWorldPosition();
            Vector3 direction = (targetPosition - playerObject.transform.position).normalized;
            Vector3 stretchPosition = playerObject.transform.position + direction * maxStretchDistance;
            aimObject.transform.position = stretchPosition; 

            if (moveHandCoroutine != null)
            {
                StopCoroutine(moveHandCoroutine);
            }

            moveHandCoroutine = StartCoroutine(MoveHandToPosition(stretchPosition));
        }

        if (isReturning)
        {
            handObject.transform.position = Vector3.MoveTowards(handObject.transform.position, originalPosition, stretchSpeed * Time.deltaTime);
            handObject.transform.rotation = Quaternion.RotateTowards(handObject.transform.rotation, originalRotation, stretchSpeed * 100 * Time.deltaTime);
            if (Vector3.Distance(handObject.transform.position, originalPosition) < 0.1f)
            {
                isReturning = false;
                handObject.transform.position = originalPosition;
                handObject.transform.rotation = originalRotation;
            }
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z); 
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    IEnumerator MoveHandToPosition(Vector3 targetPosition)
    {
        Vector3 startPosition = handObject.transform.position;
        Quaternion startRotation = handObject.transform.rotation;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(handObject.transform.position, targetPosition) > 0.1f)
        {
            float distCovered = (Time.time - startTime) * stretchSpeed;
            float fractionOfJourney = distCovered / journeyLength;

            float newX = Mathf.Lerp(startPosition.x, targetPosition.x, fractionOfJourney);
            float newY = Mathf.Lerp(startPosition.y, targetPosition.y, fractionOfJourney);
            float newZ = Mathf.Lerp(startPosition.z, targetPosition.z, fractionOfJourney);
            handObject.transform.position = new Vector3(newX, newY, newZ);

            Vector3 direction = (targetPosition - handObject.transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                handObject.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, fractionOfJourney);
            }

            yield return null;
        }

        isReturning = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & hitLayers) != 0)
        {
            Debug.Log("Hit: " + collision.gameObject.name);
            isReturning = true;
        }
    }
}
