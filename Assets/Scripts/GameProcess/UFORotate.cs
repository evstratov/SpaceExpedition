﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFORotate : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 0, 2, Space.Self);
    }
}
