using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowNinjaScript : MonoBehaviour
{
    public Text showNinjaScript;
    string ninjaScript;
    int actionNum = 1;


    void Start(){
        ninjaScript = "float WhatCommandToInput1(){ \nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){ \nif(!IsGround()){ \nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){ \nreturn input = (-1) * HightRightOrHightLeft();\n}  \nreturn input = HightRightOrHightLeft(); \n} else if(IsSheJumping()){ \nreturn input = 5.0f; \n} \nelse if (Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToJumpLast){ \nreturn input = 5.0f;            \n} else { \nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){ \nreturn input = (-1) * RightOrLeft(); \n}  \nreturn input = RightOrLeft(); \n} \n} else { \nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){ \nreturn input = (-1) * RightOrLeft(); \n}   \nreturn input = RightOrLeft(); \n} \n} \n";
        //"float WhatCommandToInput1(){if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){if(!IsGround()){                //1.5以内で自分が空中にいるなら敵の方へ空気中移動するif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離return input = (-1) * HightRightOrHightLeft();}  return input = HightRightOrHightLeft();} else if(IsSheJumping()){      //もし自分が地面にいて、敵がジャンプしているならば自分もジャンプしろ           return input = 5.0f;} // 自分は地面にいて、相手はジャンプしていない（つまりウロウロまたは、停止else if (Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToJumpLast return input = 5.0f;       } else {if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離return input = (-1) * RightOrLeft();}  return input = RightOrLeft();}} else {if(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) > 15.5f){//ステージの大きさが31だからその半分の距離が実際の最大距離return input = (-1) * RightOrLeft();}  return input = RightOrLeft();}";
    }

    void Update()
    {
        ChangeScript();
        showNinjaScript.text = ninjaScript;
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
            ninjaScript = "float WhatCommandToInput1(){\nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){\nif(!IsGround()){\nreturn HightRightOrHightLeft()*(-1f);\n}else {\nreturn RightOrLeft()*(-1f); //逃げる\n}\n} else {\nreturn RightOrLeft(); //近寄る（攻撃届かないからね）\n          } \nreturn RightOrLeft();\n";
            break;

            case 2:
            ninjaScript = "float WhatCommandToInput2(){\nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){\nif(!IsGround()){\nreturn HightRightOrHightLeft()*(-1f);\n}else {\nreturn RightOrLeft()*(-1f); //逃げる\n}\n} else {\nreturn RightOrLeft(); //近寄る（攻撃届かないからね）\n          } \nreturn RightOrLeft();\n";
            break;

            case 3:
            ninjaScript = "float WhatCommandToInput3(){\nif(Mathf.Abs(this.transform.position.x - EnemyTrns.position.x ) < lengthToStartThink){\nif(!IsGround()){\nreturn HightRightOrHightLeft()*(-1f);\n}else {\nreturn RightOrLeft()*(-1f); //逃げる\n}\n} else {\nreturn RightOrLeft(); //近寄る（攻撃届かないからね）\n          } \nreturn RightOrLeft();\n";
            break;
        }
    }
}
