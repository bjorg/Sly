namespace Sly {
    public interface ISubject {

        //--- Methods ---
        void Do(IMessage message);
    }

    public interface ISubject<T> : ISubject { }
}