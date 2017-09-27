namespace Sly {
    internal class ImmediateSubject : ISubject {

        //--- Fields ---
        private readonly object _target;

        //--- Constructors ---
        public ImmediateSubject(object target) {
            _target = target;
        }

        //--- Methods ---
        public void Do(IMessage message) {
            ActorContext.Invoke(this, message.From, () => message.DeliverTo(_target, null));
        }
    }
}