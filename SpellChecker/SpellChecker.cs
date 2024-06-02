using System.Diagnostics;

namespace SpellChecker;

public static class SpellChecker {
    /// <summary>
    /// Gets the edit distance from string a to b using the Wagner-Fischer algorithm.
    /// The edit distance is defined as the total number of Insertions, Deletions or Replacements
    /// needed to get from one word to another. The Wagner-Fischer algorithm is a lot more effective than
    /// Levenshtein's algorithm
    /// TODO: fix
    /// </summary>
    /// <param name="a">The starting word</param>
    /// <param name="b">The target word</param>
    /// <returns>The edit distance from a to b</returns>
    public static int WagnerFischer(this string a, string b) {
        if (a.Length == 0) return b.Length;
        if (b.Length == 0) return a.Length;

        int n = a.Length + 1;
        int m = b.Length + 1;
        int[,] map = new int[n, m];

        // Initialize first row/column
        for (int i = 1; i < n; i++) map[i, 0] = i;
        for (int i = 1; i < m; i++) map[0, i] = i;

        // Flood-Fill the rest of the matrix
        for (int i = 1; i < n; i++) {
            for (int j = 1; j < m; j++) {
                int substitutionCost = a[i - 1] == b[j - 1] ? 0 : 1;
                map[i, j] = Math.Min(Math.Min(
                        map[i - 1, j] + 1,
                        map[i - 1, j - 1] + substitutionCost),
                    map[i, j - 1] + 1);
            }
        }

        return map[n - 1, m - 1];
    }

    /// <summary>
    /// Get words with the specified format
    /// - Simple => 1000 words
    /// - Fast => 50000 words
    /// - Accurate => 370000 words
    /// </summary>
    /// <param name="format">The format to retrieve the words</param>
    /// <returns>An array of all the words in lowercase</returns>
    public static string[] GetWords(Words format) {
        string pathToFile = format switch {
            Words.Fast => "50k.txt",
            Words.Accurate => "Insanity.txt",
            _ => ""
        };
        return File.ReadAllLines(pathToFile).Select(s => s.ToLower()).ToArray();
    }
}