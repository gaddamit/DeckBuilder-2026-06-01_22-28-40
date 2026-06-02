using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections.Generic;
public class DeckViewer : MonoBehaviour
{
    [SerializeField] private GameObject _deckPrefab;
    [SerializeField] private GameObject _loadingPopup;
    [SerializeField] private GameObject _scrollViewContent;
    [SerializeField] private List<CardData> _cardDataList = new List<CardData>();
    [SerializeField] private CloudStoreManager _cloudStoreManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadDecks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Starts an API call that fetches User Profile
    public void LoadDecks()
    {
        _loadingPopup.SetActive(true);
        StartCoroutine(_cloudStoreManager.GetBinData((_userProfile) => 
        {
            UserProfile userProfile = _userProfile;
            if(userProfile != null)
            {
                Debug.Log(userProfile.user_id);
                foreach(DeckContainer deckContainer in userProfile.decks)
                {
                    GameObject newDeck = Instantiate(_deckPrefab, _scrollViewContent.transform);
                    for(int i = 0; i < deckContainer.cards.Count; i++)
                    {
                        GameObject cardObject = newDeck.transform.Find("Card" + (i+1)).gameObject;
                        CardDisplay2D cardDisplay = cardObject.GetComponent<CardDisplay2D>();

                        int card = int.Parse(deckContainer.cards[i]);
                        cardDisplay.Initialize(_cardDataList[card-1]);
                    }
                }

                DOVirtual.DelayedCall(3, () =>
                {
                    _loadingPopup.SetActive(false);
                });
            }
        }));
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void BuildDeck()
    {
        SceneManager.LoadScene("DeckBuilder");
    }
}
