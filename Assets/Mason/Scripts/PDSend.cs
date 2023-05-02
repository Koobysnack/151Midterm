using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;
using UnityEngine.UI;

public class PDSend : MonoBehaviour
{
    Dictionary<string, ServerLog> servers;
    public Text countText;

    private void Awake() {
        // setup server log and OSCHandler
        servers = new Dictionary<string, ServerLog>();
        OSCHandler.Instance.Init();
    }

    private void FixedUpdate() {
        // update servers dictionary
        OSCHandler.Instance.UpdateLogs();
        servers.Clear();
		servers = OSCHandler.Instance.Servers;

        // loop through servers
		foreach (KeyValuePair<string, ServerLog> item in servers) {
            // if received packets
			if (item.Value.log.Count > 0) {
				int lastPacketIndex = item.Value.packets.Count - 1;

				//get address and data packet
				countText.text = item.Value.packets[lastPacketIndex].Address.ToString();
				countText.text += item.Value.packets[lastPacketIndex].Data[0].ToString();
			}
		}
    }

    public void SendPdMessage<T>(string message, T val) {
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/" + message, val);
    }
}
