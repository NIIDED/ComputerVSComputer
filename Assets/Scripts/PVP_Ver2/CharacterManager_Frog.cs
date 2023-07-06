using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager_Frog : MonoBehaviour 
{       //パラメータの管理をここでやる。値を保有するのもここ。変更も可能

    public GameObject frogPrefab;
    public GameObject gameManager;

        public float jumpSpeed;
        public float power;   //横移動の際かける力　
        public float hp;
        public float attack;
        public float atPower;
        public float searchAngle;

        public float lengthToStartThink;      //いつからただ進むだけじゃないか
        public float lengthToJumpLast;        //最後近すぎてジャンプする距離

        public float searchRadius;

        GameObject[] tagObjects;

    void Awake(){
        DontDestroyOnLoad(this.gameObject);
    }

    void Start(){
        gameManager = GameObject.Find("GameManager");
    }

    void Update(){   
        if(Input.GetKeyDown("a")){
            Instantiate(frogPrefab,transform.position,Quaternion.identity);
        }
    }


    public void ChangeJumpSpeed(float jumpSpeed){
        this.jumpSpeed = jumpSpeed;
    }

    public void ChangePower(float power){
        this.power = power;
    }
    
    public void ChangeHp(float hp){
        this.hp = hp;
    }
    
    public void ChangeAttack(float attack){
        this.attack = attack;
    }
    
    public void ChangeAtPower(float atPower){
        this.atPower = atPower;
    }
    
    public void ChangeSearchAngle(float searchAngle){
        this.searchAngle = searchAngle;
    }
    
    public void ChangeLengthToStartThink(float lengthToStartThink){
        this.lengthToStartThink = lengthToStartThink;
    }
    
    public void ChangeLengthToJumpLast(float lengthToJumpLast){
        this.lengthToJumpLast = lengthToJumpLast;
    }
    
    public void ChangeSearchRadius(float searchRadius){
        this.searchRadius = searchRadius;
    }

    //上げるボタン

   public void IncreaseJumpSpeed(){
       jumpSpeed += 1;
   }

   public void IncreasePower(){
       power += 1;
   }

   public void IncreaseHp(){
       hp += 1;
   }

    public void IncreaseAttack(){
       attack += 1;
   }

   public void IncreaseAtPower(){
       atPower += 10;
   }

   public void IncreaseSearchAngle(){
       searchAngle += 1;
   }

    public void IncreaseLengthToStartThink(){
       lengthToStartThink += 0.1f;
   }

   public void IncreaseLengthToJumpLast(){
       lengthToJumpLast += 0.1f;
   }

   public void IncreaseSearchRadius(){
       searchRadius += 1;
   }


//下げるボタン


    public void DecreaseJumpSpeed(){
       jumpSpeed -= 1;
   }

   public void DecreasePower(){
       power -= 1;
   }

   public void DecreaseHp(){
       hp -= 1;
   }

   public  void DecreaseAttack(){
       attack -= 1;
   }

   public void DecreaseAtPower(){
       atPower -= 10;
   }

   public void DecreaseSearchAngle(){
       searchAngle -= 1;
   }

   public  void DecreaseLengthToStartThink(){
       lengthToStartThink -= 0.1f;
   }

   public void DecreaseLengthToJumpLast(){
       lengthToJumpLast -= 0.1f;
   }

   public void DecreaseSearchRadius(){
       searchRadius -= 1;
   }

//ステージ移行

    public void GoToBattle(){
         SceneManager.LoadScene("4_PVP_Battle");
    }

    public void BackToCharacterSelection(){
        SceneManager.LoadScene("2_PVP_CharacterSelect");
        Destroy(this.gameObject);
    }

    public void ConfirmStats(){
        if(GetNumberOfTags("CharacterSelected") < 2){
            SceneManager.LoadScene("2_PVP_CharacterSelect");
        }
        if(GetNumberOfTags("CharacterSelected") == 2){
            GameObject wallBack = GameObject.Find("/Canvas/Button/WallBack");
            GameObject goToStageButton = GameObject.Find("/Canvas/Button/GoToStage");
            wallBack.SetActive(true);
            goToStageButton.SetActive(true);
        }
        
    }
    
    public float GetNumberOfTags(string tagName){
        tagObjects = GameObject.FindGameObjectsWithTag(tagName);
        if(tagObjects.Length == 0){
            Debug.Log("そんなタグが着いてるのはいないぞ！");
        }
        return tagObjects.Length;
    }
}
