using System;

namespace Sly {
    public class ActorContext {

        //--- Class Fields ---
        [ThreadStatic]
        private static ActorContext _current;

        //--- Class Properties ---
        public static ActorContext Current {
            get {
                if(_current == null) {
                    throw new InvalidOperationException("no context set");
                }
                return _current;
            }
            private set {
                _current = value;
            }
        }

        //--- Class Methods ---
        public static void Invoke(ISubject subject, ISubject origin, Action callback) {
            var previous = Current;
            try {
                Current = new ActorContext{ Subject = subject, Origin = origin };
                callback();
            } finally {
                Current = previous;
            }
        }

        //--- Properties ---
        public ISubject Subject { get; private set; }
        public ISubject Origin { get; private set; }
    }
}