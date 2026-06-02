using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardData", menuName = "Cards/CardData")]
public class CardData : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private int _cost;
    [SerializeField] private string _stats;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _illustration;

    public int ID => _id;
    public string Name => _name;
    public int Cost => _cost;
    public string Stats => _stats;
    public string Description => _description;
    public Sprite Illustration => _illustration;
}
