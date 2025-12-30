
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // make player controller script accessible from other scripts
    public static PlayerController playerControllerScript;

    // reference to rigidbody component
    public Rigidbody2D playerRigidbody;

    // reference to player animator
    public Animator playerAnimator;

    // player's movement speed
    public float playerMoveSpeed = 5;

    // player's movement controls
    private Vector2 playerMovementInput;

    // reference to player bullet
    public GameObject playerBullet;

    // position to fire bullet from
    public Transform firePosition;

    // if player has remote control
    public bool playerHasRemote;
    




    private void Awake()
    {
        // set reference to player controller script
        playerControllerScript = this;
    }




    void Update()
    {
        // get player input
        GetPlayerInput();

        // move player
        MovePlayer();
    }


    private void GetPlayerInput()
    {
        // player movement
        // horizontal movement
        // if player presses the left arrow key
        // store the value in player movement x
        playerMovementInput.x = Input.GetAxisRaw("Horizontal");

        // vertical movement
        // if player presses the left arrow key
        // store the value in player movement y
        playerMovementInput.y = Input.GetAxisRaw("Vertical");

        // normalise the player's input
        playerMovementInput.Normalize();

        SetPlayerSpriteDirection();

        // player firing
        // if the player is holding the remote control
        if (playerHasRemote)
        {
            // if the player presses the space bar
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // fire a bullet
                Instantiate(playerBullet, firePosition.position, firePosition.rotation);

                // play player firing sound
                int playerFiringSound = 16;

                AudioController.audioControllerScript.PlaySFX(playerFiringSound);
            }
        }
    }


    private void SetPlayerSpriteDirection()
    {
        // if player is moving left
        if (playerMovementInput.x < 0)
        {
            // flip sprite to face left
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        // if player is moving right
        if (playerMovementInput.x > 0)
        {
            // flip player to face right
            transform.localScale = Vector3.one;
        }
    }


    private void MovePlayer()
    {
        playerRigidbody.linearVelocity = playerMovementInput * playerMoveSpeed;
    }


    public void DamagePlayer(int enemyDamage)
    {
        // subtract health from player
        GameController.gameControllerScript.playerHealth -= enemyDamage;

        DisplayPizzaHealth();

        // if the player's health is less than or equal to zero
        if (GameController.gameControllerScript.playerHealth <= 0)
        {
            // subtract one from player lives
            GameController.gameControllerScript.lives--;

            // if player lives is greater than or equal to zero
            if (GameController.gameControllerScript.lives >= 0)
            {
                // hide one of the player lives sprites
                GameController.gameControllerScript.playerLives[GameController.gameControllerScript.lives].SetActive(false);

                // reset player health to maximum health
                GameController.gameControllerScript.playerHealth = GameController.MAXIMUM_HEALTH;

                ResetPizza();
            }
        }


        // otherwise
        // if player is out of lives
        if (GameController.gameControllerScript.lives < 0)
        {
            PlayerDead();
        }
    }


    public void PlayerDead()
    {
        // deactivate the player
        gameObject.SetActive(false);

        // play player death sound
        int playerDeathSound = 8;

        AudioController.audioControllerScript.PlaySFX(playerDeathSound);

        // show game over screen
        GameController.gameControllerScript.GameOver();

        // play game over music
        AudioController.audioControllerScript.PlayGameOverMusic();

    }


    private void DisplayPizzaHealth()
    {
        // display player's pizza health based on player health
        if (GameController.gameControllerScript.playerHealth < 100 && GameController.gameControllerScript.playerHealth > 60)
        {
            GameController.gameControllerScript.playerPizzaHealth[0].SetActive(false);

            GameController.gameControllerScript.playerPizzaHealth[1].SetActive(true);
        }

        if (GameController.gameControllerScript.playerHealth < 60 && GameController.gameControllerScript.playerHealth > 30)
        {
            GameController.gameControllerScript.playerPizzaHealth[1].SetActive(false);

            GameController.gameControllerScript.playerPizzaHealth[2].SetActive(true);
        }

        if (GameController.gameControllerScript.playerHealth < 30 && GameController.gameControllerScript.playerHealth > 0)
        {
            GameController.gameControllerScript.playerPizzaHealth[2].SetActive(false);

            GameController.gameControllerScript.playerPizzaHealth[3].SetActive(true);
        }
    }


    private void ResetPizza()
    {
        GameController.gameControllerScript.playerPizzaHealth[0].SetActive(false);
        GameController.gameControllerScript.playerPizzaHealth[1].SetActive(false);
        GameController.gameControllerScript.playerPizzaHealth[2].SetActive(false);
        GameController.gameControllerScript.playerPizzaHealth[3].SetActive(false);

        GameController.gameControllerScript.playerPizzaHealth[0].SetActive(true);
    }



    private void OnTriggerEnter2D(Collider2D collidingObject)
    {
        // if player collides with the remote
        if (collidingObject.CompareTag("Remote Control"))
        {
            // set player has remote flag
            playerHasRemote = true;

            //player remote setActive in player UI panel
            GameController.gameControllerScript.remoteControl.SetActive(true);

            // and destroy the remote
            Destroy(collidingObject.gameObject);

            // play remote collected sound
            int remoteCollectedSound = 5;

            AudioController.audioControllerScript.PlaySFX(remoteCollectedSound);
        }


        // if the player collides with the pizza slice
        if (collidingObject.CompareTag("Health"))
        {
            // increase player's health
            GameController.gameControllerScript.playerHealth += GameController.HEALTH_BOOST;

            // if player's health goes above maximum health
            if (GameController.gameControllerScript.playerHealth >= GameController.MAXIMUM_HEALTH)
            {
                // set player's health to maximum health
                GameController.gameControllerScript.playerHealth = GameController.MAXIMUM_HEALTH;

                DisplayPizzaHealth();

                // play health collected sound
                int healthCollectedSound = 6;

                AudioController.audioControllerScript.PlaySFX(healthCollectedSound);
            }

            // and destroy the pizza slice
            Destroy(collidingObject.gameObject);
        }

        // if the player has escaped
        if (collidingObject.CompareTag("Escape To Victory"))
        {
            // destroy the television
            Destroy(collidingObject.gameObject);

            // play escape to victory sound
            int escapeToVictorySound = 17;

            AudioController.audioControllerScript.PlaySFX(escapeToVictorySound);

            // and show escape to victory screen
            GameController.gameControllerScript.Victory();

            AudioController.audioControllerScript.PlayVictoryMusic();

        }

    }


} // end of class
