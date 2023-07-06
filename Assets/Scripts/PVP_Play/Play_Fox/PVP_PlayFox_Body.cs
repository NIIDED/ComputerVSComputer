using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PVP_PlayFox_Body : MonoBehaviour
{
    PVP_PlayFox PVP_PlayFox;
    GameObject  PVP_PlayFrog;
    GameObject  PVP_PlayNinja;
    GameObject  PVP_PlayEagle;
    Rigidbody2D rb;

    public float windForce;
    public GameObject upArrow;
    public GameObject leftArrow;
    public GameObject rightArrow;
    public GameObject leftUpArrow;
    public GameObject rightUpArrow;
    public GameObject rightUpArrow2;
    public GameObject leftUpArrow2;

    void Start(){
        PVP_PlayFox = GetComponentInParent<PVP_PlayFox>();        //親のスクリプト（コンポーネント）を得るにはGetComponentParent<>()やって、そのコンポーネント名.関数でいける
        PVP_PlayFrog = GameObject.Find("PlayerFrog");           //親とかでなく、関係ないスクリプトを得るには、そのコンポーネントが入っているオブジェクトを探し出しGameObject.FInd()、オブジェクト名.GetComponent<>()で初めてコンポーネントを拾えて、その後に関数名を入れる
        PVP_PlayNinja =GameObject.Find("PlayerNinja");
        PVP_PlayEagle =GameObject.Find("PlayerEagle");
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player2Body"){
        PVP_PlayFrog_Body enemy = collision.GetComponent<PVP_PlayFrog_Body>(); //攻撃する相手のスクリプトいじった時はここも変える！！
            if(this.transform.position.y  >  enemy.transform.position.y )
            {        
                PVP_PlayFrog.GetComponent<PVP_PlayFrog>().AttackedByEnemy(PVP_PlayFox.attack);　//敵にアタック
                PVP_PlayFox.Jump();   //相手を踏んで自分がジャンプ
            }//０.３離れていないとお互いダメージ喰らう else にも含まれない
            else    
            {
                Debug.Log("(1) プレイヤー１がダメージを食らった！！");
            }
        }
        
        if(collision.tag == "Player3Body"){
        PVP_PlayNinja_Body enemy = collision.GetComponent<PVP_PlayNinja_Body>(); //攻撃する相手のスクリプトいじった時はここも変える！！
            if(this.transform.position.y  >  enemy.transform.position.y )
            {        
                PVP_PlayNinja.GetComponent<PVP_PlayNinja>().AttackedByEnemy(PVP_PlayFox.attack);　//敵にアタック
                PVP_PlayFox.Jump();   //相手を踏んで自分がジャンプ
            }//０.３離れていないとお互いダメージ喰らう else にも含まれない
            else    
            {
                Debug.Log("(1) プレイヤー１がダメージを食らった！！");
            }
        }

        if(collision.tag == "Player4Body"){ //Eagle
        PVP_PlayEagle_Body enemy = collision.GetComponent<PVP_PlayEagle_Body>(); //攻撃する相手のスクリプトいじった時はここも変える！！
            if(this.transform.position.y  >  enemy.transform.position.y )
            {        
                PVP_PlayEagle.GetComponent<PVP_PlayEagle>().AttackedByEnemy(PVP_PlayFox.attack);　//敵にアタック
                PVP_PlayFox.Jump();   //相手を踏んで自分がジャンプ
            }//０.３離れていないとお互いダメージ喰らう else にも含まれない
            else    
            {
                Debug.Log("(1) プレイヤー１がダメージを食らった！！");
            }
        }

        
        if(collision.tag == "WindUp"){
            rb =PVP_PlayFox.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector3 (rb.velocity.x, windForce,0f);
            //PVP_PlayFox.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * windForce);
            //ArrowSetFalse();
            //Invoke("ArrowSetActive",3f);
            Destroy(collision.gameObject);
            Invoke("RemakeUpArrow",3f);
        }
        if(collision.tag == "WindRight"){
            rb =PVP_PlayFox.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector3 (windForce,rb.velocity.y,0f);
            //PVP_PlayFox.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * windForce);
            //ArrowSetFalse();
            //Invoke("ArrowSetActive",3f); 
            Destroy(collision.gameObject);
            Invoke("RemakeRightArrow",3f);
        }
        if(collision.tag == "WindLeft"){
            rb =PVP_PlayFox.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector3 (windForce * (-1),rb.velocity.y,0f);
            //PVP_PlayFox.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right*(-1) * windForce);
            //ArrowSetFalse();
            //Invoke("ArrowSetActive",3f); 
            Destroy(collision.gameObject);
            Invoke("RemakeLeftArrow",3f);
        }
        if(collision.tag == "WindRightUp"){
            Vector3 windVector = new Vector3 (windForce,windForce,0);
            PVP_PlayFox.gameObject.GetComponent<Rigidbody2D>().velocity = windVector;
            //PVP_PlayFox.gameObject.GetComponent<Rigidbody2D>().AddForce(windVector);
            //ArrowSetFalse();
            //Invoke("ArrowSetActive",3f);
            Destroy(collision.gameObject);
            Invoke("RemakeRightUpArrow",3f);
        }
        if(collision.tag == "WindRightUp2"){
            Vector3 windVector = new Vector3 (windForce,windForce,0);
            PVP_PlayFox.gameObject.GetComponent<Rigidbody2D>().velocity = windVector;
            //PVP_PlayFox.gameObject.GetComponent<Rigidbody2D>().AddForce(windVector);
            //ArrowSetFalse();
            //Invoke("ArrowSetActive",3f);
            Destroy(collision.gameObject);
            Invoke("RemakeRightUpArrow2",3f);
        }
        if(collision.tag == "WindLeftUp"){
            Vector3 windVector = new Vector3 (windForce*(-1),windForce,0);
            PVP_PlayFox.gameObject.GetComponent<Rigidbody2D>().velocity = windVector;
            //PVP_PlayFox.gameObject.GetComponent<Rigidbody2D>().AddForce(windVector);
            //ArrowSetFalse();
            //Invoke("ArrowSetActive",3f);
            Destroy(collision.gameObject);
            Invoke("RemakeLeftUpArrow",3f);
        }
        if(collision.tag == "WindLeftUp2"){
            Vector3 windVector = new Vector3 (windForce*(-1),windForce,0);
            PVP_PlayFox.gameObject.GetComponent<Rigidbody2D>().velocity = windVector;
            //PVP_PlayFox.gameObject.GetComponent<Rigidbody2D>().AddForce(windVector);
            //ArrowSetFalse();
            //Invoke("ArrowSetActive",3f);
            Destroy(collision.gameObject);
            Invoke("RemakeLeftUpArrow2",3f);
        }
    }

    void RemakeUpArrow(){
        Instantiate(upArrow,upArrow.transform.position,Quaternion.Euler(0, 0, 90f));   
    }
    void RemakeLeftArrow(){
        Instantiate(leftArrow,leftArrow.transform.position,Quaternion.Euler(0, 0, 180f));   
    }
    void RemakeRightArrow(){
        Instantiate(rightArrow,rightArrow.transform.position,Quaternion.Euler(0, 0, 0));   
    }
    void RemakeLeftUpArrow(){
        Instantiate(leftUpArrow,leftUpArrow.transform.position,Quaternion.Euler(0, 0, 135f));   
    }
    void RemakeRightUpArrow2(){
        Instantiate(rightUpArrow2,rightUpArrow2.transform.position,Quaternion.Euler(0, 0, 45f));   
    }
    void RemakeRightUpArrow(){
        Instantiate(rightUpArrow,rightUpArrow.transform.position,Quaternion.Euler(0, 0, 45f));   
    }
    void RemakeLeftUpArrow2(){
        Instantiate(leftUpArrow2,leftUpArrow2.transform.position,Quaternion.Euler(0, 0, 135f));   
    }

}
