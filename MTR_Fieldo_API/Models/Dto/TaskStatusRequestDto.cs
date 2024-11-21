using System.Reflection.Metadata.Ecma335;

namespace MTR_Fieldo_API.Models.Dto
{
    public class TaskStatusRequestDto
    {
        public List<IFormFile> Files { get; set; }
        public int UpdatedBy { get; set; }
        public int TaskId { get; set; }
        public string Description { get; set; }
    }
}
