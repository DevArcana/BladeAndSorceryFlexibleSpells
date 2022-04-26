using FlexibleSpells.Operators;
using FluentAssertions;
using Xunit;

namespace FlexibleSpells.Tests
{
    public class EventTests
    {
        [Fact]
        public void RaiseEventsWhenCleared()
        {
            var sut = new StackInterpreter();
            using (var monitor = sut.Monitor())
            {
                sut.Clear();
                monitor
                    .Should()
                    .Raise(nameof(sut.OperatorsCleared))
                    .WithSender(sut);
            }
        }
        
        [Fact]
        public void RaiseEventsWhenOperatorAdded()
        {
            var sut = new StackInterpreter();
            using (var monitor = sut.Monitor())
            {
                var op = new Constant<int>(7);
                sut.AddOperator(op);
                monitor
                    .Should()
                    .Raise(nameof(sut.OperatorAdded))
                    .WithSender(sut)
                    .WithArgs<OperatorAddedEventArgs>(args => args.Operator == op);
            }
        }
        
        [Fact]
        public void RaiseEventsWhenOperatorRemoved()
        {
            var sut = new StackInterpreter();
            using (var monitor = sut.Monitor())
            {
                var op = new Constant<int>(7);
                sut.AddOperator(op);
                sut.AddOperator(op);
                sut.RemoveLastOperator();
                monitor
                    .Should()
                    .Raise(nameof(sut.OperatorRemoved))
                    .WithSender(sut)
                    .WithArgs<OperatorRemovedEventArgs>(args => args.Operator == op);
            }
        }
        
        [Fact]
        public void RaiseEventsWhenLastOperatorRemoved()
        {
            var sut = new StackInterpreter();
            using (var monitor = sut.Monitor())
            {
                var op = new Constant<int>(7);
                sut.AddOperator(op);
                sut.RemoveLastOperator();
                monitor
                    .Should()
                    .Raise(nameof(sut.OperatorRemoved))
                    .WithSender(sut)
                    .WithArgs<OperatorRemovedEventArgs>(args => args.Operator == op);
                monitor
                    .Should()
                    .Raise(nameof(sut.OperatorsCleared))
                    .WithSender(sut);
            }
        }
        
        [Fact]
        public void RaiseEventsWhenNoOperatorRemoved()
        {
            var sut = new StackInterpreter();
            using (var monitor = sut.Monitor())
            {
                sut.RemoveLastOperator();
                monitor
                    .Should()
                    .NotRaise(nameof(sut.OperatorRemoved));
                monitor
                    .Should()
                    .NotRaise(nameof(sut.OperatorsCleared));
            }
        }
    }
}