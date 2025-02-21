using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Microsoft.Maui.Essentials.Implementations
{
	public partial class GeolocationImplementation : IGeolocation
	{
		public async Task<Location> GetLastKnownLocationAsync()
		{
			// no need for permissions as AllowFallbackToConsentlessPositions
			// will allow the device to return a location regardless

			var geolocator = new Geolocator
			{
				DesiredAccuracy = PositionAccuracy.Default,
			};
			geolocator.AllowFallbackToConsentlessPositions();

			var location = await geolocator.GetGeopositionAsync().AsTask();

			return location?.Coordinate?.ToLocation();
		}

		public async Task<Location> GetLocationAsync(GeolocationRequest request, CancellationToken cancellationToken)
		{
			await Permissions.EnsureGrantedAsync<Permissions.LocationWhenInUse>();

			var geolocator = new Geolocator
			{
				DesiredAccuracyInMeters = request.PlatformDesiredAccuracy
			};

			CheckStatus(geolocator.LocationStatus);

			cancellationToken = Utils.TimeoutToken(cancellationToken, request.Timeout);

			var location = await geolocator.GetGeopositionAsync().AsTask(cancellationToken);

			return location?.Coordinate?.ToLocation();

			void CheckStatus(PositionStatus status)
			{
				switch (status)
				{
					case PositionStatus.Disabled:
					case PositionStatus.NotAvailable:
						throw new FeatureNotEnabledException("Location services are not enabled on device.");
				}
			}
		}
	}
}
