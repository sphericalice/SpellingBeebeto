namespace SpellingBeebeto.Models.GameElements
{
    public class WordValidity
    {
        public enum Validity
        {
            Empty,
            TooShort,
            TooLong,
            MissingKeyLetter,
            AlreadyFound,
            NotInWordList,
            Valid,
        }
        public static readonly Dictionary<Validity, string> RejectionMessages = new()
        {
            [Validity.Empty] = "",
            [Validity.TooShort] = "Too short!",
            [Validity.TooLong] = "Too long!",
            [Validity.MissingKeyLetter] = "Missing key letter!",
            [Validity.AlreadyFound] = "Already found!",
            [Validity.NotInWordList] = "Not in word list!",
            [Validity.Valid] = "",
        };
    }
}
