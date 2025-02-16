using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

public class GrappleGun : MonoBehaviour
{
    const KeyCode useButton = KeyCode.Mouse1;
    const float FORCE_AMOUNT = 40;
    const float CLOSE_ENOUGH = 1;

    private bool isHolding = false;
    private NPC_Character heldChar = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(useButton) && !isHolding) {
            Grab();
        } else if (Input.GetKeyDown(useButton) && isHolding) {
            ReleaseChar();
        }
        if (isHolding) {
            if (Vector3.Distance(transform.position, heldChar.transform.position) <= CLOSE_ENOUGH) {
                ReleaseChar();
            }
            ApplyGrapplePhysics();
        }
    }

    void Grab() {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            GameObject target = hit.collider.gameObject;
            if (target.CompareTag("Character")) // Make sure characters have this tag
            {
                isHolding = true;
                heldChar = target.GetComponent<NPC_Character>();
                heldChar.DisableNavMeshAgent();
                heldChar.GetComponent<Rigidbody>().useGravity = false;
            }
        }
    }

    void ApplyGrapplePhysics() {
        if (heldChar != null) {
            Vector3 dir = Vector3.Normalize(transform.position - heldChar.transform.position);
            heldChar.GetComponent<Rigidbody>().AddForce(FORCE_AMOUNT * dir, ForceMode.Acceleration);
        }
    }

    void ReleaseChar() {
        heldChar.GetComponent<Rigidbody>().useGravity = true;
        StartCoroutine(WaitToWander(heldChar.gameObject));
        heldChar = null;
        isHolding = false;
    }

    IEnumerator WaitToWander(GameObject prevHeldCharacter)
    {
        NavMeshHit h;
        while (heldChar != prevHeldCharacter && !NavMesh.SamplePosition(prevHeldCharacter.transform.position, out h, 0.65f, NavMesh.AllAreas)) {
            print("NOT FOUND");
            yield return null;
        }
        if (heldChar != prevHeldCharacter)
        {
            // enable ai character
            prevHeldCharacter.GetComponent<NPC_Character>().EnableNavMeshAgent();
        }
    }
}
