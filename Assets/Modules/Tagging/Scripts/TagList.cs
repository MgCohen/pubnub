﻿using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TagList: ScriptableObject
{
    public List<GameTag> Tags = new List<GameTag>();
}