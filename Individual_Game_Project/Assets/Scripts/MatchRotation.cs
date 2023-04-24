using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchRotation : MonoBehaviour
{

    private GameObject attachpointGameobject;

    void Start() {
        attachpointGameobject = this.transform.GetChild(0).GetChild(0).gameObject;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerPiece") || other.CompareTag("EnemyPiece")) {
            attachpointGameobject.transform.rotation = Quaternion.Euler(0, other.gameObject.transform.rotation.eulerAngles.y, 0);
        }
    }

}
