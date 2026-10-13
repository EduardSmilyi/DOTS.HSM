using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EB.GameHSM
{
    public static class HsmTools
    {
        public static bool IsAppQuitting { get; private set; }

        public static bool IsHsmRunning()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            return world is { IsCreated: true } && world.EntityManager.UniversalQuery.HasSingleton<GameHsmRoot>();
        }

        public static GameHsmRoot GetHsmRoot()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            if (world is not { IsCreated: true } || !world.EntityManager.UniversalQuery.HasSingleton<GameHsmRoot>())
            {
                return null;
            }

            return world.EntityManager.UniversalQuery.GetSingleton<GameHsmRoot>();
        }
        
        
        
        public static void InitHsm(this World world, ISubGameState<GameHsmRoot> initState, List<Type> newRequiredSystems = null)
        {
            //world.EntityManager.CreateSingleton(new GameHsmRoot(initState));
        }
        
        public static void InitHsm(ISubGameState<GameHsmRoot> initState)
        {
            //World.DefaultGameObjectInjectionWorld.EntityManager.CreateSingleton(new HsmRoot(initState, newRequiredSystems));
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Application.quitting += OnApplicationQuitting;
        }
    
        private static void OnApplicationQuitting()
        {
            IsAppQuitting = true;
        }
    }

}