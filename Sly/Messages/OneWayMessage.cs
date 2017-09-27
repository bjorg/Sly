using System;
using System.Linq.Expressions;

namespace Sly {
    internal class OneWayMessage<T> : IMessage {

        //--- Fields ---
        private readonly Func<T, None> _delivery;

        //--- Constructors ---
        public OneWayMessage(ISubject @from, ISubject to, Expression action, Func<T, None> delivery) {
            this.From = @from;
            this.To = to;
            this.Action = action;
            _delivery = delivery;
        }

        //--- Properties ---
        public ISubject From { get; private set; }
        public ISubject To { get; private set; }
        public Expression Action { get; private set; }

        //--- Methods ---
        public void DeliverTo(object actor, Action completed) {
            try {
                if(actor is T) {
                    _delivery((T)actor);
                } else if(actor is ISubject) {
                    ((IUnhandledMessage)actor).DoUndeliverableMessage(this);
                } else {
                    throw new Exception("undeliverable");
                }
            } catch(Exception e) {

                // TODO: log exception
            } finally {
                if(completed != null) {
                    completed();
                }
            }
        }
    }
}