using System.Collections;
using System.Collections.Generic;
using Avocado.ObjectPoolSystem;
using UnityEngine;

namespace Avocado.Interfaces
{
    public interface IObjectPoolItem
    {
        void SetObjectPool<T>(ObjectPool pool, T comp) where T : Component;

        void Release();
    }
}
