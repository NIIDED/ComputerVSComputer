using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player2Body : MonoBehaviour
{
    GameObject PVP_PlayerAI;
    PVP_Player2AI PVP_Player2AI;

    void Start(){
        PVP_Player2AI = GetComponentInParent<PVP_Player2AI>();  //親のスクリプト（コンポーネント）を得るにはGetComponentParent<>()やって、そのコンポーネント名.関数でいける
        PVP_PlayerAI = GameObject.Find("Player");               //親とかでなく、関係ないスクリプトを得るには、そのコンポーネントが入っているオブジェクトを探し出しGameObject.FInd()、オブジェクト名.GetComponent<>()で初めてコンポーネントを拾えて、その後に関数名を入れる
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player1Body"){
        Player1Body enemy = collision.GetComponent<Player1Body>(); //攻撃する相手のスクリプトいじった時はここも変える！！
            if(this.transform.position.y  > enemy.transform.position.y )
            {
                PVP_PlayerAI.GetComponent<PVP_PlayerAI>().AttackedByEnemy(PVP_Player2AI.attack);
                PVP_Player2AI.Jump();
                
            }
            else    // エネミーに当たったとき
            {
                Debug.Log("(2) プレイヤー２がダメージを食らった！！");
            }
        }
    }
}
