using System.Collections.Generic;
using FlexibleSpells.Operators;
using FlexibleSpells.Operators.Math;
using FluentAssertions;
using UnityEngine;
using Xunit;

namespace FlexibleSpells.Tests
{
    public class Tests
    {
        [Fact]
        public void CanAddTwoNumbers()
        {
            var operators = new List<Operator>
            {
                new Constant<int>(1),
                new Constant<int>(2),
                new Add(),
                new Constant<int>(3),
                new Constant<int>(4),
                new Add(),
                new Add()
            };

            var interpreter = new StackInterpreter();

            foreach (var @operator in operators)
            {
                interpreter.AddOperator(@operator);
            }
            
            interpreter.Execute();
            interpreter.Values
                .Should()
                .HaveCount(1)
                .And
                .ContainInOrder(10);
        }
        
        [Fact]
        public void CanCreateVectorsWithThreeIntegers()
        {
            var operators = new List<Operator>
            {
                new Constant<int>(1),
                new Constant<int>(2),
                new Constant<int>(3),
                new Vector()
            };

            var interpreter = new StackInterpreter();

            foreach (var @operator in operators)
            {
                interpreter.AddOperator(@operator);
            }
            
            interpreter.Execute();
            interpreter.Values
                .Should()
                .HaveCount(1)
                .And
                .ContainInOrder(new Vector3(1, 2, 3));
        }
        
        [Fact]
        public void CanCreateVectorsWithTwoIntegers()
        {
            var operators = new List<Operator>
            {
                new Constant<int>(1),
                new Constant<int>(2),
                new Vector()
            };

            var interpreter = new StackInterpreter();

            foreach (var @operator in operators)
            {
                interpreter.AddOperator(@operator);
            }
            
            interpreter.Execute();
            interpreter.Values
                .Should()
                .HaveCount(1)
                .And
                .ContainInOrder(new Vector3(1, 2, 0));
        }
        
        [Fact]
        public void CanCreateVectorsWithThreeFloats()
        {
            var operators = new List<Operator>
            {
                new Constant<float>(1.0f),
                new Constant<float>(2.0f),
                new Constant<float>(3.0f),
                new Vector()
            };

            var interpreter = new StackInterpreter();

            foreach (var @operator in operators)
            {
                interpreter.AddOperator(@operator);
            }
            
            interpreter.Execute();
            interpreter.Values
                .Should()
                .HaveCount(1)
                .And
                .ContainInOrder(new Vector3(1.0f, 2.0f, 3.0f));
        }
        
        [Fact]
        public void CanCreateVectorsWithTwoFloats()
        {
            var operators = new List<Operator>
            {
                new Constant<float>(1.0f),
                new Constant<float>(2.0f),
                new Vector()
            };

            var interpreter = new StackInterpreter();

            foreach (var @operator in operators)
            {
                interpreter.AddOperator(@operator);
            }
            
            interpreter.Execute();
            interpreter.Values
                .Should()
                .HaveCount(1)
                .And
                .ContainInOrder(new Vector3(1.0f, 2.0f, 0.0f));
        }
    }
}