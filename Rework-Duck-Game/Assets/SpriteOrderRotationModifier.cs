using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrderRotationModifier : MonoBehaviour
{
    public SpriteOrder spriteOrderBehaviour;
    private void Update() {
        if(!spriteOrderBehaviour)
            return;
            
        if(transform.eulerAngles.z >= 0 && transform.eulerAngles.z <= 180) {
            spriteOrderBehaviour.offset= -1;
        } else {
            spriteOrderBehaviour.offset= 1;
        }
    }
}
