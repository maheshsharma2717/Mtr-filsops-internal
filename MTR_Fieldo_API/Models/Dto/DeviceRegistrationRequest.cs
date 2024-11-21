namespace MTR_Fieldo_API.Models.Dto
{
    public class DeviceRegistrationRequest
    {

        /// <summary>
        /// The unique token assigned to the device by Firebase Cloud Messaging (FCM).
        /// </summary>
        public string DeviceToken { get; set; }

        /// <summary>
        /// (Optional) The type of device (e.g., Android, iOS, Web).
        /// This can be used to differentiate between types of devices.
        /// </summary>
        public string DeviceType { get; set; }

    }
}
