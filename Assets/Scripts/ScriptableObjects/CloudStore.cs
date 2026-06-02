using UnityEngine;

[CreateAssetMenu(fileName = "CloudStore", menuName = "Config/CloudStore")]
public class CloudStore : ScriptableObject
{
    [SerializeField] private string _masterKey;
    public string MasterKey => _masterKey;
    [SerializeField] private string _binID;
    public string BinID => _binID;
}
