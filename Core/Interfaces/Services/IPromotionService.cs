﻿using Core.Models;
using Core.Request;

namespace Core.Interfaces.Services;

public interface IPromotionService
{
    Task<List<PromotionDTO>> GetFiltered(FilterPromotionModel filter);
    Task<PromotionDTO> Add(CreatePromotionModel model);
    Task<PromotionDTO> Update(UpdatePromotionModel model);
    Task<bool> Delete(int id);
}