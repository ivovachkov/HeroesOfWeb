using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Store.Services.Models;
using Store.Data;
using Store.Models;
using System.Web.Http.ValueProviders;
using Store.Services.Attributes;

namespace Store.Services.Controllers
{
    public class HeroController : BaseApiController
    {
        public HttpResponseMessage GetAll(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var response = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new StoreContext();

                var user = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new InvalidOperationException("Invalid session key");
                }

                var heros = context.Heros.Select(HeroModel.FromHero);

                var responseMsg = this.Request.CreateResponse(HttpStatusCode.OK, heros);
                return responseMsg;
            });

            return response;
        }

        public HttpResponseMessage GetUsersHero(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var response = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new StoreContext();

                var user = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new InvalidOperationException("Invalid session key");
                }

                var hero = context.Heros
                    .Where(h => h.User.UserId == user.UserId)
                    .Select(HeroModel.FromHero);

                var responseMsg = this.Request.CreateResponse(HttpStatusCode.OK, hero);
                return responseMsg;
            });

            return response;
        }

        public HttpResponseMessage GetById(int id,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var response = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new StoreContext();

                var user = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new InvalidOperationException("Invalid session key");
                }

                var hero = context.Heros.Where(h => h.HeroId == id).Select(HeroModel.FromHero);

                var responseMsg = this.Request.CreateResponse(HttpStatusCode.OK, hero);
                return responseMsg;
            });

            return response;
        }

        public HttpResponseMessage PostCreateHero(HeroCreateModel heroModel,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var response = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new StoreContext();

                var user = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new InvalidOperationException("Invalid session key");
                }

                Hero newHero = new Hero
                {
                    Name = heroModel.Name,
                    MaxHP = heroModel.MaxHP,
                    HP = heroModel.HP,
                    MaxMP = heroModel.MaxMP,
                    MP = heroModel.MP,
                    MagicAttack = heroModel.MagicAttack,
                    MeleAttack = heroModel.MeleAttack,
                    MagicDefense = heroModel.MagicDefense,
                    MeleDefense = heroModel.MeleDefense,
                    Experience = heroModel.Experience,
                    Level = heroModel.Level,
                    User = user
                };

                context.Heros.Add(newHero);
                context.SaveChanges();

                user.Hero = newHero;
                context.SaveChanges();

                var entity = new HeroCreateReturnModel
                {
                    ID = newHero.HeroId,
                    Name = newHero.Name
                };

                var responseMsg = this.Request.CreateResponse(HttpStatusCode.Created, entity);
                return responseMsg;
            });

            return response;
        }

        public HttpResponseMessage PutUpdateHero(HeroUpdateModel heroModel,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                         () =>
                         {
                             var context = new StoreContext();
                             using (context)
                             {
                                 var user = context.Users.FirstOrDefault(u => u.SessionKey == sessionKey);
                                 if (user == null)
                                 {
                                     throw new ArgumentOutOfRangeException("Invalid session key!");
                                 }

                                 var hero = user.Hero;
                                 if (hero == null)
                                 {
                                     throw new ArgumentNullException("hero", "Cannot update non-existing hero!");
                                 }

                                 UpdateHero(hero, heroModel, context);
                                 hero.Items = hero.Items;
                                 context.SaveChanges();

                                 var response =
                                     this.Request.CreateResponse(HttpStatusCode.OK, new LoggedUserModel());
                                 return response;
                             }
                         });

            return responseMsg;
        }

        private void UpdateHero(Hero hero, HeroUpdateModel model, StoreContext context)
        {
            if (model.Name != null)
            {
                hero.Name = model.Name;
            }

            if (model.Items != null)
            {
                //foreach (var i in hero.Items)
                //{
                //    i.Heroes.Remove(hero);
                //}

                while (hero.Items.Count > 0)
                {
                    hero.Items.Remove(hero.Items.First());
                }
                foreach (var i in model.Items)
                {
                    var item = context.Items.FirstOrDefault(x => x.ItemId == i.ItemId);
                    if (item == null)
                    {
                        throw new ArgumentNullException("item", "Cannot use invalid item!");
                    }

                    hero.Items.Add(item);
                }
            }

            if (model.Id <= 0)
            {
                throw new ArgumentNullException("HeroId", "Invalid hero id!");
            }

            hero.MagicAttack = model.MagicAttack;
            hero.MagicDefense = model.MagicDefense;
            hero.MeleAttack = model.MeleAttack;
            hero.MeleDefense = model.MeleDefense;
            hero.Experience = model.Experience;
            hero.MaxHP = model.MaxHP;
            hero.HP = model.HP;
            hero.MaxMP = model.MaxMP;
            hero.MP = model.MP;
            hero.Level = model.Level;
        }
    }

}
