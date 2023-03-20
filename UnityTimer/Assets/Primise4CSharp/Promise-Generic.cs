using System;

namespace Primise4CSharp
{
    public interface IPromise<PromiseT>
    {
        IPromise Then (Action onResolved);
        IPromise<PromiseT> Then (Action<PromiseT> onResolved);
        IPromise<T> Then<T> (Func<T> predicate);
        IPromise<T> Then<T> (Func<Promise<T>> predicate);
        IPromise Then (Func<Promise> predicate);
        IPromise<PromiseT> Catch (Action<Exception> onRejected);
    }

    public class Promise<PromiseT> : IPromise<PromiseT>, IPromiseReject
    {
        private PromiseState _state = PromiseState.Pending;
        private Action<PromiseT> _onResolve;
        private Action<Exception> _onReject;
        private PromiseT _currentParam;
        private Exception _ex;

        private void AddResolvedHandle (Action<PromiseT> onResolved)
        {
            if ( _state == PromiseState.Resolved )
            {
                InvokeResolvedHander (onResolved , _currentParam);
            }
            else
            {
                _onResolve += onResolved;
            }
        }

        private void InvokeResolvedHander (Action<PromiseT> onResolved , PromiseT @param)
        {
            try
            {
                onResolved?.Invoke (@param);
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
            AddResolvedHandle ((t) =>
            {
                onResolved?.Invoke ();
                promise.Resolve ();
            });
            return promise;
        }

        public IPromise<PromiseT> Then (Action<PromiseT> onResolved)
        {
            Promise<PromiseT> promise = new Promise<PromiseT> ();
            AddResolvedHandle ((t) =>
            {
                onResolved?.Invoke (t);
                promise.Resolve (t);
            });
            return promise;
        }

        public IPromise<T> Then<T> (Func<T> predicate)
        {
            Promise<T> promise = new Promise<T> ();
            AddResolvedHandle ((t) =>
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

        public IPromise<T> Then<T> (Func<Promise<T>> predicate)
        {
            Promise<T> promise = new Promise<T> ();
            AddResolvedHandle ((t) =>
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

        public IPromise Then (Func<Promise> predicate)
        {
            Promise promise = new Promise ();
            AddResolvedHandle ((t) =>
            {
                if ( predicate == null )
                {
                    promise.Resolve ();
                    return;
                }

                Promise param = predicate.Invoke ();
                if ( param == null )
                {
                    promise.Resolve ();
                    return;
                }

                param.Then (promise.Resolve);
            });
            return promise;
        }

        public IPromise<PromiseT> Catch (Action<Exception> onRejected)
        {
            Promise<PromiseT> promise = new Promise<PromiseT> ();
            AddRejectedHandle ((Exception ex) =>
            {
                try
                {
                    onRejected.Invoke (ex);
                    promise.Resolve (_currentParam);
                }
                catch ( Exception e )
                {
                    promise.Reject (e);
                }
            });
            return promise;
        }

        public void Resolve (PromiseT @param)
        {
            if ( _state != PromiseState.Pending )
            {
                return;
            }

            InvokeResolvedHander (_onResolve , param);
            _state = PromiseState.Resolved;
            _currentParam = param;
            _onResolve = null;
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
    }
}