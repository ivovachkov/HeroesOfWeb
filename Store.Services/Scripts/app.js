/// <reference path="libs/_references.js" />
(function () {
    var appLayout = new kendo.Layout('<div id="main-nav"></div><div id="main-view"></div>');
    //<aside id="actions"></aside>
    var data = persisters.get("api/");

    VMfactory.setPersister(data);

    function rerout(route) {
        router.navigate(route);
    }

    var router = new kendo.Router();
    router.route("/", function () {
        if (!data.users.currentUser()) {
            $("#app").children().children().not("#main-nav").hide();
            router.navigate("/login");
        } else {
            router.navigate("/main");
        }
    });
    router.route("/login", function () {
        if (data.users.currentUser()) {
            router.navigate("/");
        } else {
            viewsFactory.getView('login-form')
                .then(function (viewHtml) {
                    var viewModel = VMfactory.getLoginVM(
                        function () {
                            router.navigate("/main");
                        });
                    var view = new kendo.View(viewHtml, { model: viewModel });
                    appLayout.showIn("#main-nav", view);
                }, function (err) {
                    alert(err);
                });
        }
    });
    router.route("/main", function () {
        if (!data.users.currentUser()) {
            router.navigate("/");
        } else {
            viewsFactory.getView("home")
                .then(function (viewHtml) {
                    var viewModel = VMfactory.getGameVM(
                        function () {
                            router.navigate("/");
                        });
                    var view = new kendo.View(viewHtml, { model: viewModel });
                    $("#app").children().children().not("#main-nav").show();
                    appLayout.showIn("#main-nav", view);
                })
                .then(function () {
                    viewsFactory.getView("hero")
                        .then(function (viewHtml) {
                            VMfactory.getHeroVM(rerout,
                                function (vm) {
                                    viewsFactory.getView("stats")
                                    .then(function (viewHtml) {
                                        view = new kendo.View(viewHtml, { model: vm });
                                        appLayout.showIn(".stats", view);
                                    });
                                }
                            )
                            .then(function (vm) {
                                var view = new kendo.View(viewHtml, { model: vm });
                                appLayout.showIn("#main-view", view);
                                viewsFactory.getView("stats")
                                .then(function (viewHtml) {
                                    view = new kendo.View(viewHtml, { model: vm });
                                    appLayout.showIn(".stats", view);
                                });
                            }, function (err) {
                                alert(err);
                            });
                        });
                });
        }
    });
    router.route("/shop", function () {
        if (!data.users.currentUser()) {
            router.navigate("/");
        } else {
            viewsFactory.getView("home")
                .then(function (viewHtml) {
                    var viewModel = VMfactory.getGameVM(
                        function () {
                            $("#app").children().children().not("#main-nav").html("");
                            router.navigate("/");
                        });
                    var view = new kendo.View(viewHtml, { model: viewModel });
                    appLayout.showIn("#main-nav", view);
                })
                .then(function () {
                    viewsFactory.getView("shop")
                    .then(function (viewHtml) {
                        VMfactory.getShopVM(rerout)
                        .then(function (vm) {
                            var view = new kendo.View(viewHtml, { model: vm });
                            appLayout.showIn("#main-view", view);
                        }, function (err) {
                            alert(err);
                        });
                    });
                });
        }
    });
    router.route("/monster", function () {
        if (!data.users.currentUser()) {
            router.navigate("/");
        } else {
            viewsFactory.getView("home")
                .then(function (viewHtml) {
                    var viewModel = VMfactory.getGameVM();
                    var view = new kendo.View(viewHtml, { model: viewModel });
                    appLayout.showIn("#main-nav", view);
                })
                .then(function () {
                    viewsFactory.getView("monster")
                    .then(function (viewHtml) {
                        VMfactory.getMonsterVM(rerout)
                        .then(function (vm) {
                            var view = new kendo.View(viewHtml, { model: vm });
                            appLayout.showIn("#main-view", view);
                        }, function (err) {
                            alert(err);
                        });
                    });
                });
        }
    });
    router.route("/battleMonster/:id", function (id) {
        if (!data.users.currentUser()) {
            router.navigate("/");
        } else {
            viewsFactory.getView("home")
                .then(function (viewHtml) {
                    var viewModel = VMfactory.getGameVM(
                        function () {
                            $("#app").children().children().not("#main-nav").html("");
                            router.navigate("/");
                        });
                    var view = new kendo.View(viewHtml, { model: viewModel });
                    appLayout.showIn("#main-nav", view);
                })
                .then(function () {
                    viewsFactory.getView("battle")
                    .then(function (viewHtml) {
                        VMfactory.getBattleMonsterVM(id, rerout)
                        .then(function (vm) {
                            var view = new kendo.View(viewHtml, { model: vm });
                            appLayout.showIn("#main-view", view);
                        })
                    })
                });
        }
    });
    router.route("/item/:id", function (id) {
        if (data.users.currentUser() === undefined) {
            router.navigate("/");
        } else {
            viewsFactory.getView("home")
                .then(function (viewHtml) {
                    var viewModel = VMfactory.getGameVM(
                        function () {
                            $("#app").children().children().not("#main-nav").html("");
                            router.navigate("/");
                        });
                    var view = new kendo.View(viewHtml, { model: viewModel });
                    appLayout.showIn("#main-nav", view);
                })
                .then(function () {
                    viewsFactory.getView('item')
                        .then(function (viewHtml) {
                            VMfactory.getItemVM(id, rerout)
                            .then(function (vm) {
                                var view = new kendo.View(viewHtml, { model: vm });
                                appLayout.showIn("#main-view", view);
                            },
                            function (err) {
                                alert(err);
                            });
                        });
                });
        }
    });
    router.route("/construction", function () {
        if (!data.users.currentUser()) {
            router.navigate("/");
        } else {
            viewsFactory.getView("home")
                .then(function (viewHtml) {
                    var viewModel = VMfactory.getGameVM(
                        function () {
                            $("#app").children().children().not("#main-nav").html("");
                            router.navigate("/");
                        });
                    var view = new kendo.View(viewHtml, { model: viewModel });
                    appLayout.showIn("#main-nav", view);
                })
                .then(function () {
                    viewsFactory.getView("construction")
                    .then(function (viewHtml) {
                        var viewModel = VMfactory.getConstuctionVM(rerout);
                        var view = new kendo.View(viewHtml, { model: viewModel });
                        appLayout.showIn("#main-view", view);
                    });
                });
        }
    });
    router.route("/PvP", function () {
        if (!data.users.currentUser()) {
            router.navigate("/");
        } else {
            viewsFactory.getView("home")
                .then(function (viewHtml) {
                    var viewModel = VMfactory.getGameVM(
                        function () {
                            $("#app").children().children().not("#main-nav").html("");
                            router.navigate("/");
                        });
                    var view = new kendo.View(viewHtml, { model: viewModel });
                    appLayout.showIn("#main-nav", view);
                })
                .then(function () {
                    viewsFactory.getView("PvP")
                    .then(function (viewHtml) {
                        VMfactory.getPvPVM(rerout)
                        .then(function (vm) {
                            var view = new kendo.View(viewHtml, { model: vm });
                            appLayout.showIn("#main-view", view);
                        });
                    });
                });
        }
    });

    $(function () {
        appLayout.render($('#app'));
        router.start();
    });
}());