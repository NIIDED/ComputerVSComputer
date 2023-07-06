using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeParameter : MonoBehaviour 
{       //パラメータの管理をここでやる。値を保有するのもここ。変更も可能

    [SerializeField] GameManager gameManager;

        public float jumpSpeed;
        public float power;   //横移動の際かける力　
        public float highPower;
        public float hp;
        public float attack;
        public float atPower;
        public float searchAngle;

        public float lengthToStartThink;      //いつからただ進むだけじゃないか
        public float lengthToJumpLast;        //最後近すぎてジャンプする距離

        public float searchRadius;
        public float reactionRadius;

        public int attackStyle;


        GameObject[] tagObjects;
        int i;

        public Text ChangeAttackStyleText;

    void Awake(){
        DontDestroyOnLoad(this.gameObject);
    }

    void Start(){
        i = 1;
        attackStyle = 1;
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

    public void GoToBattle2(){
        GameObject characterEagle = GameObject.Find("CharacterManagerEagle");       //2
        GameObject characterFox = GameObject.Find("CharacterManagerFox");           //3
        GameObject characterFrog = GameObject.Find("CharacterManagerFrog");          //5
        GameObject characterNinja = GameObject.Find("CharacterManagerNinja");       //7
         
        if((characterFox != null) && (characterFrog != null)){
            SceneManager.LoadScene("4_PVP_Battle6");
        } 
        if((characterFox != null) && (characterEagle != null)){
            SceneManager.LoadScene("4_PVP_Battle10");
        } 
        if((characterFox != null) && (characterNinja != null)){
            SceneManager.LoadScene("4_PVP_Battle14");
        } 
        if((characterFrog != null) && (characterEagle != null)){
            SceneManager.LoadScene("4_PVP_Battle15");
        } 
        if((characterFrog != null) && (characterNinja != null)){
            SceneManager.LoadScene("4_PVP_Battle21");
        } 
        if((characterEagle != null) && (characterNinja != null)){
            SceneManager.LoadScene("4_PVP_Battle35");
        } 
    }

    public void BackToCharacterSelection(){
        SceneManager.LoadScene("2_PVP_CharacterSelect");
        Destroy(this.gameObject);
    }

    public void BackToCharacterSelection2(){
        SceneManager.LoadScene("2_CharacterSelectVer2");
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
    
    public void ConfirmStats2(){
        if(GetNumberOfTags("CharacterSelected") < 2){
            SceneManager.LoadScene("2_CharacterSelectVer2");
        }
        if(GetNumberOfTags("CharacterSelected") == 2){
            GameObject wallBack = GameObject.Find("/Canvas/Button/WallBack");
            GameObject goToStageButton = GameObject.Find("/Canvas/Button/GoToStage");
            wallBack.SetActive(true);
            goToStageButton.SetActive(true);
        }
    }

    public void ChangeAttackStyle(){
        i++;
        ChangeAttackStyleText.text = "AttackStyle\n"+ i;
        attackStyle = i;
        if(i == 3){
            i = 0;
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
