using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float bgVol;
    private PDSend send;

    private void Awake() {
        send = GetComponent<PDSend>();
    }

    private void Start() {
        send.SendPdMessage("bgVol", bgVol);
        send.SendPdMessage("bgSelect", 0f);
    }
}
