using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVP_PlayEagle : MonoBehaviour
{
    //PVP プレイヤー3（忍者）のコントローラ
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

    public GameObject poopPrefab;
    public GameObject attackPoint;

    public GameObject gravityField;

    public float chargeTime;
    float time =0;

    float input;
    public float direction;
    float command; // 数字によって与えるコマンドを変えてる
    float seconds= 0f;
    float randomSecond = 0f;

    float tmpDis=0;
    float nearDis=0;

    public float jumpSpeed;
    // public float jumpPower;
    public float power;   //横移動の際かける力　
    public float highPower;   //横移動の際かける力　
    public float hp;
    public float attack;
    public float atPower;
    public float searchAngle;

    public float lengthToStartThink;      //いつからただ進むだけじゃないか
    public float lengthToJumpLast;        //最後近すぎてジャンプする距離

    public Transform SearchPoint;
    public Transform ReactionPoint;
    public float searchRadius;
    public float reactionRadius;

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
        moveDirection = MOVE_DIRECTION.RIGHT;
        Debug.Log("(3) 敵の数は"+gameManager.ammountOfEnemy+"体");
    }

    void Update(){
        //索敵            
        time += Time.deltaTime;
        if(AtWall()){       //左の壁に当たってるなら右いけ
            ChangeDirection();
        }

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
            case MOVE_DIRECTION.POWERRIGHT:
                direction =1;
                transform.localScale= new Vector3 (1,1,1);
                break;
            case MOVE_DIRECTION.LEFT:
                direction = -1;
                transform.localScale= new Vector3 (-1,1,1);
                break;
            case MOVE_DIRECTION.POWERLEFT:
                direction =-1;
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
    
/*    float WhatCommandToInput(){
         if(AtWall()){       //壁に当たった時は
            Debug.Log("壁当たったんごね");
            if(command == 1){
                return -2.0f;
            } else if(command == -1){
                return 2.0f;
            }
        } else  
        if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){
                    return RightOrLeft()*(-1f); //逃げる
            } else {
                    return RightOrLeft(); //近寄る（攻撃届かないからね）          
            } 
        return RightOrLeft();
    }
*/
    float WhatCommandToInput(){
        if(!IsGround()){                //空中にいたら攻撃するために敵に近づけ
            if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離
                return input = (-1) * RightOrLeft();
            }  
                return input = RightOrLeft();

        }else{
            if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink) { //地面にいて２mいないなら逃げろ,逃げてジャンプして空中へ
                    return input = RightOrLeft()*(-1);
            } else {
                    return input = 5.0f;
            }
        }
        
    }

    void CommandOutput(float x){
        if(x==0f){
            moveDirection = MOVE_DIRECTION.STOP;
        }else if(x==1.0f){
            moveDirection = MOVE_DIRECTION.RIGHT;
        }else if(x==2.0f){
            moveDirection = MOVE_DIRECTION.POWERRIGHT;
        }
        else if(x==-1.0f){
            moveDirection = MOVE_DIRECTION.LEFT;
        }else if(x==-2.0f){
            moveDirection = MOVE_DIRECTION.POWERLEFT;
        }

        if(x==4.0f){
            Poop();
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
            Poop();     //索敵範囲に入ってないと攻撃できない（そもそもactionが呼び出されない）
    }

    void Poop(){
        if(chargeTime < time ){ 
                Instantiate(poopPrefab,attackPoint.transform.position,Quaternion.identity);   
                time = 0f;
        }
    }

    void ChangeDirectionInTime(){            //索敵中に敵が見つからないから右進んだり、左進んだりを管理　
        seconds += Time.deltaTime;
        if(AtWall()){       //左の壁に当たってるなら右いけ
            Debug.Log("(3) 壁当たったから反対を向いて進む");
            seconds = 0;
        }
        if(seconds>randomSecond){
            ChangeDirection();
            seconds=0;
            randomSecond = Random.Range(1.0f,6.0f);
            Debug.Log("(3) " + randomSecond+"秒間"+ direction +"に向かう");
        }
    }

    void ChangeDirection(){         //右向かってたら、左へ方向変換するように（力を違う方向にかける）
            if(moveDirection == MOVE_DIRECTION.RIGHT){
                    moveDirection = MOVE_DIRECTION.LEFT;
            } else {
                    moveDirection =　MOVE_DIRECTION.RIGHT;
            }
    }

    void ChangeDirectionByCommand(){
        if(command == 1.0){
                command = -1.0f;
        } 
        if(command == -1.0){
                command = 1.0f;
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
            WithGravity();
            // rb.AddForce(Vector2.up*jumpPower);
    }

    bool JumpRandom(){
        float seed = Random.Range(0.0f,3.0f);
        if(seed < 1){
            return true;
        } else {
            return false;
        }
    }

    public void GetEnemyInsideSearch(){
        Collider2D[] searchedEnemys = Physics2D.OverlapCircleAll(SearchPoint.position,searchRadius,enemyLayer);     //レイヤー変えないと自分自分もスキャン対象
           if(searchedEnemys.Length == 0){      //配列に何も入っていない！nullと空は違うから　searchedEnemy == null　はだめ
                    EnemyTrns = null;
                    // Debug.Log("(3) 検知なし");
                }
            if(searchedEnemys.Length > 0){
                nearDis = 0;            // nearDis がリセットされずに最初の値より近くなって初めて敵の座標を得るようになってたため、毎回リセット。
                foreach(Collider2D searchedEnemy in searchedEnemys){
                    Vector2 posVec = searchedEnemy.transform.position- this.transform.position;
                    float angle = Vector2.Angle(transform.right* direction, posVec);
                    if(searchAngle <= angle){   //
                        EnemyTrns = null;
                        Debug.Log("(3) 敵が見つからない。。。");
                        break;
                    }                           //
                    Debug.Log("(3) 敵を検知！！！");
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
        //rb.AddForce(Vector2.left*direction*atPower);
        //rb.velocity = new Vector2 (rb.velocity.x,atPower*(-1));
        gravityField.SetActive(false);
        WithGravity();
        //rb.velocity = new Vector2 (atPower*(-1)*direction,rb.velocity.y); //x方向へのぶっ飛び
        rb.velocity = new Vector2 (atPower*direction*(-1),atPower*(-1)); //y方向へのぶっ飛び
        Invoke("NoGravity",3f);
        Debug.Log("攻撃食らった。落ちる。");
        //攻撃喰らうと後ろに相手を下げさせるコマンド欲しい
        if(hp <= 0){
            //一定ダメージ超えると破壊
            Destroy(this.gameObject);
            Debug.Log("(3) PLAYER1 を倒したぞ！！"); //相手に攻撃れて自分のHP減ったり倒されたりする
            gameManager.DecreaseAmmountOfEnemy();
            Debug.Log("PLAYER2 WIN!!!"); 
        } 
    }



    public void WithGravity(){
        rb.gravityScale = 3f;
    }

    public void NoGravity(){
        Debug.Log("重力もどした");
        rb.gravityScale = -3f;      //重力−３にした。
        gravityField.SetActive(true);
    }

    public void NoGravityText(){
        Debug.Log("重力もどした");
        gravityField.SetActive(true);
    }

    public void AttackedByFireball(){
        rb.velocity = new Vector2 (direction*atPower*(-1),rb.velocity.y);
    }
}
