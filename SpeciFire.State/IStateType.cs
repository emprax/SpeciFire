using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeciFire.State
{
    public interface IState<TContext> where TContext : class, IContext<TContext>
    {

    }

    public interface IContext<TContext> where TContext : class, IContext<TContext>
    {
        IState<TContext> State { get; }

        void ChangeState(IState<TContext> state);
    }

    public interface IEvent<TContext> where TContext : class, IContext<TContext>
    {

    }

    public interface IEventHandler<TContext, TEvent>
        where TContext : class, IContext<TContext>
        where TEvent : class, IEvent<TContext>
    {
        Task Handle(TContext context, TEvent @event);
    }

    public abstract class StateEventHandler<TContext, TEvent> : IEventHandler<TContext, TEvent>
        where TContext : class, IContext<TContext>
        where TEvent : class, IEvent<TContext>
    {
        protected abstract IState<TContext> MatchingState { get; }

        protected abstract Task Execute(TContext context, TEvent @event);

        public async Task Handle(TContext context, TEvent @event)
        {
            if (this.MatchingState == context.State)
            {
                await this.Handle(context, @event);
            }
        }
    }

    public class Transition<TContext> where TContext : class, IContext<TContext>
    {
        public Transition(IState<TContext> from) : this(from, from) { }
        
        public Transition(IState<TContext> from, IState<TContext> to)
        {
            this.From = from;
            this.To = to;
        }

        public IState<TContext> From { get; }

        public IState<TContext> To { get; }
    }

    public abstract class AutoStateEventHandler<TContext, TEvent> : IEventHandler<TContext, TEvent>
        where TContext : class, IContext<TContext>
        where TEvent : class, IEvent<TContext>
    {
        protected abstract Transition<TContext> Transition { get; }

        protected abstract Task Execute(TContext context, TEvent @event);

        public async Task Handle(TContext context, TEvent @event)
        {
            if (this.Transition.From == context.State)
            {
                await this.Handle(context, @event);
                context.ChangeState(this.Transition.To);
            }
        }
    }

    public interface IEventDispatcher<TContext, TEvent>
        where TContext : class, IContext<TContext>
        where TEvent : class, IEvent<TContext>
    {
        Task Dispatch(TContext context, TEvent @event);
    }

    public interface IDistributedEventDispatcher<TContext> where TContext : class, IContext<TContext>
    {
        Task Dispatch<TEvent>(TContext context, TEvent @event) where TEvent : class, IEvent<TContext>;
    }

    public class EventDispatcher<TContext> : IDistributedEventDispatcher<TContext> where TContext : class, IContext<TContext>
    {
        private readonly IDictionary<Type, IReadOnlyList<Func<object>>> dispatchers;

        public EventDispatcher(IDictionary<Type, IReadOnlyList<Func<object>>> dispatchers)
        {
            this.dispatchers = dispatchers;
        }

        public async Task Dispatch<TEvent>(TContext context, TEvent @event) where TEvent : class, IEvent<TContext>
        {
            if (!this.dispatchers.TryGetValue(typeof(TEvent), out var dispatchers))
            {
                return;
            }

            foreach (var dispatcher in dispatchers)
            {
                await ((dispatcher.Invoke() as IEventDispatcher<TContext, TEvent>)?.Dispatch(context, @event) ?? Task.CompletedTask);
            }
        }
    }



    //public interface IStateType<TContext> where TContext : class
    //{
    //    uint Id { get; }

    //    string Name { get; }
    //}

    //public interface IActionRecord<TContext> where TContext : class
    //{
    //    Task<IStateType<TContext>> Apply(Func<Task> action);
    //}

    //internal class ActionRecord<TContext, TCommand> : IActionRecord<TContext>
    //    where TContext : class
    //    where TCommand : class
    //{
    //    private readonly IStateType<TContext> to;
    //    private readonly IStateType<TContext> from;
    //    private readonly IStateType<TContext> error;
    //    private readonly IReadOnlyList<ISpec<TCommand>> commandSpecs;

    //    internal ActionRecord(
    //        IStateType<TContext> to,
    //        IStateType<TContext> from,
    //        IStateType<TContext> error,
    //        IReadOnlyList<ISpec<TCommand>> commandSpecs)
    //    {
    //        this.to = to;
    //        this.from = from;
    //        this.error = error;
    //        this.commandSpecs = commandSpecs;
    //    }

    //    public bool IsSatisfiedBy(TCommand command) => this.commandSpecs.All(spec => spec?.IsSatisfiedBy(command) ?? true);

    //    public async Task<IStateType<TContext>> Apply(Func<Task> action)
    //    {
    //        try
    //        {
    //            await action.Invoke();
    //            return this.to;
    //        }
    //        catch (Exception exception)
    //        {
    //            if (this.error is null)
    //            {
    //                throw exception;
    //            }

    //            return this.error;
    //        }
    //    }
    //}

    //public interface IStateMachine<TContext> where TContext : class
    //{
    //    Task Apply<TCommand>(TCommand command) where TCommand : class;
    //}

    //public interface IStateMachineBuilder<TContext> where TContext : class
    //{
    //    IStateMachineFromBuilder<TContext> From<TStateType>(TStateType state) where TStateType : class, IStateType<TContext>;
    //}

    //public interface IStateMachineFromBuilder<TContext> where TContext : class
    //{
    //    IStateMachineOnBuilder<TContext, TCommand> On<TCommand>() where TCommand : class;
    //}

    //public interface IStateMachineOnBuilder<TContext, TCommand>
    //    where TContext : class
    //    where TCommand : class
    //{
    //    IStateMachineOnBuilder<TContext, TCommand> When<TSpecification>() where TSpecification : class, ISpec<TCommand>;

    //    IStateMachineOnBuilder<TContext, TCommand> OnError<TStateType>(TStateType state) where TStateType : class, IStateType<TContext>;

    //    void To<TStateType>(TStateType state) where TStateType : class, IStateType<TContext>;
    //}

    //public abstract class StateMachine<TContext> : IStateMachine<TContext> where TContext : class, IContext<TContext>
    //{
    //    private readonly IDictionary<Type, IDictionary<uint, IActionRecord<TContext>>> actions;
    //    private readonly TContext context;

    //    protected StateMachine(TContext context, IDictionary<Type, IDictionary<uint, IActionRecord<TContext>>> actions)
    //    {
    //        this.actions = actions;
    //        this.context = context;
    //    }

    //    protected abstract Task Execute<TCommand>(TCommand command) where TCommand : class;

    //    public async Task Apply<TCommand>(TCommand command) where TCommand : class
    //    {
    //        var id = this.context.CurrentState.Id;
    //        if (!actions.TryGetValue(typeof(TCommand), out var commandRegistration) || commandRegistration is null)
    //        {
    //            return;
    //        }

    //        if (!commandRegistration.TryGetValue(id, out var record))
    //        {
    //            return;
    //        }

    //        var newState = await record.Apply(() => this.Execute(command));
    //        this.context.CurrentState = newState;
    //    }
    //}

    //public interface IContext<TContext> where TContext : class, IContext<TContext>
    //{
    //    IStateType<TContext> CurrentState { get; set; }
    //}

    //public interface IStateMachine
    //{
    //    IStateMachine<TContext> For<TContext>(TContext context) where TContext : class, IContext<TContext>;
    //}

    //public class StateMachineBuilder<TContext> : IStateMachineBuilder<TContext> where TContext : class
    //{
    //    public IDictionary<Type, IDictionary<uint, IActionRecord<TContext>>> Actions { get; }

    //    public StateMachineBuilder()
    //    {
    //        this.Actions = new ConcurrentDictionary<Type, IDictionary<uint, IActionRecord<TContext>>>();
    //    }

    //    public IStateMachineFromBuilder<TContext> From<TStateType>(TStateType state) where TStateType : class, IStateType<TContext>
    //    {
    //        return new StateMachineFromBuilder<TContext>(this.Actions, state);
    //    }
    //}

    //public class StateMachineFromBuilder<TContext> : IStateMachineFromBuilder<TContext> where TContext : class
    //{
    //    private readonly IDictionary<Type, IDictionary<uint, IActionRecord<TContext>>> actions;
    //    private readonly IStateType<TContext> stateType;

    //    public StateMachineFromBuilder(
    //        IDictionary<Type, IDictionary<uint, IActionRecord<TContext>>> actions,
    //        IStateType<TContext> stateType)
    //    {
    //        this.actions = actions;
    //        this.stateType = stateType;
    //    }

    //    public IStateMachineOnBuilder<TContext, TCommand> On<TCommand>() where TCommand : class
    //    {
    //        if (!actions.ContainsKey(typeof(TContext)))
    //        {
    //            this.actions.Add(typeof(TCommand), new ConcurrentDictionary<uint, IActionRecord<TContext>>());
    //        }

    //        return new StateMachineOnBuilder<TContext, TCommand>(this.actions, this.stateType);
    //    }
    //}

    //public class StateMachineOnBuilder<TContext, TCommand> : IStateMachineOnBuilder<TContext, TCommand>
    //    where TContext : class
    //    where TCommand : class
    //{
    //    private readonly IDictionary<Type, IDictionary<uint, IActionRecord<TContext>>> actions;
    //    private readonly IStateType<TContext> stateType;
    //    private readonly List<ISpec<TCommand>> specs;

    //    private IStateType<TContext> error;

    //    public StateMachineOnBuilder(
    //        IDictionary<Type, IDictionary<uint, IActionRecord<TContext>>> actions,
    //        IStateType<TContext> stateType)
    //    {
    //        this.actions = actions;
    //        this.stateType = stateType;
    //        this.specs = new List<ISpec<TCommand>>();
    //    }

    //    public IStateMachineOnBuilder<TContext, TCommand> When<TSpecification>() where TSpecification : class, ISpec<TCommand>
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IStateMachineOnBuilder<TContext, TCommand> OnError<TStateType>(TStateType state) where TStateType : class, IStateType<TContext>
    //    {
    //        this.error = state;
    //        return this;
    //    }

    //    public void To<TStateType>(TStateType state) where TStateType : class, IStateType<TContext>
    //    {
    //        var key = typeof(TCommand);
    //        var record = new ActionRecord<TContext, TCommand>(this.stateType, state, this.error, this.specs);

    //        if (!this.actions[key].ContainsKey(this.stateType.Id))
    //        {
    //            this.actions[key].Add(this.stateType.Id, record);
    //            return;
    //        }

    //        this.actions[key][this.stateType.Id] = record;
    //    }
    //}
}
