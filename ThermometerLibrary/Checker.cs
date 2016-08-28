using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ThermometerLibrary.ObservablePattern;

namespace ThermometerLibrary
{
    public class Checker : IChecker, IObsNotify<double>
    {
        #region Constructor

        public Checker(string name, double boilingPoint, double freezingPoint, double threshold)
            : this(name, boilingPoint, freezingPoint, threshold, Direction.Both, 1)
        {
        }

        public Checker(string name, double boilingPoint, double freezingPoint, double threshold, Direction direction, int notifiedTimes)
        {
            _direction = direction;
            _boilingPoint = boilingPoint;
            _freezingPoint = freezingPoint;
            _threshold = threshold;
            _notifiedTimes = notifiedTimes;
            _name = name;
            NotifiedTempList = new List<double>();
        }

        #endregion

        #region Private Properties
        private readonly string _name;
        private readonly double _freezingPoint;
        private readonly double _boilingPoint;
        private readonly double _threshold;
        private readonly int _notifiedTimes;

        private double _actualTemp;
        private double _uLimit;
        private double _lLimit;
        private readonly Direction _direction;
        private bool _tempHit;
        private bool _alreadyStarted;
        private Guid? _guid;

        private bool Freezed() => _actualTemp <= GetFreezingPoint();
        private bool Boiled() => _actualTemp >= GetBoilingPoint();
        #endregion

        #region Public properties
        public List<double> NotifiedTempList { get; set; }
        public string GetName() => _name;
        public double GetFreezingPoint() => _freezingPoint;
        public double GetBoilingPoint() => _boilingPoint;
        public double GetFahrenheit() => new TemperatureConverter().CelsiusToFahrenheit(_actualTemp);
        public double GetCelsius() => _actualTemp;
        public string Action() => Freezed() ? "Freezed" : "Boiled";
        #endregion

        #region Methods

        public void Notify(double actualTemp)
        {
            SetVars(actualTemp);

            if (NotifiedTempList.Count(a => a <= _uLimit && a >= _lLimit) >= _notifiedTimes)
                return;

            switch (_direction)
            {
                case Direction.Both:
                    if (Freezed() || Boiled())
                        HitTemp(actualTemp);
                    break;
                case Direction.DownToFreezing:
                    if (Freezed())
                        HitTemp(actualTemp);
                    break;
                case Direction.UpToBoiling:
                    if (Boiled())
                        HitTemp(actualTemp);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HitTemp(double actualTemp)
        {
            NotifiedTempList.Add(actualTemp);
            _tempHit = true;
        }

        private void SetVars(double actualTemp)
        {
            _tempHit = false;
            _actualTemp = actualTemp;
            _uLimit = (_actualTemp + _threshold);
            _lLimit = (_actualTemp - _threshold);
        }

        #endregion

        public void OnNext(double value)
        {
            Debug.WriteLine($"\tChecking: {value}");
            Notify(value);
        }

        public void OnNext(Checker value)
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            Debug.WriteLine("Error");
        }

        public void OnCompleted()
        {
            if (_tempHit)
                Debug.WriteLine($"\tTemperature Hit: {_actualTemp}");
        }

        public void OnBegin()
        {
            if (_alreadyStarted)
                return;

            _alreadyStarted = true;
            Debug.WriteLine($"{GetName()} - Freeze At: {GetFreezingPoint()} - Boiling At: {GetBoilingPoint()}");
        }

        public Guid GetCode()
        {
            return (Guid)(_guid ?? (_guid = new Guid()));
        }

        public void OnNotify(double mod, PropertyInfo propertyInfo)
        {
            throw new NotImplementedException();
        }

        public Expression<Func<double, object>>[] PropsMonitored { get; set; }

        public void OnExecute(double mod, string pMsg)
        {
            throw new NotImplementedException();
        }
    }
}