using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public Camera playerCamera;
    public LayerMask characterLayer;
    private GameObject currentTarget;
    private Outline currentOutline;

    void Update()
    {
        SelectCharacter();
    }

    void SelectCharacter()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, characterLayer))
        {
            GameObject newTarget = hit.collider.gameObject;

            if (newTarget != currentTarget)
            {
                RemoveOutline();
                currentTarget = newTarget;
                ApplyOutline(currentTarget);
            }
        }
        else
        {
            RemoveOutline();
        }
    }

    void ApplyOutline(GameObject target)
    {
        Outline outline = target.GetComponent<Outline>();
        if (outline != null) // Ensure the character has an outline component
        {
            outline.enabled = true; // Enable it instead of adding/removing
            currentOutline = outline;
        }
    }

    void RemoveOutline()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false; // Simply disable the outline
            currentOutline = null;
        }
        currentTarget = null;
    }
}
