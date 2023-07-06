using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
   //敵が左右で移動するのがenemymanager
   //敵が左右なしで、ジャンプするのがenemymanager2

    Rigidbody2D rigidbody2D;
    float direction;
    public float power;
    public float jumpPower;

    float seconds=1;

    FrictionManager frictionManager;
    EnemyRespawn enemyRespawn;
    [SerializeField] LayerMask blockLayer;

    public enum MOVE_DIRECTION{
        STOP,
        LEFT,
        RIGHT,
    }
    MOVE_DIRECTION moveDirection = MOVE_DIRECTION.RIGHT;

    void Start(){ 
        frictionManager = GetComponent<FrictionManager>();
        enemyRespawn = GetComponent<EnemyRespawn>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDirectionInTime();
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
            this.rigidbody2D.AddForce(Vector2.up*jumpPower);
        }
    
    void ChangeDirectionInTime(){
        seconds += Time.deltaTime;
        if(seconds>2){
            ChangeDirection();
            seconds=0;
        }
    }
    void ChangeDirection(){
        if(moveDirection == MOVE_DIRECTION.RIGHT){
                moveDirection = MOVE_DIRECTION.LEFT;
        } else {
                moveDirection =　MOVE_DIRECTION.RIGHT;
        }
    }

    bool IsGround(){
        Vector3 startVec = transform.position + transform.right*0.5f;
        Vector3 endVec = startVec - transform.up*0.5f;
        Debug.DrawLine(startVec,endVec,Color.red);
        return Physics2D.Linecast(startVec,endVec,blockLayer);
    } 

    public float GetDirection(){
        return direction;
    }
   
   public void DestroyEnemy(){
       Destroy(this.gameObject);
       // Invoke("enemyRespawn.Respawn()",1f);          //復活させる
   }


}

