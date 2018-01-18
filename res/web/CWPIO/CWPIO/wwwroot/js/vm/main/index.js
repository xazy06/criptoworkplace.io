(function (app, w) {

    var Vm = function (app, w) {

        var _ko = ko.observable, controller = c = this;

        this.api = {
            form: {
                send: '/Home/Subscribe'
            }
        };

        this.models = {
            feedModel: {
                name: _ko(''),
                email: _ko('')
            }
        };

        this.strings = {
            form: {
                send: {
                    fail: app.resources('homeFormSendFail'),
                    success: app.resources('homeFormSendSuccess')
                }
            }
        };

        this.actions = {
            form: {

                refresh: function () {
                    this.models.feedModel.name('');
                    this.models.feedModel.email('');

                }.bind(this),

                send: function () {
                    var name = this.models.feedModel.name(),
                        email = this.models.feedModel.email();

                    $.post(this.api.form.send, { name: name, email: email })
                        .done(controller.actions.form.sendDone)
                        .fail(controller.actions.form.sendFail);
                    
                    name = null;
                    email = null;

                }.bind(this),

                filled: function (fields) {
                    var res = true;

                    for (var i in fields) {
                        if (fields.hasOwnProperty(i) && ko.unwrap(fields[i]).length === 0) {
                            res = false; break;
                        }
                    }

                    return res;
                },

                sendEnabled: ko.pureComputed(function () {
                    //return c.actions.form.filled.apply(null, c.models.feedModel.name, c.models.feedModel.email);
                    return controller.models.feedModel.name().length > 0 && controller.models.feedModel.email().length > 0;
                }),

                sendDone: function (response) {
                    if (response && response.result === true) {
                        this.actions.form.refresh();
                        w.hel.modal(controller.strings.form.send.success);
                    }
                    else
                        w.hel.modal(controller.strings.form.send.fail);

                }.bind(this),

                sendFail: function (response) {
                    w.hel.modal(controller.strings.form.send.fail);
                }

            }
        };

    };

    Vm = new Vm(app, w);

    return ko.applyBindings(Vm, w.hel.ge('contact'));

}).call(null, App, window);
