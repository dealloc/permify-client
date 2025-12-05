using System.Globalization;

namespace Permify.Client.Mappers;

/// <summary>
/// Contains a helper method to parse Permify's returned timestamps into <see cref="DateTimeOffset" />.
/// </summary>
public static class DateTimeOffsetMapper
{
    /// <summary>
    /// Maps a string timestamp to a <see cref="DateTimeOffset" />.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c></exception>
    public static DateTimeOffset MapDateTimeOffset(string? source)
        => DateTimeOffset.ParseExact(
            source ?? throw new ArgumentNullException(nameof(source)),
            "yyyy-MM-dd HH:mm:ss zzz 'UTC'",
            CultureInfo.InvariantCulture
        );
}