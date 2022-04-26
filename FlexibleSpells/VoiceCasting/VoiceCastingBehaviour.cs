using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IngameDebugConsole;
using UnityEngine;

namespace FlexibleSpells.VoiceCasting
{
    public class VoiceCastingBehaviour : MonoBehaviour
    {
        public TextMesh display;

        private StackInterpreter _interpreter;
        private VoiceInterface _voiceInterface;

        private bool _executing = false;

        [ConsoleMethod("incantate", "incantates words")]
        public static void DebugIncantate(params string[] words)
        {
            var @this = FindObjectOfType<VoiceCastingBehaviour>();

            foreach (var word in words)
            {
                @this._voiceInterface.Incantate(word);
            }
        }

        private void Start()
        {
            _interpreter = new StackInterpreter();
            _interpreter.OperatorsCleared += OnOperatorsCleared;
            _interpreter.OperatorAdded += OnOperatorAdded;
            _interpreter.OperatorRemoved += OnOperatorRemoved;
            _interpreter.OperatorProcessed += OnOperatorProcessed;
            _interpreter.ProcessingStarted += OnProcessingStarted;
            _interpreter.ProcessingEnded += OnProcessingEnded;

            _voiceInterface = new VoiceInterface(OperatorDictionary.Keys);
            _voiceInterface.VoiceTokenReceived += OnVoiceTokenReceived;

            _voiceInterface.Listen();
            Debug.Log("Voice casting initialized!");
        }

        private void OnProcessingEnded(object sender, ProcessingEndedEventArgs e)
        {
        }

        private void OnProcessingStarted(object sender, ProcessingStartedEventArgs e)
        {
        }

        private void OnOperatorRemoved(object sender, OperatorRemovedEventArgs e)
        {
            RefreshDisplay(_interpreter.Operators.Select(x => x.Name));
        }

        private void OnOperatorAdded(object sender, OperatorAddedEventArgs e)
        {
            RefreshDisplay(_interpreter.Operators.Select(x => x.Name));
        }

        private void OnOperatorProcessed(object sender, OperatorProcessedEventArgs e)
        {
            Debug.Log($"{e.OperatorIndex + 1}/{e.OperatorsCount} {e.Operator.Name} {e.Progress:P}");
            RefreshDisplay(_interpreter.Operators.Select(x => x.Name), e);
        }

        private void OnOperatorsCleared(object sender, EventArgs e)
        {
            Debug.Log("Operators cleared!");
            display.text = "";
        }

        private void RefreshDisplay(IEnumerable<string> operators, OperatorProcessedEventArgs processing = null)
        {
            var builder = new StringBuilder("INCANTATION");

            var progressIndex = processing?.OperatorIndex ?? -1;
            var index = -1;

            foreach (var op in operators)
            {
                index++;

                if (index % 7 == 0)
                {
                    builder.AppendLine();
                }
                else
                {
                    builder.Append(" ");
                }

                if (index == progressIndex)
                {
                    if (processing.Success)
                    {
                        builder.Append("<color=green>");
                    }
                    else
                    {
                        builder.Append("<color=red>");
                    }
                }

                builder.Append(op);

                if (index == progressIndex)
                {
                    builder.Append("</color>");
                }
            }

            builder.AppendLine();

            if (processing != null)
            {
                builder.Append(processing.Progress.ToString("P"));
                builder.Append(" ");
                builder.Append(processing.OperatorIndex + 1);
                builder.Append("/");
                builder.Append(processing.OperatorsCount);
            }

            display.text = builder.ToString();
        }

        private IEnumerator ExecuteSpell()
        {
            if (_executing)
            {
                yield break;
            }

            _executing = true;
            var enumerator = _interpreter.ExecuteAsync();

            while (enumerator.MoveNext())
            {
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(0.1f);
            _interpreter.Clear();
            _voiceInterface.StopInvocation();
            _executing = false;
        }

        private void OnVoiceTokenReceived(object sender, VoiceTokenReceivedEventArgs e)
        {
            if (_executing)
            {
                return;
            }

            Debug.Log($"[{e.TokenType}] {e.Token}");

            switch (e.TokenType)
            {
                case VoiceToken.Start:
                    _interpreter.Clear();
                    display.text = "INCANTATION";
                    break;
                case VoiceToken.Cancel:
                    _interpreter.Clear();
                    break;
                case VoiceToken.Delete:
                    _interpreter.RemoveLastOperator();
                    break;
                case VoiceToken.Operator:
                    var op = OperatorDictionary.Get(e.Token);
                    if (op != null)
                    {
                        _interpreter.AddOperator(op);
                        if (op.Terminal)
                        {
                            StartCoroutine(ExecuteSpell());
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDestroy()
        {
            if (_interpreter != null)
            {
                _interpreter.OperatorsCleared -= OnOperatorsCleared;
                _interpreter.OperatorAdded -= OnOperatorAdded;
                _interpreter.OperatorRemoved -= OnOperatorRemoved;
                _interpreter.OperatorProcessed -= OnOperatorProcessed;
                _interpreter.ProcessingStarted -= OnProcessingStarted;
                _interpreter.ProcessingEnded -= OnProcessingEnded;
            }

            if (_voiceInterface != null)
            {
                _voiceInterface.VoiceTokenReceived -= OnVoiceTokenReceived;
                _voiceInterface.StopListening();
                _voiceInterface.Dispose();
            }
        }
    }
}