using System.Text;

namespace EB.GameHSM
{
    public class GameHsmRoot : BaseGameState
    {
        public void SetSubState(ISubGameState<GameHsmRoot> subState)
        {
            base.SetSubState(subState as BaseGameState);
        }

        internal override void SetParent(BaseGameState subState)
        {
            throw new System.NotSupportedException("GameHsmRoot is the root state. It cannot have a parent.");
        }
        
        public GameHsmRoot(ISubGameState<GameHsmRoot> initState)
        {
            SetSubState(initState);
        }

        public override string ToString()
        {
            var builder = new StringBuilder("GameHsm states stack:");
            var state = SubState;
            while (state != null)
            {
                builder.AppendLine($"{state.GetType().Name}");
                state = state.SubState;
            }
            
            return builder.ToString();
        }

        public override void OnEnter()
        {
            RecursiveTryChangeSubState(this);
        }

        public override void OnUpdate()
        {
            var subState = SubState;
            while (subState != null)
            {
                subState.OnUpdate();
                subState = subState.SubState;
            }
        }

        public override void OnLateUpdate()
        {
            if (SubState != null)
            {
                SubLateUpdate(SubState);
            }
            
            RecursiveTryChangeSubState(this);
            

            void SubLateUpdate(BaseGameState state)
            {
                if (state.SubState != null)
                {
                    SubLateUpdate(state.SubState);
                }
                
                state.OnLateUpdate();
            }
        }

        public override void OnExit()
        {
            if (SubState != null)
            {
                RecursiveOnExit(SubState);
            }
            SubState = null;
        }
        
        private static void RecursiveOnExit(BaseGameState state)
        {
            if (state.SubState != null)
            {
                RecursiveOnExit(state.SubState);
            }
                
            state.OnExit();
            state.SubState = null;
        }

        private static void RecursiveTryChangeSubState(BaseGameState state)
        {
            if (state.NextRequestedSubState == null)
            {
                if (state.SubState != null)
                {
                    RecursiveTryChangeSubState(state.SubState);
                }

                return;
            }
            
            if (state.SubState != null)
            {
                RecursiveOnExit(state.SubState);
            }
            
            state.SubState = state.NextRequestedSubState == VoidState.Ref ? null : state.NextRequestedSubState;
            state.NextRequestedSubState = null;
                
            if (state.SubState == null)
            {
                return;
            }
            
            state.SubState.SetParent(state);
            state.SubState.OnEnter();
            RecursiveTryChangeSubState(state.SubState);
        }
    }
}