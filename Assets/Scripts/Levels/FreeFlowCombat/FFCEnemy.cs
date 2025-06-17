using UnityEngine;

public class FFCEnemy : MonoBehaviour
{
    [SerializeField] 
    protected GameObject gameCore;
    protected GameCore GameCoreRef;
    [SerializeField] protected GameManagerSo gameManager;
    
    private bool isActive = false;
    public bool IsActive { get { return isActive; } set { isActive = value; } }

    [SerializeField] private float speed;
    [SerializeField] private float timeScale;
    
    [SerializeField] 
    protected GameObject player;
    [SerializeField] public float radius = 5f; // Radius of the circular path
    
    private float angle = 0f; 
    
    void Start()
    {
        GameCoreRef = gameCore.GetComponent<GameCore>();
        GameCoreRef.RegisterEnemy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
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
        transform.position = player.transform.position + offset;
        
        if (isActive)
            Debug.Log(gameObject.name);
    }
}
