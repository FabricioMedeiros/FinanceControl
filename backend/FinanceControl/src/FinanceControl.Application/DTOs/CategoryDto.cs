﻿using FinanceControl.Domain.Enums;

namespace FinanceControl.Application.DTOs
{
    public class CategoryDto
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public CategoryType? Type { get; set; }
    }
}
