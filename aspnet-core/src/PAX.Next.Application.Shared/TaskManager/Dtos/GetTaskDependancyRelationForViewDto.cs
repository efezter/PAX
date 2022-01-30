namespace PAX.Next.TaskManager.Dtos
{
    public class GetTaskDependancyRelationForViewDto
    {
        public TaskDependancyRelationDto TaskDependancyRelation { get; set; }

        public string PaxTaskHeader { get; set; }

        public string PaxTaskHeader2 { get; set; }

    }
}