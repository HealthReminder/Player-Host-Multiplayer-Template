using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBehaviour : MonoBehaviour
{
    public int foodGain;
    public int life;
    public void OnConsume(float pID) {
        PlayerManager player = GameHost.instance.players_array[(int)pID].GetComponent<PlayerManager>();
        player.playerData.currentHunger+= foodGain;
        life--;
        if(life<=0)
            Destroy(this.gameObject);
    }
}
