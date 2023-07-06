using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVP_Player2AI : MonoBehaviour
{
    //PVP プレイヤー２（カエル）のコントローラ
    //HP 実装
    //攻撃実装

    FrictionManager frictionManager;       
    Rigidbody2D rb;     // playerのリギッドボディー
    Transform EnemyTrns;      //enemyの座標入れる
    GameObject targetObj = null;

    [SerializeField] LayerMask blockLayer;
    [SerializeField] GameManager gameManager;
    [SerializeField] LayerMask enemy1Layer;

    float input;
    float direction;
    float command; // 数字によって与えるコマンドを変えてる
    float seconds= 0f;
    float randomSecond = 0f;

    float tmpDis=0;
    float nearDis=0;

    float jumpSpeed;
    float power;   //横移動の際かける力　
    float hp;
    public float attack;
    float atPower;
    float searchAngle;

    float lengthToStartThink;      //いつからただ進むだけじゃないか
    float lengthToJumpLast;        //最後近すぎてジャンプする距離

    public Transform SearchPoint;
    float searchRadius;

    float enemyAttack;

    public enum MOVE_DIRECTION{
        STOP,
        LEFT,
        RIGHT,
    }
        
    MOVE_DIRECTION moveDirection;

    void Start(){
        frictionManager = GetComponent<FrictionManager>();
        rb = GetComponent<Rigidbody2D>();
        EnemyTrns = null;
        GameObject characterParameterManager = GameObject.Find("CharacterManagerFrog");
        Debug.Log(characterParameterManager);
        ChangeParameter changeParameter = characterParameterManager.GetComponent<ChangeParameter>();

        jumpSpeed 　= changeParameter.jumpSpeed;
        power = changeParameter.power;
        hp = changeParameter.hp;
        attack =changeParameter.attack;
        atPower =changeParameter.atPower;
        searchAngle = changeParameter.searchAngle;

        lengthToStartThink=changeParameter.lengthToStartThink;   //いつからただ進むだけじゃないか
        lengthToJumpLast= changeParameter.lengthToJumpLast;    //最後近すぎてジャンプする距離

        searchRadius=changeParameter.searchRadius;
        gameManager.ChangePlayer2HpText(hp);
        gameManager.ShowPlayer2Status(jumpSpeed,power, attack,  atPower,  searchAngle,  searchRadius,  lengthToStartThink,  lengthToJumpLast);

        Debug.Log("(2) 敵の数は"+gameManager.ammountOfEnemy+"体");
    }

    void Update(){
        //索敵
        if(EnemyTrns == null){               // 敵見つかってないとき
            if(gameManager.EnemyLeft()){                //もし敵が残っているなら
                ChangeDirectionInTime();    //スキャン範囲に入れるためにウロウロする
                GetEnemyInsideSearch(); 　  //索敵範囲内に敵が入ったら相手の座標を得る
            } 
            else                            //もし敵残っていないなら
            {
                moveDirection = MOVE_DIRECTION.STOP;
                gameManager.PlayAgain();
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
        if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){
                if(!IsGround()){                //1.5以内で自分が空中にいるなら敵の方へ空気中移動する
                    return input = RightOrLeft();
                } else if(IsSheJumping()){      //もし自分が地面にいて、敵がジャンプしているならば自分もジャンプしろ    //一旦退避してからジャンプ
                    Debug.Log("(2) PLAYER1 がジャンプしていた！！");        //下の"相手がジャンプしないけど、とりあえず近寄る！"で近寄って敵がジャンプしたときにジャンプするともう手遅れ
                    return input = 5.0f;
                } // 自分は地面にいて、相手はジャンプしていない（つまりウロウロまたは、停止
                else if (Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToJumpLast){
                        Debug.Log("(2) 近すぎるからジャンプした！！");
                        return input = 5.0f;            // 近すぎたらジャンプ
                } else {
                    Debug.Log("(2) 相手がジャンプしないけど、とりあえず近寄る！");
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

    void ChangeDirectionInTime(){            //索敵中に敵が見つからないから右進んだり、左進んだりを管理　
        seconds += Time.deltaTime;
        if(AtWall()){       //左の壁に当たってるなら右いけ
            Debug.Log("(2) 壁当たったから反対を向いて進む");
            ChangeDirection();
            seconds = 0;
        }
        if(seconds>randomSecond){
            ChangeDirection();
            seconds=0;
            randomSecond = Random.Range(1.0f,6.0f);
            Debug.Log("(2) " + randomSecond+"秒間"+ direction +"に向かう");
        }
    }

    void ChangeDirection(){         //右向かってたら、左へ方向変換するように（力を違う方向にかける）
            if(moveDirection == MOVE_DIRECTION.RIGHT){
                    moveDirection = MOVE_DIRECTION.LEFT;
            } else {
                    moveDirection =　MOVE_DIRECTION.RIGHT;
            }
    }

    float RightOrLeft(){            //敵がどっちにいるか把握するため
        if(this.transform.position.x < EnemyTrns.position.x ){
            return input = 1.0f;
        } 
        else{   //的が左なら負の向きを返す
            return input = -1.0f;
        }
    }

    public void Jump(){
            // ジャンプさせる！
            rb.velocity = new Vector2 (rb.velocity.x,jumpSpeed);
            // rb.AddForce(Vector2.up*jumpPower);
    }

    public void GetEnemyInsideSearch(){
        Collider2D[] searchedEnemys = Physics2D.OverlapCircleAll(SearchPoint.position,searchRadius,enemy1Layer);     //レイヤー変えないと自分自分もスキャン対象
           if(searchedEnemys.Length == 0){      //配列に何も入っていない！nullと空は違うから　searchedEnemy == null　はだめ
                    EnemyTrns = null;
                    // Debug.Log("(2) 検知なし");
                }
            if(searchedEnemys.Length > 0){
                nearDis = 0;            // nearDis がリセットされずに最初の値より近くなって初めて敵の座標を得るようになってたため、毎回リセット。
                foreach(Collider2D searchedEnemy in searchedEnemys){
                    Vector2 posVec = searchedEnemy.transform.position- this.transform.position;
                    float angle = Vector2.Angle(transform.right* direction, posVec);
                    if(searchAngle <= angle){   //
                        EnemyTrns = null;
                        Debug.Log("(2) 敵が見つからない。。。");
                        break;
                    }                           //
                    Debug.Log("(2) 敵を検知！！！");
                    //見つかったら敵の中で一番近いやつを得ろ
                    tmpDis = Vector3.Distance(this.transform.position, searchedEnemy.transform.position);
                        if(nearDis == 0 || nearDis > tmpDis){
                            nearDis = tmpDis;
                            EnemyTrns = searchedEnemy.transform;
                        }
                }
           } 
    }

    private void OnDrawGizmosSelected(){        //索敵範囲を紫色で描いてくれる
        Gizmos.color=Color.white;
        Gizmos.DrawWireSphere(SearchPoint.position,searchRadius);//球の索敵範囲

        Vector3 endPosUp = transform.position + direction * Vector3.right*5f +  Vector3.up*5f;
        Vector3 endPosDown = transform.position + direction * Vector3.right*5f -  Vector3.up*5f;
        Gizmos.DrawLine(transform.position, endPosUp);
        Gizmos.DrawLine(transform.position, endPosDown);    //視野としての索敵範囲
    }

    bool IsSheOnGround(){
        //自分が地面にいて、敵のy座標が自分とほとんど変わらないとき
        if(IsGround() && Mathf.Abs(EnemyTrns.position.y - this.transform.position.y)  < 0.1f ){
                return true;
        } 
        else    return false;                
    }


    bool IsSheJumping(){
         //もし自分が床にいて相手が自分より上にいる　→　ジャンプしてる！
        if(IsGround() && (EnemyTrns.position.y - this.transform.position.y) > 0.5f ){
                return true;
        } 
        else    return false;
    }

    bool AtWall(){          //壁にぶつかっているかどうかのチェック
        Vector3 upVector = new Vector3(0f,0.3f,0f);
        Vector3 startVec = this.transform.position + upVector;
        Debug.DrawLine(startVec, startVec + transform.right*0.3f* direction ,Color.white);
        return Physics2D.Linecast(startVec, startVec + transform.right*0.3f* direction,blockLayer);
    }

    bool IsGround(){        //床に着いてるかどうかの判定
        Debug.DrawLine(transform.position - transform.right*0.2f,transform.position - transform.up*0.1f,Color.red);
        Debug.DrawLine(transform.position + transform.right*0.2f,transform.position - transform.up*0.1f,Color.red);
        return Physics2D.Linecast(transform.position - transform.right*0.2f,transform.position - transform.up*0.1f,blockLayer)
            || Physics2D.Linecast(transform.position + transform.right*0.2f,transform.position - transform.up*0.1f,blockLayer);
    }

    public void AttackedByEnemy(float at){
        hp -= at;
        gameManager.ChangePlayer2HpText(hp);
        Debug.Log("(2) HPが"+hp+"になった");
        rb.AddForce(Vector2.left*direction*atPower);
        //攻撃喰らうと後ろに相手を下げさせるコマンド欲しい
        if(hp <= 0){
            //一定ダメージ超えると破壊
            Destroy(this.gameObject);
            Debug.Log("(1) PLAYER2 を倒したぞ！！"); //相手に攻撃れて自分のHP減ったり倒されたりする
            gameManager.DecreaseAmmountOfEnemy();
            Debug.Log("PLAYER1 WIN!!!"); 
        } 
    }
}
