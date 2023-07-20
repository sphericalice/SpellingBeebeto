using System.Reflection;
using WeCantSpell.Hunspell;

namespace SpellingBeebeto.Utilities
{
    public static class WordListHelper
    {
        public enum WordListLanguage
        {
            English,
            Unsupported,
        }
        public static WordList GetWordList(WordListLanguage language)
        {
            (Stream dictionaryStream, Stream affixStream) = GetStreams(language);
            WordList wordList = WordList.CreateFromStreams(dictionaryStream, affixStream);

            if (!wordList.HasEntries) throw new Exception($"Failed to retrieve dictionary for language {language}!");
            return wordList;
        }
        public static Stream ReadResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = assembly.GetManifestResourceNames();
            string streamName = resourcePath.FirstOrDefault(str => str.EndsWith(resourceName))
                ?? throw new Exception($"No resource {resourceName} found in manifest!");
            return assembly.GetManifestResourceStream(streamName);
        }
        public static (Stream dictionaryStream, Stream affixStream) GetStreams(WordListLanguage language)
        {
            string streamName = language switch
            {
                WordListLanguage.English => "en_us",
                _ => throw new NotImplementedException($"Cannot retrieve dictionary for language {language}!")
            };
            Stream dictionaryStream = ReadResource($"{streamName}.dic");
            Stream affixStream = ReadResource($"{streamName}.aff");
            return (dictionaryStream, affixStream);
        }
    }
}
