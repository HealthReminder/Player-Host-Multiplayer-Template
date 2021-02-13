using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteController : MonoBehaviour
{
    public bool isChoosing = false;
    public Camera playerCamera;
    public List<EmoteDisplay> choicesDisplayed;
    public EmoteDisplay lastChoice;
    public EmoteDisplay selectedChoice;
    public int page;

    public EmoteDisplay nextPage,previousPage,cancel;
    public Transform choicesContainer;
    
    public EmoteView timeline;

    private void Update() {

        if(!isChoosing)
        return;

        lastChoice = selectedChoice;
        selectedChoice = GetClosestDisplayToMouse();
        if(selectedChoice != lastChoice && lastChoice){
            lastChoice.isSelected = false;
            StartCoroutine(lastChoice.UpdateSelectionFeedback());
            selectedChoice.isSelected = true;
            StartCoroutine(selectedChoice.UpdateSelectionFeedback());
        }
    }


    public void PostEmote(int index) {
        timeline.AddEmote(EmoteData.instance.availableEmotes[index]);
    }

    public void OnSelectedChoice(PlayerManager pM) {
        ToggleChoices(0);
        if(selectedChoice == null)
            return;
        if(selectedChoice.index >= 0 && selectedChoice.index < EmoteData.instance.availableEmotes.Length)
        {
            pM.PostEmote(selectedChoice.index);
            //send the message through the player with the index information
        } else if(selectedChoice == previousPage){
            ChangePage(-1);
        } else if(selectedChoice == nextPage){
            ChangePage(1);
        } else 
            Debug.Log("Player canceled emote.");
        
        //if emote is in range of data
        //send the selected emote index to the controller
        //if it is not check if pageAddition is != 0
        //If it is update page
        //else don't do anything because the player pressed on exit button
    }
    public void ChangePage(int skippedQtd){
        //Debug.Log("Changing page from "+page);
        page+=skippedQtd;
        //Debug.Log("Changing page to "+page);
        if(page < 0)
            page = (EmoteData.instance.availableEmotes.Length/choicesDisplayed.Count);
        if( page > EmoteData.instance.availableEmotes.Length/choicesDisplayed.Count)
            page = 0;

         Debug.Log("Changed to page "+page+" choices should be updated after.");
    }
    public void UpdateChoices() {
        for (int i = 0; i < choicesDisplayed.Count; i++)
        {
            if(i+(choicesDisplayed.Count*page) >= 0 && i+(choicesDisplayed.Count*page) < EmoteData.instance.availableEmotes.Length){
                choicesDisplayed[i].sprt.sprite = EmoteData.instance.availableEmotes[i+(choicesDisplayed.Count*page)];
                choicesDisplayed[i].index = i+(choicesDisplayed.Count*page);
                choicesDisplayed[i].sprt.gameObject.SetActive(true);
            }else {
                choicesDisplayed[i].sprt.gameObject.SetActive(false);
            }
        }
        Debug.Log("Updated displayed choices.");
    }

    public void ToggleChoices(int toggle) {

        if(toggle == 1) {
            
            foreach(EmoteDisplay e in choicesDisplayed){
                e.sprt.gameObject.SetActive(true);
                
            }
            isChoosing = true;

            UpdateChoices();
            previousPage.sprt.gameObject.SetActive(true);
            nextPage.sprt.gameObject.SetActive(true);
            cancel.sprt.gameObject.SetActive(true);
            Vector3 mP = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            choicesContainer.position = new Vector2(mP.x,mP.y);
        } else {
            foreach(EmoteDisplay e in choicesDisplayed){
                e.sprt.gameObject.SetActive(false);
                
            }
            previousPage.sprt.gameObject.SetActive(false);
            nextPage.sprt.gameObject.SetActive(false);
            cancel.sprt.gameObject.SetActive(false);
            isChoosing = false;

        }

    }
    public EmoteDisplay GetClosestDisplayToMouse () {
        float closestDist = 999;
        EmoteDisplay cView = choicesDisplayed[0];
        Vector2 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 objPos;
        for (int i = 0; i < choicesDisplayed.Count; i++)
        {
            if(choicesDisplayed[i].sprt.gameObject.activeSelf) {
                objPos = new Vector2(choicesDisplayed[i].transform.position.x,choicesDisplayed[i].transform.position.y);
                if(Vector2.Distance(objPos, mousePos) < closestDist){
                    closestDist = Vector3.Distance(objPos, mousePos);
                    cView = choicesDisplayed[i];
                }
            }   
        }

            objPos = new Vector2(previousPage.transform.position.x,previousPage.transform.position.y);
            if(Vector2.Distance(objPos, mousePos) < closestDist){
                closestDist = Vector3.Distance(objPos, mousePos);
                cView = previousPage;
            }
            objPos = new Vector2(nextPage.transform.position.x,nextPage.transform.position.y);
            if(Vector2.Distance(objPos, mousePos) < closestDist){
                closestDist = Vector3.Distance(objPos, mousePos);
                cView = nextPage;
            }
            objPos = new Vector2(cancel.transform.position.x,cancel.transform.position.y);
            if(Vector2.Distance(objPos, mousePos) < closestDist){
                closestDist = Vector3.Distance(objPos, mousePos);
                cView = cancel;
            }
        
        return (cView);
    }




}
