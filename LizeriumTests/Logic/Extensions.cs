/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 26 апреля 2026 06:56:26
 * Version: 1.0.16
 */

public static class Extensions
{
    private static readonly object _logLock = new();

    public static void Log(string msg, StreamWriter logWriter)
    {
        Console.WriteLine(msg);
        lock (_logLock)
        {
            logWriter?.WriteLine($"[{DateTime.Now:HH:mm:ss}] {msg}");
        }
    }

    public static List<string> KeyValueConvert(string line)
    {
        var key = line.Substring(0, line.IndexOf("=")).Trim();
        var value = line.Substring(line.IndexOf("=") + 1).Trim();
        return new List<string> { key, value };
    }
}