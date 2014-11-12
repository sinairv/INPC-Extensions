using System;
using System.ComponentModel;

namespace NotifyPropertyChangedExtensions.Tests
{
    public class SourceClass : INotifyPropertyChanged
    {
        private int _number;
        private int? _optionalNumber;
        private string _string;
        private DateTime _date;
        private DateTime? _optionalDate;
        private bool _flag;
        private bool? _optionalFlag;

        public int Number
        {
            get { return _number; }
            set { _number = value; this.RaisePropertyChanged(() => Number); }
        }

        public int? OptionalNumber
        {
            get { return _optionalNumber; }
            set { _optionalNumber = value; this.RaisePropertyChanged(() => OptionalNumber); }
        }

        public string String
        {
            get { return _string; }
            set { _string = value; this.RaisePropertyChanged(() => String); }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; this.RaisePropertyChanged(() => Date); }
        }

        public DateTime? OptionalDate
        {
            get { return _optionalDate; }
            set { _optionalDate = value; this.RaisePropertyChanged(() => OptionalDate); }
        }

        public bool Flag
        {
            get { return _flag; }
            set { _flag = value; this.RaisePropertyChanged(() => Flag); }
        }

        public bool? OptionalFlag
        {
            get { return _optionalFlag; }
            set { _optionalFlag = value; this.RaisePropertyChanged(() => OptionalFlag); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}