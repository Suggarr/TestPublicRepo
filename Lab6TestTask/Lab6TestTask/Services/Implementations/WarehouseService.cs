using Lab6TestTask.Data;
using Lab6TestTask.Models;
using Lab6TestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Lab6TestTask.Enums;
using System.Linq;

namespace Lab6TestTask.Services.Implementations;

/// <summary>
/// WarehouseService.
/// Implement methods here.
/// </summary>
public class WarehouseService : IWarehouseService
{
    private readonly ApplicationDbContext _dbContext;

    public WarehouseService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Warehouse> GetWarehouseAsync()
    {
        return await _dbContext.Warehouses
            .Select(w => new Warehouse
            {
                WarehouseId = w.WarehouseId,
                Name = w.Name,
                Location = w.Location,
                Products = w.Products
                    .Where(p => p.Status == ProductStatus.ReadyForDistribution)
                    .ToList()
            })
            .OrderByDescending(w => w.Products.Sum(p => p.Quantity * p.Price))
            .FirstAsync();
    }


    public async Task<IEnumerable<Warehouse>> GetWarehousesAsync()
    {
        return await _dbContext.Warehouses
            .Where(w => w.Products.Any(p =>
                p.ReceivedDate.Year == 2025 &&
                p.ReceivedDate.Month >= 4 &&
                p.ReceivedDate.Month <= 6))
            .Select(w => new Warehouse
            {
                WarehouseId = w.WarehouseId,
                Name = w.Name,
                Location = w.Location,
                Products = w.Products
                    .Where(p =>
                        p.ReceivedDate.Year == 2025 &&
                        p.ReceivedDate.Month >= 4 &&
                        p.ReceivedDate.Month <= 6)
                    .ToList()
            })
            .ToListAsync();
    }

}
