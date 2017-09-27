using System;

namespace Sly {
    public interface IMessage {

        //--- Properties ---
        ISubject From { get; }
        ISubject To { get; }

        //--- Methods ---
        void DeliverTo(object actor, Action completed);
    }
}