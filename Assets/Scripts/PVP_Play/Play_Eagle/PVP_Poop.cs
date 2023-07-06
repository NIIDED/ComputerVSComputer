using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVP_Poop : MonoBehaviour
{
    //下へただただ進む攻撃
    GameObject PVP_PlayFox;
    GameObject PVP_PlayFrog;
    GameObject  PVP_PlayNinja;
    public float poopAttack;
    
    // Start is called before the first frame update
    void Start()
    {
        PVP_PlayFox = GameObject.Find("PlayerFox");           
        PVP_PlayFrog = GameObject.Find("PlayerFrog");         
        PVP_PlayNinja =GameObject.Find("PlayerNinja"); 
    }

    // Update is called once per frame
    void Update()
    {      

    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player1Body"){
                PVP_PlayFox.GetComponent<PVP_PlayFox>().AttackedByEnemy(poopAttack);　//敵にアタック
                PVP_PlayFox.GetComponent<PVP_PlayFox>().AttackedByPoop();
                Debug.Log("(1) プレイヤー１が糞を食らった！！");
                Destroy(this.gameObject);
        }
        if(collision.tag == "Player2Body"){
                PVP_PlayFrog.GetComponent<PVP_PlayFrog>().AttackedByEnemy(poopAttack);　//敵にアタック
                PVP_PlayFrog.GetComponent<PVP_PlayFrog>().AttackedByPoop();
                Debug.Log("(1) プレイヤー１が糞を食らった！！");
                Destroy(this.gameObject);
        }
        if(collision.tag == "Player3Body"){
                PVP_PlayNinja.GetComponent<PVP_PlayNinja>().AttackedByEnemy(poopAttack);　//敵にアタック
                PVP_PlayNinja.GetComponent<PVP_PlayNinja>().AttackedByPoop();
                Debug.Log("(1) プレイヤー１が糞を食らった！！");
                Destroy(this.gameObject);
        }

        if(collision.tag == "Ground"){
            Destroy(this.gameObject);
        }
    }   
    //敵が右側に最初にいたんだったらそのまま一生右に進んでもらう

}
