using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;
    void Update()
    {
        //look at player
        transform.LookAt(player);
    }
}
