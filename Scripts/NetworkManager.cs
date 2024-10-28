using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    public TMP_InputField playerNameInput;
    public GameObject panel;
    public TMP_Text username;
    public TMP_Text playerListText;
    public TMP_InputField input;
    public TMP_Text chatbox;

    //public TMP_Text gameover;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        //view1 = GetComponent<PhotonView>();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            gameObject.SetActive(false);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public override void OnConnectedToMaster()
    {
        print("Connected!!!");
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom("room1");
    }
    public override void OnCreatedRoom()
    {
        print("Room Created!!");
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("room1");
    }
    public override void OnJoinedRoom()
    {
        print("Room Joined!!");
        panel.SetActive(false);
        PhotonNetwork.Instantiate("Hero", new Vector3(0, 5, 0), new Quaternion());
        PhotonNetwork.NickName = username.text;
        UpdatePlayerList();
        Debug.Log("Updated");
        Debug.Log(PhotonNetwork.NickName);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
        Debug.Log("enter");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void UpdatePlayerList()
    {
        playerListText.text = "Players:\n";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerListText.text += player.NickName + "\n";
            Debug.Log(playerListText.text);
        }
    }

    public void SendMessageToChat()
    {
        string message = input.text;
        if (!string.IsNullOrWhiteSpace(message))
        {
            // Add the message to the chat display
            AddMessageToChat(PhotonNetwork.NickName, message);

            // Clear the input field
            input.text = "";
        }
    }

    private void AddMessageToChat(string username, string message)
    {
        // Append the message to the chat display
        chatbox.text += $"{PhotonNetwork.NickName}: {message}\n";
    }


    /*public void CallMessageRPC()
    {
	    string message = input.text;
	    RPC_SendMessage(message, username);
        Debug.Log("Chat");
    }

	private void RPC_SendMessage(string message, TMP_Text username)
	{
		throw new NotImplementedException();
	}

	[PunRPC]
    public void RPC_SendMessage(TMPro username, string message, RpcInfo rpcInfo = default)
    {
	    messages.text += $"{username}: {message}\n*;
    }*/


}
