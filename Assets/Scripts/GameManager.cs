using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI restartText;

    HashSet<int> playerIDs;
    bool gameOver = false;

    private void Awake()
    {
        playerIDs = new HashSet<int>(GameObject.FindGameObjectsWithTag(GlobalConstants.TagNames.BOMBERMAN).Select(go => go.GetInstanceID()));
        EventManager.StartListening(EventName.PLAYER_DIED, OnPlayerDeath);
    }

    private void Update()
    {
        if(gameOver) { 
            if(Input.GetKeyDown(KeyCode.R)) {
                Restart();
            }
        }
    }

    void OnPlayerDeath(int playerID) {
        playerIDs.Remove(playerID);
        if(playerIDs.Count == 0) {
            GameOver();   
        }
    }

    void GameOver() {
        restartText.gameObject.SetActive(true);
        gameOver = true;
    }

    void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameOver = false;
    }

}
