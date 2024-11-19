using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class CategorieController : ControllerBase
{
    private readonly IRepository<Categorie> _repo;
    private readonly IMapper _mapper;

    public CategorieController(IRepository<Categorie> repository,
        IMapper mapper)
    {
        _repo = repository;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<IReadOnlyList<CategorieDto>> GetAllCategorie()
    {
        var categorie = await _repo.ListAsync();
        var categorieDto = _mapper.Map<IReadOnlyList<CategorieDto>>(categorie);
        return categorieDto;
    }

    [HttpGet("id")]
    public async Task<CategorieDto> GetCategorieById(int id)
    {
        var categorie = await _repo.GetbyIDAsync(id);
        var categorieDto = _mapper.Map<CategorieDto>(categorie);
        return categorieDto;
    }

    [HttpPost]
    public async Task<CategorieDto> AddCategorie(CategorieDto categorieDto)
    {
        var categorie =  _mapper.Map<Categorie>(categorieDto);
        await _repo.AddAsync(categorie);
        categorieDto = _mapper.Map<CategorieDto>(categorie);
        return categorieDto;
    }

}