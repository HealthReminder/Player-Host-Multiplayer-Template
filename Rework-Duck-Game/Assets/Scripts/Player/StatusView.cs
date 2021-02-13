using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusView : MonoBehaviour
{
    public TMP_Text statusText;
    public bool isWorking = false;

    public void DisplayText(string text, float timeDuration) {
        StopAllCoroutines();
        StartCoroutine(DisplayTextRoutine(text, timeDuration));
    }

    IEnumerator DisplayTextRoutine(string text, float timeDuration) {
        isWorking = false;
        yield return null;
        statusText.text = text;
        statusText.color += new Color(0,0,0,1);
        isWorking = true;
        float timeLeft = timeDuration;
        while(isWorking) {
            if(timeLeft > 0){
                timeLeft-=Time.deltaTime;
            } else {
                if(statusText.color.a > 0) {
                    statusText.color+= new Color(0,0,0,-0.05f);
                } else {
                    //statusText.color += new Color(0,0,0,-1);
                    isWorking = false;
                    yield break;
                }
            }
            yield return null;
        }
        yield break;
    }

}
