using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public BrickGenerator brickGenerator;
    public int remainingBricks;
    public bool gameStarted = false;

    public int lives = 3; // Nombre de vies du joueur
    public GameObject ballPrefab; // Prefab de la balle
    public GameObject paddlePrefab; // Prefab de la barre (paddle)
    public Transform leftBarrier; // Barrière gauche
    public Transform rightBarrier; // Barrière droite
    public float zOffset = 1.0f; // Offset sur l'axe Z

    public float gameDuration = 300f; // Durée du jeu en secondes
    private float timeRemaining;
    public TextMeshProUGUI timerText; // Texte pour afficher le timer
    public TextMeshProUGUI livesText; // Texte pour afficher les vies restantes

    private BallController currentBall;
    private GameObject paddleInstance; // Instance de la barre
    private GameObject playerNameInputField; // Instance du champ de saisie du nom

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Charger les prefabs nécessaires
            if (ballPrefab == null)
            {
                ballPrefab = Resources.Load<GameObject>("BallPrefab"); // Assurez-vous que le prefab est dans un dossier Resources
            }

            if (paddlePrefab == null)
            {
                paddlePrefab = Resources.Load<GameObject>("PaddlePrefab"); // Assurez-vous que le prefab est dans un dossier Resources
            }

            // Assurer que les barrières sont bien assignées
            if (leftBarrier == null || rightBarrier == null)
            {
                leftBarrier = GameObject.Find("LeftBarrier").transform;
                rightBarrier = GameObject.Find("RightBarrier").transform;
                DontDestroyOnLoad(leftBarrier);
                DontDestroyOnLoad(rightBarrier);
            }

            // Assigner et conserver le champ de saisie du nom
            playerNameInputField = GameObject.Find("PlayerNameInputField");
            if (playerNameInputField != null)
            {
                DontDestroyOnLoad(playerNameInputField);
            }
            else
            {
                Debug.LogError("PlayerNameInputField GameObject not found.");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeGame();
    }

    void Update()
    {
        if (gameStarted)
        {
            UpdateTimer();

            // Lancer la balle si le joueur appuie sur la barre d'espace
            if (currentBall != null && Input.GetKeyDown(KeyCode.Space))
            {
                currentBall.LaunchBall();
                currentBall = null; // Réinitialiser la référence à la balle actuelle
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!string.IsNullOrEmpty(PlayerNameInput.playerName))
            {
                PlayerNameInput.HideInputField(); // Masquer le champ de saisie du nom
                StartNewGame();
            }
            else
            {
                Debug.Log("Please enter a player name to start the game.");
            }
        }
    }

    void UpdateTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
            if (timeRemaining <= 0)
            {
                EndGame();
            }
        }
    }

    void UpdateTimerText()
    {
        if (timerText == null)
        {
            timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
            if (timerText == null)
            {
                Debug.LogError("TimerText not found in the scene.");
                return;
            }
        }

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void AddScore(int value)
    {
        if (gameStarted && ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(value);
        }
    }

    public void EndGame()
    {
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.SaveScore();
        }
        gameStarted = false;
        GoToHighScores();
    }

    public void BrickDestroyed()
    {
        remainingBricks--;
        if (remainingBricks <= 0)
        {
            RegenerateLevel();
        }
    }

    void RegenerateLevel()
    {
        if (brickGenerator != null)
        {
            brickGenerator.GenerateBricks();
            remainingBricks = FindObjectsOfType<Brick>().Length;
        }
        else
        {
            Debug.LogError("BrickGenerator is not assigned in GameManager.");
        }
    }

    public void ResetGame()
    {
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.ResetScore();
        }

        if (brickGenerator != null)
        {
            if (brickGenerator.bricksContainer == null)
            {
                brickGenerator.bricksContainer = new GameObject("BricksContainer").transform;
            }

            brickGenerator.GenerateBricks();
            remainingBricks = FindObjectsOfType<Brick>().Length;
        }
        else
        {
            Debug.LogError("BrickGenerator is not assigned in GameManager.");
        }

        PlayerNameInput.ClearPlayerName();
        PlayerNameInput.ShowInputField();

        // Réinitialiser le timer
        timeRemaining = gameDuration;

        // Réinitialiser les vies
        lives = 3;
        UpdateLivesText();

        // Initialiser timerText
        InitializeTimerText();
        UpdateTimerText();

        // Réinitialiser ou instancier le paddle
        ResetOrInstantiatePaddle();
    }

    void InitializeGame()
    {
        if (brickGenerator == null)
        {
            Debug.LogError("BrickGenerator is not assigned in GameManager.");
            return;
        }
        ResetGame();
    }

    public void StartNewGame()
    {
        ResetGame();
        gameStarted = true;
        SpawnBall();
    }

    public void SpawnBall()
    {
        // Réassigner les références si elles sont nulles
        if (ballPrefab == null)
        {
            ballPrefab = Resources.Load<GameObject>("BallPrefab"); // Assurez-vous que le prefab est dans un dossier Resources
        }

        if (paddleInstance == null)
        {
            ResetOrInstantiatePaddle();
        }

        Vector3 spawnPosition = paddleInstance.transform.position + new Vector3(0, 0, zOffset);
        GameObject newBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        currentBall = newBall.GetComponent<BallController>();
        if (currentBall != null)
        {
            currentBall.SetPaddleTransform(paddleInstance.transform); // Assigner la référence de la barre à la balle
        }
        else
        {
            Debug.LogError("BallController component not found on the spawned ball.");
        }
    }

    void ResetOrInstantiatePaddle()
    {
        if (paddleInstance != null)
        {
            Destroy(paddleInstance);
        }

        paddleInstance = Instantiate(paddlePrefab);
        DontDestroyOnLoad(paddleInstance);
    }

    public void LaunchNewBall()
    {
        // Réassigner les références si elles sont nulles
        if (ballPrefab == null)
        {
            ballPrefab = Resources.Load<GameObject>("BallPrefab"); // Assurez-vous que le prefab est dans un dossier Resources
        }

        if (paddleInstance == null)
        {
            ResetOrInstantiatePaddle();
        }

        Vector3 spawnPosition = paddleInstance.transform.position + new Vector3(0, 0, zOffset);
        GameObject newBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        BallController ballController = newBall.GetComponent<BallController>();
        if (ballController != null)
        {
            ballController.SetPaddleTransform(paddleInstance.transform); // Assigner la référence de la barre à la balle
            ballController.LaunchBall();
        }
        else
        {
            Debug.LogError("BallController component not found on the spawned ball.");
        }
    }

    void GoToHighScores()
    {
        SceneManager.LoadScene("ScoreScene");
    }

    void InitializeTimerText()
    {
        if (timerText == null)
        {
            timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        }
    }

    void UpdateLivesText()
    {
        if (livesText == null)
        {
            livesText = GameObject.Find("LivesText").GetComponent<TextMeshProUGUI>();
            if (livesText == null)
            {
                Debug.LogError("LivesText not found in the scene.");
                return;
            }
        }

        livesText.text = "Lives: " + lives.ToString();
    }

    public void RespawnBall()
    {
        if (lives > 0)
        {
            lives--;
            UpdateLivesText();
            SpawnBall();
        }
        else
        {
            EndGame();
        }
    }
}
