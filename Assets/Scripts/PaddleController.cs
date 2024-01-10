using UnityEngine;

public class PaddleController : MonoBehaviour
{
    private int players;
    private static float maxY = 3.9f;
    private static float minY = -maxY;
    private static float leftMinX = -8.6f;
    private static float leftMaxX = leftMinX + 2f;
    private static float rightMaxX = -leftMinX;
    private static float rightMinX = rightMaxX - 2f;

    public float leftPaddleBaseSpeed = 1f;
    public float rightPaddleBaseSpeed = 1f;
    public float computerDifficultyModifier = 1f;

    private float leftPaddleCurrentSpeed = 1f;
    private float rightPaddleCurrentSpeed = 1f;

    public Transform mLeftPaddle;
    public Transform mRightPaddle;
    public Transform gameBall;
    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        leftPaddleCurrentSpeed = leftPaddleBaseSpeed;
        rightPaddleCurrentSpeed = rightPaddleBaseSpeed;

        if (players == 1)
        {
            rightPaddleBaseSpeed = rightPaddleBaseSpeed * computerDifficultyModifier;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveLeftPaddle();

        moveRightPaddle();
    }

    private void moveLeftPaddle()
    {
        if (Input.GetKey(KeyCode.W))
        {
            movePaddleUp(mLeftPaddle, leftPaddleCurrentSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            movePaddleDown(mLeftPaddle, leftPaddleCurrentSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //movePaddleLeft(mLeftPaddle, leftPaddleSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            //movePaddleRight(mLeftPaddle, leftPaddleSpeed);
        }
    }

    private void moveRightPaddle()
    {
        if (players == 2)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                movePaddleUp(mRightPaddle, rightPaddleCurrentSpeed);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                movePaddleDown(mRightPaddle, rightPaddleCurrentSpeed);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                //movePaddleLeft(mRightPaddle, rightPaddleSpeed);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                //movePaddleRight(mRightPaddle, rightPaddleSpeed);
            }
        }
        else if (!gameController.getGameEnded())
        {
            float centerOfPaddle = mRightPaddle.position.y;
            float centerOfBall = gameBall.position.y;

            if (Mathf.Abs(centerOfPaddle - centerOfBall) > .5f)
            {
                if (centerOfPaddle <= centerOfBall)
                {
                    movePaddleUp(mRightPaddle, rightPaddleCurrentSpeed);
                }
                else
                {
                    movePaddleDown(mRightPaddle, rightPaddleCurrentSpeed);
                }
            }
        }
    }

    private void movePaddleUp(Transform paddle, float paddleSpeed)
    {
        Vector3 desiredMovement = new Vector3(0, .1f, 0) * paddleSpeed;
        Vector3 oldPosition = paddle.localPosition;
        Vector3 desiredPosition = oldPosition + desiredMovement;

        if (desiredPosition.y <= maxY)
        {
            paddle.localPosition = oldPosition + desiredMovement;
            paddle.GetComponent<Rigidbody>().MovePosition(paddle.localPosition);
        }
    }

    private void movePaddleDown(Transform paddle, float paddleSpeed)
    {
        Vector3 desiredMovement = new Vector3(0, -.1f, 0) * paddleSpeed;
        Vector3 oldPosition = paddle.localPosition;
        Vector3 desiredPosition = oldPosition + desiredMovement;

        if (desiredPosition.y > minY)
        {
            paddle.localPosition = oldPosition + desiredMovement;
            paddle.GetComponent<Rigidbody>().MovePosition(paddle.localPosition);
        }
    }

    private void movePaddleRight(Transform paddle, float paddleSpeed)
    {
        Vector3 desiredMovement = new Vector3(.1f, 0, 0) * paddleSpeed;
        Vector3 oldPosition = paddle.localPosition;
        Vector3 desiredPosition = oldPosition + desiredMovement;

        float targetXBounds;

        if (isLeftPaddle(paddle))
        {
            targetXBounds = leftMaxX;
        }
        else
        {
            targetXBounds = rightMaxX;
        }

        if (desiredPosition.x <= targetXBounds)
        {
            paddle.localPosition = oldPosition + desiredMovement;
            paddle.GetComponent<Rigidbody>().MovePosition(paddle.localPosition);
        }
    }

    private void movePaddleLeft(Transform paddle, float paddleSpeed)
    {
        Vector3 desiredMovement = new Vector3(-.1f, 0, 0) * paddleSpeed;
        Vector3 oldPosition = paddle.localPosition;
        Vector3 desiredPosition = oldPosition + desiredMovement;

        float targetXBounds;
        bool isLeftPaddleBool = isLeftPaddle(paddle);

        if (isLeftPaddleBool)
        {
            targetXBounds = leftMinX;
        }
        else
        {
            targetXBounds = rightMinX;
        }

        if (desiredPosition.x >= targetXBounds)
        {
            paddle.localPosition = oldPosition + desiredMovement;
            paddle.GetComponent<Rigidbody>().MovePosition(paddle.localPosition);
        }

    }

    private bool isLeftPaddle(Transform paddle)
    {
        float xPos = paddle.localPosition.x;

        return (leftMinX <= xPos) && (xPos <= leftMaxX);
    }

    public void StartGame(int players)
    {
        this.players = players;
        leftPaddleCurrentSpeed = leftPaddleBaseSpeed;
        rightPaddleCurrentSpeed = rightPaddleBaseSpeed;
    }
}
