using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC_Character : MonoBehaviour
{
    // attributes
    // current way to determine shapes
    [SerializeField] private int shapeSet;
    [SerializeField] private int setElement;
    public Sprite shapeAttribute;
    [SerializeField] private GameObject attributeUI;
    private UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (shapeAttribute != null) {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            attributeUI.GetComponent<SpriteRenderer>().sprite = shapeAttribute;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    bool DoesMatch(NPC_Character other) {
        return shapeSet == other.shapeSet && setElement != other.setElement;
    }

    public void OnTriggerEnter(Collider c)
    {
        NPC_Character other = c.gameObject.GetComponent<NPC_Character>();
        if (other is not null && DoesMatch(other)) {
            // if it's a match -- 
            // TODO: add points or whatever
            GameManager.AddScore(1);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    public void DisableNavMeshAgent()
    {
        if (agent != null && agent.enabled)
        {
            print("nav mesh disabled");
            agent.ResetPath();
            agent.enabled = false;
        }
    }

    public void EnableNavMeshAgent()
    {
        if (agent != null && !agent.enabled)
        {
            print("nav mesh enabled");
            agent.enabled = true;
        }
    }
}
