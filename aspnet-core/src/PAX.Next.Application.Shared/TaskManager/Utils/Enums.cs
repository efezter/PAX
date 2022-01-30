using System;
using System.Collections.Generic;
using System.Text;

namespace PAX.Next.TaskManager.Utils
{
    public class Enums
    {
        public enum TaskType
        {
            Normal = 1,
            Repating = 2,
            DeadLine = 3
        }

        public enum TaskTypePeriod
        {
            Weekly = 1,
            Monthly = 2
        }
    }
}
