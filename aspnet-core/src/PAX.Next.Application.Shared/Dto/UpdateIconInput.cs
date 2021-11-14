using System;
using System.ComponentModel.DataAnnotations;
using Abp.Extensions;
using Abp.Runtime.Validation;

namespace PAX.Next.Dto
{
    public class UpdateIconInput : ICustomValidate
    {
        [MaxLength(400)]
        public string FileToken { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int TaskStatusId { get; set; }
        public void AddValidationErrors(CustomValidationContext context)
        {

            if (FileToken.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(FileToken));
            }
        }
    }
}