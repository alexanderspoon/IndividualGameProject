using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{   
    public GameObject diceOrigin;
    public GameObject diePrefab;

    private int totalDamage;
    private int damageInputTotal;
    private int damageInputamount;

    private Vector3 initalVelocity = new Vector3(0,-.05f,0); 

    public void RollDice(int numDice) {
        totalDamage = 0;
        damageInputTotal = numDice;

        Vector3 offset = new Vector3 (0,0,0);

        for (int i = 0; i < numDice; i++) {
            GameObject die = Instantiate(diePrefab, diceOrigin.transform.position + offset, Random.rotation);
            die.GetComponent<Rigidbody>().velocity = initalVelocity;
            offset += new Vector3(0,.1f,0);
        }
    }

    public void AddDamage(int dmg) {

        totalDamage += dmg;
        damageInputamount ++;

        if(damageInputamount == damageInputTotal) {
            damageInputamount = 0;
            damageInputTotal = 0;

            this.gameObject.GetComponent<DealDamage>().DamagePiece(totalDamage);
        }

    }


}
