using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupGame : MonoBehaviour
{
    public static SetupGame instance;
    public GameManager gameManager;
    public bool endlessMode;
    public bool inGame;
    public int diffiulty;
    public List<RoomWaves> roomwavesEasy = new List<RoomWaves>();
    public List<RoomWaves> roomwavesHard = new List<RoomWaves>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void ToggleEndlessMode()
    {
        endlessMode = !endlessMode;
    }

    public void LinkGameManager(GameManager _gameManager)
    {
        gameManager = _gameManager;
        gameManager.endlessMode = endlessMode;

        gameManager.roomWaves.Clear();
        switch (diffiulty)
        {
            case 0:
                gameManager.roomWaves = roomwavesEasy;
                break;
            case 1:
                gameManager.roomWaves = roomwavesHard;
                break;
            case 2:
                gameManager.roomWaves = roomwavesHard;
                break;

        }

        gameManager.DelayedStart();
    }
}