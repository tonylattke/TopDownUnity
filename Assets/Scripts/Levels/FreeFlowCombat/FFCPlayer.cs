using UnityEngine;

public class FFCPlayer : MonoBehaviour
{
    [SerializeField] protected GameManagerSo gameManager;
    
    [SerializeField] protected int Speed = 5;
    [SerializeField] private float enemyAcceptableDistance = 40;
    
    public FFCPlayerState CurrentState = FFCPlayerState.Idle;
    private FFCEnemy currentEnemy;
    
    public float CurrentAttackPoints = 1;
    
    void Start()
    {
        // Nothing to add for now
    }

    void Update()
    {
        UpdateMovement();
        
        UpdateAttack();
        UpdateParry();
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

    void UpdateAttack()
    {
        switch (CurrentState)
        {
            case FFCPlayerState.Idle:
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 mousePositionInAttack = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    FFCEnemy closestEnemy = gameManager.GetFFCCurrentLevelValues().ClosestEnemy(mousePositionInAttack, enemyAcceptableDistance);
                    if (closestEnemy is null)
                        return;
                    
                    CurrentState = FFCPlayerState.StartAttack;
                    currentEnemy = closestEnemy;
                    closestEnemy.IsActiveTarget = true;
                }
                break;
            case FFCPlayerState.StartAttack:
                if (Vector3.Distance(transform.position, currentEnemy.transform.position) < 0.1f)
                {
                    CurrentState = FFCPlayerState.Attacking;
                    return;
                }
                transform.position = Vector3.MoveTowards(transform.position, currentEnemy.transform.position, Time.deltaTime * Speed);
                break;
            case FFCPlayerState.Attacking:
                currentEnemy.ReceiveDamage(CurrentAttackPoints);
                CurrentState = FFCPlayerState.Idle;
                break;
        }
        
        
        
        {
            
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

    void UpdateParry()
    {
        if (!Input.GetMouseButtonDown(1))
            return;
        
        CurrentState = FFCPlayerState.Attacking;
        
        // TODO
    }

    public void ReceiveDamage(float damage)
    {
        GameInstance.Singleton.currentLifePoints -= damage;
        if (GameInstance.Singleton.currentLifePoints > 0)
            return;
     
        gameManager.PlayerDies();
    }
}
