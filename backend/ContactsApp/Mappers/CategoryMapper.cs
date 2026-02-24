using ContactsApp.Domain.Dto;
using ContactsApp.Domain.Entities;

namespace ContactsApp.Mappers;

public static class CategoryMapper
{
    public static CategoryDto ToDto(this Category category)
    {
        return new(
            category.Id,
            category.Name,
            category.Subcategories
                .Select(subcat => new SubcategoryDto(subcat.Id, subcat.Name))
                .ToList()
        );
    }
}
