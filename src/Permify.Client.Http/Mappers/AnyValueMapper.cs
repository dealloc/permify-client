using System;
using System.Collections.Generic;
using System.Linq;

using Permify.Client.Http.Generated.Models;

namespace Permify.Client.Http.Mappers;

/// <summary>
/// Handles mapping to and from <see cref="Any"/> values.
/// </summary>
internal static class AnyValueMapper
{
    private const string TypeUrlPrefix = "type.googleapis.com";

    /// <summary>
    /// Maps a CLR value to an <see cref="Any"/> value.
    /// </summary>
    /// <param name="value">The value to pack into an Any.</param>
    /// <returns>An Any instance containing the packed value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when value is null.</exception>
    /// <exception cref="NotSupportedException">Thrown when the value type is not supported.</exception>
    public static Any MapToAny(object value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        var any = new Any { AdditionalData = new Dictionary<string, object>() };

        switch (value)
        {
            case string str:
                any.Type = $"{TypeUrlPrefix}/base.v1.StringValue";
                any.AdditionalData["data"] = str;
                break;

            case int integer:
                any.Type = $"{TypeUrlPrefix}/base.v1.IntegerValue";
                any.AdditionalData["data"] = integer;
                break;

            case double dbl:
                any.Type = $"{TypeUrlPrefix}/base.v1.DoubleValue";
                any.AdditionalData["data"] = dbl;
                break;

            case float flt:
                any.Type = $"{TypeUrlPrefix}/base.v1.DoubleValue";
                any.AdditionalData["data"] = (double)flt;
                break;

            case bool boolean:
                any.Type = $"{TypeUrlPrefix}/base.v1.BooleanValue";
                any.AdditionalData["data"] = boolean;
                break;

            case string[] strArray:
                any.Type = $"{TypeUrlPrefix}/base.v1.StringArrayValue";
                any.AdditionalData["data"] = strArray;
                break;

            case int[] intArray:
                any.Type = $"{TypeUrlPrefix}/base.v1.IntegerArrayValue";
                any.AdditionalData["data"] = intArray;
                break;

            case double[] dblArray:
                any.Type = $"{TypeUrlPrefix}/base.v1.DoubleArrayValue";
                any.AdditionalData["data"] = dblArray;
                break;

            case bool[] boolArray:
                any.Type = $"{TypeUrlPrefix}/base.v1.BooleanArrayValue";
                any.AdditionalData["data"] = boolArray;
                break;

            default:
                throw new NotSupportedException($"Unsupported type: {value.GetType().FullName}");
        }

        return any;
    }
}