using System.Collections.Generic;
using UnityEngine;

public class FFCGameLevelValues : GameLevelValues
{
    private List<FFCEnemy> _enemies;
    
    public override void InitializeValues()
    {
        Initialize(LevelType.Combat);
        
        _enemies = new List<FFCEnemy>();
        
        FFCEnemy[] enemies = Object.FindObjectsByType<FFCEnemy>(FindObjectsSortMode.None);
        foreach (FFCEnemy enemy in enemies)
        {
            _enemies.Add(enemy);
        }
    }
    
    public GameObject ClosestEnemy(Vector2 mousePosition, float enemyAcceptableDistance)
    {
        foreach (FFCEnemy enemy in _enemies)
            enemy.IsActive = false;
        
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (FFCEnemy enemy in _enemies)
        {
            float currentDistanceToEnemy = Vector2.Distance(enemy.gameObject.transform.position, mousePosition);
            if (currentDistanceToEnemy <= closestDistance)
            {
                closestDistance = currentDistanceToEnemy;
                closestEnemy = enemy.gameObject;
            }
        }

        return closestDistance > enemyAcceptableDistance ? null : closestEnemy;
    }
}
