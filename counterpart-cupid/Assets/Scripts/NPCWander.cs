using UnityEngine;
using System.Collections;

public class NPCWander : MonoBehaviour
{
    public float wanderRange = 10f;  // How far the NPC can wander
    public float wanderSpeed = 2f;   // Speed at which the NPC moves
    public float waitTime = 2f;      // Time to wait at a destination
    public float rotationSpeed = 2f; // Rotation speed to face destination

    private Vector3 targetPosition;
    private bool isWandering = false;

    private void Start()
    {
        StartCoroutine(Wander());
    }

    private void Update()
    {
        if (isWandering)
        {
            // Rotate smoothly towards the target position
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, wanderSpeed * Time.deltaTime);
        }
    }

    // Coroutine to make NPC wander between random points
    private IEnumerator Wander()
    {
        while (true)
        {
            // Choose a random target position within the wander range
            targetPosition = new Vector3(
                transform.position.x + Random.Range(-wanderRange, wanderRange),
                transform.position.y,
                transform.position.z + Random.Range(-wanderRange, wanderRange)
            );

            // Move towards the target
            isWandering = true;

            // Wait until the NPC reaches the destination
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                yield return null;
            }

            // Stop wandering and wait at the destination for a while
            isWandering = false;
            yield return new WaitForSeconds(waitTime);
        }
    }
}
