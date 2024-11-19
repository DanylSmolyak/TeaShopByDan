using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IRepository<Blog> _blogRepository;
        private readonly IMapper _mapper;

        public BlogController(IRepository<Blog> blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        // Получить все блоги
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BlogDto>>> GetAllBlogs()
        {
            var blogs = await _blogRepository.ListAsync();
            var blogDtos = _mapper.Map<IReadOnlyList<BlogDto>>(blogs);
            return Ok(blogDtos);
        }

        // Получить блог по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogDto>> GetBlogById(int id)
        {
            var blog = await _blogRepository.GetbyIDAsync(id);
            if (blog == null)
                return NotFound();

            var blogDto = _mapper.Map<BlogDto>(blog);
            return Ok(blogDto);
        }

        // Добавить новый блог
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BlogDto>> AddBlog([FromBody] BlogDto blogDto)
        {
            var blog = _mapper.Map<Blog>(blogDto);

            // Назначаем UserID из токена авторизованного пользователя
            blog.UserID = GetAuthorizedUserId();

            await _blogRepository.AddAsync(blog);
            return Ok(_mapper.Map<BlogDto>(blog));
        }

        // Обновить существующий блог
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] BlogDto blogDto)
        {
            var blog = await _blogRepository.GetbyIDAsync(id);
            if (blog == null)
                return NotFound();

            // Проверяем, что авторизованный пользователь — владелец блога
            if (blog.UserID != GetAuthorizedUserId())
                return Forbid();

            // Обновляем блог
            _mapper.Map(blogDto, blog);
            await _blogRepository.EditAsync(blog);
            return NoContent();
        }

        // Удалить блог
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _blogRepository.GetbyIDAsync(id);
            if (blog == null)
                return NotFound();

            // Только автор блога может его удалить
            if (blog.UserID != GetAuthorizedUserId())
                return Forbid();

            await _blogRepository.DeleteAsync(blog);
            return NoContent();
        }

        // Метод для получения идентификатора авторизованного пользователя
        private string GetAuthorizedUserId()
        {
            // Получаем UserID из токена авторизации
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }
    }
}
