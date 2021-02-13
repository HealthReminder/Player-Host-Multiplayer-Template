using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateByTimeBehaviour : MonoBehaviour
{
    public bool isCountingDown = true;
    public float initialDelay = 0;

    private void Update() {
        if(!isCountingDown)
            return;

        if(initialDelay<=0){
            GetComponent<SmartObject>().onActivate.Invoke(-1);
            return;
        }

        if(isCountingDown)
            initialDelay-= Time.deltaTime;
        
    }
}
