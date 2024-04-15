﻿using Core.Entities;

namespace Core.Models;

public class PromotionDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DurationTime { get; set; }
    public decimal PercentageOff { get; set; }

    public BusinessDTO Business { get; set; } = null!;
}