using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Version
{
    /// <summary>
    /// A dot-separated version number such as 5, 5.0, or 5.0.5
    /// </summary>
    public abstract string Number { get; }


    /// <summary>
    /// Compares two versions to see which is greater.
    /// </summary>
    /// <param name="a">Version to compare against second param</param>
    /// <param name="b">Version to compare against first param</param>
    /// <returns>-1 if the first version is smaller, 1 if the first version is greater, 0 if they are equal</returns>
    public static int Compare(string a, string b)
    {
        var versionA = VersionStringToInts(a);
        var versionB = VersionStringToInts(b);
        for (var i = 0; i < Mathf.Max(versionA.Length, versionB.Length); i++) {
            if (VersionPiece(versionA, i) < VersionPiece(versionB, i))
                return -1;
            if (VersionPiece(versionA, i) > VersionPiece(versionB, i))
                return 1;
        }

        return 0;
    }


    private static int VersionPiece(IList<int> versionInts, int pieceIndex)
    {
        return pieceIndex < versionInts.Count ? versionInts[pieceIndex] : 0;
    }


    private static int[] VersionStringToInts(string version)
    {
        int piece;
        return version.Split('.').Select(v => int.TryParse(v, out piece) ? piece : 0).ToArray();
    }
}
