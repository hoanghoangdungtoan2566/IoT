﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvc;

namespace WinApp.Controllers
{
    class HomeController : BaseController
    {
        public object Default()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Login(string un, string pw)
        {
            WebRequest.ResponseError += r => {
                Engine.Message(r.Message);
            };

            var v = new { UserName = un, Password = pw };
            return Post(CreateApiContext(v), o => {
                this.User = o.ToObject<Models.UserInfo>();
                Redirect("device");
            });
        }
        public ActionResult Exit()
        {
            MqttController.Disconnect();
            return null;
        }
    }
}
