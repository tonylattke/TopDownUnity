using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Scriptable ObjectGameManager")]
public class GameManagerSo : ScriptableObject
{
    private LevelHandler _levelHandler;
    private GameLevelValues _currentLevelValues;
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += NewSceneLoaded;
    }

    private void NewSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _levelHandler = FindFirstObjectByType<LevelHandler>();
        if (_levelHandler == null)
        {
            Debug.LogError("LevelHandler not found");
            Application.Quit();
            return;
        }

        InitializeCurrentLevelGameValues();
    }

    private void InitializeCurrentLevelGameValues()
    {
        switch (_levelHandler.levelType)
        {
            case LevelType.Combat:
                _currentLevelValues = new FFCGameLevelValues();
                break;
            case LevelType.Explore:
                _currentLevelValues = new ExploreGameLevelValues();
                break;
            case LevelType.Menu:
                _currentLevelValues = new MenuGameLevelValues();
                break;
        }
        _currentLevelValues.InitializeValues();
    }

    public GameObject ClosestEnemy(Vector2 mousePosition, float enemyAcceptableDistance)
    {
        return (_currentLevelValues as FFCGameLevelValues).ClosestEnemy(mousePosition, enemyAcceptableDistance);
    }
    
}
