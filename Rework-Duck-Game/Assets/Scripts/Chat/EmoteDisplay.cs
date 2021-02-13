using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteDisplay : MonoBehaviour
{
    public SpriteRenderer sprt;
    public float lifeLeft;
    public int index = -1;

    //GUI

    public Vector2 finalPos;
    public bool moving = false;
    public bool isSelected = false;

    public IEnumerator UpdateSelectionFeedback() {
        if(isSelected)
            sprt.color = new Color(1,1,1,1);
        else 
            sprt.color = new Color(1,1,1,0.5f);

        while(isSelected) {
            yield return null;
        }
        
            
        yield break;
    }

    public IEnumerator Move(Vector2 translation) {
        finalPos.x+=translation.x;
        finalPos.y+=translation.y;
        moving = true;
        bool isXSet = false;
        bool isYSet = false;
        while(moving) {
            
            if(transform.localPosition.x < finalPos.x+0.05f)
                transform.localPosition+=new Vector3(0.025f,0,0);
            else if(transform.localPosition.x < finalPos.x-0.05f)
                transform.localPosition+=new Vector3(0.025f,0,0);
            else 
                isXSet = true;
           
           if(transform.localPosition.y < finalPos.y+0.05f)
                transform.localPosition+=new Vector3(0,0.025f,0);
            else if(transform.localPosition.y < finalPos.y-0.05f)
                transform.localPosition+=new Vector3(0,0.025f,0);
            else 
                isYSet = true;

            if(isYSet && isXSet)
                moving = false;
            yield return null;
        }
        yield break;
    }
    public IEnumerator FadeOut() {
        lifeLeft = -1;
        while(sprt.color.a > 0) {
            sprt.color+= new Color(0,0,0,-0.05f);
            yield return null;
        }
        Destroy(gameObject,0.1f);
        yield break;
    }
}
