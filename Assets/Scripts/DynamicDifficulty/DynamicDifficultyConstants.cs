namespace DynamicDifficulty
{
    public static class DynamicDifficultyConstants
    {
        public const Difficulty baseDifficulty = Difficulty.NORMAL;
        public const int baseDifficultyIndex = (int)baseDifficulty;
        public const int difficultiesBelowBaseDifficulty = 2;
        public const int difficultiesAboveBaseDifficulty = 2;
    }
}