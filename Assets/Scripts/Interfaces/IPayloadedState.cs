namespace Interfaces
{
    public interface IPayloadedState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }
    
    public interface IPayloadedState2<TPayload1, TPayload2, TPayload3> : IExitableState
    {
        void Enter(TPayload1 payload1, TPayload2 payload2, TPayload3 payload3);
    }
}