using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVP_FoxVer2 : MonoBehaviour
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
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask attackLayer;

    bool isCalledOnce = false;

    int attackStyle;

    float input;
    float direction;
    float command; // 数字によって与えるコマンドを変えてる
    float seconds= 0f;
    float randomSecond = 0f;

    float tmpDis=0;
    float nearDis=0;

    float jumpSpeed;
    float power;   //横移動の際かける力　
    float highPower;   //空中横移動の際かける力　
    float hp;
    public float attack;
    float atPower;
    float fireForce;
    float searchAngle;

    float lengthToStartThink;      //いつからただ進むだけじゃないか
    float lengthToJumpLast;        //最後近すぎてジャンプする距離

    public Transform SearchPoint;
    public Transform ReactionPoint;
    float searchRadius;
    float reactionRadius;

    public int player;

    public enum MOVE_DIRECTION{
        STOP,
        LEFT,
        RIGHT,
        POWERLEFT,
        POWERRIGHT,
    }
        
    MOVE_DIRECTION moveDirection;

    void Start(){
        frictionManager = GetComponent<FrictionManager>();
        rb = GetComponent<Rigidbody2D>();
        EnemyTrns = null;

        GameObject characterParameterManager = GameObject.Find("CharacterManagerFox");
        Debug.Log(characterParameterManager);
        ChangeParameter changeParameter = characterParameterManager.GetComponent<ChangeParameter>();

        jumpSpeed 　= changeParameter.jumpSpeed;
        power = changeParameter.power;
        highPower = changeParameter.highPower;
        hp = changeParameter.hp;
        attack =changeParameter.attack;
        atPower =changeParameter.atPower;
        searchAngle = changeParameter.searchAngle;

        lengthToStartThink=changeParameter.lengthToStartThink;   //いつからただ進むだけじゃないか
        lengthToJumpLast= changeParameter.lengthToJumpLast;    //最後近すぎてジャンプする距離

        searchRadius=changeParameter.searchRadius;
        reactionRadius = changeParameter.reactionRadius;

        attackStyle = changeParameter.attackStyle;

        switch ( player ){
            case 1:
                gameManager.ChangePlayer1HpText(hp);
                gameManager.ShowPlayer1Status(jumpSpeed,power, attack,  atPower,  searchAngle,  searchRadius,  lengthToStartThink,  lengthToJumpLast);
                break;

            case 2:
                gameManager.ChangePlayer2HpText(hp);
                gameManager.ShowPlayer2Status(jumpSpeed,power, attack,  atPower,  searchAngle,  searchRadius,  lengthToStartThink,  lengthToJumpLast);
                break;
        }
        moveDirection = MOVE_DIRECTION.LEFT;
        Debug.Log("(1) 敵の数は"+gameManager.ammountOfEnemy+"体");
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
                if(!isCalledOnce){
                    isCalledOnce = true;
                    gameManager.PlayAgain();
                }
            }
        } 

        if(EnemyTrns!= null){           //敵が見つかったとき          
            ReactToEnemyAttack();
            switch(attackStyle){
                case 1:
                    Action1();
                    break;
                case 2:
                    Action2();
                    break;
                case 3:
                    Action3();
                    break;
            }
                               //横移動したり、ジャンプしたりさせる
        }

        //終端速度（摩擦）の発生（速度に応じて反対方向に力が働く）
        frictionManager.TerminalVelocityFriction();
        //後ろ後退　立て直しとか
    }
        
    void FixedUpdate(){
        switch (moveDirection){
            case MOVE_DIRECTION.STOP:
                direction =0f;
                rb.velocity= new Vector2(0,0);
                break;
            case MOVE_DIRECTION.RIGHT:
                direction =1f;
                transform.localScale= new Vector3 (1,1,1);
                break;
            case MOVE_DIRECTION.POWERRIGHT:
                direction =1f;
                transform.localScale= new Vector3 (1,1,1);
                break;
            case MOVE_DIRECTION.LEFT:
                direction = -1f;
                transform.localScale= new Vector3 (-1,1,1);
                break;
            case MOVE_DIRECTION.POWERLEFT:
                direction =-1f;
                transform.localScale= new Vector3 (1,1,1);
                break;
        }
        // 方向を進める！
        //rb.velocity = new Vector2 (direction*power,rb.velocity.y);
        if(moveDirection == MOVE_DIRECTION.RIGHT || moveDirection == MOVE_DIRECTION.LEFT || moveDirection == MOVE_DIRECTION.STOP){
            rb.AddForce(Vector2.right*direction*power);
        }
        if(moveDirection == MOVE_DIRECTION.POWERRIGHT || moveDirection == MOVE_DIRECTION.POWERLEFT){
            rb.AddForce(Vector2.right*direction*highPower);
        }
    }
    
/* 壁あるとき
    float WhatCommandToInput(){
        if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){
                if(!IsGround()){                //1.5以内で自分が空中にいるなら敵の方へ空気中移動する
                    return input = HightRightOrHightLeft();
                } else if(IsSheJumping()){      //もし自分が地面にいて、敵がジャンプしているならば自分もジャンプしろ    //一旦退避してからジャンプ
                    Debug.Log("(1) PLAYER1 がジャンプしていた！！");        //下の"相手がジャンプしないけど、とりあえず近寄る！"で近寄って敵がジャンプしたときにジャンプするともう手遅れ
                    return input = 5.0f;
                } // 自分は地面にいて、相手はジャンプしていない（つまりウロウロまたは、停止
                else if (Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToJumpLast){
                        Debug.Log("(1) 近すぎるからジャンプした！！");
                        return input = 5.0f;            // 近すぎたらジャンプ
                } else {
                    Debug.Log("(1) 相手がジャンプしないけど、とりあえず近寄る！");
                    return input = RightOrLeft();       //それ以外は接近
                }
        } else {
            return input = RightOrLeft();
        }
    }
*/

    float WhatCommandToInput1(){
        if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){
                if(!IsGround()){                //1.5以内で自分が空中にいるなら敵の方へ空気中移動する
                    if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離
                        return input = (-1) * HightRightOrHightLeft();
                    }  
                        return input = HightRightOrHightLeft();
                } else if(IsSheJumping()){      //もし自分が地面にいて、敵がジャンプしているならば自分もジャンプしろ    //一旦退避してからジャンプ
                    Debug.Log("(2) PLAYER1 がジャンプしていた！！");        //下の"相手がジャンプしないけど、とりあえず近寄る！"で近寄って敵がジャンプしたときにジャンプするともう手遅れ
                    return input = 5.0f;
                } // 自分は地面にいて、相手はジャンプしていない（つまりウロウロまたは、停止
                else if (Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToJumpLast){
                        Debug.Log("(2) 近すぎるからジャンプした！！");
                        return input = 5.0f;            // 近すぎたらジャンプ
                } else {
                    Debug.Log("(2) 相手がジャンプしないけど、とりあえず近寄る！");
                    if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離
                        return input = (-1) * RightOrLeft();
                }  
                        return input = RightOrLeft();
                }
        } else {
            if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離
                return input = (-1) * RightOrLeft();
            }  
                return input = RightOrLeft();
        }
    }

    float WhatCommandToInput2(){
        if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){
                if(!IsGround()){                //1.5以内で自分が空中にいるなら敵の方へ空気中移動する
                    if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離
                        return input = (-1) * HightRightOrHightLeft();
                    }  
                        return input = HightRightOrHightLeft();
                } else if(IsSheJumping()){      //もし自分が地面にいて、敵がジャンプしているならば自分もジャンプしろ    //一旦退避してからジャンプ
                    if( (EnemyTrns.position.y - transform.position.y) > 5.0f){ //yの距離が３より大きい時は
                        Debug.Log("(2) 敵がジャンプしていたから一旦退避！！");        //退避
                        return input = RightOrLeft()*(-1);
                    }   else {
                        return input = 5.0f;
                    }
                } 
                else {
                    Debug.Log("一回様子見");
                    return input = 0;
                }
        } else {
            if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離
                return input = (-1) * RightOrLeft();
            }  
                return input = RightOrLeft();
        }
    }

    float WhatCommandToInput3(){
        if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){
                if(!IsGround()){                //1.5以内で自分が空中にいるなら敵の方へ空気中移動する
                    if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離
                        return input = (-1) * HightRightOrHightLeft();
                    }  
                        return input = HightRightOrHightLeft();
                }
                else {
                    return 5.0f;        //ひたすらジャンプ　
                }
        } else {
            if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離
                return input = (-1) * RightOrLeft();
            }  
                return input = RightOrLeft();
        }
    }


    void CommandOutput(float x){
        if(x==0f){
            moveDirection = MOVE_DIRECTION.STOP;
        }else if(x==1.0f){
            moveDirection = MOVE_DIRECTION.RIGHT;
        }else if(x==2.0f){
            moveDirection = MOVE_DIRECTION.POWERRIGHT;
        }else if(x==-1.0f){
            moveDirection = MOVE_DIRECTION.LEFT;
        }else if(x==-2.0f){
            moveDirection = MOVE_DIRECTION.POWERLEFT;
        }

        if(IsGround()){
            if(x==5.0f) {
                Jump();
            }
        }
    }

    void Action1(){
                command = WhatCommandToInput1(); 
                CommandOutput(command);
    }

    void Action2(){
                command = WhatCommandToInput2(); 
                CommandOutput(command);
    }

    void Action3(){
                command = WhatCommandToInput3(); 
                CommandOutput(command);
    }

    void ChangeDirectionInTime(){            //索敵中に敵が見つからないから右進んだり、左進んだりを管理　
        seconds += Time.deltaTime;
        if(AtWall()){       //左の壁に当たってるなら右いけ
            Debug.Log("(1) 壁当たったから反対を向いて進む");
            ChangeDirection();
            seconds = 0;
        }
        if(seconds>randomSecond){
            ChangeDirection();
            seconds=0;
            randomSecond = Random.Range(1.0f,6.0f);
            Debug.Log("(1) " + randomSecond+"秒間"+ direction +"に向かう");
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

    float HightRightOrHightLeft(){            //敵がどっちにいるか把握するため
        if(this.transform.position.x < EnemyTrns.position.x ){
            return input = 2.0f;
        } 
        else{   //的が左なら負の向きを返す
            return input = -2.0f;
        }
    }

    public void Jump(){
            // ジャンプさせる！
            rb.velocity = new Vector2 (rb.velocity.x,jumpSpeed);
            //rb.AddForce(Vector2.up*jumpPower);
    }

    public void GetEnemyInsideSearch(){
        Collider2D[] searchedEnemys = Physics2D.OverlapCircleAll(SearchPoint.position,searchRadius,enemyLayer);     //レイヤー変えないと自分自分もスキャン対象
           if(searchedEnemys.Length == 0){      //配列に何も入っていない！nullと空は違うから　searchedEnemy == null　はだめ
                    EnemyTrns = null;
                    // Debug.Log("(1) 検知なし");
                }
            if(searchedEnemys.Length > 0){
                nearDis = 0;            // nearDis がリセットされずに最初の値より近くなって初めて敵の座標を得るようになってたため、毎回リセット。
                foreach(Collider2D searchedEnemy in searchedEnemys){
                    Vector2 posVec = searchedEnemy.transform.position- this.transform.position;
                    float angle = Vector2.Angle(transform.right* direction, posVec);
                    if(searchAngle <= angle){   //
                        EnemyTrns = null;
                        Debug.Log("(1) 敵が見つからない。。。");
                        break;
                    }                           //
                    Debug.Log("(1) 敵を検知！！！");
                    //見つかったら敵の中で一番近いやつを得ろ
                    tmpDis = Vector3.Distance(this.transform.position, searchedEnemy.transform.position);
                        if(nearDis == 0 || nearDis > tmpDis){
                            nearDis = tmpDis;
                            EnemyTrns = searchedEnemy.transform;
                        }
                }
           } 
    }

    public void ReactToEnemyAttack(){
        Collider2D[] enemyAttack = Physics2D.OverlapCircleAll(ReactionPoint.position,reactionRadius,attackLayer);     //レイヤー変えないと自分自分もスキャン対象
            if(enemyAttack.Length == 0){      //配列に何も入っていない！nullと空は違うから　searchedEnemy == null　はだめ
                
                }
            if(enemyAttack.Length > 0){
                if(IsGround()){
                    Jump();
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
         switch ( player ){
            case 1:
                gameManager.ChangePlayer1HpText(hp);
                break;

            case 2:
                gameManager.ChangePlayer2HpText(hp);
                break;
        }
        Debug.Log("(3) HPが"+hp+"になった");
        rb.velocity = new Vector2 (direction*atPower*(-1),rb.velocity.y);
        //攻撃喰らうと後ろに相手を下げさせるコマンド欲しい
        if(hp <= 0){
            //一定ダメージ超えると破壊
            Destroy(this.gameObject);
            Debug.Log("(1) PLAYER2 を倒したぞ！！"); //相手に攻撃れて自分のHP減ったり倒されたりする
            gameManager.DecreaseAmmountOfEnemy();
            Debug.Log("PLAYER1 WIN!!!"); 
        } 
    }

    public void AttackedByFireball(){
        rb.velocity = new Vector2 (direction*atPower*(-1),rb.velocity.y);
    }

    public void  AttackedByPoop(){
        rb.velocity = new Vector2 (direction*atPower*(-1),rb.velocity.y);
    }

}
