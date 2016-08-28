using System.Collections.Generic;

namespace ThermometerLibrary.ObservablePattern
{
    public class DataTemperatureHolder : Observable<double>
    {
        private List<double> _tempHistory = new List<double>();
        public void AddNewTemp(double temp)
        {
            Notify(temp);
            _tempHistory.Add(temp);
        }
    }
}
