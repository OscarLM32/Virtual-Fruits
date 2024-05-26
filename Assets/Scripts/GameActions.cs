using Enemies;
using System;
using UnityEngine;

public static class GameActions
{
    //OLD EVENTS
    public static Action<int, int> ItemPicked;
    [Obsolete]
    public static Action CheckpointReached;
    [Obsolete]
    public static Action LevelEndReached;
    [Obsolete]
    public static Action PlayerDeath;
    public static Action RetrieveWeapon;
    public static Action<bool> GamePause;

    //[NEW]
    //Player might have died to environment hazards
    public static Action<EnemyType?> OnPlayerDeath;
    public static Action<EnemyType> OnEnemyKilled;

    public static Action OnLevelCompleted;
    public static Action<Vector2> OnCheckPointReached;
}
