using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    GameObject player1;
    GameObject player2;
    GameObject player3;

    void Start()
    {
        player1 = GameObject.Find("Player");
        player2= GameObject.Find("Player2(Frog)");
        player3= GameObject.Find("PlayerNinja");
        this.transform.position = (player1.transform.position + player2.transform.position)/2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if((player1 == null  && player2 == null) || (player1 != null  && player2 != null)){
            this.transform.position = (player1.transform.position + player2.transform.position)/2.0f;
        }
    }
}
