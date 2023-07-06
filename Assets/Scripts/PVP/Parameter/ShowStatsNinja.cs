using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStatsNinja : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    float jumpSpeed;
    float power;   //横移動の際かける力　
    float hp;
    public float attack;
    float atPower;
    float searchAngle;

    float lengthToStartThink;      //いつからただ進むだけじゃないか
    float lengthToJumpLast;        //最後近すぎてジャンプする距離

    float searchRadius;

    void Update(){
        GameObject characterParameterManager = GameObject.Find("CharacterManagerNinja");
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

        gameManager.ShowPlayerStatusWithHP(hp,jumpSpeed,power, attack,  atPower,  searchAngle,  searchRadius,  lengthToStartThink,  lengthToJumpLast);
    }

}
