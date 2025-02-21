using Tizen.Sensor;
using TizenAccelerometer = Tizen.Sensor.Accelerometer;

namespace Microsoft.Maui.Essentials.Implementations
{
	public partial class AccelerometerImplementation
	{
		internal static TizenAccelerometer DefaultSensor =>
			(TizenAccelerometer)Platform.GetDefaultSensor(SensorType.Accelerometer);

		internal static bool IsSupported =>
			TizenAccelerometer.IsSupported;

		void PlatformStart(SensorSpeed sensorSpeed)
		{
			DefaultSensor.Interval = sensorSpeed.ToPlatform();
			DefaultSensor.DataUpdated += DataUpdated;
			DefaultSensor.Start();
		}

		void PlatformStop()
		{
			DefaultSensor.DataUpdated -= DataUpdated;
			DefaultSensor.Stop();
		}

		void DataUpdated(object sender, AccelerometerDataUpdatedEventArgs e)
		{
			OnChanged(new AccelerometerData(e.X, e.Y, e.Z));
		}
	}
}
