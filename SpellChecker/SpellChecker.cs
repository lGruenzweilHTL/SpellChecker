namespace SpellChecker;

public static class SpellChecker {
    /// <summary>
    /// Gets the edit distance from string a to b using Levenshtein's algorithm.
    /// The edit distance is defined as the total number of Insertions, Deletions or Replacements
    /// needed to get from one word to another
    /// </summary>
    /// <param name="a">The starting word</param>
    /// <param name="b">The target word</param>
    /// <returns>The edit distance from a to b</returns>
    public static int GetEditDistanceTo(this string a, string b) {
        if (b.Length == 0) return a.Length;
        if (a.Length == 0) return b.Length;

        if (a[0] == b[0]) return GetEditDistanceTo(a[1..], b[1..]);

        return 1 + Math.Min(Math.Min(a[1..].GetEditDistanceTo(b), a.GetEditDistanceTo(b[1..])), a[1..].GetEditDistanceTo(b[1..]));
    }
    
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
        
        int n = a.Length+1;
        int m = b.Length+1;
        int[,] map = new int[n, m];

        // Initialize first row/column
        for (int i = 1; i < n; i++) map[i, 0] = i;
        for (int i = 1; i < m; i++) map[0, i] = i;
        
        // Flood-Fill the rest of the matrix
        for (int i = 1; i < n; i++) {
            for (int j = 1; j < m; j++) {
                int substitutionCost = a[i-1] == b[j-1] ? 0 : 1;
                map[i, j] = Math.Min(Math.Min(map[i - 1, j], map[i - 1, j - 1]), map[i, j - 1]) + substitutionCost;
            }
        }

        return map[n-1, m-1];
    }

    /// <summary>
    /// Get every word out of the list of 1000 selected words
    /// </summary>
    /// <returns></returns>
    public static string[] GetSimpleWords() {
        string pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Words.txt");
        return File.ReadAllLines(pathToFile);
    }

    /// <summary>
    /// Get every word out of the 370105 selected words.
    /// Source: https://raw.githubusercontent.com/dwyl/english-words/master/words_alpha.txt
    /// </summary>
    /// <returns></returns>
    public static string[] GetAllWords() {
        string pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Insanity.txt"); 
        return File.ReadAllLines(pathToFile);
    }
    
    /// <summary>
    /// Get every word out of the 50000 most common English words.
    /// Source: https://gist.github.com/h3xx/1976236
    /// </summary>
    /// <returns></returns>
    public static string[] GetMostWords() {
        string pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "50k.txt"); 
        return File.ReadAllLines(pathToFile).Select(s => s.ToLower()).ToArray();
    }
}