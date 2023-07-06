using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene_CharacterSelect : MonoBehaviour
{
    public GameObject CharacterManagerEagle;
    public GameObject CharacterManagerFox;
    public GameObject CharacterManagerFrog;
    public GameObject CharacterManagerNinja;
    // public GameObject CharacterParameterManager;
    // public GameObject CharacterParameterManager2;

    void Start(){
        CharacterManagerEagle = GameObject.Find("CharacterManagerEagle");
        CharacterManagerFox = GameObject.Find("CharacterManagerFox");
        CharacterManagerFrog = GameObject.Find("CharacterManagerFrog");
        CharacterManagerNinja = GameObject.Find("CharacterManagerNinja");
        //CharacterParameterManager = GameObject.Find("CharacterParameterManager");
        //CharacterParameterManager2 = GameObject.Find("CharacterParameterManager2");
    }

    public void GoToFoxStats(){
        SceneManager.LoadScene("3_StatusChangeFOX");
        /* if(CharacterParameterManager != null){
            Destroy (CharacterParameterManager);
        }
        */
   }

    public void GoToFrogPlayerStats(){
       SceneManager.LoadScene("3_StatusChangeFROG");
       /*if(CharacterParameterManager2 != null){
            Destroy (CharacterParameterManager2);
        }
        */
   }

   public void GoToEaglePlayerStats2(){
       SceneManager.LoadScene("3_StatusChangeEAGLEVer2");
       if(CharacterManagerEagle != null){
           Destroy(CharacterManagerEagle);
       }
   }

    public void GoToFrogPlayerStats2(){
       SceneManager.LoadScene("3_StatusChangeFROGVer2");
        if(CharacterManagerFrog != null){
            Destroy(CharacterManagerFrog);
       }
   }

    public void GoToFoxPlayerStats2(){
       SceneManager.LoadScene("3_StatusChangeFOXVer2");
        if(CharacterManagerFox != null){
             Destroy(CharacterManagerFox);
       }
   }
   
    public void GoToNinjaPlayerStats2(){
       SceneManager.LoadScene("3_StatusChangeNINJAVer2");
        if(CharacterManagerNinja != null){
            Destroy(CharacterManagerNinja);
       }
   }

    public void GoToChangeScript(){
       SceneManager.LoadScene("2_ScriptChange");
   }

    public void BackToCharacterSelection2(){
        SceneManager.LoadScene("2_CharacterSelectVer2");
    }

    public void GoToChangeScriptFox(){
        SceneManager.LoadScene("7_ScriptsFox");
    }
    public void GoToChangeScriptFrog(){
        SceneManager.LoadScene("7_ScriptsFrog");
    }
    public void GoToChangeScriptEagle(){
        SceneManager.LoadScene("7_ScriptsEagle");
    }
    public void GoToChangeScriptNinja(){
        SceneManager.LoadScene("7_ScriptsNinja");
    }
    
    public void GoToTutorial(){
        SceneManager.LoadScene("StageOne(1enemy)");
    }
}
