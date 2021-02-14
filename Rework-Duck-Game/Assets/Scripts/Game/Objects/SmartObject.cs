using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Create an Event that gets an integet
[System.Serializable]   public class UnityEventFloat : UnityEvent<float>   {}
[System.Serializable]   public class UnityEventString : UnityEvent<string>   {}

public class SmartObject : MonoBehaviour
{
    
    public new string name;
    public int objID;
    public bool hasOwner;
    public bool isInteractable = true;
    [Header("Events")]
    public bool hasActivateEvent;
    public UnityEventFloat onActivate;
    public bool hasOnCollisionEnterEvent;
    public UnityEventFloat onCollisionEnter;
    public bool hasOnTriggerEnterEvent;
    public UnityEventFloat onTriggerEnter;
    public bool hasOnPickedUpEvent;
    public UnityEventFloat onPickedUp;
    public bool hasOnDroppedEvent;
    public UnityEventFloat onDropped;
    public bool hasOnHitEvent;
    public UnityEventFloat onHit;
    public SerializableBehaviour[] synchronizableBehaviours;
    //public UnityEventFloat event2;
    //public UnityEventString event3;
}



