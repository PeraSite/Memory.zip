using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    public GameObject player;

    private void Update()
    {
        transform.position = new Vector3(0, player.transform.position.y, -10);
    }
}
