using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowFrogScript : MonoBehaviour
{
    public Text showFrogScript;
    string frogScript;
    int actionNum = 1;


    void Start(){
        frogScript = "float WhatCommandToInput1(){ \nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){ \nif(!IsGround()){ \nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){ \nreturn input = (-1) * HightRightOrHightLeft();\n}  \nreturn input = HightRightOrHightLeft(); \n} else if(IsSheJumping()){ \nreturn input = 5.0f; \n} \nelse if (Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToJumpLast){ \nreturn input = 5.0f;            \n} else { \nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){ \nreturn input = (-1) * RightOrLeft(); \n}  \nreturn input = RightOrLeft(); \n} \n} else { \nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){ \nreturn input = (-1) * RightOrLeft(); \n}   \nreturn input = RightOrLeft(); \n} \n} \n";
        //"float WhatCommandToInput1(){if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){if(!IsGround()){                //1.5以内で自分が空中にいるなら敵の方へ空気中移動するif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離return input = (-1) * HightRightOrHightLeft();}  return input = HightRightOrHightLeft();} else if(IsSheJumping()){      //もし自分が地面にいて、敵がジャンプしているならば自分もジャンプしろ           return input = 5.0f;} // 自分は地面にいて、相手はジャンプしていない（つまりウロウロまたは、停止else if (Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToJumpLast return input = 5.0f;       } else {if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離return input = (-1) * RightOrLeft();}  return input = RightOrLeft();}} else {if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離return input = (-1) * RightOrLeft();}  return input = RightOrLeft();}";
    }

    void Update()
    {
        ChangeScript();
        showFrogScript.text = frogScript;
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
            frogScript = "float WhatCommandToInput1(){\nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){\nif(!IsGround()){                //1.5以内で自分が空中にいるなら敵の方へ空気中移動する\nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離\nreturn input = (-1) * HightRightOrHightLeft();\n}  \nreturn input = HightRightOrHightLeft();\n} else if(IsSheJumping()){      //もし自分が地面にいて、敵がジャンプしているならば自分もジャンプしろ    //一旦退避してからジャンプ\nDebug.Log(\"(2) PLAYER1 がジャンプしていた！！\");        //下の\"相手がジャンプしないけど、とりあえず近寄る！\"で近寄って敵がジャンプしたときにジャンプするともう手遅れ\nreturn input = 5.0f;\n} // 自分は地面にいて、相手はジャンプしていない（つまりウロウロまたは、停止\nelse if (Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToJumpLast){\nDebug.Log(\"(2) 近すぎるからジャンプした！！\");\nreturn input = 5.0f;            // 近すぎたらジャンプ\n} else {\nDebug.Log(\"(2) 相手がジャンプしないけど、とりあえず近寄る！\");\nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離\nreturn input = (-1) * RightOrLeft();\n}  \nreturn input = RightOrLeft();\n}\n} else {\nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離\nreturn input = (-1) * RightOrLeft();\n}  \nreturn input = RightOrLeft();\n}\n}";
            break;

            case 2:
            frogScript = "float WhatCommandToInput2(){\nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){\nif(!IsGround()){                //1.5以内で自分が空中にいるなら敵の方へ空気中移動する\nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離\nreturn input = (-1) * HightRightOrHightLeft();\n}  \nreturn input = HightRightOrHightLeft();\n} else if(IsSheJumping()){      //もし自分が地面にいて、敵がジャンプしているならば自分もジャンプしろ    //一旦退避してからジャンプ\nif( (EnemyTrns.position.y - transform.position.y) > 5.0f){ //yの距離が5より大きい時は\nDebug.Log(\"(2) 敵がジャンプしていたから一旦退避！！\");        //退避\nreturn input = RightOrLeft()*(-1);\n}   else {\nreturn input = 5.0f;\n}\n} \nelse {\nDebug.Log(\"一回様子見\");\nreturn input = 0;\n}\n} else {\nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離\nreturn input = (-1) * RightOrLeft();\n}  \nreturn input = RightOrLeft();\n}\n}";
            break;

            case 3:
            frogScript = "float WhatCommandToInput3(){\nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){\nif(!IsGround()){                //1.5以内で自分が空中にいるなら敵の方へ空気中移動する\nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離\nreturn input = (-1) * HightRightOrHightLeft();\n}  \nreturn input = HightRightOrHightLeft();\n}\nelse {\nreturn 5.0f;        //ひたすらジャンプ　\n}\n} else {\nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離\nreturn input = (-1) * RightOrLeft();\n}  \nreturn input = RightOrLeft();\n}\n}";
            break;
        }
    }
}
