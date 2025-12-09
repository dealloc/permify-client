using System.Linq;

using Microsoft.Kiota.Abstractions.Serialization;

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

    /// <summary>
    /// Maps an <see cref="Any"/> value to a CLR value.
    /// </summary>
    public static object MapToObject(Any value) => value.Type switch
    {
        $"{TypeUrlPrefix}/base.v1.StringValue" => value.AdditionalData["data"] as string ?? throw new InvalidCastException("Failed to cast value to string"),
        $"{TypeUrlPrefix}/base.v1.IntegerValue" => (int?)(value.AdditionalData["data"] as decimal?) ?? throw new InvalidCastException("Failed to cast value to int32"),
        $"{TypeUrlPrefix}/base.v1.DoubleValue" => (double?)(value.AdditionalData["data"] as decimal?) ?? throw new InvalidCastException("Failed to cast value to double"),
        $"{TypeUrlPrefix}/base.v1.BooleanValue" => value.AdditionalData["data"] as bool? ?? throw new InvalidCastException("Failed to cast value to bool"),
        $"{TypeUrlPrefix}/base.v1.StringArrayValue" => MapToStringArray(value.AdditionalData["data"]),
        $"{TypeUrlPrefix}/base.v1.IntegerArrayValue" => MapToIntArray(value.AdditionalData["data"]),
        $"{TypeUrlPrefix}/base.v1.DoubleArrayValue" => MapToDoubleArray(value.AdditionalData["data"]),
        $"{TypeUrlPrefix}/base.v1.BooleanArrayValue" => MapToBoolArray(value.AdditionalData["data"]),
        _ => throw new NotSupportedException($"Unsupported type: '{value.Type}'")
    };

    private static string[] MapToStringArray(object data)
    {
        if (data is UntypedArray untypedArray)
        {
            return untypedArray.GetValue()
                .Cast<UntypedString>()
                .Select(node => node.GetValue() ?? throw new InvalidCastException("Failed to extract string from UntypedNode"))
                .ToArray();
        }

        if (data is string[] stringArray)
            return stringArray;

        throw new InvalidCastException("Failed to cast value to string[]");
    }

    private static int[] MapToIntArray(object data)
    {
        if (data is UntypedArray untypedArray)
        {
            return untypedArray.GetValue()
                .Cast<UntypedInteger>()
                .Select(node => Convert.ToInt32(node.GetValue()))
                .ToArray();
        }

        if (data is int[] intArray)
            return intArray;

        throw new InvalidCastException("Failed to cast value to int32[]");
    }

    private static double[] MapToDoubleArray(object data)
    {
        if (data is UntypedArray untypedArray)
        {
            return untypedArray.GetValue()
                .Cast<UntypedDecimal>()
                .Select(node => Convert.ToDouble(node.GetValue()))
                .ToArray();
        }

        if (data is double[] doubleArray)
            return doubleArray;

        throw new InvalidCastException("Failed to cast value to double[]");
    }

    private static bool[] MapToBoolArray(object data)
    {
        if (data is UntypedArray untypedArray)
        {
            return untypedArray.GetValue()
                .Cast<UntypedBoolean>()
                .Select(node => node.GetValue() as bool? ?? throw new InvalidCastException("Failed to extract bool from UntypedNode"))
                .ToArray();
        }

        if (data is bool[] boolArray)
            return boolArray;

        throw new InvalidCastException("Failed to cast value to bool[]");
    }
}