using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IRepository<Review> _repo;
    private readonly IMapper _mapper;
    private readonly IRatingService _ratingService;

    public ReviewController(IRepository<Review> repo,
        IMapper mapper,
        IRatingService ratingService)
    {
        _repo = repo;
        _mapper = mapper;
        _ratingService = ratingService;
    }
    
    [HttpGet]
    public async Task<IReadOnlyList<ReviewDto>> GetAllReviews()
    {
        var reviews = await _repo.ListAsync();
        var reviewsDto = _mapper.Map<IReadOnlyList<ReviewDto>>(reviews);
        return reviewsDto;
    }
    
    
    [HttpGet("id")]
    [Authorize]
    public async Task<ReviewDto> GetReviewById(int id)
    {
        var review = await _repo.GetbyIDAsync(id);
        var reviewDto = _mapper.Map<ReviewDto>(review);
        return reviewDto;
    }
    
    [HttpGet("product/{productId}")]
    [Authorize]
    public async Task<IReadOnlyList<ReviewDto>> GetReviewByProductId(int productId)
    {
        var reviews = await _repo.ListAsync(x => x.ProductID == productId);
        var reviewDto = _mapper.Map<IReadOnlyList<ReviewDto>>(reviews);
        return reviewDto;
    }
    
    
    [HttpPost]
    [Authorize]
    public async Task<ReviewDto> AddReview(ReviewDto reviewDto)
    {
        reviewDto.UserID = GetAuthorizedUserId();
        var review =  _mapper.Map<Review>(reviewDto);  
        await _repo.AddAsync(review);
        reviewDto = _mapper.Map<ReviewDto>(review);
        await _ratingService.UpdateAverageRating(review.ProductID);

        return reviewDto;
    }
    
    private string GetAuthorizedUserId()
    {
        var  userId= User.Claims.Where(x => x.Type == "uid").FirstOrDefault()?.Value;
        return userId;
    }
}