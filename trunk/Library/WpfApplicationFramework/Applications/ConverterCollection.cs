using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace System.Waf.Applications
{
    /// <summary>
    /// Obsolete: Represents a collection that converts all elements of an original collection into converted elements. 
    /// When the original collection notifies a change via the <see cref="INotifyCollectionChanged"/> interface then
    /// this collection synchronizes it's converted elements with the original one.
    /// </summary>
    /// <typeparam name="TNew">The type of the converted elements in the collection.</typeparam>
    /// <typeparam name="TOld">The type of elements in the original collection.</typeparam>
    [Obsolete("This collection is obsolete. Please use the SynchronizingCollection instead. The SynchronizingCollection is compatible to this one.")]
    public sealed class ConverterCollection<TNew, TOld> : SynchronizingCollection<TNew, TOld>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConverterCollection&lt;TNew, TOld&gt;"/> class.
        /// </summary>
        /// <param name="originalCollection">The original collection.</param>
        /// <param name="converter">The converter used to create the elements in this collection.</param>
        /// <exception cref="ArgumentNullException">The argument originalCollection must not be null.</exception>
        /// <exception cref="ArgumentNullException">The argument converter must not be null.</exception>
        public ConverterCollection(IEnumerable<TOld> originalCollection, Func<TOld, TNew> converter)
            : base(originalCollection, converter)
        {
        }
    }
}
