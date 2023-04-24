using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindFaceUp : MonoBehaviour
{
    int damage = 0;
    private bool stopped = false;
    GameObject interactionManager;

    void Start() {
        interactionManager = GameObject.Find("Interaction Manager");
    }

    void Update() {

        if (this.gameObject.GetComponent<Rigidbody>().velocity.magnitude < .01 && !stopped)  {
            stopped = true;
            damage = DetermineSideUp();
            interactionManager.GetComponent<DiceManager>().AddDamage(damage);
            Invoke("DestroyDie", .7f);
        }
    }

    private int DetermineSideUp() {
        float upThreshold = 0.99f;
    
        float dotFwd = Vector3.Dot(transform.forward, Vector3.up);
        if (dotFwd >= upThreshold) return 1;
        if (dotFwd <= -(upThreshold)) return 3;
                
        float dotRight = Vector3.Dot(transform.right, Vector3.up);
        if (dotRight >= upThreshold) return 2;
        if (dotRight <= -(upThreshold)) return 4;
                
        float dotUp = Vector3.Dot(transform.up, Vector3.up);
        if (dotUp >= upThreshold) return 6;
        if (dotUp <= -(upThreshold)) return 5;
        
        return 0;
    }

    void DestroyDie() {
        Destroy(this.gameObject);
    }
    
}
