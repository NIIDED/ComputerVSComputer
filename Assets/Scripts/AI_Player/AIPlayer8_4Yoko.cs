using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer8_4Yoko : MonoBehaviour
{
    // 敵が常にジャンプしている。　→ どんな対策で敵を倒せば良いか

    FrictionManager frictionManager;       
    Rigidbody2D rb;     // playerのリギッドボディー
    Transform EnemyTrns;      //enemyの座標入れる
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

    float tmpDis=0;
    float nearDis=0;

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
        EnemyTrns = null;
        
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies){
                ammountOfEnemy += 1;
            }
        Debug.Log("残り"+ammountOfEnemy+"体");
    }

    void Update(){
        //索敵
        if(EnemyTrns == null){               // 敵見つかってないとき
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

        if(EnemyTrns!= null){           //敵が見つかったとき          
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
        if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < 1.5f){
                if(!IsGround()){                //2.0以内で自分が空中にいるなら敵の方へ空気中移動する
                    Debug.Log("今自分は空気中にいる");
                    return input = RightOrLeft();
                } else if(IsSheJumping()){      //もし自分が地面にいて、敵がジャンプしているならば自分もジャンプしろ
                    Debug.Log("相手がジャンプしていた！！");        //下の"相手がジャンプしないけど、とりあえず近寄る！"で近寄って敵がジャンプしたときにジャンプするともう手遅れ
                    return input = 5.0f;
                } // 自分は地面にいて、相手はジャンプしていない（つまりウロウロまたは、停止
                else if (Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < 0.8f){
                        Debug.Log("近すぎるからジャンプした！！");
                        return input = 5.0f;            // 近すぎたらジャンプ
                } else {
                    Debug.Log("相手がジャンプしないけど、とりあえず近寄る！");
                    return input = RightOrLeft();       //それ以外は接近
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
                CommandOutput(command);
    }

    void ChangeDirectionInTime(){   
        seconds += Time.deltaTime;
        if(AtWall()){       //左の壁に当たってるなら右いけ
            Debug.Log("壁当たったから反対を向いて進む");
            ChangeDirection();
            seconds = 0;
        }
        if(seconds>randomSecond){
            ChangeDirection();
            seconds=0;
            randomSecond = Random.Range(1.0f,6.0f);
            Debug.Log(randomSecond+"秒間"+ direction +"に向かう");
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
        if(this.transform.position.x < EnemyTrns.position.x ){
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
        Collider2D[] searchedEnemys = Physics2D.OverlapCircleAll(SearchPoint.position,searchRadius,enemyLayer);
           if(searchedEnemys.Length == 0){      //配列に何も入っていない！nullと空は違うから　searchedEnemy == null　はだめ
                    EnemyTrns = null;
                    Debug.Log("検知なし");
                }
            if(searchedEnemys.Length > 0){
                Debug.Log(searchedEnemys.Length+"体の敵を検知！！！");
                nearDis = 0;            // nearDis がリセットされずに最初の値より近くなって初めて敵の座標を得るようになってたため、毎回リセット。
                foreach(Collider2D searchedEnemy in searchedEnemys){
                    //見つかったら敵の中で一番近いやつを得ろ
                    tmpDis = Vector3.Distance(this.transform.position, searchedEnemy.transform.position);
                        if(nearDis == 0 || nearDis > tmpDis){
                            nearDis = tmpDis;
                            EnemyTrns = searchedEnemy.transform;
                        }
                }
           } 
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(SearchPoint.position,searchRadius);
    }

    bool EnemyLeft(){
        if(ammountOfEnemy <= 0f){
            return false;
        } else {
            return true;
        }
    }
    
    bool IsSheOnGround(){
        //自分が地面にいて、敵のy座標が自分とほとんど変わらないとき
        if(IsGround() && Mathf.Abs(EnemyTrns.position.y - this.transform.position.y)  < 0.1f ){
                return true;
        } else  return false;        
                
    }


    bool IsSheJumping(){
         //もし自分が床にいる時
            if(IsGround() && (EnemyTrns.position.y - this.transform.position.y) > 0.5f ){
                return true;
            } 
            else return false;
    }

    bool AtWall(){
        Vector3 upVector = new Vector3(0f,0.3f,0f);
        Vector3 startVec = this.transform.position + upVector;
        Debug.DrawLine(startVec, startVec + transform.right*0.3f* direction ,Color.white);
        return Physics2D.Linecast(startVec, startVec + transform.right*0.3f* direction,blockLayer);
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
        EnemyManager enemy = collision.GetComponent<EnemyManager>(); //enemymanager いじった時はここも変える！！
            if(this.transform.position.y +0.3f > enemy.transform.position.y )
            {
                Jump();
                enemy.DestroyEnemy();
                ammountOfEnemy -= 1f;
                Debug.Log("敵を倒したぞ！！");
                Debug.Log("残り"+ammountOfEnemy+"体");
            }
            else    // エネミーに当たったとき
            {
                Destroy(this.gameObject);
                gameManager.GameOver();
            }
        }
    }
}
