using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.UIElements;

public class playservices : MonoBehaviour
{
    private Label scoreText;
    private Button btnsign;
    private VisualElement root;

   
    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        scoreText = root.Q<Label>("Sign");
        btnsign = root.Q<Button>("Signin");
        scoreText.text = "Hello";
        
        btnsign.clicked += () =>
            PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication); 
    }
    void Start()
    {
        SignIN();
    }

    void SignIN()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services

            scoreText.text = "Signin: Succeed";
        }
        else
        {
            scoreText.text = "Signin: Failed";
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
        }
    }

}
