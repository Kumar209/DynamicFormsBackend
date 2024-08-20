using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.Models.Dto
{
    public class TemplateSectionDto
    {
        public int? Id { get; set; }

        public int FormId { get; set; }

        public string SectionName { get; set; }

        public string? Description { get; set; }

        public int? Slno { get; set; }

        public int? CreatedBy { get; set; }

        public List<int> SelectedQuestions { get; set; }
    }
}
