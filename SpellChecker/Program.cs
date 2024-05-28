using System.Diagnostics;

namespace SpellChecker;

// TODO: Add accepted edit distance, iterative checks, sort words by usage

internal static class Program {
    private const int WRITE_DISTANCE = 15;
    private const int SUGGESTED_WORDS_COUNT = 6;

    private static void Main() {
        Console.Write("Write some text: ");
        string[] wordsToCheck = Console.ReadLine()!.Split(' ');
        Console.WriteLine("\n");

        Stopwatch timer = new();
        timer.Start();

        int cursorY = Console.GetCursorPosition().Top;
        for (int i = 0; i < wordsToCheck.Length; i++) {
            string word = FormatWord(wordsToCheck[i]);
            string[] registeredWords = SpellChecker.GetMostWords();

            Console.SetCursorPosition(i * WRITE_DISTANCE, cursorY);
            if (registeredWords.Contains(word)) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(word);
                Console.ForegroundColor = ConsoleColor.White;
                continue;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(word);
            Console.ForegroundColor = ConsoleColor.White;


            var weightedSuggestions = (from suggestion in registeredWords
                let distance = suggestion.WagnerFischer(word)
                orderby distance
                select suggestion)
                .ToArray();
            
            weightedSuggestions = weightedSuggestions[..Math.Min(weightedSuggestions.Length, SUGGESTED_WORDS_COUNT)];

            for (int j = 0; j < weightedSuggestions.Length; j++) {
                Console.SetCursorPosition(i * WRITE_DISTANCE, cursorY + (j + 1) * 2);
                Console.Write(weightedSuggestions[j]);
            }
        }

        Console.SetCursorPosition(0, Console.WindowHeight - 2);
        Console.Write($"Spell checks completed. Time [ms]: {timer.ElapsedMilliseconds}");

        Console.ReadLine();
    }

    private static string FormatWord(string word) => word.ToLower().Replace(",", "").Replace(".", "");
}