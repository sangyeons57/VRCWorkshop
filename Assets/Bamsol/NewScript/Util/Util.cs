
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Util : UdonSharpBehaviour
{
    public static bool Any(bool[] array)
    {
        foreach (bool b in array)
        {
            if (b)
                return true;
        }

        return false;
    }

    public static bool All(bool[] array)
    {
        foreach (bool b in array)
        {
            if(!b)
                return false;
        }

        return true;
    }


    public static int countTrue(bool[] boolArray)
    {
        int result = 0;
        foreach (bool b in boolArray)
        {
            if(b) result++;
        }
        return result;
    }
}
