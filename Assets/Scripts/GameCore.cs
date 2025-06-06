using UnityEngine;

public class GameCore : MonoBehaviour
{
    [SerializeField]
    public GameObject levelHandler;
    private LevelHandler _levelHandlerRef;
    
    private void Start()
    {
        _levelHandlerRef = levelHandler.GetComponent<LevelHandler>();
    }

    void Update()
    {
        
    }
}
