using UnityEngine;
using TMPro;

public class FFCCounterUpdater : MonoBehaviour
{
    private TextMeshProUGUI _counterUIText;
   
    void Start()
    {
        _counterUIText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _counterUIText.text = "x" + GameInstance.Singleton.currentCounter;
    }
}
