using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.Collections.Generic;

public class MinigameManager : MonoBehaviour
{
    //GENERICO SIRVE PARA TODOS LOS MINIJUEGOS, debe mantenerse en todas las escenas pero no tener dontdestroyonload (se encarga PersistentRoot)
    #region Singleton y variables generales
    public static MinigameManager Instance;

    [Header("Minigame Settings")]
    [SerializeField] public string minigameName; //Se asigna solo
    public string MinigameName => minigameName;

    [SerializeField] private float minigameDuration = 90f;

    private float currentTime;
    private bool isRunning = false;
    private bool hasWonMinigame = false;

    private IMinigamePlayerMovement playerMovement;

    public static event Action OnMinigameStarted;
    public static event Action OnMinigameEnded;

    [Header("Insignias")]
    [SerializeField] private InsigniaDatabase insigniaDatabase;

    private MinigameInsigniaData currentInsigniaData;
    #endregion

    #region UI (Scene Refs)
    [Header("UI (Scene Refs)")]
    [SerializeField] private GameObject endMinigameUI;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Image insigniaImage;

    [Header("Contrareloj")]
    [SerializeField] private Sprite estrellaSprite;
    #endregion

    #region Awake y On
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SCN_Menu" || scene.name == "SCN_HUB")
        {
            Debug.Log($"[MinigamManager] Escena '{scene.name}' ignorada para referencias.");
            return;
        }

        minigameName = scene.name;
        Debug.Log($"[MinigameManager] MinigameName asignado: {minigameName}");

        hasWonMinigame = false;
        isRunning = false;

        if (minigameName != "SCN_MContrareloj")
        {
            currentInsigniaData = insigniaDatabase != null
                ? insigniaDatabase.GetByScene(minigameName)
                : null;

            if (currentInsigniaData == null)
            {
                Debug.LogWarning($"[MinigameManager] No hay insigniadata para la escena '{minigameName}'");
            }
        }
        else
        {
            currentInsigniaData = null;
        }

        if (scene.name == "SCN_MBasura") 
        {
            FindAnyObjectByType<TrashSpawner>()?.SpawnAllTrash();
        }

        FindPlayerMovementInScene();
        FindUIReferences();
    }
    #endregion

    #region Find in scene
    void FindPlayerMovementInScene()
    {
        playerMovement = FindObjectsByType<MonoBehaviour>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
            )
            .OfType<IMinigamePlayerMovement>()
            .FirstOrDefault();

        if (playerMovement != null)
            playerMovement.SetCanMove(false);
    }

    void FindUIReferences()
    {
        endMinigameUI = FindDeepChildInScene("EndMinigameUI")?.gameObject;
        finalScoreText = FindDeepChildInScene("FinalScoreText")?.GetComponent<TMP_Text>();
        messageText = FindDeepChildInScene("MessageText")?.GetComponent<TMP_Text>();
        insigniaImage = FindDeepChildInScene("InsigniaImage")?.GetComponent<Image>();

        Debug.Log($"[MinigameManager] UI references - EndUI: {endMinigameUI}, ScoreText: {finalScoreText}, MessageText: {messageText}, InsigniaImage: {insigniaImage}");
    }

    Transform FindDeepChildInScene(string name)
    {
        foreach (GameObject root in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            Transform result = FindDeepChild(root.transform, name);
            if (result != null)
                return result;
        }
        return null;
    }

    Transform FindDeepChild(Transform parent, string name)
    {
        if (parent.name == name) return parent;

        foreach (Transform child in parent)
        {
            Transform result = FindDeepChild(child, name);
            if (result != null)
                return result;
        }
        return null;
    }
    #endregion

    #region Update y Minijuego
    private void Update()
    {
        if (!isRunning) return;

        currentTime -=Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            EndMinigame(false);
        }
    }

    public void StartMinigame()
    {
        currentTime = minigameDuration;
        isRunning = true;
        hasWonMinigame = true; //Por defecto asumimos que es victoria

        SetPlayerMovement(true);
        ScoreManager.Instance?.ResetScore();

        OnMinigameStarted?.Invoke();
    }

    public void EndMinigame(bool won)
    {
        hasWonMinigame = won;
        EndMinigame();
    }

    public void EndMinigame()
    {
        isRunning = false;
        SetPlayerMovement(false);

        OnMinigameEnded?.Invoke();

        int puntos = ScoreManager.Instance != null ? ScoreManager.Instance.GetTotalPoints() : 0;
        int insignia = GetInsigniaByScore(puntos);

        if (minigameName != "SCN_MContrareloj")
        {
            InsigniaManager.Instance?.GuardarInsignia(minigameName, insignia);
        }

        UpdateUI(puntos, insignia);
    }

    public void SetPlayerMovement(bool canMove)
    {
        if (playerMovement != null) playerMovement.SetCanMove(canMove);
    }
    #endregion

    #region UI Updates
    void UpdateUI(int puntos, int insignia)
    {
        if (endMinigameUI)
            endMinigameUI.SetActive(true);

        UpdateEndGameUI(puntos, insignia);
    }

    void UpdateEndGameUI(int puntos, int insignia)
    {
        if (minigameName == "SCN_MContrareloj")
        {
            //Solo para contrareloj

            bool estrellaGanada = hasWonMinigame;

            if (messageText)
            {
                messageText.text = estrellaGanada ? "Start earned!" : "Try again :(";
            }

            if (finalScoreText)
                finalScoreText.gameObject.SetActive(false);

            if (insigniaImage)
            {
                insigniaImage.sprite = estrellaSprite;
                insigniaImage.enabled = estrellaGanada;
            }

            return;
        }

        //Demas minijuegos

        if (messageText)
            messageText.text = GetMessageByInsignia(insignia);

        if (finalScoreText)
            finalScoreText.text = $"You got {puntos} points!";

        if (insigniaImage)
        {
            insigniaImage.sprite = GetSpriteByInsignia(insignia);
            insigniaImage.enabled = insignia > 0;
        }
    }
    #endregion

    #region Score/Insignia
    string GetMessageByInsignia(int insignia)
    {
        switch (insignia)
        {
            case 3: return "Wow!";
            case 2: return "Great job!";
            case 1: return "Nice try;";
            default: return "Better luck next time!";
        }
    }

    Sprite GetSpriteByInsignia(int insignia)
    {
        if (currentInsigniaData == null) return null;

        return currentInsigniaData.GetSpriteByInsignia(insignia);
    }

    int GetInsigniaByScore(int score)
    {
        if (score >= 300) return 3;
        if (score >= 200) return 2;
        if (score >= 100) return 1;
        return 0;
    }
    #endregion

    #region Getters
    public float GetCurrentTime()
    {
        return currentTime;
    }

    public float GetMaxTime()
    {
        return minigameDuration;
    }
    #endregion
}