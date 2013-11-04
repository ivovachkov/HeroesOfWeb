/// <reference path="../_references.js" />

window.controllers = (function () {
    //var baseUrl = "http://heroesofjavascript.apphb.com/api/";
    var baseUrl = "http://localhost:44998/api/";

    var nickname = localStorage.getItem("nickname");
    var sessionKey = localStorage.getItem("sessionKey");
    var monster = {};
    var item = {};
    function saveUserData(userData) {
        localStorage.setItem("nickname", userData.displayName);
        localStorage.setItem("sessionKey", userData.sessionKey);
        nickname = userData.displayName;
        sessionKey = userData.sessionKey;
    }
    function clearUserData() {
        localStorage.removeItem("nickname");
        localStorage.removeItem("sessionKey");
        nickname = "";
        sessionKey = "";
    }
    function isUserLoggedIn() {
        var isLoggedIn = localStorage.getItem("sessionKey") != null;
        return isLoggedIn;
    }

    function AdminController($scope, $http, $location) {
        $scope.user = {
            username: "",
            displayName: "",
            authCode: ""
        };

        $scope.loginAdmin = function () {
            console.log($scope.user);
            var userToLogin = {
                username: $scope.user.username,
                authCode: CryptoJS.SHA1($scope.user.authCode).toString()
            };

            //var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } };

            //var pairs = [];
            //for (var key in userToLogin) {
            //    if (userToLogin.hasOwnProperty(key)) {
            //        pairs.push(encodeURIComponent(key) + "=" + encodeURIComponent(userToLogin[key]));
            //    }
            //}

            //var data = pairs.join("&");

            $http.post(baseUrl + "admins/login", userToLogin)
                .success(function (data) {
                    saveUserData(data);
                    $scope.user.authCode = "";
                    $location.path("/logout");
                }).error(function (err) {
                    $location.path("/admin/login");
                    alert(err.Message);
                });
        }

        $scope.logoutAdmin = function () {
            var config = { headers: { "X-sessionKey": sessionKey } };
            $http.put(baseUrl + "admins/logout", {}, config)
                .success(function () {
                    $scope.user = {
                        username: "",
                        displayName: "",
                        authCode: ""
                    };
                    clearUserData();
                    $location.path("/");
                }).error(function (err) {
                    alert(err.Message);
                });
        }

        $scope.registerAdmin = function () {
            var userToRegister = {
                username: $scope.user.username,
                displayName: $scope.user.displayName,
                authCode: CryptoJS.SHA1($scope.user.authCode).toString()
            };
            var config = { headers: { "X-sessionKey": sessionKey } };
            $http.post(baseUrl + "admins/register", userToRegister, config)
                .success(function (data) {
                    saveUserData(data);
                    $scope.user.authCode = "";
                    alert("Admin created!");
                }).error(function (err) {
                    alert(err.Message);
                });
        }
    }

    function HomeController($location) {
        if (isUserLoggedIn()) {
            $location.path("/logout");
        }
    }

    function UsersController($scope, $http) {
        $scope.users = [];
        var config = { headers: { "X-sessionKey": sessionKey } };

        $http.get(baseUrl + "users", config)
            .success(function (data) {
                for (var i in data) {
                    $scope.users.push(data[i]);
                }
            }).error(function (err) {
                alert(err.Message);
            });
    }

    function SingleUserController($scope, $http, $routeParams) {
        var id = $routeParams.id;
        var config = { headers: { "X-sessionKey": sessionKey } };

        $http.get(baseUrl + "users/" + id, config)
            .success(function (data) {
                $scope.user = data;
                console.log(data);
            }).error(function (err) {
                alert(err.Message);
            });
    }

    function MonstersController($scope, $http, $routeParams) {
        $scope.monster = {
            id: "",
            name: "",
            meleAttack: "",
            magicAttack: "",
            magicDefense: "",
            meleDefense: "",
            hp: ""
        };

        if (monster) {
            $scope.monster = monster;
        }

        $scope.monsters = [];

        var config = { headers: { "X-sessionKey": sessionKey } };

        if ($routeParams.id) {
            $http.get(baseUrl + "monsters/" + $routeParams.id, config)
                .success(function (data) {
                    $scope.monster = data;
                }).error(function (err) {
                    alert(err.Message);
                });
        }
        else {
            $http.get(baseUrl + "monsters", config)
            .success(function (data) {
                for (var i in data) {
                    $scope.monsters.push(data[i]);
                }
            }).error(function (err) {
                alert(err.Message);
            });
        }

        $scope.createMonster = function () {
            $http.post(baseUrl + "monsters/", $scope.monster, config)
                .success(function (data) {
                    $scope.monster = data;
                    console.log(data);
                    alert("Monster created");
                }).error(function (err) {
                    alert(err.Message);
                });
        }

        $scope.getMonster = function (id) {
            $http.get(baseUrl + "monsters/" + id, config)
            .success(function (data) {
                $scope.monster = data;
            }).error(function (err) {
                alert(err.Message);
            });
        }

        $scope.updateMonster = function () {
            var config = { headers: { "X-sessionKey": sessionKey } };

            $http.put(baseUrl + "monsters/" + $routeParams.id, $scope.monster, config)
                .success(function () {
                    alert("Monster updated");
                }).error(function (err) {
                    alert(err.Message);
                });
        }
    }

    function ItemsController($scope, $http, $routeParams) {
        $scope.item = {
            id: "",
            name: "",
            description: "",
            meleAttack: "",
            magicAttack: "",
            magicDefense: "",
            meleDefense: "",
            itemCategory: "",
            imageUrl: "",
            hp: "",
            mp: ""
        };

        if (item) {
            $scope.item = item;
        }

        $scope.items = [];

        var config = { headers: { "X-sessionKey": sessionKey } };

        if ($routeParams.id) {
            $http.get(baseUrl + "items/" + $routeParams.id, config)
                .success(function (data) {
                    $scope.item = data;
                }).error(function (err) {
                    alert(err.Message);
                });
        }
        else {
            $http.get(baseUrl + "items", config)
            .success(function (data) {
                for (var i in data) {
                    $scope.items.push(data[i]);
                }
            }).error(function (err) {
                alert(err.Message);
            });
        }

        $scope.createItem = function () {
            $http.post(baseUrl + "items/", $scope.item, config)
                .success(function (data) {
                    $scope.item = data;
                    console.log(data);
                    alert("Item "+ data.name +" created");
                }).error(function (err) {
                    alert(err.Message);
                });
        }

        $scope.getItem = function () {
            $http.get(baseUrl + "items/" + $routeParams.id, config)
            .success(function (data) {
                $scope.item = data;
            }).error(function (err) {
                alert(err.Message);
            });
        }

        $scope.updateItem = function () {
            var config = { headers: { "X-sessionKey": sessionKey } };

            $http.put(baseUrl + "items/" + $routeParams.id, $scope.item, config)
                .success(function () {
                    alert("Item  updated");
                }).error(function (err) {
                    alert(err.Message);
                });
        }
    }

    return {
        getHomeController: HomeController,
        getAdminController: AdminController,
        getUsersController: UsersController,
        getSingleUserController: SingleUserController,
        getMonsterController: MonstersController,
        getItemsController: ItemsController
    };
})();