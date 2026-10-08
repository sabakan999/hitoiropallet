public enum Difficulty
{
    Easy,
    Normal,
    Hard,
    Hell
}

public static class GameSettings
{
    public static Difficulty SelectedDifficulty =
        Difficulty.Normal;
}