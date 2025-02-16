using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NPCWander : MonoBehaviour
{
    public float wanderRange = 10f;  // How far the NPC can wander
    public float waitTime = 2f;      // Time to wait at a destination

    private NavMeshAgent agent;
    private Vector3 targetPosition;
    private bool isWandering = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Wander());
    }

    void Update()
    {

        // Ensure NavMeshAgent is enabled and placed on a valid NavMesh before checking its distance
        if (agent.enabled && IsOnNavMesh())
        {
            // If NPC has reached the target and is idle, wait at the destination before moving again
            if (!isWandering && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                StartCoroutine(Wander());  // Start wandering again
            }
        }
    }

    // Check if the NPC is on a valid NavMesh
    private bool IsOnNavMesh()
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas);
    }

    // Coroutine to make NPC wander between random points
    private IEnumerator Wander()
    {
        while (agent.enabled)
        {
            // Choose a random target position within the wander range
            targetPosition = new Vector3(
                transform.position.x + Random.Range(-wanderRange, wanderRange),
                transform.position.y,
                transform.position.z + Random.Range(-wanderRange, wanderRange)
            );

            // Set the agent's destination to the target position
            agent.SetDestination(targetPosition);
            isWandering = true;

            // Wait until the NPC reaches the destination
            while (agent.enabled && (agent.pathPending || agent.remainingDistance > agent.stoppingDistance))
            {
                yield return null;
            }

            // NPC has arrived at the destination, wait before wandering again
            isWandering = false;
            yield return new WaitForSeconds(waitTime);
        }
    }
}
