﻿using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Request;

namespace Infrastructure.Services;

public class BankService : IBankService
{
    private readonly IBankRepository _bankRepository;

    public BankService(IBankRepository bankRepository)
    {
        _bankRepository = bankRepository;
    }

    public async Task<BankDTO> Add(CreateBankModel model)
    {
        bool isNameTaken = await _bankRepository.NameIsAlreadyTaken(model.Name);
        if (isNameTaken)
        {
            throw new BusinessLogicException($"Bank name {model.Name} is already taken");
        }

        return await _bankRepository.Add(model);
    }


    public async Task<bool> Delete(int id)
    {
        return await _bankRepository.Delete(id);
    }

    public async Task<List<BankDTO>> GetAll()
    {
        return await _bankRepository.GetAll();
    }

    public async Task<BankDTO> GetById(int id)
    {
        return await _bankRepository.GetById(id);
    }

    public async Task<BankDTO> Update(UpdateBankModel model)
    {
        return await _bankRepository.Update(model);
    }
}
