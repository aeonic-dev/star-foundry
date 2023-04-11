using System.Collections;

namespace StarFoundry.Misc;

public static class BitArrayExtensions {
    public static void EnsureLength(this BitArray bitArray, int length) {
        if (length > bitArray.Length) bitArray.Length = length;
    }
}