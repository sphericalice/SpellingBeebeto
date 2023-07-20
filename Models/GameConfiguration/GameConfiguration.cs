namespace SpellingBeebeto.Models.GameConfiguration
{
    public static class GameConfiguration
    {
        public static Random Random => random ??= new();
        private static Random random = null;
        public const int WordSize = 6;
        public const int LatestWordsLength = 5;
    }
}
