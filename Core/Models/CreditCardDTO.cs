﻿using Core.Constants;
using Core.Entities;

namespace Core.Models; 

public class CreditCardDTO
{
    public int Id { get; set; }
    public string Designation { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public int Cvv { get; set; }
    public ECreditCardStatus CreditCardStatus { get; set; } = ECreditCardStatus.Enabled;


    public decimal CreditLimit { get; set; }
    public decimal AvailableCredit { get; set; }
    public decimal CurrentDebt { get; set; }
    public decimal InterestRate { get; set; }

    public string Currency { get; set; } = null!;
    public string Customer { get; set; } = null!;

    public string MaskedCardNumber
    {
        get
        {
            if (string.IsNullOrEmpty(CardNumber) || CardNumber.Length < 16)
                return string.Empty;

            return "****-****-****-" + CardNumber.Substring(CardNumber.Length - 4);
        }
    }
}
