using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteData : MonoBehaviour
{
    public static EmoteData instance;
	void Awake(){
        if(instance == null){
			instance = this;
			DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
	}
    public Sprite[] availableEmotes;
}
