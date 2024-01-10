using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int currentLevel;
    private DateTime mStartTime;
    private bool gameStarted;
    private bool gameEnded;
    private bool leftPlayerWon;
    private bool rightPlayerWon;
    private static String INITIAL_GAME_TEXT = "Press Space Bar to start...";
    private int players;

    public int levelTime = 5;
    public Transform gameBall;
    public SpriteRenderer gameBallSprite;
    public BallController ballController;
    public TextMeshProUGUI levelText;
    public CountdownTimer countdownTimer;
    public PaddleController paddleController;
    public AudioSource backgroundMusic;
    public ScoreController scoreController;
    public TextMeshProUGUI instructionsText;

    void Start()
    {
        players = PlayerPrefs.GetInt("Players");
        paddleController.StartGame(players);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu 3D", LoadSceneMode.Single);
        }

        if (!gameStarted || hasGameEnded())
        {
            return;
        }

        updateLevel();
    }

    private void updateLevel()
    {
        DateTime currentTime = DateTime.Now;
        TimeSpan elapsedTime = currentTime - mStartTime;
        double elapsedTimeDouble = elapsedTime.TotalMilliseconds;

        if (elapsedTimeDouble > (levelTime * 1000))
        {
            levelText.SetText("Level " + ++currentLevel);
            ballController.incrementMaxVelocity();
            countdownTimer.ResetFill();
            mStartTime = DateTime.Now;
        }
    }

    private bool hasGameEnded()
    {
        if (gameEnded)
        {
            return true;
        }

        if (gameBall.localPosition.x <= -12f || gameBall.localPosition.y > 50f)
        {
            levelText.SetText("Player 2 wins!");
            gameEnded = true;
            rightPlayerWon = true;
            leftPlayerWon = false;
            ballController.hideBall();

        }
        else if (gameBall.localPosition.x > 12f || gameBall.localPosition.y < -50f)
        {
            levelText.SetText("Player 1 wins!");
            gameEnded = true;
            leftPlayerWon = true;
            rightPlayerWon = false;
            ballController.hideBall();

        }

        if (gameEnded)
        {
            gameBallSprite.enabled = false;
            StartCoroutine(Reset());
        }

        return gameEnded;
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(3f);

        gameStarted = false;
        gameEnded = false;
        currentLevel = 1;
        mStartTime = DateTime.Now;
        levelText.SetText(INITIAL_GAME_TEXT);
        ballController.Reset();
        countdownTimer.Reset();
    }

    public bool hasGameStarted()
    {
        return gameStarted;
    }

    public bool getGameEnded()
    {
        return gameEnded;
    }

    public bool hasLeftPlayerWon()
    {
        return leftPlayerWon;
    }

    public bool hasRightPlayerWon()
    {
        return rightPlayerWon;
    }

    public void startGame()
    {
        currentLevel = 1;
        mStartTime = DateTime.Now;
        levelText.SetText("Level " + currentLevel);
        gameStarted = true;
        gameEnded = false;
        leftPlayerWon = false;
        rightPlayerWon = false;
        countdownTimer.startGame();
        scoreController.startGame();
        instructionsText.enabled = false;
    }

    public int getLevelTime()
    {
        return levelTime;
    }
}
