using System.Collections.Generic;
using System.Linq;

namespace Primise4CSharp
{
    public static class PromiseHelper
    {
        public static IPromise All (IEnumerable<IPromise> pomiseList)
        {
            Promise promise = new Promise ();
            int resolvedCount = 0;
            int totalCount = pomiseList.Count ();

            foreach ( var item in pomiseList )
            {
                item.Then (() =>
                {
                    resolvedCount++;
                    if ( resolvedCount >= totalCount )
                    {
                        promise.Resolve ();
                    }
                });
            }
            return promise;
        }

        public static IPromise Any (IEnumerable<IPromise> pomiseList)
        {
            Promise promise = new Promise ();

            foreach ( var item in pomiseList )
            {
                item.Then (() =>
                {
                    promise.Resolve ();
                });
            }
            return promise;
        }
    }
}