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

    //TODO: Move Death SFX to bomberman somehow
    AudioSource audioSource;
    AudioClip deathSFX;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerNames = new HashSet<string>(GameObject.FindGameObjectsWithTag(GlobalConstants.TagNames.BOMBERMAN).Select(go => go.name));
        EventManager.StartListening(EventName.PLAYER_DIED, OnPlayerDeath);
        EventManager.StartListening(EventName.TIME_UP, GameOver);
        deathSFX = Resources.Load<AudioClip>("SFX/Death SFX");
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
        audioSource.PlayOneShot(deathSFX);
        playerNames.Remove(playerName);
        if(playerNames.Count == 1) {
            GameOver();   
        }
    }

    void GameOver() {
        gameOverText.gameObject.SetActive(true);
        if (playerNames.Count == 1)
        {
            gameOverText.text = string.Format("{0} wins!", playerNames.ToArray()[0]);
        }
        else {
            gameOverText.text = "Draw!";
        }
        gameOver = true;
        EventManager.TriggerEvent(EventName.GAME_OVER);
    }

    void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameOver = false;
    }

}
