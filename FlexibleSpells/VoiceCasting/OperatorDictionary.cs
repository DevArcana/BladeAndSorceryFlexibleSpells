using System;
using System.Collections.Generic;
using System.Linq;
using FlexibleSpells.Modules;

namespace FlexibleSpells.VoiceCasting
{
    public static class OperatorDictionary
    {
        private static readonly Dictionary<string, Operator> Operators = new Dictionary<string, Operator>();

        public static string[] Keys => Operators.Keys.ToArray();

        public static void Populate(OperatorRegistration[] registrations)
        {
            foreach (var registration in registrations)
            {
                var op = Activator.CreateInstance(Type.GetType(registration.path) ?? throw new InvalidOperationException()) as Operator;

                if (op == null)
                {
                    throw new InvalidOperationException($"invalid operator registration! Word: {registration.word} Path: {registration.path}");
                }
                
                op.Name = registration.word;
                Operators[registration.word] = op;
            }
        }
        
        public static Operator Get(string name)
        {
            return Operators.TryGetValue(name, out var value)
                ? value
                : null;
        }
    }
}