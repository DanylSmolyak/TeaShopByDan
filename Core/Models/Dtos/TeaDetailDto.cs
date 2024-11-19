using Core.Entities;

namespace Core.Dtos;

public class TeaDetailDto : BaseEntitie
{
    public string History { get; set; }
    public string PreparationGuide { get; set; }
    public string TastingNotes { get; set; }
    public string Origin { get; set; }
    public string StorageInstructions { get; set; }
    public int ProductId { get; set; } 
}