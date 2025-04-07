using AutoMapper;
using FinanceControl.API.Extensions;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Application.Validators;
using FinanceControl.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CategoryController : MainController
    {
        readonly ICategoryService _categoryService;
        readonly IMapper _mapper;

        public CategoryController(INotificator notificator, ICategoryService categoryService, IMapper mapper) : base(notificator)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Dictionary<string, string>? filters, [FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null)
        {
            var categories = await _categoryService.GetAllAsync(filters, pageNumber, pageSize, Guid.Parse(UserId));
            return CustomResponse(categories);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null) return NotFound();

            return CustomResponse(category);
        }

        [HttpPost]
        [ValidationContext(typeof(CategoryCreateValidator))]
        public async Task<IActionResult> Create([FromBody] CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            category.UserId = Guid.Parse(UserId);

            var createdCategory = await _categoryService.AddAsync(category);
            return CustomResponse(createdCategory);
        }

        [HttpPut("{id:Guid}")]
        [ValidationContext(typeof(CategoryUpdateValidator))]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryDto categoryDto)
        {
            if (id != categoryDto.Id)
            {
                NotifyError("O ID informado não é o mesmo que foi passado na query.");
                return CustomResponse();
            }

            var category = await _categoryService.GetByIdAsync(categoryDto.Id ?? Guid.Empty, true);

            if (category == null)
                return NotFound();

            _mapper.Map(categoryDto, category);

            await _categoryService.UpdateAsync(category);

            return CustomResponse(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null) return NotFound();

            await _categoryService.DeleteAsync(id);
            return CustomResponse();
        }
    }
}
