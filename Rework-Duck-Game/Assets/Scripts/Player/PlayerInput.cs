using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInput : MonoBehaviour
{
    //bool isEnabled = false;
    private PhotonView photonView;
    private void Start() {
        photonView = PhotonView.Get(this);
    }
    private void Update() {
        //Let's you control this object if you own it
        if(!photonView.IsMine)
            return;
            
        if(Input.GetKeyDown(KeyCode.Space)){
                //Do something!
        }
        if(Input.GetKeyDown(KeyCode.KeypadEnter)){
            //Call an rpc!
        }
        
    }

}
