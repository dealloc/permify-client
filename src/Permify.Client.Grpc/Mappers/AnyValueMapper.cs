using System;
using System.Linq;

using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

using BaseV1 = Base.V1;

namespace Permify.Client.Grpc.Mappers;

/// <summary>
/// Handles mapping to <see cref="Any"/> values.
/// </summary>
public static class AnyValueMapper
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

        return value switch
        {
            string str => Any.Pack(new BaseV1.StringValue { Data = str }, TypeUrlPrefix),
            int integer => Any.Pack(new BaseV1.IntegerValue { Data = integer }, TypeUrlPrefix),
            double dbl => Any.Pack(new BaseV1.DoubleValue { Data = dbl }, TypeUrlPrefix),
            float flt => Any.Pack(new BaseV1.DoubleValue { Data = flt }, TypeUrlPrefix),
            bool boolean => Any.Pack(new BaseV1.BooleanValue { Data = boolean }, TypeUrlPrefix),
            string[] strArray => Any.Pack(new BaseV1.StringArrayValue { Data = { strArray } }, TypeUrlPrefix),
            int[] intArray => Any.Pack(new BaseV1.IntegerArrayValue { Data = { intArray } }, TypeUrlPrefix),
            double[] dblArray => Any.Pack(new BaseV1.DoubleArrayValue { Data = { dblArray } }, TypeUrlPrefix),
            bool[] boolArray => Any.Pack(new BaseV1.BooleanArrayValue { Data = { boolArray } }, TypeUrlPrefix),
            IMessage message => Any.Pack(message, TypeUrlPrefix),
            _ => throw new NotSupportedException($"Unsupported type: {value.GetType().FullName}")
        };
    }
}