using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager2 : MonoBehaviour
{
   //敵が左右で移動するのがenemymanager
   //敵が左右なしで、ジャンプするのがenemymanager2

    Rigidbody2D rigidbody2D;
    float direction;
    public float power;

    float jumpSeconds = 0;
    float dirSeconds= 0;
    float randomSecond= 0;

    FrictionManager frictionManager;
    [SerializeField] LayerMask blockLayer;

    public enum MOVE_DIRECTION{
        STOP,
        LEFT,
        RIGHT,
    }
    MOVE_DIRECTION moveDirection = MOVE_DIRECTION.STOP;

    void Start(){ 
        frictionManager = GetComponent<FrictionManager>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // ChangeDirectionInTime();
        JumpInTime();
        frictionManager.TerminalVelocityFriction();
    }

    private void FixedUpdate(){
        switch (moveDirection){
            case MOVE_DIRECTION.STOP:
                direction =0;
                break;
            case MOVE_DIRECTION.RIGHT:
                direction =1;
                transform.localScale = new Vector3 (1,1,1);
                break;
            case MOVE_DIRECTION.LEFT:
                direction = -1;
                transform.localScale = new Vector3 (-1,1,1);
                break;
        }
        // 方向を進める！
        this.rigidbody2D.AddForce(Vector2.right*direction*power);
    }

    public void Jump(){
            // ジャンプさせる！
            // 力ではなく、速度として与えている
            this.rigidbody2D.velocity= new Vector2(rigidbody2D.velocity.x,10f);
        }

    void JumpInTime(){
        jumpSeconds += Time.deltaTime;
        if(IsGround() && jumpSeconds > randomSecond){
            Jump();
            jumpSeconds = 0;
            randomSecond = Random.Range(1.0f, 3.0f);
        }
    }
    
    void ChangeDirectionInTime(){dirSeconds += Time.deltaTime;
        if(AtWall()){       //左の壁に当たってるなら右いけ
            Debug.Log("壁当たったから反対を向いて進む");
            ChangeDirection();
            dirSeconds = 0;
        }
        if(dirSeconds>randomSecond){
            ChangeDirection();
            dirSeconds=0;
            randomSecond = Random.Range(1.0f,6.0f);
            Debug.Log(randomSecond+"秒間　"+ direction +"　に向かう");
        }
    }

    void ChangeDirection(){
        if(moveDirection == MOVE_DIRECTION.RIGHT){
                moveDirection = MOVE_DIRECTION.LEFT;
        } else {
                moveDirection =　MOVE_DIRECTION.RIGHT;
        }
    }

    bool AtWall(){
        Vector3 upVector = new Vector3(0f,0.3f,0f);
        Vector3 startVec = this.transform.position + upVector;
        Debug.DrawLine(startVec, startVec + transform.right*0.4f* direction ,Color.white);
        return Physics2D.Linecast(startVec, transform.position + transform.right*0.4f*direction,blockLayer);
    }

    bool IsGround(){
        Vector3 startVec = transform.position + transform.right*0.5f;
        Vector3 endVec = startVec - transform.up*0.5f;
        Debug.DrawLine(startVec,endVec,Color.red);
        return Physics2D.Linecast(startVec,endVec,blockLayer);
    } 
   
   public void DestroyEnemy(){
       Destroy(this.gameObject);
   }


}

