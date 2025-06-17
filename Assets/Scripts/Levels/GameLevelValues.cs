using UnityEngine;

public abstract class GameLevelValues
{
    protected LevelType levelType;

    protected void Initialize(LevelType _levelType)
    {
        levelType = _levelType;
    }

    public abstract void InitializeValues();
}
