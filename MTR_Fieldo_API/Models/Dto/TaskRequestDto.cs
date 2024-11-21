using Application.Common;
using System.Reflection.Metadata.Ecma335;

namespace MTR_Fieldo_API.Models.Dto
{
    public class TaskRequestDto
    {
       // public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Address { get; set; }
        public int CategoryId { get; set; }
        public List<string>? Documents { get; set; }

        // Add these for status management
        //public string Status { get; set; }
        //public RequestStatus ViewStatus { get; set; }
        //public string PaymentStatus { get; set; }
    }
}
