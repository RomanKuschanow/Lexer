#nullable disable
namespace Lexer.Extensions;
public static class IEnumerableExtensions
{
    public static bool ScrambledEquals<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
    {
        var cnt = new Dictionary<T, int>();
        foreach (T s in list1)
        {
            if (cnt.ContainsKey(s))
            {
                cnt[s]++;
            }
            else
            {
                cnt.Add(s, 1);
            }
        }
        foreach (T s in list2)
        {
            if (cnt.ContainsKey(s))
            {
                cnt[s]--;
            }
            else
            {
                return false;
            }
        }
        return cnt.Values.All(c => c == 0);
    }

    public static bool ScrambledEquals<T>(this IEnumerable<T> list1, IEnumerable<T> list2, IEqualityComparer<T> comparer)
    {
        var cnt = new Dictionary<T, int>(comparer);
        foreach (T s in list1)
        {
            if (cnt.ContainsKey(s))
            {
                cnt[s]++;
            }
            else
            {
                cnt.Add(s, 1);
            }
        }
        foreach (T s in list2)
        {
            if (cnt.ContainsKey(s))
            {
                cnt[s]--;
            }
            else
            {
                return false;
            }
        }
        return cnt.Values.All(c => c == 0);
    }

    public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
    {
        foreach (T value in values)
            action(value);
    }
}
