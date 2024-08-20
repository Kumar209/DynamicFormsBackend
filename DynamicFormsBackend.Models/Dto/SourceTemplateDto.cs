using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.Models.Dto
{
    public class SourceTemplateDto
    {
        public int? Id { get; set; }

        public string FormName { get; set; }

        public string? Description { get; set; }

        public bool? IsPublish { get; set; }

        public int? Version { get; set; }

        public List<TemplateSectionDto> Sections { get; set; }
    }
}
