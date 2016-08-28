using System.Collections.Generic;

namespace ThermometerLibrary
{
    public class TemperatureHandler
    {
        private readonly double[] _dataTemp;
        public List<IChecker> HandlersActions = new List<IChecker>();

        public TemperatureHandler(double[] dataTemp)
        {
            _dataTemp = dataTemp;
        }

        public void AddListener(IChecker checker)
        {
            HandlersActions.Add(checker);
        }

        public void Start()
        {
            foreach (var data in _dataTemp)
            {
                HandlersActions.ForEach(c => c.Notify(data));
            }
        }
    }
}