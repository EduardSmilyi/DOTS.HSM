namespace EB.GameHSM
{
    internal class VoidState : BaseGameState
    {
        public static readonly BaseGameState Ref = new VoidState();
        
        internal override void SetParent(BaseGameState subState) { }
    }
    
    public abstract class BaseGameState
    {
        public BaseGameState SubState { get; protected internal set; }
        internal BaseGameState NextRequestedSubState { get; set; }

        internal void SetSubState(BaseGameState subState)
        {
            NextRequestedSubState = subState ?? VoidState.Ref;
        }

        internal abstract void SetParent(BaseGameState subState);

        public virtual void OnEnter(){}
        public virtual void OnUpdate(){}
        public virtual void OnLateUpdate(){}
        public virtual void OnExit(){}
    }

    public interface ISubGameState<TParent> where TParent : BaseGameState
    {
        public TParent Parent { get;}

        internal void SetParent(TParent parent);
    }
    
    public abstract class BaseGameState<TSelf, TParent> : BaseGameState, ISubGameState<TParent>
        where TSelf : BaseGameState
        where TParent : BaseGameState 
    {
        public TParent Parent { get; private set; }

        void ISubGameState<TParent>.SetParent(TParent parent)
        {
            Parent = parent;
        }

        internal sealed override void SetParent(BaseGameState subState)
        {
            (this as ISubGameState<TParent>).SetParent(subState as TParent);
        }
        
        protected void SetState<TState>(TState state) where TState : ISubGameState<TParent>
        {
            Parent.SetSubState(state as BaseGameState);
        }


        protected void SetSubState<TState>(TState subState)
        {
            base.SetSubState(subState as BaseGameState);
        }
        
        public BaseGameState(){}
        
        public BaseGameState(ISubGameState<TSelf> initState)
        {
            SetSubState(initState);
        }
    }
}