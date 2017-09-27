using System;
using System.Linq.Expressions;
using MindTouch.Tasking;

namespace Sly {
    public static class SubjectEx {

        //--- Extension Methods ---
        public static Result<U> Do<T, U>(ISubject<T> subject, Expression<Func<T, Result<U>>> action) {
            var result = new Result<U>();
            Func<T, Result<U>> delivery = to => action.Compile()(to);
            var message = new TypedResultMessage<T, U>(ActorContext.Current.Subject, subject, action, delivery, result);
            subject.Do(message);
            return result;
        }

        public static Result<U> Do<T, U>(ISubject<T> subject, Expression<Func<T, U>> action) {
            var result = new Result<U>();
            Func<T, Result<U>> delivery = to => {
                var inner = new Result<U>();
                inner.Return(action.Compile()(to));
                return result;
            };
            var message = new TypedResultMessage<T, U>(ActorContext.Current.Subject, subject, action, delivery, result);
            subject.Do(message);
            return result;
        }

        public static Result Do<T>(ISubject<T> subject, Expression<Func<T, Result>> action) {
            var result = new Result();
            Func<T, Result> delivery = to => action.Compile()(to);
            var message = new VoidResultMessage<T>(ActorContext.Current.Subject, subject, action, delivery, result);
            subject.Do(message);
            return result;
        }

        public static Result Do<T>(ISubject<T> subject, Expression<Action<T>> action) {
            var result = new Result();
            Func<T, Result> delivery = to => {
                var inner = new Result();
                action.Compile()(to);
                inner.Return();
                return result;
            };
            var message = new VoidResultMessage<T>(ActorContext.Current.Subject, subject, action, delivery, result);
            subject.Do(message);
            return result;
        }

        public static void Do<T>(ISubject<T> subject, Expression<Func<T, None>> action) {
            Func<T, None> delivery = to => action.Compile()(to);
            var message = new OneWayMessage<T>(ActorContext.Current.Subject, subject, action, delivery);
            subject.Do(message);
        }
    }
}