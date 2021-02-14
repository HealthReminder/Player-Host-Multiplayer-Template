using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTriggerBehaviour : MonoBehaviour
{
    public int ignoringPlayerID = -1;
    public bool detectCollision,detectTrigger;

    private void OnTriggerEnter2D(Collider2D other) {
        if(detectTrigger)
            if(ignoringPlayerID!=-1)
                if(other.gameObject.GetComponent<PlayerManager>())
                    if(other.gameObject.GetComponent<PlayerManager>().playerID == ignoringPlayerID)
                        return;
                    else
                       GetComponent<SmartObject>().onTriggerEnter.Invoke(-1);
                else
                    GetComponent<SmartObject>().onTriggerEnter.Invoke(-1); 
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(detectCollision){
            GetComponent<SmartObject>().onCollisionEnter.Invoke(-1);
            Debug.Log("Collided!");}
    }
}
