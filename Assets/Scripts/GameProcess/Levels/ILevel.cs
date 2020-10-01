using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface ILevel
    {
        void Create(GameController gameController, GameObject rocket);
        void Destroy();
    }
