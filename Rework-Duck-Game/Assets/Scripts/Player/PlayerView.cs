using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    public bool isOn = true;
    public GameObject simContainer;

    public Text hpText;
    public Text hungerText;

    public GameObject  deathContainer;


    public void ToggleDeathUI(int toggle) {
        if(toggle == 1)
            deathContainer.SetActive(true);
        else 
            deathContainer.SetActive(false);
    }

    public void ToggleSimUI(int toggle){
        
        if(toggle == 1 && !isOn) {
            Debug.Log("Toggled UI on");
            isOn = true;
            simContainer.SetActive(true);
            hpText.gameObject.SetActive(true);
            hungerText.gameObject.SetActive(true);
        } else if (toggle == 0 && isOn) {
            Debug.Log("Toggled UI off");
            isOn = false;
            simContainer.SetActive(false);
            hpText.gameObject.SetActive(false);
            hungerText.gameObject.SetActive(false);
        }
    }
    public void Setup(int maxHp, int maxHunger) {
        Debug.Log("Setup player UI");
        ToggleSimUI(1);
        hpText.text = "HP: " + maxHp.ToString();
        hungerText.text = "HUNGER: "+maxHunger.ToString();
    }

    public void UpdatesimUI(int currentHp, int currentHunger){
        hpText.text = "HP: " + currentHp.ToString();
        hungerText.text = "HUNGER: "+currentHunger.ToString();

    }
    



}
