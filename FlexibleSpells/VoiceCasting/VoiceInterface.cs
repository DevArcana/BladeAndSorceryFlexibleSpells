using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace FlexibleSpells.VoiceCasting
{
    public enum VoiceToken
    {
        Start,
        Cancel,
        Delete,
        Operator
    }

    public class VoiceTokenReceivedEventArgs : EventArgs
    {
        public readonly VoiceToken TokenType;
        public readonly string Token;

        public VoiceTokenReceivedEventArgs(VoiceToken tokenType, string token)
        {
            TokenType = tokenType;
            Token = token;
        }
    }
    
    public class VoiceInterface : IDisposable
    {
        public event EventHandler<VoiceTokenReceivedEventArgs> VoiceTokenReceived; 

        private readonly PhraseRecognizer _recognizer;
        private bool _invoking = false;

        private readonly string StartToken = "incantation";
        private readonly string CancelToken = "nevermind";
        private readonly string DeleteToken = "no";

        public VoiceInterface(IEnumerable<string> keywords)
        {
            _recognizer = new KeywordRecognizer(keywords.Append(StartToken).Append(CancelToken).Append(DeleteToken).ToArray(), ConfidenceLevel.Low);
            _recognizer.OnPhraseRecognized += OnPhraseRecognized;
        }

        public void Incantate(string word)
        {
            ProcessToken(word);
        }

        private void ProcessToken(string token)
        {
            if (token == StartToken)
            {
                _invoking = true;
                Debug.Log("Voice Interface: _invoking = true;");
                VoiceTokenReceived?.Invoke(this, new VoiceTokenReceivedEventArgs(VoiceToken.Start, token));
            }
            else if (_invoking)
            {
                Debug.Log("Voice Interface: _invoking == true;");
                if (token == CancelToken)
                {
                    _invoking = false;
                    Debug.Log("Voice Interface: _invoking = false;");
                    VoiceTokenReceived?.Invoke(this, new VoiceTokenReceivedEventArgs(VoiceToken.Cancel, token));
                }
                else if (token == DeleteToken)
                {
                    Debug.Log("Voice Interface: delete token");
                    VoiceTokenReceived?.Invoke(this, new VoiceTokenReceivedEventArgs(VoiceToken.Delete, token));
                }
                else if (true)
                {
                    Debug.Log("Voice Interface: operator token");
                    VoiceTokenReceived?.Invoke(this, new VoiceTokenReceivedEventArgs(VoiceToken.Operator, token));
                }
            }
        }

        private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            var token = args.text;
            Debug.Log($"[{args.confidence}] {args.text}");
            ProcessToken(token);
        }

        public void Listen()
        {
            _recognizer.Start();
        }

        public void StopInvocation()
        {
            _invoking = false;
        }

        public void StopListening()
        {
            _recognizer.Stop();
        }
        
        public void Dispose()
        {
            if (_recognizer == null)
            {
                return;
            }
            
            _recognizer.OnPhraseRecognized -= OnPhraseRecognized;
            _recognizer.Dispose();
        }
    }
}