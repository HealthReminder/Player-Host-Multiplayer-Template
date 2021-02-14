using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EmoteView : MonoBehaviour
{
    public GameObject displayTemplate;
    [SerializeField]public List<EmoteDisplay> displaying;
    public Transform emoteContainer;
    public int maxDisplays;

    private void Start() {
        displaying = new List<EmoteDisplay>();
    }
    private void Update() {
        for (int i = displaying.Count-1; i >= 0 ; i--)
        {
            if(displaying[i]){
                displaying[i].lifeLeft -= Time.deltaTime;
                if(displaying[i].lifeLeft <= 0){
                    GameObject excessObj = displaying[i].sprt.gameObject;
                    StartCoroutine(displaying[i].FadeOut());
                    displaying.RemoveAt(i);
                }
            }
        }
    }
    
    public void AddEmote(Sprite sprt) {

        //Create new data
        EmoteDisplay newEmoteDisplay = Instantiate(displayTemplate).GetComponent<EmoteDisplay>();
        newEmoteDisplay.gameObject.SetActive(true);
        //Set up displayer
        newEmoteDisplay.sprt.sprite = sprt;
        newEmoteDisplay.sprt.transform.parent = emoteContainer;
        
        //Move other emotes
        foreach (EmoteDisplay ed in displaying)
        {
            ed.StartCoroutine(ed.Move(new Vector2(0,1.5f)));
            
        }
        //Move the new display
        newEmoteDisplay.sprt.transform.position = emoteContainer.position;
        newEmoteDisplay.sprt.transform.localScale = new Vector3(10,10,1);
        newEmoteDisplay.finalPos = new Vector2(newEmoteDisplay.sprt.transform.localPosition.x,newEmoteDisplay.sprt.transform.localPosition.y);
        newEmoteDisplay.StartCoroutine(newEmoteDisplay.Move(new Vector2(0,1.5f)));
        //Reference it
        displaying.Add(newEmoteDisplay);
        //Check if there are too many displayers;
        while(displaying.Count > maxDisplays) {
            StartCoroutine(displaying[0].FadeOut());
            displaying.RemoveAt(0);
            Debug.Log(displaying.Count);
        }

    }
}
