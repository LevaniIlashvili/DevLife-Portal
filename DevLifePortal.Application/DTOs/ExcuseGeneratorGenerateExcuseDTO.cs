using DevLifePortal.Domain.Enums;

namespace DevLifePortal.Application.DTOs
{
    public class ExcuseGeneratorGenerateExcuseDTO
    {
        public string Category { get; set; }
        public ExcuseType Type { get; set; }
    }
}
