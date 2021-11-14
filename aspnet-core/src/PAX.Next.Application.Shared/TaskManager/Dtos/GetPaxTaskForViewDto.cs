namespace PAX.Next.TaskManager.Dtos
{
    public class GetPaxTaskForViewDto
    {
        public PaxTaskDto PaxTask { get; set; }

        public string UserName { get; set; }

        public string UserName2 { get; set; }

        public string SeverityName { get; set; }

        public string TaskStatusName { get; set; }

    }
}