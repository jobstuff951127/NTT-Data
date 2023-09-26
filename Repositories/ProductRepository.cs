﻿using Microsoft.EntityFrameworkCore;
using NTT_Data.Data;
using NTT_Data.Interfaces;
using NTT_Data.Models;
using System.Diagnostics;

namespace NTT_Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductsRepository 
    {
        public ProductRepository(NTTDataContext NTTDataDb) : base(NTTDataDb) {}
       
        /// <summary>
        /// Keep this method for backwards compatibility.
        /// </summary>
        public override Task<List<Product>> GetAllAsync() => base.GetAllAsync();
        public override async Task<Product?> GetAsync(int id)
        {
            try
            {
                return await DbSet.FirstOrDefaultAsync(item => item.ProductId == id);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw;
            }
        }
        public override async Task<bool> AddEntity(Product entity)
        {
            try
            {
                await DbSet.AddAsync(entity);
                return true;

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw;
            }
        }
        public override async Task<bool> UpdateEntity(Product entity)
        {
            try
            {
                var existdata = await DbSet.FirstOrDefaultAsync(item => item.ProductId == entity.ProductId);
                if (existdata != null)
                {
                    existdata.Name = entity.Name;
                    existdata.UnitPrice = entity.UnitPrice;
                    existdata.Cost = entity.Cost;
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw;
            }
        }

        public override async Task<bool> DeleteEntity(int id)
        {
            var existdata = await DbSet.FirstOrDefaultAsync(item => item.ProductId == id);
            if (existdata != null)
            {
                DbSet.Remove(existdata);
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
