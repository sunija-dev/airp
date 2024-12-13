using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class TrackedObject
{
    public Guid guid = new Guid();
    public DateTime dateCreated = DateTime.Now;
    public Guid guidProfileCreated = default;
    public DateTime dateEditedLast = DateTime.Now;
    public Guid guidProfileEditedLast = default;
}
