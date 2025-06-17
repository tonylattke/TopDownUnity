using UnityEngine;

public class FFCEnemy : MonoBehaviour
{
    [SerializeField] protected GameManagerSo gameManager;
    
    [SerializeField] private GameObject _markedTarget;
    private bool _isActiveTarget = false;
    public bool IsActiveTarget { get { return _isActiveTarget; } set { _isActiveTarget = value; } }

    [SerializeField] private float speed;
    [SerializeField] private float timeScale;
    
    [SerializeField] public float radius = 5f; // Radius of the circular path
    
    private float angle = 0f;

    private FFCPlayer _player;

    [SerializeField] private GameObject alertSign;
    private bool surpriseAttack = false;
    
    private FFCEnemyState _state;
    public FFCEnemyState State { get { return _state; } set { _state = value; } }
    
    [SerializeField] private float damage;
    [SerializeField] private float damageRadius;

    [SerializeField] private float lifePoints;
    
    void Start()
    {
        _player = gameManager.GetFFCCurrentLevelValues().Player;
    }

    void Update()
    {
        alertSign.SetActive(surpriseAttack);
        _markedTarget.SetActive(_isActiveTarget);
        
        if (speed <= 0)
            return;
        
        Time.timeScale = timeScale;

        
        // Increment the angle based on speed and time
        angle += speed * Time.deltaTime;

        // Calculate the circular position around the player
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        // Set the enemy's position relative to the player
        Vector3 offset = new Vector3(x, y, 0);
        transform.position = _player.transform.position + offset;
        
        if (_isActiveTarget)
            Debug.Log(gameObject.name);
    }
}
