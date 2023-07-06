using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLeft : MonoBehaviour
{
    GameObject gb;

    void OnTriggerEnter2D(Collider2D collision){ 
        if(collision.tag == "Enemy"){
            gb = collision.gameObject;
            gb.transform.position = new Vector3 (21.0f,gb.transform.position.y,0f);
        }
    }
}
