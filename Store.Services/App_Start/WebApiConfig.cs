using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Store.Services
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "UsersREgApi",
                routeTemplate: "api/users/register",
                defaults: new
                {
                    controller = "users",
                    action = "register"
                }
            );

            config.Routes.MapHttpRoute(
                name: "UsersLogApi",
                routeTemplate: "api/users/login",
                defaults: new
                {
                    controller = "users",
                    action = "login"
                }
            );

            config.Routes.MapHttpRoute(
                name: "UsersLogoutApi",
                routeTemplate: "api/users/logout",
                defaults: new
                {
                    controller = "users",
                    action = "logout"
                }
            );

            config.Routes.MapHttpRoute(
                name: "UsersUpdateApi",
                routeTemplate: "api/users/update",
                defaults: new
                {
                    controller = "users",
                    action = "update"
                }
            );

            config.Routes.MapHttpRoute(
                name: "AdminsIdApi",
                routeTemplate: "api/admins/{action}",
                defaults: new
                {
                    controller = "admins"
                }
            );

            config.Routes.MapHttpRoute(
                name: "HeroesApi",
                routeTemplate: "api/hero/{action}",
                defaults: new
                {
                    controller = "hero"
                }
            );

            config.Routes.MapHttpRoute(
                name: "BattleAttackWithWeaponApi",
                routeTemplate: "api/battle/{battleId}/attackWithWeapon",
                defaults: new
                {
                    controller = "battle",
                    action = "attackWithWeapon"
                }
            );

            config.Routes.MapHttpRoute(
                name: "BattleAttackWithMagicApi",
                routeTemplate: "api/battle/{battleId}/attackWithMagic",
                defaults: new
                {
                    controller = "battle",
                    action = "attackWithMagic"
                }
            );

            //config.Routes.MapHttpRoute(
            //    name: "ItemsApi",
            //    routeTemplate: "api/items/{action}",
            //    defaults: new
            //    {
            //        controller = "items"
            //    }
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional
                }
            );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
        }
    }
}
