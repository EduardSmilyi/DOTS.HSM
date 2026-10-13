using UnityEngine;

namespace EB.GameHSM
{
    public class GameHsm_MonoRunner : MonoBehaviour
    {
        private GameHsmRoot _rootState;
        
        public string debugString = "HSM not started";

        public void InitHsm(ISubGameState<GameHsmRoot> initState)
        {
            if (_rootState != null)
            {
                throw new System.InvalidOperationException("HSM already started");
            }
            
            DontDestroyOnLoad(this);
            
            _rootState = new GameHsmRoot(initState);
            _rootState.OnEnter();
        }
        
        private void Update()
        {
            _rootState?.OnUpdate();
            debugString = _rootState?.ToString() ?? "HSM not started";
        }

        private void LateUpdate()
        {
            _rootState?.OnLateUpdate();
        }
        
        private void OnDestroy()
        {
            _rootState?.OnExit();
        }
    }
}