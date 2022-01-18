namespace PAX.Next.TaskManager.Dtos
{
    public class GetTaskLabelForViewDto
    {
        public TaskLabelDto TaskLabel { get; set; }

        public string PaxTaskHeader { get; set; }

        public string LabelName { get; set; }

    }
}