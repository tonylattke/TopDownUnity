using UnityEngine;

public class FFCPlayer : MonoBehaviour
{
    [SerializeField] protected GameManagerSo gameManager;
    
    [SerializeField] protected int Speed = 5;
    [SerializeField] private float enemyAcceptableDistance = 40;
    
    void Start()
    {
 
    }

    void Update()
    {
        UpdateMovement();
        
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject closestEnemy = gameManager.ClosestEnemy(mousePosition, enemyAcceptableDistance);
            if (closestEnemy is null)
                return;
            
            FFCEnemy enemy = closestEnemy.GetComponent<FFCEnemy>();
            enemy.IsActive = true;

            transform.position = Vector3.MoveTowards(transform.position, closestEnemy.transform.position, Time.deltaTime * Speed);
        }   
        
        
        /*
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider is not null)
            {
                // Log the name of the clicked object
                Debug.Log("Clicked on: " + hit.collider.gameObject.name);

                // Example: Access the clicked object
                GameObject clickedObject = hit.collider.gameObject;

                // Perform any action on the clicked object
                // e.g., clickedObject.GetComponent<SpriteRenderer>().color = Color.red;
            }

        } */
    }

    private void UpdateMovement()
    {
        // Left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(MathConstants.Left * (Time.deltaTime * Speed));
        }
        
        // Right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(MathConstants.Right * (Time.deltaTime * Speed));
        }
        
        // Up
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(MathConstants.Up * (Time.deltaTime * Speed));
        }
        
        // Down
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(MathConstants.Down * (Time.deltaTime * Speed));
        }
    }
}
