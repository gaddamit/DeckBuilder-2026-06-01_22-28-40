using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

// A 3D version of the Cards
public class CardDisplay : MonoBehaviour
{
    private bool isFlipped = false;
    [SerializeField] private CardData _cardData;

    [SerializeField] private TMP_Text _cardName;
    [SerializeField] private TMP_Text _cardDescription;
    [SerializeField] private TMP_Text _cardCost;
    [SerializeField] private MeshRenderer _cardBacking;
    [SerializeField] private MeshRenderer _cardFront;
    [SerializeField] private MeshRenderer _cardIllustration;

    private RectTransform _targetCardOnDeck;
    private Action OnCompleteCallbackAction;
    void Awake()
    {
        
    }

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
        _cardName.text = cardData.Name;
        _cardCost.text = cardData.Cost.ToString();
        _cardDescription.text = cardData.Description;

        Material mat = _cardIllustration.material;
        mat.SetTexture("_BaseMap", cardData.Illustration.texture);
    }

    public void SetTarget(RectTransform cardDeck)
    {
        _targetCardOnDeck = cardDeck;
    }

    public void OnActionComplete(Action OnCompleteCallback)
    {
        OnCompleteCallbackAction = OnCompleteCallback;
    }

    public void EnableCard()
    {
        BoxCollider meshCollider = GetComponent<BoxCollider>();
        meshCollider.enabled = true;
    }

    public void DisableCard()
    {
        BoxCollider meshCollider = GetComponent<BoxCollider>();
        meshCollider.enabled = false;
    }

    private void OnMouseDown()
    {
        DisableCard();
        if(!isFlipped)
        {   
            isFlipped = true;
            transform.DOLocalMove(new Vector3(0f, 0f, -2f), 1f).OnComplete(FlipCard);
        }
        else
        {
            Vector3 uiScreenPos = _targetCardOnDeck.position;
            uiScreenPos.z = 5f;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(uiScreenPos);
            transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 1f);   
            transform.DOMove(worldPos, 1f).OnComplete(OnCompleteCallback);     
        }
    }

    private void FlipCard()
    {
        transform.DORotate(new Vector3(0f, 180f, 0f), 2f).OnComplete(EnableCard);
    }

    private void OnCompleteCallback()
    {
        OnCompleteCallbackAction?.Invoke();
    }
}
