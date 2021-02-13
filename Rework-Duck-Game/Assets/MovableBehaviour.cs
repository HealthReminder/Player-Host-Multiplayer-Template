using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovableBehaviour : SerializableBehaviour
{

    public override byte[] SerializeData() {
        byte[] positionData = new byte[sizeof(float)*3];  
              
        Buffer.BlockCopy( BitConverter.GetBytes( transform.position.x ), 0, positionData, 0*sizeof(float), sizeof(float) );
        Buffer.BlockCopy( BitConverter.GetBytes( transform.position.y ), 0, positionData, 1*sizeof(float), sizeof(float) );
        Buffer.BlockCopy( BitConverter.GetBytes( transform.position.z ), 0, positionData, 2*sizeof(float), sizeof(float) );

        return(positionData);
    }
    public override void DeserializeData(byte[] data) {
        byte[] buff = data;
        Vector3 vect = Vector3.zero;
        vect.x = BitConverter.ToSingle(buff,0*sizeof(float));
        vect.y = BitConverter.ToSingle(buff,1*sizeof(float));
        vect.z = BitConverter.ToSingle(buff,2*sizeof(float));
        transform.position = vect;
        Debug.Log("Deseralized item position to "+vect);
    }
}
