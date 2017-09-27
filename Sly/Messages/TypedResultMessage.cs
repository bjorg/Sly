using System;
using System.Linq.Expressions;
using MindTouch.Tasking;

namespace Sly {
    internal class TypedResultMessage<T, U> : IMessage {

        //--- Fields ---
        private readonly Func<T, Result<U>> _delivery;
        private readonly Result<U> _result;

        //--- Constructors ---
        public TypedResultMessage(ISubject @from, ISubject to, Expression action, Func<T, Result<U>> delivery, Result<U> result) {
            this.From = @from;
            this.To = to;
            this.Action = action;
            _delivery = delivery;
            _result = result;
        }

        //--- Properties ---
        public ISubject From { get; private set; }
        public ISubject To { get; private set; }
        public Expression Action { get; private set; }

        //--- Methods ---
        public void DeliverTo(object actor, Action completed) {
            try {
                if(actor is T) {
                    _delivery((T)actor).WhenDone(r => {
                        _result.Return(r);
                        if(completed != null) {
                            completed();
                        }
                    });
                } else if(actor is ISubject) {
                    ((IUnhandledMessage)actor).DoUndeliverableMessage(this);
                    if(completed != null) {
                        completed();
                    }
                } else {
                    throw new Exception("undeliverable");
                }
            } catch(Exception e) {
                _result.Throw(e);
                if(completed != null) {
                    completed();
                }
            }
        }
    }
}