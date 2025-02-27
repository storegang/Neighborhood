﻿using Microsoft.EntityFrameworkCore;
using webapi.DataContexts;
using webapi.Models;

namespace webapi.Repositories;

public interface ICategoryRepository
{
    ICollection<Category> GetAll();
    Category GetById(string id);
    void Add(Category category);
    void Update(Category category);
    void Delete(Category category);
}

public class CategoryRepository(NeighborhoodContext context) : ICategoryRepository
{
    private readonly NeighborhoodContext _context = context;

    public ICollection<Category> GetAll()
    {
        return _context.Categories.ToList();
    }

    public Category GetById(string id)
    {
        return _context.Categories.Find(id);
    }

    public void Add(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }

    public void Update(Category category)
    {
        var existingCategory = _context.Categories.Find(category.Id);
        if (existingCategory != null)
        {
            _context.Entry(existingCategory).State = EntityState.Detached;
        }

        _context.Categories.Update(category);
        _context.SaveChanges();
    }

    public void Delete(Category category)
    {
        _context.Categories.Remove(category);
        _context.SaveChanges();
    }
}
