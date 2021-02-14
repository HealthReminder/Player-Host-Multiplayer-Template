using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartObjectManager : MonoBehaviour
{
    public List<SmartObject> objectsInScene;
    public static SmartObjectManager instance;
    private void Update() {
    }

    private void Awake() {
        instance = this;
        objectsInScene = new List<SmartObject>();

        List<SmartObject> objList = new List<SmartObject>();
        int index = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            for (int o = 0; o < transform.GetChild(i).childCount; o++)
            {
                objList.Add(transform.GetChild(i).GetChild(o).GetComponent<SmartObject>());
                objList[objList.Count-1].objID = index;
                index++;
            }
        }
        objectsInScene = objList;
    }
}
