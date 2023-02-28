using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int player1Life = 5;
    public int player2Life = 5;
    public int player3Life = 5;
    public int player4Life = 5;
    public int player5Life = 5;

    public GameObject gameOver;

    public GameObject[] playerHealthDisplay;
    public GameObject[] player1Hearts;
    public GameObject[] player2Hearts;
    public GameObject[] player3Hearts;
    public GameObject[] player4Hearts;
    public GameObject[] player5Hearts;

    private void OnEnable()
    {
        SpawnManager.OnPlayerStart += setPlayerHealthDisplay;
        PlayerController.OnPlayerDeath += HurtPlayer;
    }

    private void OnDisable()
    {
        SpawnManager.OnPlayerStart -= setPlayerHealthDisplay;
        PlayerController.OnPlayerDeath -= HurtPlayer;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindGameObjectsWithTag("Player").Length <= 1)
        {
            gameOver.SetActive(true);
        }
    }

    public void setPlayerHealthDisplay(int playerCount)
    {
        for (int i = 0; i < playerCount; i++)
        {
            playerHealthDisplay[i].SetActive(true);
        }
    }
    public void HurtPlayer(int playerIndex)
    {
        Debug.Log(playerIndex);
        switch (playerIndex)
        {
            case 0:
                player1Life -= 1;

                for (int i = 0; i < player1Hearts.Length; i++)
                {
                    if (player1Life > i)
                    {
                        player1Hearts[i].SetActive(true);
                    }
                    else
                    {
                        player1Hearts[i].SetActive(false);
                    }
                }
                break;
            case 1:
                player2Life -= 1;

                for (int i = 0; i < player2Hearts.Length; i++)
                {
                    if (player2Life > i)
                    {
                        player2Hearts[i].SetActive(true);
                    }
                    else
                    {
                        player2Hearts[i].SetActive(false);
                    }
                }
                break;
            case 2:
                player3Life -= 1;

                for (int i = 0; i < player3Hearts.Length; i++)
                {
                    if (player3Life > i)
                    {
                        player3Hearts[i].SetActive(true);
                    }
                    else
                    {
                        player3Hearts[i].SetActive(false);
                    }
                }
                break;
            case 3:
                player4Life -= 1;

                for (int i = 0; i < player4Hearts.Length; i++)
                {
                    if (player4Life > i)
                    {
                        player4Hearts[i].SetActive(true);
                    }
                    else
                    {
                        player4Hearts[i].SetActive(false);
                    }
                }
                break;
            case 4:
                player5Life -= 1;

                for (int i = 0; i < player5Hearts.Length; i++)
                {
                    if (player5Life > i)
                    {
                        player5Hearts[i].SetActive(true);
                    }
                    else
                    {
                        player5Hearts[i].SetActive(false);
                    }
                }
                break;

        }

    }
}
