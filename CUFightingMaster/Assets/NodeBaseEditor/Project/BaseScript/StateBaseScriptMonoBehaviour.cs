using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUEngine.Pattern
{
    public abstract class StateBaseScriptMonoBehaviour : MonoBehaviour, IEventable
    {
        public string stateName;
        public virtual void FixedUpdateGame()
        {
        }
        public virtual void LateUpdateGame()
        {
        }
        public virtual void UpdateGame()
        {
        }
    }
}
