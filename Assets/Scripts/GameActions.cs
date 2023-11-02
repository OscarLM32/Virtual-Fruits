using System;

public static class GameActions
{
    public static Action<int, int> ItemPicked;
    public static Action CheckpointReached;
    public static Action LevelEndReached;
    public static Action PlayerDeath;
    public static Action RetrieveWeapon;
    public static Action<bool> GamePause;
}
