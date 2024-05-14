using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    VisualElement veroot;
    VisualElement pnlMain;
    public static GameManager instance;
    public ScoreManager scoreManager;
    [SerializeField] AdsInitializer ads;
    [SerializeField] private GameObject PlatformSpawn;
    [HideInInspector]
    public bool isGameStarted = false;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void OnEnable()
    {
        veroot = GetComponent<UIDocument>().rootVisualElement;
        pnlMain = veroot.Q<VisualElement>("Main");
        pnlMain.RegisterCallback<ClickEvent>(evt =>
        {
            isGameStarted = true;
        });
    }

    void Update()
    {
        if(isGameStarted)
        {
            GameStart();
        }
    }
    public void GameStart()
    {
        isGameStarted = true;
        PlatformSpawn.SetActive(true);
    }

    public void GameOver()
    {
        isGameStarted = false;
        PlatformSpawn.SetActive(false);
        ads.GetComponent<RewardedAdsButton>().ShowAd();
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RevivePlayer()
    {
        Debug.Log("Ad watched");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        scoreManager.LevelScore = scoreManager.lastLevelScore;
    }
}
