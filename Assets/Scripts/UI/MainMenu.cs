using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _newUser;
    [SerializeField] private Button _continue;
    [SerializeField] private GameObject _loadingPopup;
    [SerializeField] private CloudStoreManager _cloudStoreManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _continue.interactable = false;
        _continue.interactable = CheckExistingPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool CheckExistingPlayer()
    {
        return UserSaveData.HasPreviousUser();
    }

    public void CreateUser()
    {
        bool success = !string.IsNullOrEmpty(UserSaveData.CreateUser());
        if(success)
        {
            _loadingPopup.SetActive(true); 
            StartCoroutine(_cloudStoreManager.CreateBinData(UserSaveData.UserID, (isSuccess) => 
            {
                if (isSuccess)
                {
                    Debug.Log("User created");
                    DOVirtual.DelayedCall(3, () =>
                    {
                        SceneManager.LoadScene("DeckBuilder");
                    });
                }
                else
                {
                    Debug.LogError("Error user creation.");
                }
            }));
        }
    }

    public void ContinueUser()
    {
        UserSaveData.LoadPreviousUser();
        SceneManager.LoadScene("DeckBuilder");
    }
}
