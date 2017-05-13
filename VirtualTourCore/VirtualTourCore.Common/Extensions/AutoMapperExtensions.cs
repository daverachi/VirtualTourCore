using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualTourCore.Common.Extensions
{
    /// <summary>
    /// Represents an extension to extend the AutoMapper.
    /// </summary>
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// Configures the AutoMapper to ignorei all members that do not exist in the destination.
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TDestination">Destination type.</typeparam>
        /// <param name="expression">Lambda expression</param>
        /// <returns>Instance of an IMappingExpression from the specified expression (Fluent.)</returns>
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            Type sourceType = typeof(TSource);
            Type destinationType = typeof(TDestination);
            TypeMap existingMaps = Mapper.GetAllTypeMaps().First(x => x.SourceType.Equals(sourceType) && x.DestinationType.Equals(destinationType));

            foreach (string property in existingMaps.GetUnmappedPropertyNames())
            {
                expression.ForMember(property, opt => opt.Ignore());
            }
            return expression;
        }
    }
}
