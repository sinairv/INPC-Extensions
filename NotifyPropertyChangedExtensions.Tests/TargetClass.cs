using System;

namespace NotifyPropertyChangedExtensions.Tests
{
    public class TargetClass
    {
        public int Number { get; set; }
        public int? OptionalNumber { get; set; }
        public string String { get; set; }
        public DateTime Date { get; set; }
        public DateTime? OptionalDate { get; set; }
        public bool Flag { get; set; }
        public bool? OptionalFlag { get; set; }
    }
}