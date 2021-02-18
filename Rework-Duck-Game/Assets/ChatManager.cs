using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using System;
using System.Text;

public struct ChatEntry
{
    //This structure will be used by the chat front end 
    //to keep track of posted messages
    int sender_id;
    string sender_name;
    string entry_text;
    float time;
}
public class ChatManager : MonoBehaviour
{
    //This class will be reusable
    //All its calls have to be assigned on the inspector
    //This class can post chat message from named players

    //This variable allows for testing without connecting to photon
    public bool is_offline = false;
    public PhotonView photon_view;

    //The following variable are to prevent players from spamming chat
    protected float timeout = 0;
    private void Update()
    {
        if (timeout > 0)
            timeout -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
            PostMessage(0,"PLAYER_NAME", "Hello world");
    }
    public void PostMessage(int player_id ,string player_name, string message_text)
    {
        //Player just wrote message and clicked send
        //Will send message to everyone by RPC_PostMessage

        if (timeout > 0)
        {
            //Player is spamming
        }
        else
        {
            //Player is posting a message
            timeout += 0.5f;
            if(!is_offline)
                photon_view.RPC("RPC_PostMessage", RpcTarget.AllBuffered,
                BitConverter.GetBytes(player_id),
                System.Text.Encoding.UTF8.GetBytes(player_name),
                System.Text.Encoding.UTF8.GetBytes(message_text));
            else
                RPC_PostMessage(
                BitConverter.GetBytes(player_id),
                System.Text.Encoding.UTF8.GetBytes(player_name),
                System.Text.Encoding.UTF8.GetBytes(message_text));
        }
    }
    [PunRPC] public void RPC_PostMessage(byte[] sender_id,byte[] player_name_bytes, byte[] message_text_bytes)
    {
        //Players received sent message through network
        //Players will now receive the decoded bytes
        int player_id_received = BitConverter.ToInt32(sender_id, 0);
        string player_name_received = System.Text.Encoding.UTF8.GetString(player_name_bytes);
        string message_text_received = System.Text.Encoding.UTF8.GetString(message_text_bytes);
        ReceiveMessage(player_name_received, message_text_received);
        Debug.Log(string.Format("<color=blue>Player of id {0} posted a message.</color>", player_id_received));
    }
    public void ReceiveMessage(string sent_by, string text)
    {
        //Players got message and it can be displayed
        Debug.Log(string.Format("{0}: \n {1}", sent_by, text));
    }
}
