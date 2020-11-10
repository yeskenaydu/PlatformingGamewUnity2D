 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerControl : MonoBehaviour
{
    [SerializeField] GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.GetComponent<PlayerControl>().onGround = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    { 
        player.GetComponent<PlayerControl>().onGround = false;
    }
}
