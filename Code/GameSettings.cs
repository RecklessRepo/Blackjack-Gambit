// GameSettings.cs
public static class GameSettings
{
    public enum Difficulty { Easy, Regular, Hard }

    // default so nothing breaks if scene is loaded directly
    public static Difficulty DifficultyChosen = Difficulty.Regular;

    // convenience property you can query anywhere
    public static bool UseGambits => DifficultyChosen == Difficulty.Hard;
}
