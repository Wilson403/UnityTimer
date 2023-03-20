using System;

namespace Primise4CSharp
{
    public enum PromiseState
    {
        Pending,
        Rejected,
        Resolved
    }

    public interface IPromiseReject
    {
        void Reject (Exception ex);
    }

    public interface IPromise
    {
        IPromise Then (Action onResolved);
        IPromise<T> Then<T> (Func<T> predicate);
        IPromise Then (Func<Promise> predicate);
        IPromise<T> Then<T> (Func<Promise<T>> predicate);
        IPromise Catch (Action<Exception> onRejected);
    }

    public class Promise : IPromise, IPromiseReject
    {
        private PromiseState _state = PromiseState.Pending;
        private Action _onResolve;
        private Action<Exception> _onReject;
        private Exception _ex;

        private void AddResolvedHandle (Action onResolved)
        {
            if ( _state == PromiseState.Resolved )
            {
                InvokeResolvedHander (onResolved);
            }
            else
            {
                _onResolve += onResolved;
            }
        }

        private void InvokeResolvedHander (Action onResolve)
        {
            try
            {
                onResolve?.Invoke ();
            }
            catch ( Exception ex )
            {
                Reject (ex);
            }
        }

        private void AddRejectedHandle (Action<Exception> onReject)
        {
            if ( _state == PromiseState.Rejected )
            {
                onReject?.Invoke (_ex);
            }
            else
            {
                _onReject += onReject;
            }
        }

        public IPromise Then (Action onResolved)
        {
            Promise promise = new Promise ();
            AddResolvedHandle (() =>
            {
                onResolved?.Invoke ();
                promise.Resolve ();
            });
            return promise;
        }

        public IPromise<PromiseT> Then<PromiseT> (Func<PromiseT> predicate)
        {
            Promise<PromiseT> promise = new Promise<PromiseT> ();
            AddResolvedHandle (() =>
            {
                if ( predicate != null )
                {
                    promise.Resolve (predicate.Invoke ());
                }
                else
                {
                    promise.Resolve (default);
                }
            });
            return promise;
        }

        public IPromise Then (Func<Promise> predicate)
        {
            Promise promise = new Promise ();
            AddResolvedHandle (() =>
            {
                if ( predicate == null )
                {
                    promise.Resolve ();
                    return;
                }

                IPromise param = predicate.Invoke ();
                if ( param == null )
                {
                    promise.Resolve ();
                    return;
                }

                param.Then (promise.Resolve);
            });
            return promise;
        }

        public IPromise<T> Then<T> (Func<Promise<T>> predicate)
        {
            Promise<T> promise = new Promise<T> ();
            AddResolvedHandle (() =>
            {
                if ( predicate == null )
                {
                    promise.Resolve (default);
                    return;
                }

                Promise<T> param = predicate.Invoke ();
                if ( param == null )
                {
                    promise.Resolve (default);
                    return;
                }

                param.Then (promise.Resolve);
            });
            return promise;
        }

        public IPromise Catch (Action<Exception> onRejected)
        {
            Promise promise = new Promise ();
            AddRejectedHandle ((Exception ex) =>
            {
                try
                {
                    onRejected.Invoke (ex);
                    promise.Resolve ();
                }
                catch ( Exception e )
                {
                    promise.Reject (e);
                }
            });
            return promise;
        }

        public void Reject (Exception ex)
        {
            if ( _state != PromiseState.Pending )
            {
                return;
            }

            _onReject?.Invoke (ex);
            _state = PromiseState.Rejected;
            _ex = ex;
            _onReject = null;
        }

        public void Resolve ()
        {
            if ( _state == PromiseState.Resolved )
            {
                return;
            }

            InvokeResolvedHander (_onResolve);
            _state = PromiseState.Resolved;
            _onResolve = null;
        }
    }
}