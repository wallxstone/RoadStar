using Google.Play.Review;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;


public class ScoreManager : MonoBehaviour
{
    private ReviewManager _reviewManager;
    private Label scoreText;
    private Label HighScore;
    private VisualElement Tap;
    private VisualElement MenuScreen;
    private VisualElement root;

    private Button btnrate;

    private float levelScore = 0;
    public float lastLevelScore;
    private int _iscoreMultiplier = 3;
    private float levelTimeEnd = 0;
    private AudioManager _audioManager;

    public float LevelScore
    {
        get => levelScore;
        set => levelScore = value;
    }
    public float LevelTimeEnd
    {
        get => levelTimeEnd;
    }
    private void OnEnable()
    {
        _reviewManager = new ReviewManager();
        root = GetComponent<UIDocument>().rootVisualElement;
        scoreText = root.Q<Label>("GameScore");
        HighScore = root.Q<Label>("HighScore");
        Tap = root.Q<VisualElement>("Tap");
        MenuScreen = root.Q<VisualElement>("MenuScreen");
        btnrate = root.Q<Button>("Rate");

        MenuScreen.style.display = DisplayStyle.Flex;
        scoreText.style.display = DisplayStyle.None;
        scoreText.text = "0";
        HighScore.text = $"High Score: {Mathf.FloorToInt(PlayerPrefs.GetFloat("HighScore", 0))}";
        StartCoroutine(Transition());

        btnrate.RegisterCallback<ClickEvent>(evt =>
        {
            StartCoroutine(InAppreview());
        });
        
    }

    /**
     * Enumerator for in-app review process.
     * 
     * This method will request review flow from Google Play and launch it
     * if request is successful. If not, it will log the error to the console.
     */
    IEnumerator InAppreview()
    {
        // Request review flow from Google Play
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        
        // Check if operation was successful
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // If not, log the error
            Debug.Log(requestFlowOperation.Error.ToString());
            yield break;
        }

        // If successful, get the ReviewInfo object
        PlayReviewInfo _playReviewInfo = requestFlowOperation.GetResult();

        // Launch the review flow
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;

        // Clean up
        _playReviewInfo = null;
    }


    IEnumerator Transition()
    {
        yield return new WaitForSeconds(0.1f);
        Tap.ToggleInClassList("Game-tapLabel-Transition--end");
        Tap.RegisterCallback<TransitionEndEvent>((evt) =>
        {
            Tap.ToggleInClassList("Game-tapLabel-Transition--end");
        });
    }

    private void Update()
    {
        if(_audioManager == null)
        {
            _audioManager = FindObjectOfType<AudioManager>();
        }
        if(GameManager.instance.isGameStarted)
        {
            btnrate.style.display = DisplayStyle.None;
            scoreText.style.display = DisplayStyle.Flex;
            MenuScreen.style.display = DisplayStyle.None;
            levelScore += _iscoreMultiplier * Time.deltaTime;

            scoreText.text = $"{Mathf.FloorToInt(levelScore)}";
            UpdateHighScore();
        }
    }

    public void AddScore(int value)
    {
        _audioManager.PlayNotify();
        levelScore += value;
    }

    public void SubtractScore(int value)
    {
        _audioManager.PlayNotify();
        levelScore -= value;
        if (levelScore < 0)
        {
            levelScore = 0;
        }
    }
    private void UpdateHighScore()
    {
        lastLevelScore = levelScore;
        if (levelScore > PlayerPrefs.GetFloat("HighScore",0))
        {
            PlayerPrefs.SetFloat("HighScore", levelScore);
        }
    }

    
}
