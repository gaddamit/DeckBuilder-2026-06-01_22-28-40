using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class DeckBuilder : MonoBehaviour
{
    private int _cardDataSize = 15;
    private const int _deckSize = 8;
    private int _topCard = 0;

    [SerializeField] private CardDisplay _cardPrefab;
    [SerializeField] private List<CardData> _cardDataList = new List<CardData>();
    [SerializeField] private List<CardDisplay2D> _cardsOnHand = new List<CardDisplay2D>();
    public List<CardDisplay> _cardDisplay = new List<CardDisplay>();
    [SerializeField] private GameObject _cardDeck;
    [SerializeField] private Button _saveDeck;
    [SerializeField] private GameObject _loadingPopup;

    [SerializeField] private CloudStoreManager _cloudStoreManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Shuffles the card list
        _cardDataList = _cardDataList.OrderBy(x => Random.value).ToList();
        _cardDataSize = _cardDataList.Count;

        CreateDeck();
        InitializeCardsOnHand();
        EnableTopCard();
    }

    // Creates the down facing 3D cards on the scene
    void CreateDeck()
    {
        for(int i=0; i < _cardDataSize; i++)
        {
            CardDisplay newCard = Instantiate(_cardPrefab, this.transform);
            _cardDisplay.Add(newCard);
            newCard.Initialize(_cardDataList[i]);
            if(i < _deckSize)
            {
                newCard.SetTarget(_cardDeck.GetComponent<RectTransform>());
                newCard.OnActionComplete(PlaceCardOnDeck);
            }
        }
    }

    void InitializeCardsOnHand()
    {
        for(int i=0; i < _deckSize; i++)
        {
            CardDisplay2D card2D = _cardsOnHand[i];
            card2D.Initialize(_cardDataList[i]);
        }
    }

    void EnableTopCard()
    {
        if(_topCard < _deckSize)
        {
            _cardDisplay[_topCard].EnableCard();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlaceCardOnDeck()
    {
        _cardDisplay[_topCard].gameObject.SetActive(false);
        _cardsOnHand[_topCard].gameObject.SetActive(true);
        _topCard++;
        EnableTopCard();

        if(_topCard >= _deckSize)
        {
            _saveDeck.interactable = true;
        }
    }

    public void SaveDeck()
    {
        List<string> elements = new List<string>();
        foreach(CardDisplay2D card in _cardsOnHand)
        {
            elements.Add(card.CardData.ID.ToString());
        }
       
        _loadingPopup.SetActive(true); 
        StartCoroutine(_cloudStoreManager.UpdateBinData(elements, (isSuccess) => 
        {
            if (isSuccess)
            {
                Debug.Log("Deck created");
                DOVirtual.DelayedCall(3, () =>
                {
                    SceneManager.LoadScene("DeckViewer");
                });
            }
            else
            {
                Debug.LogError("Error deck creation.");
            }
        }));
    }
}
