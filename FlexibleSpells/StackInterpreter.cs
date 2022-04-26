using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FlexibleSpells
{
    public abstract class Operator
    {
        public string Name { get; set; } = "Unnamed";
        public virtual bool Terminal => false;
    }

    public class OperatorAddedEventArgs : EventArgs
    {
        public readonly Operator Operator;

        public OperatorAddedEventArgs(Operator @operator)
        {
            Operator = @operator;
        }
    }
    
    public class OperatorRemovedEventArgs : EventArgs
    {
        public readonly Operator Operator;

        public OperatorRemovedEventArgs(Operator @operator)
        {
            Operator = @operator;
        }
    }
    
    public class ProcessingStartedEventArgs : EventArgs
    {
        public readonly List<Operator> Operators;

        public ProcessingStartedEventArgs(List<Operator> operators)
        {
            Operators = operators;
        }
    }
    
    public class ProcessingEndedEventArgs : EventArgs
    {
        public readonly List<Operator> Operators;

        public ProcessingEndedEventArgs(List<Operator> operators)
        {
            Operators = operators;
        }
    }
    
    public class OperatorProcessedEventArgs : EventArgs
    {
        public readonly Operator Operator;
        public readonly bool Success;
        public readonly int OperatorIndex;
        public readonly int OperatorsCount;
        public readonly float Progress;

        public OperatorProcessedEventArgs(Operator @operator, int operatorIndex, int operatorsCount, bool success)
        {
            Operator = @operator;
            OperatorIndex = operatorIndex;
            OperatorsCount = operatorsCount;
            Success = success;
            Progress = (operatorIndex + 1) / (float) operatorsCount;
        }
    }
    
    public class StackInterpreter
    {
        public readonly List<Operator> Operators = new List<Operator>();
        
        public List<object> Values = new List<object>();

        public event EventHandler OperatorsCleared; 
        public event EventHandler<OperatorAddedEventArgs> OperatorAdded; 
        public event EventHandler<OperatorRemovedEventArgs> OperatorRemoved; 
        public event EventHandler<ProcessingStartedEventArgs> ProcessingStarted;
        public event EventHandler<ProcessingEndedEventArgs> ProcessingEnded;
        public event EventHandler<OperatorProcessedEventArgs> OperatorProcessed;
        
        public void Clear()
        {
            Operators.Clear();
            OperatorsCleared?.Invoke(this, EventArgs.Empty);
        }

        public void AddOperator(Operator op)
        {
            Operators.Add(op);
            OperatorAdded?.Invoke(this, new OperatorAddedEventArgs(op));
        }

        public void RemoveLastOperator()
        {
            var count = Operators.Count;

            if (count < 1)
            {
                return;
            }
            
            var op = Operators[count - 1];
            Operators.RemoveAt(count - 1);
            OperatorRemoved?.Invoke(this, new OperatorRemovedEventArgs(op));

            if (Operators.Count == 0)
            {
                OperatorsCleared?.Invoke(this, EventArgs.Empty);
            }
        }

        public IEnumerator ExecuteAsync()
        {
            ProcessingStarted?.Invoke(this, new ProcessingStartedEventArgs(Operators));
            
            var values = new Stack<object>();
            var temp = new Stack<object>();
            var index = -1;
            foreach (var op in Operators)
            {
                index++;
                var type = op.GetType();

                var processed = false;
                foreach (var method in type.GetMethods().Where(m => m.Name == "Apply").OrderByDescending(m => m.GetParameters().Length))
                {
                    var parameters = method.GetParameters();

                    if (values.Count < parameters.Length) continue;

                    var abort = false;
                    for (var i = 0; i < parameters.Length; i++)
                    {
                        var parameter = values.Pop();
                        temp.Push(parameter);
                        
                        if (parameter.GetType().Name != parameters[parameters.Length - i - 1].ParameterType.Name)
                        {
                            abort = true;
                            break;
                        }
                    }
                    
                    if (abort)
                    {
                        foreach (var x in temp)
                        {
                            values.Push(x);
                        }
                        temp.Clear();
                    }
                    else
                    {
                        var arguments = temp.ToArray();
                        temp.Clear();
                        var value = method.Invoke(op, arguments);
                        if (value != null)
                        {
                            values.Push(value);
                        }
                        processed = true;
                        break;
                    }
                }
                
                yield return op;
                OperatorProcessed?.Invoke(this, new OperatorProcessedEventArgs(op, index, Operators.Count, processed));
            }

            Values = values.ToList();
            ProcessingEnded?.Invoke(this, new ProcessingEndedEventArgs(Operators));
        }
        
        public void Execute()
        {
            var enumerator = ExecuteAsync();
            while (enumerator.MoveNext())
            {
                
            }
        }
    }
}