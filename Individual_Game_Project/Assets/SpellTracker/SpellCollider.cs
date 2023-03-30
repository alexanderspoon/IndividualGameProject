using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCollider : MonoBehaviour
{
    public int order;
    private bool isFinal;
    public bool hasCollided;
    public bool wrongOrder;

    void Start() {
        hasCollided = false;
        wrongOrder = false;
    }

    void OnTriggerEnter(UnityEngine.Collider other)
    {   

        // We can use this if we want to check what the player hit the collider with
        // if (other.CompareTag(targetTag)) {

        // }

        GameObject spellController = transform.parent.gameObject;
        hasCollided = true;

        //Checks if this collider was hit in order
        if(spellController.GetComponent<SpellManager>().currentPlace != order) {
            wrongOrder = true;
        }

        spellController.GetComponent<SpellManager>().currentPlace++;

        //Finishes the spell if it's the last collider
        if(isFinal) {
            spellController.GetComponent<SpellManager>().SpellFinished();
        }

        //Disables this gameobject so you can't hit the collider more than once
        this.gameObject.SetActive(false);
    }

}