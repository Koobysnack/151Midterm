using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private List<Vector3> positions;
    private int positionIndex;

    private void Awake() {
        positionIndex = 0;

        if(positions.Count > 0)
            transform.position = positions[positionIndex];
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            positionIndex++;
            if(positionIndex < positions.Count)
                transform.position = positions[positionIndex];
            else
                Destroy(gameObject);
        }
    }
}
