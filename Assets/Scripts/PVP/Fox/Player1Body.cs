using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player1Body : MonoBehaviour
{
    PVP_PlayerAI PVP_PlayerAI;
    GameObject  PVP_Player2AI;

    void Start(){
        PVP_PlayerAI = GetComponentInParent<PVP_PlayerAI>();        //親のスクリプト（コンポーネント）を得るにはGetComponentParent<>()やって、そのコンポーネント名.関数でいける
        PVP_Player2AI = GameObject.Find("Player2(Frog)");           //親とかでなく、関係ないスクリプトを得るには、そのコンポーネントが入っているオブジェクトを探し出しGameObject.FInd()、オブジェクト名.GetComponent<>()で初めてコンポーネントを拾えて、その後に関数名を入れる
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player2Body"){
        Player2Body enemy = collision.GetComponent<Player2Body>(); //攻撃する相手のスクリプトいじった時はここも変える！！
            if(this.transform.position.y  > enemy.transform.position.y )
            {        
                PVP_Player2AI.GetComponent<PVP_Player2AI>().AttackedByEnemy(PVP_PlayerAI.attack);　//敵にアタック
                PVP_PlayerAI.Jump();   //相手を踏んで自分がジャンプ
            }//０.３離れていないとお互いダメージ喰らう else にも含まれない
            else    
            {
                Debug.Log("(1) プレイヤー１がダメージを食らった！！");
            }
        }
    }
}
