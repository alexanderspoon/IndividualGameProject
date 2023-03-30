using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{

    //All of the colliders for the spell
    private List<GameObject> spellColliders = new List<GameObject>();

    //Which colliders have been hit
    private List<GameObject> collidersHit = new List<GameObject>();

    //Tracks how many colliders have been hit
    public int currentPlace = 0;

    [Tooltip("Percentage of colliders that needs to be hit to cast the spell. Ex: .8 means 80% of colliders need to be hit to cast the spell.")]
    public float successCollidedPercent = .8f; 

    [Tooltip("Percentage of colliders that you can hit in the wrong order without the spell fizziling. Ex: .1 means you can hit 10% of the colliders in the wrong order and the spell will still cast.")]
    public float wrongOrderTolerance = .1f; 

    [Tooltip("How long until the spell fizzles by itself.")]
    public float expirationTime = 40f;

    void Start() {
        SpawnSpell();
    }

    void SpawnSpell() {
        //Adds all colliders to the spellColliders list
        foreach (Transform child in transform) {
            GameObject sCollider = child.gameObject;
            spellColliders.Add(sCollider);
        }

        //Fizzles the spell after set amount of time
        Invoke("PlayFizzle", expirationTime);
    }

    //Playes when isFinished collider is hit
    public void SpellFinished() {

        //Checks if too many colliders were missed or in the wrong order
        if(PercentCollided() && PercentInOrder()) {
            PlayVFX();
        } else {
            PlayFizzle();
        }
    }

    //Destroys all colliders and the spell manager object
    private void DestroySpell() {
        foreach (GameObject collider in spellColliders)
        {
            Destroy(collider);
        }
        Destroy(this.gameObject);
    }

    //Play spell VFX here
    void PlayVFX() {
        Debug.Log("VFX played");
        DestroySpell();
    }

    //Play fizzle VFX here
    void PlayFizzle() {
        Debug.Log("Spell fizzled");
        DestroySpell();
    }

    //Check how many colliders have been hit 
    bool PercentCollided() {
        float numCollided = 0;

        foreach (GameObject collider in spellColliders)
        {
            if(collider.GetComponent<SpellCollider>().hasCollided) {
                numCollided ++;
            }
        }
        float totalColliders = spellColliders.Count;

        float percentHit = numCollided/totalColliders;

        if(percentHit >= successCollidedPercent) {
            Debug.Log("Enough hit");
            return true;
        } else {
            Debug.Log("Not enough hit");
            return false;
        }
    }

    //Check how many colliders were hit out of order
    bool PercentInOrder() {
        float numOutOfOrder = 0;

        foreach (GameObject collider in spellColliders)
        {
            if(collider.GetComponent<SpellCollider>().wrongOrder) {
                numOutOfOrder ++;
            }
        }

        float totalColliders = spellColliders.Count;

        float percentWrong = numOutOfOrder/totalColliders;

        if(percentWrong <= wrongOrderTolerance) {
            Debug.Log("Breakpoint not reached");
            return true;
        } else {
            Debug.Log("Breakpoint reached");
            return false;
        }
    }

}
