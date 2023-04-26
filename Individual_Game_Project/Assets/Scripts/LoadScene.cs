using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : GameManager
{
    void Start() {
        // ReloadLevel();
    }

    public void ReloadLevel(){
        if(playerPieces.Count > 0 || enemyPieces.Count > 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void LoadLevelOne() {
        SceneManager.LoadScene("Kurikaesu"); 
    }

    public void LoadLevelTwo() {
        SceneManager.LoadScene("LevelTwo"); 
    }

    public void Restart() {
        SceneManager.LoadScene("Tutorial"); 
    }
}
