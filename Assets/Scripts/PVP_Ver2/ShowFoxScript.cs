using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowFoxScript : MonoBehaviour
{
    public Text showFoxScript;
    string foxScript;
    int actionNum = 1;


    void Start(){
        foxScript = "float WhatCommandToInput1(){ \nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){ \nif(!IsGround()){ \nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){ \nreturn input = (-1) * HightRightOrHightLeft();\n}  \nreturn input = HightRightOrHightLeft(); \n} else if(IsSheJumping()){ \nreturn input = 5.0f; \n} \nelse if (Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToJumpLast){ \nreturn input = 5.0f;            \n} else { \nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){ \nreturn input = (-1) * RightOrLeft(); \n}  \nreturn input = RightOrLeft(); \n} \n} else { \nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){ \nreturn input = (-1) * RightOrLeft(); \n}   \nreturn input = RightOrLeft(); \n} \n} \n";
        //"float WhatCommandToInput1(){if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){if(!IsGround()){                //1.5以内で自分が空中にいるなら敵の方へ空気中移動するif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離return input = (-1) * HightRightOrHightLeft();}  return input = HightRightOrHightLeft();} else if(IsSheJumping()){      //もし自分が地面にいて、敵がジャンプしているならば自分もジャンプしろ           return input = 5.0f;} // 自分は地面にいて、相手はジャンプしていない（つまりウロウロまたは、停止else if (Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToJumpLast return input = 5.0f;       } else {if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離return input = (-1) * RightOrLeft();}  return input = RightOrLeft();}} else {if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離return input = (-1) * RightOrLeft();}  return input = RightOrLeft();}";
    }

    void Update()
    {
        ChangeScript();
        showFoxScript.text = foxScript;
    }

    public void PlusActionNum(){
        actionNum +=1;
        if(actionNum == 4){
            actionNum = 0;
        }
    }

    public void BackToCharacterSelection2(){
        SceneManager.LoadScene("2_CharacterSelectVer2");
    }

    public void ChangeScript(){
        switch (actionNum){
            case 1:
            foxScript = "float WhatCommandToInput1(){ \nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){ \nif(!IsGround()){ \nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){ \nreturn input = (-1) * HightRightOrHightLeft();\n}  \nreturn input = HightRightOrHightLeft(); \n} else if(IsSheJumping()){ \nreturn input = 5.0f; \n} \nelse if (Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToJumpLast){ \nreturn input = 5.0f;            \n} else { \nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){ \nreturn input = (-1) * RightOrLeft(); \n}  \nreturn input = RightOrLeft(); \n} \n} else { \nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){ \nreturn input = (-1) * RightOrLeft(); \n}   \nreturn input = RightOrLeft(); \n} \n} \n";
            break;

            case 2:
            foxScript = "float WhatCommandToInput2(){\nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){\nif(!IsGround()){ \nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){\nreturn input = (-1) * HightRightOrHightLeft();\n}  \nreturn input = HightRightOrHightLeft();\n} else if(IsSheJumping()){      \nif( (EnemyTrns.position.y - transform.position.y) > 5.0f){ \nDebug.Log(\"(2) 敵がジャンプしていたから一旦退避！！\");        //退避\nreturn input = RightOrLeft()*(-1);\n}   else {\nreturn input = 5.0f;\n}\n} \nelse {\nDebug.Log(\"一回様子見\");\nreturn input = 0;\n}\n} else {\nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離\nreturn input = (-1) * RightOrLeft();\n}  \nreturn input = RightOrLeft();\n}\n}";
            break;

            case 3:
            foxScript = "float WhatCommandToInput3(){\nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){\nif(!IsGround()){                //1.5以内で自分が空中にいるなら敵の方へ空気中移動する\nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離\nreturn input = (-1) * HightRightOrHightLeft();\n}  \nreturn input = HightRightOrHightLeft();\n}\nelse {\nreturn 5.0f;        //ひたすらジャンプ　\n}\n} else {\nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離\nreturn input = (-1) * RightOrLeft();\n}  \nreturn input = RightOrLeft();\n}\n}";
            break;
        }
    }
}
