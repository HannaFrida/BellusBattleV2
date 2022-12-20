using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    public bool GEquals(Hat other)
    {
        if (gameObject.name == other.gameObject.name) return true;
        return false;
    }
}
