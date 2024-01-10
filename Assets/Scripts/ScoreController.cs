using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public GameController gameController;
    public BallController ballController;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI currentScoreText;

    private float highScore = 0f;
    private float currentScore = 0f;

    private string HIGH_SCORE_TEXT = "High Score: ";
    private string CURRENT_SCORE_TEXT = "Current Score: ";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.hasLeftPlayerWon())
        {
            if (currentScore > highScore)
            {
                highScore = currentScore;
                highScoreText.text = HIGH_SCORE_TEXT + highScore;
            }
        }
    }

    public void startGame()
    {
        currentScore = 0f;
        currentScoreText.text = CURRENT_SCORE_TEXT + currentScore;
    }

    public void incrementCurrentScore()
    {
        currentScore += 100f;
        currentScoreText.text = CURRENT_SCORE_TEXT + currentScore;
    }
}
