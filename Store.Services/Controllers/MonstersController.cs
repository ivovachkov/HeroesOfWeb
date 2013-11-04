using Store.Data;
using Store.Models;
using Store.Services.Attributes;
using Store.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ValueProviders;

namespace Store.Services.Controllers
{
    public class MonstersController : BaseApiController
    {
        [HttpGet]
        public IQueryable<MonsterModel> GetAll(
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

                var monsterEntities = context.Monsters;
                var models =
                     from monsterEntity in monsterEntities
                     select new MonsterModel()
                     {
                         MonsterId = monsterEntity.MonsterId,
                         Name = monsterEntity.Name,
                         MeleAttack = monsterEntity.MeleAttack,
                         MagicAttack = monsterEntity.MagicAttack,
                         MagicDefense = monsterEntity.MagicDefense,
                         MeleDefense = monsterEntity.MeleDefense,
                         HP = monsterEntity.HP,
                         MP = monsterEntity.MP
                     };

                return models;
            });

            return responseMsg;
        }

        public MonsterModel GetById(
            int id,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var models = this.GetAll(sessionKey)
                .Where(i => i.MonsterId == id);
            return models.FirstOrDefault();
        }

        public HttpResponseMessage PostCreateMonster(
            MonsterModel model,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    if (model.Name == null)
                    {
                        throw new ArgumentNullException("name", "The name cannot be null!");
                    }

                    var context = new StoreContext();
                    using (context)
                    {
                        var admin = context.Admins.FirstOrDefault(adm => adm.SessionKey == sessionKey);
                        if (admin == null)
                        {
                            throw new InvalidOperationException("You are not admin!");
                        }

                        var monster = new Monster()
                        {
                            Name = model.Name,
                            MeleAttack = model.MeleAttack,
                            MagicAttack = model.MagicAttack,
                            MagicDefense = model.MagicDefense,
                            MeleDefense = model.MeleDefense,
                            HP = model.HP,
                            MP = model.MP
                        };

                        context.Monsters.Add(monster);
                        context.SaveChanges();

                        var createdMonsterModel = new CreatedMonsterModel()
                        {
                            Monsterid = monster.MonsterId,
                            Name = monster.Name
                        };

                        var response =
                            this.Request.CreateResponse(HttpStatusCode.Created, createdMonsterModel);
                        return response;
                    }
                });

            return responseMsg;
        }

        public HttpResponseMessage PutUpdateMonster(
            int id,
            [FromBody] MonsterModel model,
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

                      var monster = context.Monsters.FirstOrDefault(m => m.MonsterId == id);
                      if (monster == null)
                      {
                          throw new ArgumentOutOfRangeException("monsterId", "Invalid monster");
                      }

                      UpdateMonster(monster, model, context);
                      context.SaveChanges();

                      var response =
                          this.Request.CreateResponse(HttpStatusCode.OK);
                      return response;
                  }
              });

            return responseMsg;
        }

        private void UpdateMonster(Monster monster, MonsterModel model, StoreContext context)
        {
            if (model.Name != null)
            {
                monster.Name = model.Name;
            }

            monster.MagicAttack = model.MagicAttack;
            monster.MagicDefense = model.MagicDefense;
            monster.MeleAttack = model.MeleAttack;
            monster.MeleDefense = model.MeleDefense;
            monster.HP = model.HP;
            monster.MP = model.MP;
        }
    }
}
