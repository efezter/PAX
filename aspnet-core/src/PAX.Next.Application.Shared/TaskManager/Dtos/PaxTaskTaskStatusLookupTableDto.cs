using Abp.Application.Services.Dto;

namespace PAX.Next.TaskManager.Dtos
{
    public class PaxTaskTaskStatusLookupTableDto
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string IconUrl { get; set; }
    }
}