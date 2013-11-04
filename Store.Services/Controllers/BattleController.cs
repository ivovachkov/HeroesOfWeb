using Store.Data;
using Store.Services.Attributes;
using Store.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ValueProviders;
using Store.Models;

namespace Store.Services.Controllers
{
    public class BattleController : BaseApiController
    {
        [HttpPost]
        public HttpResponseMessage CreateBattle([FromBody] CreateBattleModel model,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new StoreContext();

                var user = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new InvalidOperationException("Invalid session key!");
                }

                var secondUser = context.Users.FirstOrDefault(usr => usr.UserId == model.SecondUserId);

                var battle = new Battle
                {
                    FirstUser = user,
                    SecondUser = secondUser,
                    UserInTurn = model.UserInTurnId == model.FirstUserId ? user : secondUser
                };

                context.Battles.Add(battle);
                context.SaveChanges();

                var createdBattleModel = new BattleCreateReturnModel()
                {
                    ID = battle.Id
                };

                var response =
                    this.Request.CreateResponse(HttpStatusCode.Created, createdBattleModel);
                return response;
            });

            return responseMsg;
        }

        [HttpGet]
        public BattleCreateReturnModel GetBattle([FromBody] CreateBattleModel model,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new StoreContext();

                var user = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new InvalidOperationException("Invalid session key!");
                }

                var secondUser = context.Users.FirstOrDefault(usr => usr.UserId == model.SecondUserId);

                var battle = context.Battles
                    .Where(b =>
                        (b.SecondUser.UserId == model.SecondUserId && b.FirstUser.UserId == model.FirstUserId) ||
                        (b.FirstUser.UserId == model.SecondUserId && b.SecondUser.UserId == model.FirstUserId)).FirstOrDefault();

                if (battle == null)
                {
                    throw new ArgumentNullException("Cannot find the requested battle!");
                }

                var createdBattleModel = new BattleCreateReturnModel()
                {
                    ID = battle.Id
                };

                return createdBattleModel;
            });

            return responseMsg;
        }

        [HttpPut]
        [ActionName("attackWithWeapon")]
        public HttpResponseMessage PerformAttackWithWeapon(int battleId, [FromBody] CreateBattleModel model,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new StoreContext();

                var user = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new InvalidOperationException("Invalid session key!");
                }

                var secondUser = context.Users.FirstOrDefault(usr =>
                    (usr.UserId == model.SecondUserId && usr.UserId != user.UserId) ||
                    usr.UserId == model.FirstUserId && usr.UserId != user.UserId);

                var battle = context.Battles.FirstOrDefault(b => b.Id == battleId);

                if (battle == null)
                {
                    throw new ArgumentNullException("Invalid battle!");
                }

                if (battle.UserInTurn != user)
                {
                    throw new ArgumentException("You are not in turn!");
                }

                Attack(user.Hero.MeleAttack, secondUser.Hero.MeleDefense, secondUser);

                battle.UserInTurn = battle.UserInTurn == secondUser ? user : secondUser;
                context.SaveChanges();

                var createdBattleModel = new BattleCreateReturnModel()
                {
                    ID = battle.Id
                };

                var response =
                    this.Request.CreateResponse(HttpStatusCode.OK, createdBattleModel);
                return response;
            });

            return responseMsg;
        }

        [HttpPut]
        [ActionName("attackWithMagic")]
        public HttpResponseMessage PerformAttackWithMagic(int battleId, [FromBody] CreateBattleModel model,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new StoreContext();

                var user = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new InvalidOperationException("Invalid session key!");
                }

                var secondUser = context.Users.FirstOrDefault(usr =>
                    (usr.UserId == model.SecondUserId && usr.UserId != user.UserId) ||
                    usr.UserId == model.FirstUserId && usr.UserId != user.UserId);

                var battle = context.Battles.FirstOrDefault(b => b.Id == battleId);

                if (battle == null)
                {
                    throw new ArgumentNullException("Invalid battle!");
                }

                if (battle.UserInTurn != user)
                {
                    throw new ArgumentException("You are not in turn!");
                }

                Attack(user.Hero.MagicAttack, secondUser.Hero.MagicDefense, secondUser);

                battle.UserInTurn = battle.UserInTurn == secondUser ? user : secondUser;
                context.SaveChanges();

                var createdBattleModel = new BattleCreateReturnModel()
                {
                    ID = battle.Id
                };

                var response =
                    this.Request.CreateResponse(HttpStatusCode.OK, createdBattleModel);
                return response;
            });

            return responseMsg;
        }

        private void Attack(int attack, int defense, User defender)
        {
            var damage = attack - defense;
            if (damage < 0)
            {
                damage = 0;
            }

            defender.Hero.HP = defender.Hero.HP - damage < 0 ? 0 : defender.Hero.HP - damage;
        }
    }
}
