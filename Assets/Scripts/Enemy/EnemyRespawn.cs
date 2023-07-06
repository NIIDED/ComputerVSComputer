using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawn : MonoBehaviour
{
    public GameObject prefab;

    public void Respawn(){
        Instantiate(prefab,new Vector3(0,0,0),Quaternion.identity);
    }
}
