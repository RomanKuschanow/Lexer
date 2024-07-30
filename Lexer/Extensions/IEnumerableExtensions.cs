#nullable disable
namespace Lexer.Extensions;
public static class IEnumerableExtensions
{
    /// <summary>
    /// Determines whether two sequences are equal when the elements are in any order.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    /// <param name="list1">The first sequence.</param>
    /// <param name="list2">The second sequence.</param>
    /// <returns><c>true</c> if the sequences contain the same elements in any order; otherwise, <c>false</c>.</returns>
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

    /// <summary>
    /// Determines whether two sequences are equal when the elements are in any order, using a specified comparer.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    /// <param name="list1">The first sequence.</param>
    /// <param name="list2">The second sequence.</param>
    /// <param name="comparer">The comparer to use for comparing elements.</param>
    /// <returns><c>true</c> if the sequences contain the same elements in any order; otherwise, <c>false</c>.</returns>
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

    /// <summary>
    /// Performs the specified action on each element of the <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="values">The sequence of elements.</param>
    /// <param name="action">The action to perform on each element.</param>
    public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
    {
        foreach (T value in values)
            action(value);
    }
}
