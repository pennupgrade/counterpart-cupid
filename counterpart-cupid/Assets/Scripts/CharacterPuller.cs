using UnityEngine;

public class CharacterPuller : MonoBehaviour
{
    public Camera playerCamera;         // Assign your main camera
    public float pullSpeed = 5f;        // Speed of pulling the character
    public Transform holdPosition;      // Where the character will be held
    public KeyCode pullKey = KeyCode.E; // Key to pull the character

    // bad practice to make it completely public
    // but this is for a game jam so it's ok if it's bad
    public GameObject heldCharacter = null;
    public bool isPulling = false;

    void Update()
    {
        if (Input.GetKeyDown(pullKey) && heldCharacter == null)
        {
            TryPullCharacter();
        }
        else if (Input.GetKeyDown(pullKey) && heldCharacter != null)
        {
            ReleaseCharacter();
        }

        if (isPulling && heldCharacter != null)
        {
            MoveCharacterToHoldPosition();
        }
    }

    void TryPullCharacter()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            GameObject target = hit.collider.gameObject;

            if (target.CompareTag("Character")) // Make sure characters have this tag
            {                
                heldCharacter = target;
                isPulling = true;

                // Disable physics so the object moves smoothly
                Rigidbody rb = heldCharacter.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;
            }
        }
    }

    void MoveCharacterToHoldPosition()
    {
        if (heldCharacter != null)
        {
            heldCharacter.transform.position = Vector3.Lerp(
                heldCharacter.transform.position,
                holdPosition.position,
                pullSpeed * Time.deltaTime
            );
        }
    }

    void ReleaseCharacter()
    {
        if (heldCharacter != null)
        {
            // Re-enable physics if it has a Rigidbody
            Rigidbody rb = heldCharacter.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = false;

            heldCharacter = null;
            isPulling = false;
        }
    }
}
