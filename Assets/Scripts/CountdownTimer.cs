using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public GameController gameController;
    public Slider countdownBar;

    public float countDownTime = 5;
    private float timeRemaining;

    private GameObject levelProgressBar;

    private void Start()
    {
        levelProgressBar = GameObject.Find("LevelProgressBar");
        levelProgressBar.SetActive(false);
    }

    private void Update()
    {
        if (!gameController.hasGameStarted())
        {
            return;
        }

        if (gameController.getGameEnded())
        {
            Reset();
            return;
        }

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            float fillAmount = 1 - (timeRemaining / countDownTime);

            countdownBar.value = fillAmount;
        }
    }

    public void startGame()
    {
        levelProgressBar.SetActive(true);
        ResetFill();
    }

    public void ResetFill()
    {
        if (gameController == null)
        {
            return;
        }

        timeRemaining = gameController.getLevelTime();
        countdownBar.value = 0;
    }

    public void Reset()
    {
        ResetFill();
        levelProgressBar.SetActive(false);
    }
}