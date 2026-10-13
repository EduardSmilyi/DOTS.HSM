using System;

namespace EB.GameHSM
{
    public class SceneTransitionGameState : BaseGameState<SceneTransitionGameState, GameHsmRoot>
    {
        private string _sceneName;
        private Func<ISubGameState<GameHsmRoot>> _nextStateBuilder;

        public SceneTransitionGameState(string sceneName, Func<ISubGameState<GameHsmRoot>> nextStateBuilder)
        {
            _sceneName = sceneName;
            _nextStateBuilder = nextStateBuilder;
        }

        public override void OnEnter()
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_sceneName)
            .completed += (_) => SetState(_nextStateBuilder());
        }
    }
}