using Enemies;
using System;

public static class GameActions
{
    //OLD EVENTS
    public static Action<int, int> ItemPicked;
    public static Action CheckpointReached;
    public static Action LevelEndReached;
    public static Action PlayerDeath;
    public static Action RetrieveWeapon;
    public static Action<bool> GamePause;

    //Player might have died to environment hazards
    public static Action<EnemyType?> OnPlayerDeath;
    public static Action<EnemyType> OnEnemyKilled;
}
