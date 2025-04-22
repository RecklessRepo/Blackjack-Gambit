public static class GameSettings
{
    public enum Difficulty    { Easy, Regular, Hard }
    public enum CountingMode  { Help, NoHelp }

    public static Difficulty   DifficultyChosen   = Difficulty.Regular;
    public static CountingMode CountingChosen     = CountingMode.NoHelp;

    // NEW!
    public static bool IsCountingSession = false;

    public static bool UseGambits       => DifficultyChosen == Difficulty.Hard;
    public static bool ShowRunningCount => IsCountingSession  
                                         && CountingChosen  == CountingMode.Help;
}