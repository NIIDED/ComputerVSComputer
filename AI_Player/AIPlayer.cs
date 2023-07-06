using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    // WhatCommandToInput() が甘い。ジャンプしても範囲内だと移動しないで、ジャンプして終了。
    
    FrictionManager frictionManager;       
    Rigidbody2D rb;
    GameObject enemy;
    Rigidbody2D rbEnemy;

    [SerializeField] LayerMask blockLayer;
    [SerializeField] GameManager gameManager;

    float input;
    float direction;
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
        enemy = GameObject.Find("Enemy");
        rbEnemy = enemy.GetComponent<Rigidbody2D>();
    }

    void Update(){
        //索敵
        float command = WhatCommandToInput(); 
        //ジャンプ
        //横移動
        Debug.Log(command);
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
        
        if(Mathf.Abs(this.transform.position.x - enemy.transform.position.x) < 1.0f){
            return input = 5.0f;
        }
        else if(this.transform.position.x < enemy.transform.position.x ){
            return input = 1.0f;
        } 
        else if(this.transform.position.x > enemy.transform.position.x )
        {
            return input = -1.0f;
        }
        else{
            return input = 0f;
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

    public void Jump(){
            // ジャンプさせる！
            rb.AddForce(Vector2.up*jumpPower);
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
