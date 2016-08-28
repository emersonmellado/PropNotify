namespace ThermometerLibrary
{
    public class TemperatureConverter
    {
        public double CelsiusToFahrenheit(double temp)
        {
            return (temp * 9 / 5 + 32);
        }

        public double FahrenheitToCelsius(double temp)
        {
            return ((temp - 32) * 5 / 9);
        }
    }
}