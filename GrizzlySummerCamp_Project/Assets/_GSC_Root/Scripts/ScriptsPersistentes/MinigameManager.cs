using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

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
    #endregion

    #region UI (Scene Refs)
    [Header("UI (Scene Refs)")]
    [SerializeField] private GameObject endMinigameUI;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Image insigniaImage;

    [Header("Insignia Sprites")]
    [SerializeField] private Sprite bronzeSprite;
    [SerializeField] private Sprite silverSprite;
    [SerializeField] private Sprite goldSprite;
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
            EndMinigame();
        }
    }

    public void StartMinigame()
    {
        currentTime = minigameDuration;
        isRunning = true;
        hasWonMinigame = true;

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
        if (messageText)
        {
            if (minigameName == "SCN_MContrareloj")
            {
                messageText.text = hasWonMinigame
                    ? "Well done"
                    : "Try again :(";
            }
            else
            {
                messageText.text = GetMessageByInsignia(insignia);
            }
        }

        if (minigameName == "SCN_MContrareloj")
        {
            if (finalScoreText)
                finalScoreText.gameObject.SetActive(false);

            if (insigniaImage)
                insigniaImage.enabled = false;

            return;
        }

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
        switch (insignia)
        {
            case 3: return goldSprite;
            case 2: return silverSprite;
            case 1: return bronzeSprite;
            default: return null;
        }
    }

    int GetInsigniaByScore(int score)
    {
        if (score >= 200) return 3;
        if (score >= 150) return 2;
        if (score >= 50) return 1;
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