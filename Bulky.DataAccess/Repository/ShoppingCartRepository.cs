﻿using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private  ApplicationDbContext db;

        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public void Update(ShoppingCart shoppingCart)
        {
            db.shoppingCarts.Update(shoppingCart);
        }
    }
}
