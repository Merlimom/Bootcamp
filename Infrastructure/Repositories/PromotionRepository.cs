using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Request;
using Infrastructure.Contexts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PromotionRepository : IPromotionRepository
{
    private readonly BootcampContext _context;

    public PromotionRepository(BootcampContext context)
    {
        _context = context;
    }

    public async Task<PromotionDTO> Add(CreatePromotionModel model)
    {
        var query = _context.Promotions
                          .Include(a => a.Business)
                          .AsQueryable();

        var promotionToCreate = model.Adapt<Promotion>();

        _context.Promotions.Add(promotionToCreate);

        await _context.SaveChangesAsync();

        var result = await query.ToListAsync();

        var promotionDTO = promotionToCreate.Adapt<PromotionDTO>();

        return promotionDTO;
    }

    public async Task<bool> Delete(int id)
    {
        var promotion = await _context.Promotions.FindAsync(id);

        if (promotion is null) throw new NotFoundException($"Bank with id: {id} doest not exist");

        _context.Promotions.Remove(promotion);

        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<List<PromotionDTO>> GetFiltered(FilterPromotionModel filter)
    {
        var query = _context.Promotions
                          .Include(a => a.Business)
                          .AsQueryable();

        if (filter.Id is not null)
        {
            query = query.Where(x =>
                 x.Id != null &&
                (x.Id).Equals(filter.Id));
        }

        if (filter.Name is not null)
        {
            string normalizedFilterName = filter.Name.ToLower();
            query = query.Where(x =>
                (x.Name).ToLower().Equals(normalizedFilterName));
        }

        var result = await query.ToListAsync();
        var promotionDTO = result.Adapt<List<PromotionDTO>>();
        return promotionDTO;
    }

    public async Task<PromotionDTO> Update(UpdatePromotionModel model)
    {
        var query = _context.Promotions
                         .Include(a => a.Business)
                         .AsQueryable();

        var promotion = await _context.Promotions.FindAsync(model.Id);

        if (promotion is null) throw new Exception("Bank was not found");

        model.Adapt(promotion);

        _context.Promotions.Update(promotion);

        await _context.SaveChangesAsync();

        var result = await query.ToListAsync();

        var promotionDTO = promotion.Adapt<PromotionDTO>();

        return promotionDTO;
    }
}
