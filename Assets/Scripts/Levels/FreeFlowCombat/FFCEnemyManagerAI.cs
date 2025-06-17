using UnityEngine;

public class FFCEnemyManagerAI
{
    private FFCGameLevelValues _ffcGameLevelValues;
    
    
    public FFCEnemyManagerAI(FFCGameLevelValues ffcGameLevelValues)
    {
        _ffcGameLevelValues = ffcGameLevelValues;
        
    }

    public void Update()
    {
        if (_ffcGameLevelValues is null)
            return;

        switch (_ffcGameLevelValues.Player.CurrentState)
        {
            case FFCPlayerState.Idle:
                break;
            case FFCPlayerState.StartAttack:
                break;
            case FFCPlayerState.Attacking:
                break;
            case FFCPlayerState.Dead:
                break;
            default:
                break;
        }
        // Select attacker
        
        // Player win?
    }

    public void UpdateAttacker(FFCEnemy ffcEnemy)
    {
        if (ffcEnemy.CurrentState == FFCEnemyState.Attack)
            return;
        
        foreach (FFCEnemy enemy in _ffcGameLevelValues.Enemies)
        {
            if (ffcEnemy != enemy && enemy.CurrentState != FFCEnemyState.Dead)
                enemy.CurrentState = FFCEnemyState.Patrol;
        }
        
        ffcEnemy.CurrentState = FFCEnemyState.Attack;
    }
}
