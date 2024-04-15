﻿namespace Core.Request;

public class UpdateBusinessModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string? Email { get; set; } = null;
}