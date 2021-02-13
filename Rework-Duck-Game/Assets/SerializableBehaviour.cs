using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SerializableBehaviour : MonoBehaviour
{
    public virtual byte[] SerializeData() {
        byte[] data = new byte[0];
        return(data);
    }
    public virtual void DeserializeData(byte[] data) {
    }

}
