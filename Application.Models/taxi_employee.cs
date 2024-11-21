using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class taxi_employee
    {
        [Key]
        public int Id { get; set; }
        public string? LoginId { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? HomePhone { get; set; }
        public string? CellPhone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? StateId { get; set; }
        public string? Position { get; set; }
        public DateTime DateHired { get; set; }
        public DateTime TerminationDate { get; set; }
        public string? Notes { get; set; }
        public int? domainid { get; set; }
        public int? IsAdmin { get; set; }
        public int? IsFlatPayroll { get; set; }
        public int? HourlyRate { get; set; }
        public int? FlatRate { get; set; }
        public string? StartDay { get; set; }
        public string? EndDay { get; set; }
        public DateTime? DOB { get; set; }
        public string? SSNo { get; set; }
        public int? AccountingUser { get; set; }
        public int? AllowRestrictions { get; set; }
        public string? ExtNo { get; set; }
        public bool? Status { get; set; }
        public string? Profile { get; set; }
        public string? CountryCode { get; set; }
        public int? HotelId { get; set; }
    }
}
