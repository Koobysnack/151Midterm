using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Jingle")]
    [Range(0f, 1f)]
    [SerializeField] private float jingleVolume;

    [Header("Distance Tone")]
    [SerializeField] private float frequency;
    [Range(0f, 1f)]
    [SerializeField] private float fmVolume;
    private float mainVolume;
    private float playerAngle;

    [Space]
    [SerializeField] private List<Vector3> positions;
    private int positionIndex;

    private PDSend send;
    private Transform player;
    private int bgIndex;

    private void Awake() {
        // setup collectible position and PDSend
        positionIndex = 0;
        if(positions.Count > 0)
            transform.position = positions[positionIndex];
        bgIndex = 0;

        // start distance tone
        send = GetComponent<PDSend>();
        send.SendPdMessage("startDist", 0);
        send.SendPdMessage("setFreq", frequency);
        send.SendPdMessage("fmVol", fmVolume);
    }

    private void Update() {
        if(player == null) {
            player = GameManager.instance.player;
            return;
        }
        
        // get player transform details
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        mainVolume = distToPlayer > 1 ? (1 / distToPlayer) : 1;
        playerAngle = (player.eulerAngles.y - transform.eulerAngles.y + 180) % 360;

        // send messages to PD
        send.SendPdMessage("mainVol", mainVolume);
        send.SendPdMessage("pAngle", playerAngle);
    }

    private void OnTriggerEnter(Collider other) {
        // on collision with player
        if(other.tag == "Player") {
            // send message to PD to play jingle and move to next BG sound
            send.SendPdMessage("jingle", jingleVolume);
            bgIndex++;
            send.SendPdMessage("bgSelect", bgIndex);

            // update position or destroy gameobject
            positionIndex++;
            if(positionIndex < positions.Count)
                transform.position = positions[positionIndex];
            else {
                send.SendPdMessage("mainVol", 0);
                Destroy(gameObject);
            }
        }
    }
}
