namespace PAX.Next.TaskManager.Dtos
{
    public class GetTaskHistoryForViewDto
    {
        public TaskHistoryDto TaskHistory { get; set; }

        public string PaxTaskHeader { get; set; }

        public string UserName { get; set; }

    }
}