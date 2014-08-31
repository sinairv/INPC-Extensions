using System;

namespace NotifyPropertyChangedExtensions
{
    [Flags]
    public enum BindPropertyOptions
    {
        None = 0,

        /// <summary>
        /// When set, initialises the target property with the value of the source property, without waiting for a PropertyChanged event being raised.
        /// </summary>
        InitialiseTarget = 1,

        /// <summary>
        /// If this option is not set, target will become 0 for integral types, or DateTime.MinValue for DateTime
        /// </summary>
        DontUpdateWhenSourceIsNullAndTargetIsNotNullable = 2
    }
}