using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableBehaviour : MonoBehaviour
{
    public bool selfDestroy;
    public GameObject[] destroyingObjects;

    public int currentLife = 0;

    public void Hit(int damage){
        currentLife-=damage;
        if(currentLife<=0)
            Destroy();
    }

    public void Destroy(){
        if(selfDestroy)
            Destroy(gameObject);
        for (int i = 0; i < destroyingObjects.Length; i++)
            Destroy(destroyingObjects[i]);
        destroyingObjects = new GameObject[0];
        
    }
}
