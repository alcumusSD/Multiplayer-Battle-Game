using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Fusion;

public class ChatManager : MonoBehaviourPunCallbacks
{
	public TMP_Text messages;
	public TMP_InputField input;
	public TMP_InputField usernameInput;
	public string username = "default";

	public void SetUserName()
	{
		username = usernameInput.text;
	}
    public void CallMessageRPC()
	{
		SetUserName();
		string message = input.text;
		RPC_SendMessage(username, message);
	}

	[Rpc(RpcSources.All, RpcTargets.All)]
	public void RPC_SendMessage(string username, string message, RpcInfo rpcInfo = default)
	{
		messages.text += $"{username}: {message} \n";
	}
}
