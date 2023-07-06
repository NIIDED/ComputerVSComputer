using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVP_FireBallLeft2 : MonoBehaviour
{
    //右へただただ進む攻撃
    
    Rigidbody2D rb;
    GameObject PVP_PlayFox;
    GameObject PVP_PlayFrog;
    GameObject  PVP_PlayEagle;
    public float speed;
    public float fireBallAttack;
    public float disappearTime;
    
    // Start is called before the first frame update
    void Start()
    {
        rb=this.gameObject.GetComponent<Rigidbody2D>();
        PVP_PlayFox =GameObject.Find("PlayerFox");
        PVP_PlayFrog = GameObject.Find("PlayerFrog");         
        PVP_PlayEagle =GameObject.Find("PlayerEagle"); 
        Invoke("DestroyThis",disappearTime);
    }

    // Update is called once per frame
    void Update()
    {   
            rb.velocity = new Vector2 (speed*(-1),rb.velocity.y);        
    }

    void DestroyThis(){
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player1Body"){
                PVP_PlayFox.GetComponent<PVP_FoxVer2>().AttackedByEnemy(fireBallAttack);　//敵にアタック
                PVP_PlayFox.GetComponent<PVP_FoxVer2>().AttackedByFireball();
                Debug.Log("(1) プレイヤー１が炎を食らった！！");
                Destroy(this.gameObject);
        }
        if(collision.tag == "Player2Body"){
                PVP_PlayFrog.GetComponent<PVP_FrogVer2>().AttackedByEnemy(fireBallAttack);　//敵にアタック
                PVP_PlayFrog.GetComponent<PVP_FrogVer2>().AttackedByFireball();
                Debug.Log("(1) プレイヤー１が炎を食らった！！");
                Destroy(this.gameObject);
        }
        if(collision.tag == "Player4Body"){
                PVP_PlayEagle.GetComponent<PVP_EagleVer2>().AttackedByEnemy(fireBallAttack);　//敵にアタック
                PVP_PlayEagle.GetComponent<PVP_EagleVer2>().AttackedByFireball();
                Debug.Log("(1) プレイヤー１が炎を食らった！！");
                Destroy(this.gameObject);
        }
    }   
    //敵が右側に最初にいたんだったらそのまま一生右に進んでもらう

}
