using System;
using System.Collections.Generic;
using System.Diagnostics;
using PowerLog;

namespace PowerLog.Filters.Predicate
{
    #region PredicateFilter Class XML
    /// <summary>
    /// Filters logs by a user-defined predicate.
    /// </summary>
    #endregion
    public class PredicateFilter : IFilter
    {
        #region Identifier String XML
        /// <summary>
        /// The filter identifier / name.
        /// </summary>
        #endregion
        public string Identifier { get; }

        #region Logger Log XML
        /// <summary>
        /// The filter logger.
        /// </summary>
        #endregion
        public Log Logger { get; }

        #region IsEnabled Boolean XML
        /// <summary>
        /// Determines if the filter is enabled.
        /// </summary>
        #endregion
        public bool IsEnabled { get; set; }

        // Private / hidden variables..
        private Predicate<Arguments> FilterPredicate { get; init; }


        #region Filter Method XML
        /// <inheritdoc/>
        #endregion
        public bool Filter(Arguments Log) {
            return ((bool)(FilterPredicate?.Invoke(Log)));
        }



        #region PredicateFilter Constructor XML
        /// <summary>
        /// The default <see cref="PredicateFilter"/> constructor.
        /// </summary>
        /// <param name="Identifier">The filter identifier.</param>
        /// <param name="Logger">The logger to apply the filter to.</param>
        /// <param name="Filter">The filter predicate method.</param>
        #endregion
        public PredicateFilter(string Identifier, Log Logger, Predicate<Arguments> Filter)
        {
            this.Identifier = Identifier;
            this.Logger = Logger;
            this.FilterPredicate = Filter;

            this.IsEnabled = true;
        }
    }



    #region PredicateFilterUtilities Class XML
    /// <summary>
    /// Contains extension methods.
    /// </summary>
    #endregion
    public static class PredicateFilterUtilities
    {
        #region FilterByPredicate Function XML
        /// <summary>
        /// Appends a new predicate filter on the logger filter stack.
        /// </summary>
        /// <param name="Logger">The logger to apply the filter to.</param>
        /// <param name="Identifier">The filter identifier.</param>
        /// <param name="FilterPredicate">The filter predicate method.</param>
        /// <returns>The current logger, to allow for method chaining.</returns>
        #endregion
        public static Log FilterByPredicate(this Log Logger, string Identifier, Predicate<Arguments> FilterPredicate)
        {
            PredicateFilter Filter = new PredicateFilter(Identifier, Logger, FilterPredicate);
            Logger.Push(Filter);

            return Logger;
        }
    }
}