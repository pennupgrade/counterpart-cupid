using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC_Character : MonoBehaviour
{
    // attributes
    // current way to determine shapes
    public int shapeSet;
    public int setElement;
    public Sprite shapeAttribute;
    [SerializeField] private GameObject attributeUI;
    private UnityEngine.AI.NavMeshAgent agent;

    public void Initialize(int shapeSet, int setElement, Sprite shapeSprite)
    {
        this.shapeSet = shapeSet;
        this.setElement = setElement;
        this.shapeAttribute = shapeSprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (shapeAttribute != null)
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            attributeUI.GetComponent<SpriteRenderer>().sprite = shapeAttribute;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    bool DoesMatch(NPC_Character other)
    {
        return shapeSet == other.shapeSet && setElement != other.setElement;
    }

    public void OnTriggerEnter(Collider c)
    {
        NPC_Character other = c.gameObject.GetComponent<NPC_Character>();
        if (other is not null && DoesMatch(other))
        {
            // if it's a match -- 
            // TODO: add points or whatever
            StartCoroutine(Matched(other));
        }
    }

    public void DisableNavMeshAgent()
    {
        if (agent != null && agent.enabled)
        {
            // print("nav mesh disabled");
            agent.ResetPath();
            agent.enabled = false;
        }
    }

    public void EnableNavMeshAgent()
    {
        if (agent != null && !agent.enabled)
        {
            // print("nav mesh enabled");
            agent.enabled = true;
        }
    }

    IEnumerator Matched(NPC_Character other)
    {
        // start particles
        this.gameObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
        DisableNavMeshAgent();
        GetComponent<Rigidbody>().isKinematic = true;
        other.GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(2);
        GameManager.AddScore(1);
        Destroy(gameObject);
        Destroy(other.gameObject);
    }
}
