using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Request;
using Infrastructure.Contexts;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

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
        var enterprises = await _context.Enterprises.ToListAsync();

        var promotion = model.Adapt<Promotion>();
    

        foreach (int enterpriseId in model.RelatedEnterpriseIds)
        {
            var promotionEnterprise = new PromotionEnterprise
            {
                Promotion = promotion,
                EnterpriseId = enterpriseId
            };
            _context.PromotionEnterprises.Add(promotionEnterprise);
        }

        _context.Promotions.Add(promotion);

        await _context.SaveChangesAsync();

        var createdPromotion = await _context.Promotions
            .FirstOrDefaultAsync(a => a.Id == promotion.Id);

 
        var promotionDTO = createdPromotion.Adapt<PromotionDTO>();

        return promotionDTO;
    }


    public async Task<bool> Delete(int id)
    {
        var promotion = await _context.Promotions.FindAsync(id);

        if (promotion is null) throw new NotFoundException($"Promotion with id: {id} doest not exist");

        _context.Promotions.Remove(promotion);

        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<List<PromotionDTO>> GetFiltered(FilterPromotionModel filter)
    {
        var query = _context.Promotions
                         .Include(a => a.PromotionsEnterprises)
                         .ThenInclude(a => a.Enterprise)
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
                         .Include(a => a.PromotionsEnterprises)
                         .ThenInclude(a => a.Enterprise)
                         .AsQueryable();

        var result = await query.ToListAsync();

        var promotion = await _context.Promotions
        .Include(p => p.PromotionsEnterprises)
        .FirstOrDefaultAsync(p => p.Id == model.Id);

        if (promotion == null)
        {
            throw new NotFoundException("Promotion not found");
        }

        model.Adapt(promotion);

        promotion.PromotionsEnterprises.Clear(); 

        foreach (int enterpriseId in model.RelatedEnterpriseIds)
        {
            var promotionEnterprise = new PromotionEnterprise
            {
                PromotionId = promotion.Id,
                EnterpriseId = enterpriseId
            };
            promotion.PromotionsEnterprises.Add(promotionEnterprise);
        }

        await _context.SaveChangesAsync();
        var promotionDTO = promotion.Adapt<PromotionDTO>();
        return promotionDTO;
    }
}
