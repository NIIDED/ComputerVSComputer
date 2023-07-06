using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{    
    float jumpSpeed;
    float power;   //横移動の際かける力　
    float hp;
    public float attack;　
    float atPower;
    float searchAngle;

    float lengthToStartThink;      //いつからただ進むだけじゃないか
    float lengthToJumpLast;        //最後近すぎてジャンプする距離

    float searchRadius;

    void Start(){
        GameObject characterParameterManager = GameObject.Find("CharacterParameterManager");
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
       jumpSpeed += 1;
   }

   public void IncreaseAtPower(){
       power += 1;
   }

   public void IncreaseSearchAngle(){
       hp += 1;
   }

    public void IncreaseLengthToStartThink(){
       lengthToStartThink += 1;
   }

   public void IncreaseLengthToJumpLast(){
       lengthToJumpLast += 1;
   }

   public void IncreaseSearchRadius(){
       searchRadius += 1;
   }


//下げるボタン


    public void DecreaseJumpSpeed(){
       jumpSpeed += 1;
   }

   public void DecreasePower(){
       power -= 1;
   }

   public void DecreaseHp(){
       hp -= 1;
   }

   public  void DecreaseAttack(){
       jumpSpeed -= 1;
   }

   public void DecreaseAtPower(){
       power -= 1;
   }

   public void DecreaseSearchAngle(){
       hp -= 1;
   }

   public  void DecreaseLengthToStartThink(){
       lengthToStartThink -= 1;
   }

   public void DecreaseLengthToJumpLast(){
       lengthToJumpLast -= 1;
   }

   public void DecreaseSearchRadius(){
       searchRadius -= 1;
   }


    public void GoToBattle(){
         SceneManager.LoadScene("4_PVP_Battle");
    }
}
