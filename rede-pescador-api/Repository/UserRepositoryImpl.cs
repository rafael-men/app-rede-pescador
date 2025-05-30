﻿using Microsoft.EntityFrameworkCore;
using rede_pescador_api.Data;
using rede_pescador_api.Models;

public class UserRepositoryImpl : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepositoryImpl(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByEmailAsync(string email)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User> GetByPhoneAsync(string phone)
        => await _context.Users.FirstOrDefaultAsync(u => u.Phone == phone);

    public async Task<User> GetByUserIdAsync(long id)
        => await _context.Users.FindAsync(id);

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user); 
        await _context.SaveChangesAsync(); 
    }

    public async Task UpdateProfileImageAsync(User user)
    {
        _context.Attach(user);
        _context.Entry(user).Property(u => u.ProfileImageUrl).IsModified = true;
        await _context.SaveChangesAsync();
    }
}

