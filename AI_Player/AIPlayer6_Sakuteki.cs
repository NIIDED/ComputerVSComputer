using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer6_Sakuteki : MonoBehaviour
{
    //Sakuteki を実装した。→ GetEnemyInsideSearch()
    //索敵に引っかからない場合はウロウロさせる　→　ChangeDirectionInTime
    //GetNearestTagObj(GameObject player, string tagName ) は索敵範囲関係なく取得してしまうため、外した。
    //なぜか倒した敵判定が二重で起きる。
    
    FrictionManager frictionManager;       
    Rigidbody2D rb;     // playerのリギッドボディー
    Transform EnemyPos;      //enemyの座標入れる
    GameObject targetObj = null;
    GameObject eventObj;
    GameObject goal;
    GameObject[] enemies;


    [SerializeField] LayerMask blockLayer;
    [SerializeField] GameManager gameManager;
    [SerializeField] LayerMask enemyLayer;

    float input;
    float direction;
    float command; // 数字によって与えるコマンドを変えてる
    float ammountOfEnemy = 0;
    float seconds= 0f;
    float randomSecond = 0f;

    public float jumpPower;
    public float power;

    public Transform SearchPoint;
    public float searchRadius;

    public enum MOVE_DIRECTION{
        STOP,
        LEFT,
        RIGHT,
    }
    MOVE_DIRECTION moveDirection = MOVE_DIRECTION.STOP;

    void Start(){
        frictionManager = GetComponent<FrictionManager>();
        eventObj = GameObject.Find("Event");
        goal = eventObj.transform.Find("GoalPoint").gameObject;
        rb = GetComponent<Rigidbody2D>();
        EnemyPos = null;
        
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies){
                ammountOfEnemy += 1;
            }
        Debug.Log(ammountOfEnemy);
    }

    void Update(){
        //索敵
        if(EnemyPos == null){               // 敵見つかってないとき
            if(EnemyLeft()){                //もし敵が残っているなら
                ChangeDirectionInTime();    //スキャン範囲に入れるためにウロウロする
                GetEnemyInsideSearch(); 　  //索敵範囲内に敵が入ったら相手の座標を得る
            } 
            else                            //もし敵残っていないなら
            {
                goal.SetActive(true);           //ゴールのオブジェクトをアクティブにする
                Debug.Log("NO MORE ENEMY"); 
                moveDirection = MOVE_DIRECTION.RIGHT;   //とにかく右へ進め
            }
        } 
        
        if(EnemyPos != null){           //敵が見つかったとき          
            Action();                   //横移動したり、ジャンプしたりさせる
        }

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
        if(Mathf.Abs(this.transform.position.x - EnemyPos.position.x ) < 2.0){
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

    void Action(){
                command = WhatCommandToInput(); 
                Debug.Log("敵いる");
                CommandOutput(command);
    }

    void ChangeDirectionInTime(){   
        seconds += Time.deltaTime;
        if(seconds>randomSecond){
            ChangeDirection();
            seconds=0;
            randomSecond = Random.Range(1.0f,6.0f);
            Debug.Log(randomSecond);
        }
    }
    void ChangeDirection(){
        if(moveDirection == MOVE_DIRECTION.RIGHT){
                moveDirection = MOVE_DIRECTION.LEFT;
        } else {
                moveDirection =　MOVE_DIRECTION.RIGHT;
        }
    }

    float RightOrLeft(){
        if(this.transform.position.x < EnemyPos.position.x ){
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

    public void GetEnemyInsideSearch(){
        Collider2D searchedEnemy = Physics2D.OverlapCircle(SearchPoint.position,searchRadius,enemyLayer);
           // Debug.Log(searchedEnemy.gameObject.name);
            if(searchedEnemy == null){
                EnemyPos = null;
                Debug.Log("見つからない");
            } else {
                Debug.Log("見つけた");
                EnemyPos = searchedEnemy.transform;
            } 
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(SearchPoint.position,searchRadius);
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
                Debug.Log("敵を倒したぞ！！");
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
