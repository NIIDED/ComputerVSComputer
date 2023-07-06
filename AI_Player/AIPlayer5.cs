using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer5 : MonoBehaviour
{
    //EnemyLeft() というboolを作成し、敵が残っているかどうかを実装させた。最初にenemyタグついた敵をstart（）で数えて、敵を倒すたびにそれを減らしていく。
    //敵を倒し終わったら右のゴールへ向かうように、EnemyLeft()がfalseならmoveDirection = MOVE_DIRECTION.RIGHT;　と直接コマンドを入力。
    
    FrictionManager frictionManager;       
    Rigidbody2D rb;     // playerのリギッドボディー
    Rigidbody2D rbEnemy;      //enemyのリギッドボディー（配列）
    Rigidbody2D rbGoal;
    GameObject targetObj = null;
    GameObject goal;
    GameObject[] enemies;


    [SerializeField] LayerMask blockLayer;
    [SerializeField] GameManager gameManager;

    float input;
    float direction;
    float command; // 数字によって与えるコマンドを変えてる
    float ammountOfEnemy = 0;
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
        goal = GameObject.Find("GoalPoint");
        rb = GetComponent<Rigidbody2D>();
        rbEnemy = GetNearestTagObj(this.gameObject,"Enemy").GetComponent<Rigidbody2D>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies){
                ammountOfEnemy += 1;
            }
        Debug.Log(ammountOfEnemy);
    }

    void Update(){
        //索敵
        if (EnemyLeft()){
                if(targetObj != null){      
                command = WhatCommandToInput(); 
                Debug.Log("敵いる");
                CommandOutput(command);
            } else if(targetObj == null) {                //狙ってる敵がいなくなった場合はまず別の敵を見つけてから次の行動を考えてもらう。
                rbEnemy = GetNearestTagObj(this.gameObject,"Enemy").GetComponent<Rigidbody2D>();
                // command = WhatCommandToInput();         // null no joutai no toki kangaero
            }
        }

        else{
            Debug.Log("NO MORE ENEMY");
            moveDirection = MOVE_DIRECTION.RIGHT;
        }

        //ジャンプ
        //横移動
                    //CommandOutput(command);
        //終端速度（摩擦）の発生（速度に応じて反対方向に力が働く）
        frictionManager.TerminalVelocityFriction();
        //後ろ後退　立て直しとか
    }
        
    void FixedUpdate(){
        switch (moveDirection){
            case MOVE_DIRECTION.STOP:
                direction =0;
                rb.velocity= new Vector2(0,0);
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
        if(Mathf.Abs(this.transform.position.x - rbEnemy.transform.position.x ) < 2.0){
            if(!IsGround()){
                return input = RightOrLeft();
            } else {
                return input = 5.0f;
            }
        } else {
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
            rb.velocity = new Vector2 (rb.velocity.x,10.0f);
            // rb.AddForce(Vector2.up*jumpPower);
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

    bool EnemyLeft(){
        if(ammountOfEnemy == 0f){
            return false;
        } else {
            return true;
        }
    }
    
    bool NoVelocityY(){
        if(rb.velocity.y < 0.01f && rb.velocity.y > -0.01f ){
            return true;
        } else {
            return false;
        }
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
        if(collision.tag == "LastGoal"){
            gameManager.GameAllClear();
        }

        if(collision.tag == "Enemy"){
            EnemyManager enemy = collision.GetComponent<EnemyManager>();
            if(this.transform.position.y +0.2f > enemy.transform.position.y )
            {
                Jump();
                enemy.DestroyEnemy();
                ammountOfEnemy -= 1f;
                Debug.Log(ammountOfEnemy);
            }
            else    // エネミーに当たったとき
            {
                Destroy(this.gameObject);
                gameManager.GameOver();
            }
        }
    }
}
