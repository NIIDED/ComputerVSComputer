using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int ammountOfEnemy;
    public Text textPlayer1Hp;
    public Text textPlayer2Hp;
    public Text textPlayer1Status;
    public Text textPlayer2Status;

    [SerializeField] GameObject gameOverTextObj;
    [SerializeField] GameObject gameClearTextObj;

    void Start()
    {
        ammountOfEnemy = 1;
    }


    public void ShowPlayer1Status(float jumpSpeed, float power, float attack, float atPower, float searchAngle, float searchRadius, float lengthToStartThink, float lengthToJumpLast){
        textPlayer1Status.text = "Jump " + jumpSpeed + "\nPower " +power + "\nAttack " + attack +"\nAttackPower " + atPower +"\nSearchAngle " +searchAngle+ "\nSearchRadius " +searchRadius+"\nlengthToStartThink "+lengthToStartThink+"\nlengthToJumpLast "+lengthToJumpLast;
    }

    public void ShowPlayerStatusWithHP(float hp,float jumpSpeed, float power, float attack, float atPower, float searchAngle, float searchRadius, float lengthToStartThink, float lengthToJumpLast){
        textPlayer1Status.text = "HP "+ hp+ "\nJump " + jumpSpeed + "\nPower " +power + "\nAttack " + attack +"\nAttackPower " + atPower +"\nSearchAngle " +searchAngle+ "\nSearchRadius " +searchRadius+"\nlengthToStartThink "+lengthToStartThink+"\nlengthToJumpLast "+lengthToJumpLast;
    }

    public void ShowPlayer2Status(float jumpSpeed, float power, float attack, float atPower, float searchAngle, float searchRadius, float lengthToStartThink, float lengthToJumpLast){
        textPlayer2Status.text = "Jump " + jumpSpeed + "\nPower " +power + "\nAttack " + attack +"\nAttackPower " + atPower +"\nSearchAngle " +searchAngle+ "\nSearchRadius " +searchRadius+"\nlengthToStartThink "+lengthToStartThink+"\nlengthToJumpLast "+lengthToJumpLast;
    }

    public void ChangePlayer1HpText(float hp){
        textPlayer1Hp.text = "[ Player1 ] \n HP: "+ hp;
    }

    public void ChangePlayer2HpText(float hp){
        textPlayer2Hp.text = "[ Player2 ] \n HP: "+ hp;
    }

    public void DecreaseAmmountOfEnemy(){
        ammountOfEnemy -=1;
    }

    public bool EnemyLeft(){
          if(ammountOfEnemy <= 0f){
            return false;
        } else {
            return true;
        }
    }

    public void GameOver(){
        gameOverTextObj.SetActive(true);
        Invoke("ReStartThisScene",1f);
    }

    public void GameClear(){
        gameClearTextObj.SetActive(true);
        Invoke("MoveToNextStage",2f);
    }

    public void GameAllClear(){
        gameClearTextObj.SetActive(true);
        Invoke("MoveToTitle",2f);
    }

    public void PlayAgain(){
        //誰々の勝ちを入れる
        Invoke("ReStartThisScene",2f);
    }

    public void ReStartThisScene(){
        ammountOfEnemy =1;
        Scene ThisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(ThisScene.name);
    }

    void MoveToNextStage(){
            SceneManager.LoadScene("StageTwo(MultipulEnemy)");
    }

    void MoveToTitle(){
            SceneManager.LoadScene("1_TitleSceneVer2");
    }

    public void BackToCharacterSelection2(){
        GameObject CharacterManagerFrog = GameObject.Find("CharacterManagerFrog");
        GameObject CharacterManagerEagle = GameObject.Find("CharacterManagerEagle");
        GameObject CharacterManagerFox = GameObject.Find("CharacterManagerFox");
        GameObject CharacterManagerNinja = GameObject.Find("CharacterManagerNinja");
        
        Destroy(CharacterManagerFrog);
        Destroy(CharacterManagerEagle);
        Destroy(CharacterManagerFox);
        Destroy(CharacterManagerNinja);
        
        SceneManager.LoadScene("2_CharacterSelectVer2");
    }

}
