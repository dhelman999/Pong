using System;
using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private float runningMaxVelocity;
    private float trailTime;

    private Rigidbody mRigidbody;
    private Vector3 oldVelocity;
    private TrailRenderer trail;

    public Transform gameBoard;
    public AudioSource bumperBump;
    public float maxVelocity = 10f;
    public bool randomInitialDirection = true;
    public float initialXForce = 10;
    public float initialYForce = 0;

    public enum BallCollisionType
    {
        UPPER_PADDLE,
        LOWER_PADDLE,
        BUMPER,
        NONE
    }

    public Rigidbody leftPaddleBody;
    public Rigidbody rightPaddleBody;
    public GameController gameController;
    public GameObject explosion;
    public ScoreController scoreController;
    public SpriteRenderer gameBallSprite;

    void Start()
    {
        trail = GetComponentInChildren<TrailRenderer>();
        trailTime = trail.time;
        mRigidbody = GetComponent<Rigidbody>();
        runningMaxVelocity = maxVelocity;
    }

    void FixedUpdate()
    {
        if (!gameController.hasGameStarted())
        {
            if (Input.GetKey(KeyCode.Space))
            {
                startGame();
                gameController.startGame();
            }
        }

        updateBallForce();
    }

    private void updateBallForce()
    {
        oldVelocity = mRigidbody.velocity;

        if (mRigidbody.velocity.magnitude > runningMaxVelocity)
        {
            mRigidbody.velocity = mRigidbody.velocity.normalized * runningMaxVelocity;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        offSetPaddleCollision(collision);

        offSetBumperCollision(collision);

        // bumper effect to speed up ball
        ContactPoint cp = collision.contacts[0];
        mRigidbody.velocity += cp.normal * 2.0f;
        bumperBump.Play();

        StartCoroutine(PlayParticle(cp));
        scoreController.incrementCurrentScore();
    }

    IEnumerator PlayParticle(ContactPoint cp)
    {
        GameObject clone = Instantiate(explosion, cp.point, explosion.transform.rotation);
        ParticleSystem psClone = clone.GetComponent<ParticleSystem>();

        Destroy(clone, 3f);

        yield return null;
    }

    private void offSetPaddleCollision(Collision collision)
    {
        ContactPoint cp = collision.contacts[0];
        Vector3 velocityOffset = Vector3.zero;
        BallCollisionType ballCollisionType = getCollisionType(collision);

        if (ballCollisionType.Equals(BallCollisionType.UPPER_PADDLE))
        {
            velocityOffset = new Vector3(0, .1f, 0);
        }
        else if (ballCollisionType.Equals(BallCollisionType.LOWER_PADDLE))
        {
            velocityOffset = new Vector3(0, -.1f, 0);
        }

        Vector3 reflectedVelocity = Vector3.Reflect(oldVelocity, cp.normal + velocityOffset);
        mRigidbody.velocity = reflectedVelocity;
    }

    private void offSetBumperCollision(Collision collision)
    {
        Rigidbody paddleBody = collision.body.GetComponent<Rigidbody>();

        if (!isPaddle(paddleBody))
        {
            if (mRigidbody.velocity.x > 0)
            {
                if (mRigidbody.velocity.x < 5f)
                {
                    mRigidbody.AddForce(new Vector3(UnityEngine.Random.Range(10f, 15f), 0, 0), ForceMode.Impulse);
                }
            }

            if (mRigidbody.velocity.x < 0)
            {
                if (mRigidbody.velocity.x > -5f)
                {
                    mRigidbody.AddForce(new Vector3(-UnityEngine.Random.Range(10f, 15f), 0, 0), ForceMode.Impulse);
                }
            }
        }
    }

    private BallCollisionType getCollisionType(Collision collision)
    {
        BallCollisionType ballCollisionType = BallCollisionType.NONE;

        if (collision == null)
        {
            return ballCollisionType;
        }

        Rigidbody paddleBody = collision.body.GetComponent<Rigidbody>();

        if (isPaddle(paddleBody))
        {
            ContactPoint contactPoint = collision.contacts[0];
            Vector3 paddleCenter = gameBoard.InverseTransformPoint(paddleBody.worldCenterOfMass);
            Vector3 cpCenter = gameBoard.InverseTransformPoint(contactPoint.point);

            if (cpCenter.y > paddleCenter.y)
            {
                ballCollisionType = BallCollisionType.UPPER_PADDLE;
            }
            else if (cpCenter.y < paddleCenter.y)
            {
                ballCollisionType = BallCollisionType.LOWER_PADDLE;
            }
            else
            {
                float rand = UnityEngine.Random.Range(0f, 1f);

                if (rand >= .5f)
                {
                    ballCollisionType = BallCollisionType.UPPER_PADDLE;
                }
                else
                {
                    ballCollisionType = BallCollisionType.LOWER_PADDLE;
                }
            }
        }
        else
        {
            ballCollisionType = BallCollisionType.BUMPER;
        }

        return ballCollisionType;
    }

    private Boolean isPaddle(Rigidbody rb)
    {
        return rb.Equals(leftPaddleBody) || rb.Equals(rightPaddleBody);
    }

    public void startGame()
    {
        addInitialBallForce();

        trail.time = trailTime;
        trail.enabled = true;
    }

    private void addInitialBallForce()
    {
        if (randomInitialDirection)
        {
            float randDirection = UnityEngine.Random.Range(0f, 1f);
            Vector3 initialForce;

            if (randDirection <= .5f)
            {
                initialForce = new Vector3(initialXForce, initialYForce, 0);

            }
            else
            {
                initialForce = new Vector3(-initialXForce, -initialYForce, 0);
            }

            mRigidbody.AddForce(initialForce, ForceMode.Impulse);
            oldVelocity = initialForce;
        }
    }

    public void Reset()
    {
        Vector3 startingPosition = new Vector3(0, 0, 0);
        transform.localPosition = startingPosition;
        mRigidbody.velocity = Vector3.zero;
        mRigidbody.angularVelocity = Vector3.zero;
        mRigidbody.MovePosition(startingPosition);
        runningMaxVelocity = maxVelocity;
        StartCoroutine(ResetTrails());
        showBall();
    }

    IEnumerator ResetTrails()
    {
        trail.time = -1f;

        yield return new WaitForEndOfFrame();

        trail.time = trailTime;
    }

    public void hideBall()
    {
        gameBallSprite.enabled = false;
        trail.time = -1f;
    }

    public void showBall()
    {
        gameBallSprite.enabled = true;
    }

    public void incrementMaxVelocity()
    {
        runningMaxVelocity += 4;
    }
}
