﻿using System;
using System.Collections;
using UnityEngine;

public class AddValueModifier : ValueModifier {

    public readonly float toAdd;

    public AddValueModifier (int sortOrder, float toAdd) : base (sortOrder)
    {
        this.toAdd = toAdd;
    }

    public override float Modify(float fromValue, float toValue)
    {
        return toValue + toAdd;
    }
}
