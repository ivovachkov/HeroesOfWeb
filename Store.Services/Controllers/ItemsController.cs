using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ValueProviders;
using Store.Data;
using Store.Models;
using Store.Services.Attributes;
using Store.Services.Models;

namespace Store.Services.Controllers
{
    public class ItemsController : BaseApiController
    {
        [HttpGet]
        public IQueryable<ItemModel> GetAll(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new StoreContext();

                var user = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);
                var admin = context.Admins.FirstOrDefault(adm => adm.SessionKey == sessionKey);
                if (user == null && admin == null)
                {
                    throw new InvalidOperationException("Invalid session key!");
                }

                var models = context.Items.Select(ItemModel.FromItem);
                //var models =
                //     from itemEntity in itemEntities
                //     select new ItemModel()
                //     {
                //         ItemId = itemEntity.ItemId,
                //         Name = itemEntity.Name,
                //         Description = itemEntity.Description,
                //         MagicAttack = itemEntity.MagicAttack,
                //         MeleAttack = itemEntity.MeleAttack,
                //         MagicDefense = itemEntity.MagicDefense,
                //         MeleDefense = itemEntity.MeleDefense,
                //         ImageUrl = itemEntity.ImageUrl,
                //         ItemCategory = itemEntity.ItemCategory.Name
                //     };

                return models;
            });

            return responseMsg;
        }

        public ItemModel GetById(
            int id,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var models = this.GetAll(sessionKey)
                .Where(i => i.ItemId == id);
            return models.FirstOrDefault();
        }

        public IQueryable<ItemModel> GetByCategory(
            string category,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var models = this.GetAll(sessionKey)
                .Where(item => item.ItemCategory == category);
            return models;
        }

        public HttpResponseMessage PostCreateItem(
            ItemModel model,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    if (model.Name == null)
                    {
                        throw new ArgumentNullException("name", "The name cannot be null!");
                    }

                    if (model.ItemCategory == null)
                    {
                        throw new ArgumentNullException("category", "The category cannot be null!");
                    }

                    var context = new StoreContext();
                    using (context)
                    {
                        var admin = context.Admins.FirstOrDefault(adm => adm.SessionKey == sessionKey);
                        if (admin == null)
                        {
                            throw new InvalidOperationException("You are not admin!");
                        }

                        string categoryNameLower = model.ItemCategory.ToLower();
                        var category = context.Categories.FirstOrDefault(c => c.Name == categoryNameLower);
                        if (category == null)
                        {
                            category = new ItemCategory()
                            {
                                Name = categoryNameLower
                            };
                        }

                        var item = new Item()
                        {
                            Name = model.Name,
                            ItemCategory = category,
                            MeleDefense = model.MeleDefense,
                            MeleAttack = model.MeleAttack,
                            MagicDefense = model.MagicDefense,
                            MagicAttack = model.MagicAttack,
                            Description = model.Description,
                            ImageUrl = model.ImageUrl,
                            Equiped = false,
                            HP = model.HP,
                            MP = model.MP,
                            Price = model.Price
                        };

                        context.Items.Add(item);
                        context.SaveChanges();

                        var createdItemModel = new CreatedItemModel()
                        {
                            ItemId = item.ItemId,
                            Name = item.Name
                        };

                        var response =
                            this.Request.CreateResponse(HttpStatusCode.Created, createdItemModel);
                        return response;
                    }
                });

            return responseMsg;
        }

        public HttpResponseMessage PutUpdateItem(
            int id,
            [FromBody] UpdatingItemModel model,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
              () =>
              {
                  var context = new StoreContext();
                  using (context)
                  {
                      var admin = context.Admins.FirstOrDefault(u => u.SessionKey == sessionKey);
                      if (admin == null)
                      {
                          throw new ArgumentOutOfRangeException("You are not admin!");
                      }

                      var item = context.Items.FirstOrDefault(i => i.ItemId == id);
                      if (item == null)
                      {
                          throw new ArgumentOutOfRangeException("itemId", "Invalid item");
                      }

                      UpdateItem(item, model, context);
                      context.SaveChanges();

                      var response =
                          this.Request.CreateResponse(HttpStatusCode.OK);
                      return response;
                  }
              });

            return responseMsg;
        }

        private void UpdateItem(Item item, UpdatingItemModel model, StoreContext context)
        {
            if (model.Name != null)
            {
                item.Name = model.Name;
            }

            if (model.Description != null)
            {
                item.Description = model.Description;
            }

            if (model.ImageUrl != null)
            {
                item.ImageUrl = model.ImageUrl;
            }

            if (model.ItemCategory != null)
            {
                var categoryNameToLower = model.ItemCategory.ToLower();
                var category = context.Categories.FirstOrDefault(c => c.Name == categoryNameToLower);
                if (category == null)
                {
                    category = new ItemCategory
                    {
                        Name = categoryNameToLower
                    };
                }

                item.ItemCategory = category;
            }

            item.MagicAttack = model.MagicAttack;
            item.MagicDefense = model.MagicDefense;
            item.MeleAttack = model.MeleAttack;
            item.MeleDefense = model.MeleDefense;
            item.Price = model.Price;
        }
    }
}
