
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//
// v2025.10.09
//

public class GameController : MonoBehaviour
{
    // make game controller script accessible from other scripts
    public static GameController gameControllerScript;


    // array for doors
    public GameObject[] doors;

    // array for lives
    public GameObject[] playerLives;

    // array for health
    public GameObject[] playerPizzaHealth;

    public GameObject remoteControl;

    // array for victory
    public GameObject[] escapeToVictory;

    // reference to background panel
    public GameObject backgroundPanel;

    // reference to title screen screen
    public GameObject titleScreen;

    // reference to options screen
    public GameObject optionsScreen;

    // reference to the pawz screen
    public GameObject pawzScreen;

    // reference to the game over screen
    public GameObject gameOverScreen;

    // reference to the victory screen
    public GameObject victoryScreen;

    // reference to the player ui panel
    public GameObject playerUiPanel;

    // reference to quit game background
    public GameObject quitGameBackground;


    // reference to displayed 'Score Text' in the player ui panel
    public TMP_Text scoreText;

    // game time text
    public TMP_Text gameTimeText;

    // reference to 'Score integers' assigned to each enemy
    public int score;

    // reference to 'Lives integer' in the player ui panel
    public int lives;

    // player health
    public int playerHealth;

    // game time left
    // 6 minutes or 360 seconds
    [HideInInspector] public float gameTime;



    // current room the player is in
    public int room;

    public const int BOSS_ROOM = 6;

    // boss sprite
    // array reference for escape to victory
    public const int THE_BOSS = 0;

    // EXIT sprite
    // array reference for escape to victory
    public const int THE_EXIT = 1;

    // television sprite
    // array reference for escape to victory
    public const int THE_TELEVISION = 2;

    // Health pickup
    public const int HEALTH_BOOST = 5;

    // maximum player health
    public const int MAXIMUM_HEALTH = 100;





    // get a reference to the audio source component
    [HideInInspector] public AudioSource audioPlayer;

    // are we playing the game
    public bool gamePawzed;

    // is the game in play
    public bool inPlay;

    // is the game over
    public bool gameOver;

    // level completed
    public bool levelComplete;

    // if we are starting the level
    public bool levelStart;

    // if we are entering a room
    public bool hasEnterdRoom;

    public float coolDownTimer = 3f;

    public float enteredRoomTimer;




    private void Awake()
    {
        gameControllerScript = this;
    }


    private void Start()
    {
        // set reference to the audio source component
        audioPlayer = GetComponent<AudioSource>();


        InitialiseLevelStart();

        // load the title screen
        TitleScreen();
    }


    private void InitialiseLevelStart()
    {
        // set starting room
        room = 0;

        // set game play flags
        gameOver = true;

        inPlay = false;

        // we are starting the level
        levelStart = true;
    }



    private void Update()
    {
        // if player has entered a room
        if (!gameOver && hasEnterdRoom)
        {
            PlayerEnteredRoom();
        }


        // if the game is not over
        // and the game is not pawzed
        // and we are not waiting for the player to enter a room
        if (!gameOver && !gamePawzed && !hasEnterdRoom)
        {
            // show the time
            DisplayGameTime();
        }


        // if the game is in play
        if (inPlay)
        {
            // and the game is not already pawzed
            if (!gamePawzed)
            {
                // and the player has pressed the escape key
                if (Input.GetKeyDown(KeyCode.P))
                {
                    PawzGame();
                }
            }
        }
    }


    private void PlayerEnteredRoom()
    {
        // countdown timer
        enteredRoomTimer -= Time.deltaTime;

        // if the time left is less than or equal to zero 
        if (enteredRoomTimer <= 0)
        {
            // set timer to zero
            enteredRoomTimer = 0;

            // indicate we are no longer waiting for the player
            hasEnterdRoom = false;

            // spawn the enemy
            CameraController.cameraControllerScript.SpawnEnemy();
        }
    }


    public void OpenDoor(int doorToOpen)
    {
        // open door to next room
        doors[doorToOpen].SetActive(false);
    }


    public void CloseDoor(int doorToClose)
    {
        // close the door from the previous room
        doors[doorToClose].SetActive(true);

        // player has entered room
        enteredRoomTimer = coolDownTimer;

        hasEnterdRoom = true;
    }


    private void PawzGame()
    {
        // pawz the game
        gamePawzed = true;

        // activate the background
        backgroundPanel.SetActive(true);

        // load the pawz screen
        pawzScreen.SetActive(true);

        // and freeze game play
        Time.timeScale = 0f;
    }


    public void ResumeGame()
    {
        // un-pawz the game
        gamePawzed = false;

        // deactivate the background
        backgroundPanel.SetActive(false);

        // close the pawz screen
        pawzScreen.SetActive(false);

        // and un-freeze game play
        Time.timeScale = 1f;
    }

    
    public void RestartGame()
    {
        // close the game over screen
        gameOverScreen.SetActive(false);

        // and un-freeze game play
        Time.timeScale = 1f;

        // restart the game
        SceneManager.LoadScene(0);
    }


    public void TitleScreen()
    {
        // activate the background
        backgroundPanel.SetActive(true);

        // close the victory screen
        victoryScreen.SetActive(false);

        // load the title screen
        titleScreen.SetActive(true);

        // play title music
        AudioController.audioControllerScript.PlayTitleMusic();
    }


    // if the play button is pressed
    public void PlayButton()
    {
        // hide the background panel
        backgroundPanel.SetActive(false);

        // close the main menu
        titleScreen.SetActive(false);

        // close the game over screen
        gameOverScreen.SetActive(false);

        // display the player ui panel
        playerUiPanel.SetActive(true);

        // play title music
        AudioController.audioControllerScript.PlayLevelMusic();

        Initialise();
    }


    private void Initialise()
    {
        // player score
        score = 0;

        // player lives
        lives = 3;

        // player health
        playerHealth = 100;

        //set remote control icon in player UI panel to false
        remoteControl.SetActive(false);

        // game time left
        // 3 minutes or 180 seconds
        gameTime = 180f;

        SetTimeFormat();

        // set the game in play flags and enter room timer
        inPlay = true;

        gameOver = false;

        levelStart = false;

        levelComplete = false;

        enteredRoomTimer = coolDownTimer;

        hasEnterdRoom = true;
    }


    // if the options button is pressed
    public void OptionsButton()
    {
        // if the game is pawzed
        if (gamePawzed)
        {
            // then close the pawz screen
            pawzScreen.SetActive(false);
        }

        // otherwise
        else
        {
            // close the title screen
            titleScreen.SetActive(false);
        }

        // if the game is over, close the game over screen
        gameOverScreen.SetActive(false);

        // open the options screen
        optionsScreen.SetActive(true);
    }


    // if we are closing the options screen 
    public void CloseOptions()
    {
        // if we are starting the level
        if (levelStart)
        {
            // close the options screen
            optionsScreen.SetActive(false);

            // and open the title screen
            titleScreen.SetActive(true);

            // play title music
            //AudioController.audioControllerScript.PlayTitleMusic();
        }


        // if we are starting the level
        if (levelComplete)
        {
            // close the options screen
            optionsScreen.SetActive(false);

            // close the game over screen
            //gameOverScreen.SetActive(false);

            // and open the title screen
            titleScreen.SetActive(true);

            // play title music
            //AudioController.audioControllerScript.PlayTitleMusic();
        }


        // if the game is pawzed
        if (gamePawzed)
        {
            // close the options screen
            optionsScreen.SetActive(false);

            // load the pawz screen
            pawzScreen.SetActive(true);
        }


        // if the game is over and we are not starting the level
        if (gameOver && !levelStart && !levelComplete)
        {
            // close the options screen
            optionsScreen.SetActive(false);

            // and open the game over screen
            gameOverScreen.SetActive(true);
        }
    }


    public void Victory()
    {
        // game over
        gameOver = true;

        levelComplete = true;

        inPlay = false;

        // activate the background
        backgroundPanel.SetActive(true);

        // load the victory screen
        victoryScreen.SetActive(true);

        // and freeze game play
        Time.timeScale = 0f;
    }


    public void GameOver()
    {
        // game over
        gameOver = true;

        inPlay = false;

        // activate the background
        backgroundPanel.SetActive(true);

        // open the game over screen
        gameOverScreen.SetActive(true);

        // and freeze game play
        Time.timeScale = 0f;
    }


    public void DisplayPlayerScore()
    {
        scoreText.text = score.ToString("000000");
    }


    public void DisplayGameTime()
    {
        // countdown the time
        gameTime -= Time.deltaTime;

        SetTimeFormat();

        // if the player is out of time
        if (gameTime < 0)
        {
            gameTime = 0f;

            GameOver();
        }
    }


    private void SetTimeFormat()
    {
        // convert time to minutes and seconds
        int minutes = ((int)gameTime / 60);

        int seconds = ((int)gameTime % 60);

        // format and display the remaining time
        gameTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    // if the quit button is pressed
    public void QuitGame()
    {
        // quit the game
        quitGameBackground.SetActive(true);

        Application.Quit();
    }


} // end of class
