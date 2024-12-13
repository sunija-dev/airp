using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// A character in the roleplay.
/// </summary>
/// 

public class RPChar : TrackedObject
{
    public string strName = "";
    public Sprite sprite;
    public string strInternalInfo = "";

    public static RPChar GetByGuid(Guid _guid)
    {
        foreach (RPChar rpchar in ChatRpManager.s_liRPChars)
        {
            if (rpchar.guid == _guid)
            {
                return rpchar;
            }
        }
        return null;
    }
}
