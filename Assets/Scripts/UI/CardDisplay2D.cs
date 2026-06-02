using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

// A Unity UI version of CardDisplay, used mainly for Deck Viewer
public class CardDisplay2D : MonoBehaviour
{

    [SerializeField] private CardData _cardData;
    public CardData CardData => _cardData;
    [SerializeField] private TMP_Text _cardName;
    [SerializeField] private TMP_Text _cardDescription;
    [SerializeField] private TMP_Text _cardCost;
    [SerializeField] private Image _cardIllustration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(CardData cardData)
    {
        _cardData = cardData;
        _cardName.text = cardData.Name;
        _cardCost.text = cardData.Cost.ToString();
        _cardDescription.text = cardData.Description;
        _cardIllustration.sprite = cardData.Illustration;
    }

    public void OnPress()
    {
        transform.DOScale(new Vector3(1.5f, 1.5f), 0.5f);
    }

    public void OnUnPress()
    {
        transform.DOScale(new Vector3(1.0f, 1.0f), 0.0f); 
    }
}
