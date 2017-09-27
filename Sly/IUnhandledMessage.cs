namespace Sly {
    interface IUnhandledMessage {
        void DoUndeliverableMessage(IMessage message);
    }
}
