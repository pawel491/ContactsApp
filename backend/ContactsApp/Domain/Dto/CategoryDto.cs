namespace ContactsApp.Domain.Dto;


public record class CategoryDto(
    int Id,
    string Name,
    List<SubcategoryDto> Subcategories);
