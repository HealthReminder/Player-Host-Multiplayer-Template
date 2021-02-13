﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SynchPosition  : MonoBehaviourPun,IPunObservable
{
    new PhotonView photonView;
    //sync vals
    Vector3 trueLoc;
    Quaternion trueRot;
    PhotonView pv;
    void Awake(){
           photonView = GetComponent<PhotonView>();
    }
    void Update()
    {
        if(!photonView.IsMine){
            transform.position = Vector3.Lerp(transform.position, trueLoc, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, trueRot, Time.deltaTime * 5);
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        //we are reicieving data
        if (stream.IsReading)   {
            //receive the next data from the stream and set it to the truLoc varible
            if(!photonView.IsMine){//do we own this photonView?????
                this.trueLoc = (Vector3)stream.ReceiveNext(); //the stream send data types of "object" we must typecast the data into a Vector3 format
            }
        }
            //we need to send our data
        else
        {
            //send our posistion in the data stream
            if(photonView.IsMine){
                stream.SendNext(transform.position);
                }
            }
        }


}
