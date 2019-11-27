using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gameOverText;

    HashSet<string> playerNames;
    bool gameOver = false;

    private void Awake()
    {
        playerNames = new HashSet<string>(GameObject.FindGameObjectsWithTag(GlobalConstants.TagNames.BOMBERMAN).Select(go => go.name));
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

    void OnPlayerDeath(string playerName) {
        playerNames.Remove(playerName);
        if(playerNames.Count == 1) {
            GameOver();   
        }
    }

    void GameOver() {
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = string.Format("{0} wins!", playerNames.ToArray()[0]);
        gameOver = true;
    }

    void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameOver = false;
    }

}
