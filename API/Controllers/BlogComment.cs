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
    public class BlogCommentController : ControllerBase
    {
        private readonly IRepository<BlogComment> _commentRepository;
        private readonly IMapper _mapper;

        public BlogCommentController(IRepository<BlogComment> commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        // Получить все комментарии
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BlogCommentDto>>> GetAllComments()
        {
            var comments = await _commentRepository.ListAsync();
            var commentDtos = _mapper.Map<IReadOnlyList<BlogCommentDto>>(comments);
            return Ok(commentDtos);
        }

        // Получить комментарий по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogCommentDto>> GetCommentById(int id)
        {
            var comment = await _commentRepository.GetbyIDAsync(id);
            if (comment == null)
                return NotFound();

            var commentDto = _mapper.Map<BlogCommentDto>(comment);
            return Ok(commentDto);
        }

        // Добавить комментарий
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BlogCommentDto>> AddComment([FromBody] BlogCommentDto commentDto)
        {
            var comment = _mapper.Map<BlogComment>(commentDto);

            // Получаем идентификатор авторизованного пользователя
            comment.UserID = GetAuthorizedUserId();

            await _commentRepository.AddAsync(comment);
            return Ok(_mapper.Map<BlogCommentDto>(comment));
        }

        // Обновить комментарий
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] BlogCommentDto commentDto)
        {
            var comment = await _commentRepository.GetbyIDAsync(id);
            if (comment == null)
                return NotFound();

            // Проверяем, принадлежит ли комментарий авторизованному пользователю
            if (comment.UserID != GetAuthorizedUserId())
                return Forbid();

            _mapper.Map(commentDto, comment);
            await _commentRepository.EditAsync(comment);
            return NoContent();
        }

        // Удалить комментарий
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _commentRepository.GetbyIDAsync(id);
            if (comment == null)
                return NotFound();

            // Проверяем, принадлежит ли комментарий авторизованному пользователю
            if (comment.UserID != GetAuthorizedUserId())
                return Forbid();

            await _commentRepository.DeleteAsync(comment);
            return NoContent();
        }

        // Метод для получения идентификатора авторизованного пользователя
        private string GetAuthorizedUserId()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }
    }
}
