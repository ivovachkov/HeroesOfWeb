/// <reference path="_references.js" />

angular.module("store", [])
	.config(["$routeProvider", function ($routeProvider) {
	    $routeProvider
			.when("/", {
			    templateUrl: "scripts/partials/home.html",
                controller: controllers.getHomeController
			})
            .when("/logout", {
                templateUrl: "scripts/partials/logout.html",
                controller: controllers.getAdminController
            })
            .when("/admin/login", {
                templateUrl: "scripts/partials/admin-login-form.html",
                controller: controllers.getAdminController
            })
            .when("/admin/register", {
                templateUrl: "scripts/partials/admin-register-form.html",
                controller: controllers.getAdminController
            })
            .when("/users", {
                templateUrl: "scripts/partials/all-users.html",
                controller: controllers.getUsersController
            })
            .when("/users/:id", {
                templateUrl: "scripts/partials/user.html",
                controller: controllers.getSingleUserController
            })
            .when("/monster/create", {
                templateUrl: "scripts/partials/create-monster-form.html",
                controller: controllers.getMonsterController
            })
            .when("/monster/:id/update", {
                templateUrl: "scripts/partials/update-monster-form.html",
                controller: controllers.getMonsterController
            })
            .when("/monsters", {
                templateUrl: "scripts/partials/all-monsters.html",
                controller: controllers.getMonsterController
            })
            .when("/monster/:id", {
                templateUrl: "scripts/partials/monster.html",
                controller: controllers.getMonsterController
            })
            .when("/item/create", {
                templateUrl: "scripts/partials/create-item-form.html",
                controller: controllers.getItemsController
            })
            .when("/item/:id/update", {
                templateUrl: "scripts/partials/update-item-form.html",
                controller: controllers.getItemsController
            })
            .when("/items", {
                templateUrl: "scripts/partials/all-items.html",
                controller: controllers.getItemsController
            })
            .when("/item/:id", {
                templateUrl: "scripts/partials/item.html",
                controller: controllers.getItemsController
            })
			.otherwise({ redirectTo: "/" });
	}]);


//.controller("MonstersController", function ($scope, $routeParams) {
//    if ($routeParams.id) {
//        $scope.getMonster();
//    }
//});
//.config(['$httpProvider', function ($httpProvider) {
//    $httpProvider.defaults.useXDomain = true;
//    //$httpProvider.defaults.headers.common['Accept'] = 'application/json, text/html';
//    delete $httpProvider.defaults.headers.common['X-Requested-With'];
//}]);