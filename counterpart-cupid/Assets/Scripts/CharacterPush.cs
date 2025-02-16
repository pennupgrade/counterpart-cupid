using UnityEngine;
using System.Collections;

public class CharacterPush : MonoBehaviour
{
    public Camera playerCamera;
    public float pushForce = 10f;       // How strong the push is
    public KeyCode pushKey = KeyCode.R; // Key to push the character
    public LayerMask characterLayer;    // To filter only character objects

    private CharacterPuller characterPuller;

    void Start()
    {
        characterPuller = GetComponent<CharacterPuller>(); // Get reference to pulling script
    }

    void Update()
    {
        if (Input.GetKeyDown(pushKey) && characterPuller.heldCharacter != null)
        {
            TryPushCharacter();
            // enable ai agent
            StartCoroutine(WaitToWander(characterPuller.heldCharacter));
        }
    }

    void TryPushCharacter()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, characterLayer))
        {
            GameObject targetCharacter = hit.collider.gameObject;

            if (targetCharacter != characterPuller.heldCharacter) // Ensure we're not pushing to itself
            {
                PushCharacter(targetCharacter);
            }
        }
    }

    void PushCharacter(GameObject target)
    {
        GameObject heldCharacter = characterPuller.heldCharacter;
        if (heldCharacter == null) return;

        // Re-enable physics on the held character
        Rigidbody rb = heldCharacter.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;

            // Calculate direction from held character to target
            Vector3 pushDirection = (target.transform.position - heldCharacter.transform.position).normalized;
            rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }

        // Clear the held character
        characterPuller.heldCharacter = null;
        characterPuller.isPulling = false;
    }
    
    IEnumerator WaitToWander(GameObject prevHeldCharacter)
    {
        yield return new WaitForSeconds(3);
        if (prevHeldCharacter)
        {
            // enable ai character
            prevHeldCharacter.GetComponent<NPC_Character>().EnableNavMeshAgent();
        }
    }
}
