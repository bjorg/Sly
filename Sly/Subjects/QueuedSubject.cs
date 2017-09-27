using System;
using MindTouch.Collections;

namespace Sly {
    internal class QueuedSubject : ISubject {

        //--- Fields ---
        private readonly object _target;
        private readonly ProcessingQueue<IMessage> _queue;

        //--- Constructors ---
        public QueuedSubject(object target) {
            _target = target;
            _queue = new ProcessingQueue<IMessage>(Dispatch, 1);
        }

        //--- Methods ---
        public void Do(IMessage message) {
            _queue.TryEnqueue(message);
        }

        private void Dispatch(IMessage message, Action completed) {
            ActorContext.Invoke(this, message.From, () => message.DeliverTo(_target, completed));
        }
    }
}