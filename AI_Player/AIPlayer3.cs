using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer3 : MonoBehaviour
{
    // 複数の敵の座標を得るように。FindGameObjectsWithTag利用（配列）、foreachも。
    // 一番近い１体の敵をまず見つけて、それを倒したら、次のを見つけて倒していく。なお狙ってる時に別の敵が近づいてきても、狙いを変えることはできない。
    
    FrictionManager frictionManager;       
    Rigidbody2D rb;     // playerのリギッドボディー
    Rigidbody2D rbEnemy;      //enemyのリギッドボディー（配列）
    GameObject targetObj = null;

    [SerializeField] LayerMask blockLayer;
    [SerializeField] GameManager gameManager;

    float input;
    float direction;
    float command; // 数字によって与えるコマンドを変えてる
    public float jumpPower;
    public float power;

    public enum MOVE_DIRECTION{
        STOP,
        LEFT,
        RIGHT,
    }
    MOVE_DIRECTION moveDirection = MOVE_DIRECTION.STOP;

    void Start(){
        frictionManager = GetComponent<FrictionManager>();
        rb = GetComponent<Rigidbody2D>();
        rbEnemy = GetNearestTagObj(this.gameObject,"Enemy").GetComponent<Rigidbody2D>();
    }

/*
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for(int i=0; i<enemies.Length; i++){
                    rbEnemy[i] =enemies[i].GetComponent<Rigidbody2D>();
                    Debug.Log(rbEnemy[i].position);
                }
*/
        
/*           enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach(GameObject enemy in enemies){
                        rbEnemy= enemy.GetComponent<Rigidbody2D>();
                        Debug.Log(rbEnemy.position);

            //成功例。全部の座標を取得することはできる。
*/

    void Update(){
        //索敵
        if(targetObj != null){      
            command = WhatCommandToInput(); 
        } else {                //狙ってる敵がいなくなった場合はまず別の敵を見つけてから次の行動を考えてもらう。
            rbEnemy = GetNearestTagObj(this.gameObject,"Enemy").GetComponent<Rigidbody2D>();
            command = WhatCommandToInput();
        }
        //ジャンプ
        //横移動
                // Debug.Log(command);
        CommandOutput(command);
        //終端速度（摩擦）の発生（速度に応じて反対方向に力が働く）
        frictionManager.TerminalVelocityFriction();
        //後ろ後退　立て直しとか
    }
        
    void FixedUpdate(){
        switch (moveDirection){
            case MOVE_DIRECTION.STOP:
                direction =0;
                break;
            case MOVE_DIRECTION.RIGHT:
                direction =1;
                transform.localScale= new Vector3 (1,1,1);
                break;
            case MOVE_DIRECTION.LEFT:
                direction = -1;
                transform.localScale= new Vector3 (-1,1,1);
                break;
        }
        // 方向を進める！
        rb.AddForce(Vector2.right*direction*power);
    }
    

    float WhatCommandToInput(){
        if(Mathf.Abs(this.transform.position.x - rbEnemy.transform.position.x) < 0.6f){
            return input = RightOrLeft();
        }
        else if(Mathf.Abs(this.transform.position.x - rbEnemy.transform.position.x) < 2.0f){
            return input = 5.0f;
        }
        else{
            return input = RightOrLeft();
        }

    }

    void CommandOutput(float x){
        if(x==0f){
            moveDirection = MOVE_DIRECTION.STOP;
        }else if(x==1.0f){
            moveDirection = MOVE_DIRECTION.RIGHT;
        }else if(x==-1.0f){
            moveDirection = MOVE_DIRECTION.LEFT;
        }

        if(IsGround()){
            if(x==5.0f) {
                Jump();
            }
        }
    }


    float RightOrLeft(){
        if(this.transform.position.x < rbEnemy.transform.position.x ){
                return input = 1.0f;
        } 
        else{   //的が左なら負の向きを返す
            return input = -1.0f;
        }
    }

    public void Jump(){
            // ジャンプさせる！
            rb.AddForce(Vector2.up*jumpPower);
    }

    public GameObject GetNearestTagObj(GameObject player, string tagName ){
        float tmpDis=0;
        float nearDis=0;

        foreach(GameObject obs in GameObject.FindGameObjectsWithTag(tagName)){
            tmpDis = Vector3.Distance(player.transform.position, obs.transform.position);
            if(nearDis ==0 || nearDis > tmpDis){
                nearDis = tmpDis;
                targetObj = obs;
            }
        }
        return targetObj;
    }
    
    

    bool IsGround(){
        Debug.DrawLine(transform.position - transform.right*0.2f,transform.position - transform.up*0.1f,Color.red);
        Debug.DrawLine(transform.position + transform.right*0.2f,transform.position - transform.up*0.1f,Color.red);
        return Physics2D.Linecast(transform.position - transform.right*0.2f,transform.position - transform.up*0.1f,blockLayer)
            || Physics2D.Linecast(transform.position + transform.right*0.2f,transform.position - transform.up*0.1f,blockLayer);
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Goal"){
            gameManager.GameClear();
        }

        if(collision.tag == "Enemy"){
            EnemyManager enemy = collision.GetComponent<EnemyManager>();
            if(this.transform.position.y +0.2f > enemy.transform.position.y )
            {
                Jump();
                enemy.DestroyEnemy();
            }
            else    // エネミーに当たったとき
            {
                Destroy(this.gameObject);
                gameManager.GameOver();
            }
        }
    }
}
