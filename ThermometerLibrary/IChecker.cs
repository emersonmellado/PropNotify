using System.Collections.Generic;

namespace ThermometerLibrary
{
    public interface IChecker : INotify
    {
        List<double> NotifiedTempList { get; set; }

        string Action();
        double GetBoilingPoint();
        double GetCelsius();
        double GetFahrenheit();
        double GetFreezingPoint();
        string GetName();
    }
}