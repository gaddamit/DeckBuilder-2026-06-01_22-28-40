using UnityEngine;
using DG.Tweening;

public class LoadingPopup : MonoBehaviour
{
    [SerializeField] private GameObject _spinner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Spins the spinner indefinitely
        _spinner.transform.DOLocalRotate(new Vector3(0f, 0f, -360f), 1.5f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)               
            .SetLoops(-1, LoopType.Incremental); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
