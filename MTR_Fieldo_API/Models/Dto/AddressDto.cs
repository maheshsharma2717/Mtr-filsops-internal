namespace MTR_Fieldo_API.Models.Dto
{
    public class AddressDto
    {
        public string? Latitude { get; set; }
        public string Longitude { get; set; } = "";
        public string FormattedAddress
        {
            get
            {
                string address = string.Empty;
                if (!string.IsNullOrEmpty(this.StreetAddress))
                {
                    address += $"{this.StreetAddress}, ";
                }
                if (!string.IsNullOrEmpty(this.City))
                {
                    address += $"{this.City}, ";
                }
                if (!string.IsNullOrEmpty(this.State))
                {
                    address += $"{this.State}, ";
                }
                if (!string.IsNullOrEmpty(this.PostalCode))
                {
                    address += $"{this.PostalCode}, ";
                }
                if (!string.IsNullOrEmpty(this.Country))
                {
                    address += $"{this.Country}.";
                }

                return address;
            }
        }
        public string? PlaceName { get; set; }   //for landmark
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string? AdditionalMetadata { get; set; }
    }
}
